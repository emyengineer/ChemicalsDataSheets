
// Product view model
function ChemicalProductViewModel(product) {
    var self = this;

    var mappingOptions = {
        key: function (data) {
            return ko.utils.unwrapObservable(data.Id);
        }
    };

    ko.mapping.fromJS(flight, mappingOptions, self);
};

// Products View Model 
function ChemicalProductsViewModel(products) {
    var self = this;

    var ChemicalProductsMappingOptions = {
        products: {
            create: function (options) {
                return new ChemicalProductViewModel(options.data);
            }
        }
    };

    self.addtblProduct = function (product) {
        self.products.push(new ChemicalProductViewModel(product));
    };


    self.updatetblProduct = function (product) {
        var productMappingOptions = {
            update: function (options) {
                ko.utils.arrayForEach(options.target, function (item) {
                    if (item.Id() === options.data.Id) {
                        item.FileContent(options.data.FileContent);
                        item.IsAvailable(options.data.IsAvailable);
                        item.Modified(options.data.Modified);
                    }
                });
            }
        };

        ko.mapping.fromJS(product, productMappingOptions, self.products);
    };

    self.removetblProduct = function (product) {
        self.products.remove(function (item) {
            return item.Id() === product.Id;
        });
    };

    self.Id = ko.observable(null);
    self.FileContent = ko.observable(null);
    self.result = ko.observable(null);


    self.save = function () {
        $.ajax({
            type: "POST",
            url: "/ChemicalProducts/Save",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ Id: self.Id(), FileContent: self.FileContent() }),
            dataType: "json",
            success: function (data) {
                self.result(data.Message);
                self.Id(null);
                self.FileContent(null);
            },
            error: function (xmlHttpRequest, textStatus, errorThrown) {
                self.result(errorThrown);
            }
        });
    };



};
