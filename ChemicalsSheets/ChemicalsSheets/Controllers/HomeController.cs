using ChemicalsSheets.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace ChemicalsSheets.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index(int startIndex ,int pageSize )
        {
            try
            {
                 using ( var db= new chemicalsEntities())
                            {
                              var products =  db.tblProducts.OrderBy(p=> p.Id).Skip(startIndex).Take(pageSize).ToList();
                              return View(products);
                            }
            }
            catch (Exception ex)
            {
                ViewBag.FileStatus = "Exception happened server could not be contacted !! Very Sorry";
                return View();
            }
           
            
        }

              
        public async Task<ActionResult> Browse(int Id, string productName, string productUrl)
        {
            try
            {
                ViewBag.Message = productName;
                ViewBag.Message2 = productUrl;
                var fileName = productName;
                //var path = String.Format(@"{0}DataSheets\", AppDomain.CurrentDomain.BaseDirectory);
                var path = @"~\DataSheets\";

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpClient client = new HttpClient();

                Uri _uri = new Uri(productUrl);

                client.BaseAddress = _uri;
                await client.GetAsync(_uri.LocalPath);
                client.DefaultRequestHeaders.Clear();
                var responce = client.GetAsync(_uri.LocalPath);
                responce.Wait();
                byte[] result = await responce.Result.Content.ReadAsByteArrayAsync();
                string totPath = fileName;
                if (!string.IsNullOrEmpty(path))
                {
                    string Folder = Server.MapPath(path);
                    totPath = Path.Combine(Folder, fileName);
                }
                bool isPdfFile = false;
                if (responce.Result.StatusCode == HttpStatusCode.OK)
                {
                    string contentType = "";
                    if (responce.Result.Content.Headers.ContentType.MediaType.ToString() == "application/pdf")
                    {
                        contentType = "application/pdf";
                        fileName = fileName + ".pdf";
                        isPdfFile = true;
                    }
                    else if (responce.Result.Content.Headers.ContentType.MediaType.ToString() == "text/html")
                    {
                        contentType = "text/html";
                        fileName = fileName + ".text";
                        isPdfFile = false;
                    }
                    using (var db = new chemicalsEntities())
                    {
                        tblProduct record = db.tblProducts.ToList().FirstOrDefault(p => p.Id == Id);
                        record.FilePath = fileName;
                        record.IsAvailable = isPdfFile;
                        record.FileContent = result;
                        db.Entry(record).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    FileContentResult fileContentResult = File(result, contentType, fileName);
                    ViewBag.FileStatus = "File Saved Success :)";
                    //return View();
                    return fileContentResult;
                }
                else
                {
                    using (var db = new chemicalsEntities())
                    {
                        tblProduct record = db.tblProducts.ToList().FirstOrDefault(p => p.Id == Id);
                        record.FilePath = fileName;
                        record.IsAvailable = false;
                        db.Entry(record).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    ViewBag.FileStatus = "File Not Found !! Very Sorry";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.FileStatus = "Exception happened server could not be contacted !! Very Sorry";
                return View();
            } 
        }

        [HttpGet]
        public ActionResult Open(int Id)
        {
            try
            {
                using (var db = new chemicalsEntities())
                {
                    tblProduct record = db.tblProducts.ToList().FirstOrDefault(p => p.Id == Id);
                    if (record.FileContent != null)
                    {
                        return File(record.FileContent, "application/pdf", record.FilePath);
                    }
                    else
                    {
                        ViewBag.FileStatus = "File Not Found";
                        return View();
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.FileStatus = "Exception happened server could not be contacted !! Very Sorry";
                return View();
            }
            
        }
    }
}