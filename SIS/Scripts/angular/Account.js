App.controller('Account', ['$scope', '$http', '$timeout', function ($scope, $http, $timeout)
{
    $scope.bag = [];
    $scope.alldata = function ()
    {
        //
        $http.get('/Accounts/sample1').then(function ($response)
        {
            $scope.bag = $response.data;
            console.log($response);

        });
    }

    $scope.delete = function (id) {
        var state = confirm("Do you want to delete this record" + id);
        console.log(state);
        if (state == true) {
            $http.get('/Accounts/delete/' + id).then(function ($response) {
                $scope.alldata();

            });
        }
    }
    $scope.edit = function (account) {
        $scope.account_info_d = account.account_info_d;
        $scope.username = account.username;
        $scope.password = account.password;
        $scope.user_type = account.user_type;
        $scope.active_flag = account.active_flag;
        $scope.login = account.login;
    }

    //execute on the page load
    $timeout(function ()
    {
        $scope.alldata();
    });
}]);
