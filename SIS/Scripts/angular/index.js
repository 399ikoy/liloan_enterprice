
var app = angular.module("SimpleApp", []);

app.controller("simpleController", function ($scope) {
    $scope.temp = 0;
    $scope.times = 0;
    $scope.collection = [];
    $scope.baga = [];
    $scope.try = "";

    $scope.allproduct = function () {
        $http.get('/Sales/ProductJson').then(function ($response) {
            $scope.baga = $response.data;
            return $scope.baga;
        });
    }

    $scope.barcodeData = function (item) {
        //item = { name: "Trieal", price: "30.00" };
        alert(item);
        console.log(item);
        $scope.collection.push(item);
    };

    $scope.add = function (item) {
        //item = { name: "Trieal", price: "30.00" };
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
    $scope.remove = function (array, index) {
        array.splice(index, 1);
    }

    $scope.change = function (item)
    {
        $scope.collection.push(item);
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

 