(function (angular) {
    'use strict';

    var app = angular.module('loginApp', ['ngRoute']);
    
    app.controller('loginController', function ($scope, $rootScope, $location, $http, UserRegistrationService, LoginService, $window) {
        console.log('Welcome to Digital Signage');
        $rootScope.parentBgHome = true;
        $scope.showmessage = false;
        // Need to add logic to differ login vs create form's
        $scope.isLoginForm = true;
        $scope.LoginUser = {
            UserName: '',
            Password: ''
        };

        $scope.loginResult = '';
        $scope.userResult = '';

        $scope.RegisterUser = {
            UserName: '',
            Email: '',
            Password: '',
            ConfrimPassword: '',
            IsActive: true,
            Role: 'User'
        };

        $scope.Register = function () {

            if ($scope.RegisterUser.Password == $scope.RegisterUser.ConfrimPassword) {
                var res = UserRegistrationService.SaveFormData($scope.RegisterUser);

                UserRegistrationService.SaveFormData($scope.RegisterUser).then(function (response) {
                    //$scope.showmessage = true;
                    //$scope.userResult = 'User registered successfully. Please login.';
                    console.log(response);
                });
            }
        }

        $scope.Login = function () {

            //console.log('login');
            //$http({
            //    url: '/User/ValidateUser',
            //    method: 'POST',
            //    data: { userData: JSON.stringify($scope.LoginUser) },
            //    headers: { 'content-type': 'application/json' }
            //}).then(function (response) {
            //    alert(response.status);
            //});
            LoginService.login($scope.LoginUser).then(function (response) {

                console.log(response);

                if (response.status == 200) {
                    $window.location.href = "/Home/Campaign";
                    $window.location.reload();
                    //$location.path('/Home/Campaign');

                }
            });
        }

    });

    function LoginService($http) {
        return {
            login: function (data) {
                return $http({
                    url: '/User/ValidateUser',
                    method: 'POST',
                    data: { userData: JSON.stringify(data) },
                    headers: { 'content-type': 'application/json' }
                });
            }
        };
    }

    app.factory('LoginService', LoginService);

    app.factory('UserRegistrationService', function ($http, $q) {
        //here $q is an angularjs service which help us to run asynchronous function and return result when processing done.
        var fac = {};
        fac.SaveFormData = function (data) {
            var defer = $q.defer();
            $http({
                url: '/User/SaveUser',
                method: 'POST',
                data: { userData: JSON.stringify(data) },
                headers: { 'content-type': 'application/json' }
            }).then(function successCallback(response) {
                console.log(response)

            }, function errorCallback(err) {
                console.log(err)
            });
            return result;
            return defer.promise;
        },
        fac.Login = function (data) {

            return $http({
                url: '/User/ValidateUser',
                method: 'POST',
                data: { userData: JSON.stringify(data) },
                headers: { 'content-type': 'application/json' }
            });
            //var defer = $q.defer();
            //$http({
            //    url: '/User/ValidateUser',
            //    method: 'POST',
            //    data: { userData: JSON.stringify(data) },
            //    headers: { 'content-type': 'application/json' }
            //}).then(function successCallback(response) {

            //    console.log(response)
            //    if (response.data == "sucess")
            //        window.location.href = '@Url.Action("Campaign", "Home")'; 
            //    // redirect to main page
            //    //$http({
            //    //    url: '/Home/Campaign',
            //    //    method: 'GET',
            //    //    headers: { 'content-type': 'application/json' }
            //    //});
            //}, function errorCallback(err) {
            //    console.log(err)
            //});
            //return defer.promise;
        },
         fac.GetAllPlayerData = function (data) {
             $http({
                 url: '/User/GetAllUsers',
                 method: 'POST',
                 data: JSON.stringify(data),
                 headers: { 'content-type': 'application/json' }
             }).then(function successCallback(response) {
                 console.log(response)
             }, function errorCallback(err) {
                 console.log(err)
             });

         }
        return fac;
    });
})(angular);
