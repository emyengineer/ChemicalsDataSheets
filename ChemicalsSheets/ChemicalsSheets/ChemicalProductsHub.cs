using ChemicalsSheets.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChemicalsSheets
{
    [HubName("ChemicalProductsTicker")]
    public class ChemicalProductsHub : Hub
    {

        private readonly ChemicalProductsService _ChemicalProductsService;

        public ChemicalProductsHub() : this(ChemicalProductsService.Instance)
        {

        }

        public ChemicalProductsHub(ChemicalProductsService chemicalProductsHub)
        {
            _ChemicalProductsService = chemicalProductsHub;
        }

        public ProductsModel GetAll()
        {
            return _ChemicalProductsService.GetAll();
        }


    }
}