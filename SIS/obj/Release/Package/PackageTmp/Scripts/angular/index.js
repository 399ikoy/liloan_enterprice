
var app = angular.module("SimpleApp", []);

app.controller("simpleController", function ($scope) {
    $scope.temp = 0;
    $scope.times = 0;
    $scope.collection = [{ name: "Sardinas", price: "40.55" }, { name: "Binagol", price: "50.10" }, { name: "Pututsiko", price: "100" }];
    //$scope.collection = [];

    $scope.add = function (item) {
        $scope.collection.push(item);
    };

    $scope.addEntry = function () {
        $scope.collection.push({ name: $scope.newData });
        //$scope.collection = [{Total: "2"}];
    };

    $scope.getTotal = function () {
        var total = 0;
        for (var i = 0; i < $scope.collection.length; i++) {
            var entry = $scope.collection[i];
            total += (entry.quantity * entry.price);
        }
        return total;
    }

});
app.filter('singleDecimal', function ($filter) {
    return function (input) {
        if (isNaN(input)) return input;
        return Math.round(input * 10) / 10;
    };
});

app.filter('setDecimal', function ($filter) {
    return function (input, places) {
        if (isNaN(input)) return input;
        // If we want 1 decimal place, we want to mult/div by 10
        // If we want 2 decimal places, we want to mult/div by 100, etc
        // So use the following to create that factor
        var factor = "1" + Array(+(places > 0 && places + 1)).join("0");
        return Math.round(input * factor) / factor;
    };
});