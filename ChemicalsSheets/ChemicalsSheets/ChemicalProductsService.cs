

using ChemicalsSheets.Models;
using Microsoft.AspNet.SignalR;
using System;
using TableDependency.SqlClient;
using Microsoft.AspNet.SignalR.Hubs;
using TableDependency.Mappers;
using System.Configuration;
using TableDependency.SqlClient.Base.Enums;
using System.Collections.Generic;

namespace ChemicalsSheets
{
    /*  a singleton service class to constitute the channel between database 
        and web application, able to be the listener for record modifications.
        For this we are going to use SqlTableDependency  */
    public class ChemicalProductsService : IDisposable
    {
        private readonly static Lazy<ChemicalProductsService> _instance = new Lazy<ChemicalProductsService>(() => new ChemicalProductsService(GlobalHost.ConnectionManager.GetHubContext<ChemicalProductsHub>().Clients));

        private SqlTableDependency<tblProduct> SqlTableDependency { get; }

        private IHubConnectionContext<dynamic> Clients;

        public static ChemicalProductsService Instance => _instance.Value;



        public ChemicalProductsService(IHubConnectionContext<dynamic> clients)
        {
            this.Clients = clients;
            
            //var mapper = new ModelToTableMapper<tblProduct>();
            //mapper.AddMapping(x => x.FilePath, "FilePath");

            // Because our C# model name differs from table name we have to specify database table name.
            this.SqlTableDependency = new SqlTableDependency<tblProduct>(ConfigurationManager.ConnectionStrings["chemicalsEntities"].ConnectionString, "tblProduct", "dbo");
            this.SqlTableDependency.OnChanged += SqlTableDependency_OnChanged;


        }

        private void SqlTableDependency_OnChanged(object sender, TableDependency.SqlClient.Base.EventArgs.RecordChangedEventArgs<tblProduct> e)
        {
            switch (e.ChangeType)
            {
                case ChangeType.Delete:
                    this.Clients.All.removetblProduct(e.Entity);
                    break;

                case ChangeType.Insert:
                    this.Clients.All.addtblProduct(e.Entity);
                    break;

                case ChangeType.Update:
                    this.Clients.All.updatetblProduct(e.Entity);
                    break;
            }
        }

        public ProductsModel GetAll()
        {
            var productsList = new List<tblProduct>();
            ProductsModel model = new ProductsModel();

            using (var db = new chemicalsEntities())
            {
                var products = db.tblProducts;
                model.productsList = products;
            }
            return model;
        }

        public void Dispose()
        {
            this.SqlTableDependency.Stop();
        }
    }
}