(function (angular) {
    'use strict';
    var app = angular.module('MyApp');

    app.controller("upload", ['$scope', '$http', 'uploadService', function ($scope, $http, uploadService) {

        $scope.SceneData = {
            "SceneId": "",
            "SceneName": ""           
        };

        $scope.$watch('file', function (newfile, oldfile) {
            if (angular.equals(newfile, oldfile)) {
                return;
            }
           

            uploadService.upload($scope.SceneData).then(function (res) {
                // DO SOMETHING WITH THE RESULT!
                console.log("result", res);
            })
        });

    }])
        .service("uploadService", function ($http, $q) {

            return ({
                upload: upload
            });
            

            function upload(file) {
                console.log("FILE DATA POST");
                console.log("FILE DATA POST");
                console.log(file);
                
                var upl = $http({
                    method: 'POST',
                    url: '/Scene/UploadScene', // /api/upload
                    data:  file,
                    headers: { 'content-type': undefined },
                    transformRequest: function (data, headersGetter) {
                        var formData = new FormData();
                        angular.forEach(data, function (value, key) {
                            formData.append(key, value);
                        });

                        var headers = headersGetter();
                        delete headers['Content-Type'];

                        return formData;
                    }
                });
                return upl.then(handleSuccess, handleError);

            } // End upload function

            // ---
            // PRIVATE METHODS.
            // ---

            function handleError(response, data) {
                if (!angular.isObject(response.data) || !response.data.message) {
                    return ($q.reject("An unknown error occurred."));
                }

                return ($q.reject(response.data.message));
            }

            function handleSuccess(response) {
                return (response);
            }

        })
        .directive("fileinput", [function () {
            return {
                scope: {
                    fileinput: "=",
                    filepreview: "="
                },
                link: function (scope, element, attributes) {
                    element.bind("change", function (changeEvent) {
                        scope.fileinput = changeEvent.target.files[0];
                        var reader = new FileReader();
                        reader.onload = function (loadEvent) {
                            scope.$apply(function () {
                                scope.filepreview = loadEvent.target.result;
                            });
                        }
                        reader.readAsDataURL(scope.fileinput);
                    });
                }
            }
        }]);

//    app.controller('FileUploadCtrl', function ($scope, $rootScope) {

//function FileUploadCtrl(scope) {
//    //============== DRAG & DROP =============

//    //============== DRAG & DROP =============

//    scope.setFiles = function (element) {
//        scope.$apply(function (scope) {
//            console.log('files:', element.files);
//            // Turn the FileList object into an Array
//            scope.files = []
//            for (var i = 0; i < element.files.length; i++) {
//                scope.files.push(element.files[i])
//            }
//            scope.progressVisible = false
//        });
//    };

//    scope.uploadFile = function () {
//        var fd = new FormData()
//        for (var i in scope.files) {
//            fd.append("uploadedFile", scope.files[i])
//        }
//        var xhr = new XMLHttpRequest()
//        xhr.upload.addEventListener("progress", uploadProgress, false)
//        xhr.addEventListener("load", uploadComplete, false)
//        xhr.addEventListener("error", uploadFailed, false)
//        xhr.addEventListener("abort", uploadCanceled, false)
//        xhr.open("POST", "/fileupload")
//        scope.progressVisible = true
//        xhr.send(fd)
//    }

//    function uploadProgress(evt) {
//        scope.$apply(function () {
//            if (evt.lengthComputable) {
//                scope.progress = Math.round(evt.loaded * 100 / evt.total)
//            } else {
//                scope.progress = 'unable to compute'
//            }
//        })
//    }

//    function uploadComplete(evt) {
//        /* This event is raised when the server send back a response */
//        alert(evt.target.responseText)
//    }

//    function uploadFailed(evt) {
//        alert("There was an error attempting to upload the file.")
//    }

//    function uploadCanceled(evt) {
//        scope.$apply(function () {
//            scope.progressVisible = false
//        })
//        alert("The upload has been canceled by the user or the browser dropped the connection.")
//    }
//        }
//    });
})(angular);
