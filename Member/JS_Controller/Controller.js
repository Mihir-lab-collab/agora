﻿/// <reference path="jquery.min.js" />
var Controller = {
    GetIncidentList: function (getParamerters, doneCallback, callbackArgs) {
        var data =   [{ ProductName: "Tea", UnitPrice: 30, UnitsInStock: 400, Discontinued: 50, date: new Date() },
                      { ProductName: "Cofee", UnitPrice: 30, UnitsInStock: 400, Discontinued: 50, date: new Date() },
                      { ProductName: "Samosa", UnitPrice: 30, UnitsInStock: 400, Discontinued: 50, date: new Date() },
                      { ProductName: "Samosa", UnitPrice: 30, UnitsInStock: 400, Discontinued: 50, date: new Date() }]
        return data;
    },
    GetIncidentList: function (getParamerters, doneCallback, callbackArgs) { var data = [{
    ProductID : 1,
    ProductName : "Chai",
    SupplierID : 1,
    CategoryID : 1,
    QuantityPerUnit : "10 boxes x 20 bags",
    UnitPrice : 18.0000,
    UnitsInStock : 39,
    UnitsOnOrder : 0,
    ReorderLevel : 10,
    Discontinued : false,
    Category : {
        CategoryID : 1,
        CategoryName : "Beverages",
        Description : "Soft drinks, coffees, teas, beers, and ales"
    }
}, {
    ProductID : 2,
    ProductName : "Chang",
SupplierID : 1,
CategoryID : 1,
QuantityPerUnit : "24 - 12 oz bottles",
UnitPrice : 19.0000,
UnitsInStock : 17,
UnitsOnOrder : 40,
ReorderLevel : 25,
Discontinued : false,
Category : {
    CategoryID : 1,
    CategoryName : "Beverages",
    Description : "Soft drinks, coffees, teas, beers, and ales"
}
}, {
    ProductID : 3,
    ProductName : "Aniseed Syrup",
    SupplierID : 1,
    CategoryID : 2,
    QuantityPerUnit : "12 - 550 ml bottles",
    UnitPrice : 10.0000,
    UnitsInStock : 13,
    UnitsOnOrder : 70,
    ReorderLevel : 25,
    Discontinued : false,
    Category : {
        CategoryID : 2,
        CategoryName : "Condiments",
        Description : "Sweet and savory sauces, relishes, spreads, and seasonings"
    }
}, {
ProductID : 4,
    ProductName : "Chef Anton's Cajun Seasoning",
SupplierID : 2,
CategoryID : 2,
QuantityPerUnit : "48 - 6 oz jars",
UnitPrice : 22.0000,
UnitsInStock : 53,
UnitsOnOrder : 0,
ReorderLevel : 0,
Discontinued : false,
Category : {
    CategoryID : 2,
    CategoryName : "Condiments",
    Description : "Sweet and savory sauces, relishes, spreads, and seasonings"
}
}, {
    ProductID : 5,
    ProductName : "Chef Anton's Gumbo Mix",
    SupplierID : 2,
    CategoryID : 2,
    QuantityPerUnit : "36 boxes",
    UnitPrice : 21.3500,
    UnitsInStock : 0,
    UnitsOnOrder : 0,
    ReorderLevel : 0,
    Discontinued : true,
    Category : {
        CategoryID : 2,
        CategoryName : "Condiments",
        Description : "Sweet and savory sauces, relishes, spreads, and seasonings"
    }
}]
}