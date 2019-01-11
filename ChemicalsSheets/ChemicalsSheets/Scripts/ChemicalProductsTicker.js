/// <reference path="jquery-1.10.2.js" />
/// <reference path="jquery.signalR-2.2.0.js" />
/// <reference path="flightBookingViewModels.js" />
/// <reference path="knockout-3.4.0.debug.js" />

$(function () {
    var viewModel = null;

    // Generated client-side hub proxy and then 
    // add client-side hub methods that the server will call

    var ticker = $.connection.ChemicalProductsTicker;

    // Add a client-side hub method that the server will call
    ticker.client.updatetblProduct = function (product) {
        viewModel.updatetblProduct(product);

        $("#" + product.Id).effect("highlight", { color: "yellow" }, 2000);
    };

    ticker.client.addtblProduct = function (product) {
        viewModel.addtblProduct(product);
    };

    ticker.client.removetblProduct = function (product) {
        viewModel.removetblProduct(product);
    };

    // Start the connection and load products
    $.connection.hub.start().done(function () {
        ticker.server.getAll().done(function (productsUpdatedList) {
            viewModel = new ChemicalProductsViewModel(productsUpdatedList);
            ko.applyBindings(viewModel);
        });
    });
});