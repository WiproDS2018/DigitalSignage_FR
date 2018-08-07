(function () {
    "use strict"
    angular.module("akFileUploader", [])
        .factory("akFileUploaderService", ["$q", "$http",
               function ($q, $http) {
                   var getModelAsFormData = function (data) {
                       console.log(data,'data formdata')
                       var dataAsFormData = new FormData();
                       angular.forEach(data, function (value, key) {
                           dataAsFormData.append(key, value);
                       });
                       dataAsFormData.append('SceneType', 'Upload');
                       return dataAsFormData;
                   };

                   var saveModel = function (data, url) {
                       console.log(data, 'inside fileuploader',url);
                       var deferred = $q.defer();
                       $http({
                           url: url,
                           method: "POST",
                           data: getModelAsFormData(data),
                           transformRequest: angular.identity,
                           headers: { 'Content-Type': undefined }
                       }).then(function successCallback(result) {
                           deferred.resolve(result);
                           console.log(result)
                       }, function errorCallback(status) {
                           deferred.reject(status);
                           console.log(status);
                       })                       
                       return deferred.promise;                   
                   };

                   return {
                       saveModel: saveModel
                   }

               }])
        .directive("akFileModel", ["$parse",
                function ($parse) {
                    return {
                        restrict: "A",
                        link: function (scope, element, attrs) {
                            var model = $parse(attrs.akFileModel);
                            var modelSetter = model.assign;
                            element.bind("change", function () {
                                scope.$apply(function () {
                                    modelSetter(scope, element[0].files[0]);
                                });
                            });
                        }
                    };
                }]);
})(window,document);
