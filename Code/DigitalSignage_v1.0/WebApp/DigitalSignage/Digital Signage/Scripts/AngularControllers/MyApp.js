var dsService = angular.module('MyApp.Service', []);
var dsFilters = angular.module('MyApp.Filter', []);
var dsFactory = angular.module('MyApp.Factory', []);
var dsDirectives = angular.module('MyApp.Directives', []);
var dsControllers = angular.module('MyApp.Controllers', ['MyApp.Service', 'MyApp.Factory', 'MyApp.Directives', 'MyApp.Filter']);
var MyApp = angular.module('MyApp', ['ngRoute', 'MyApp.Controllers', 'chieffancypants.loadingBar', 'datatables', 'dndLists', 'akFileUploader', 'ui.bootstrap'])
        .config(function (cfpLoadingBarProvider) {
            cfpLoadingBarProvider.includeSpinner = true;
        });
dsControllers.controller('loaderController', function ($scope, $rootScope, $http, cfpLoadingBar) {});

