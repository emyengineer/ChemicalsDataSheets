using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChemicalsSheets.Models
{
    public class ProductsModel
    {
        public IEnumerable<tblProduct> productsList { get; set; }
    }
}