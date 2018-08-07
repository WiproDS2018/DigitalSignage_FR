
dsControllers.controller('catalogueController', function ($scope, $rootScope, catalogueService, uploadService, $q) {
    console.log('Inside Catalogue Controller');
    $rootScope.parentBgHome = true;
    $scope.subTitle = "- Upload";
    $scope.trackScene = true;
    $scope.tutorial = {};
    //$scope.genderDropdown = ['Male', 'Female'];
    $scope.tutorial.Gender = 'Male';;
    //$scope.gValue = 'Male';
    $scope.rangeValue = ['20 years to 30 years', '31 years to 40 years', '41 years to 50 years'];
    $scope.tutorial.AgeRange = $scope.rangeValue[0];

    $scope.uploadCatalogue = function () {
        console.log('Inside uploadcatalogue');
        $scope.upload = true;
        $scope.trackScene = false;
    };

    //Saving Uploaded file
    $scope.saveTutorial = function (tutorial) {
        console.log(tutorial, 'uploadtutorial');

        //Form validation
        $scope.uploadMsg = "";
        if (tutorial == undefined || tutorial == null || tutorial == "") {
            console.log('inside upload form validation');
            $scope.uploadMsg = 'Fields should not be empty';
        } else if (Object.keys(tutorial).length < 4) {
            console.log(Object.keys(tutorial).length);
            $scope.uploadMsg = 'Fields should not be empty';
        }

        if ($scope.uploadValue != 4) {
            if ($scope.uploadValue != 5) {
                console.log(tutorial.attachment.size, 'file size in bytes');
                tutorial.UploadUrl = '';
                if (tutorial.attachment.size > 4000000) {
                    $scope.uploadMsg = 'File size should not be greater than 4MB';
                }
            }
        }

        if ($scope.uploadMsg != "") {
            console.log($scope.uploadMsg);
        }
        else {
            //calling upload service
            uploadService.saveTutorial(tutorial).then(function (res) {
                console.log(res);
                $scope.res = res;
                if (res.data == 'Success') {
                    $scope.uploadMsg = 'Uploaded Successfully !';
                    //Alert message timeout
                    setTimeout(function () {
                        window.location.reload();
                    }, 2000);
                } else {
                    $scope.uploadMsg = res.data;
                }
            });
        }
    };
        //upload code ends from here

    //Track Catalogue
    $scope.trackCatalogue = function () {
        $scope.upload = false;
        $scope.trackScene = true;
        catalogueService.getAllData().then(function (res) {
            console.log(res);
            $scope.trackData = res.data;
            $scope.totalItems = $scope.trackData.length;
            $rootScope.loader = false;
        });
    };

    //Template view started
    $scope.templatePreview = function (TemplatesceUrl, sceType) {
        console.log(TemplatesceUrl);

        document.getElementById('dy_template').innerHTML = "";
        $scope.TemplatesceUrl = "";
        $scope.showcompModal2 = !$scope.showcompModal2;

        $scope.TemplatesceUrl = TemplatesceUrl;
        document.getElementById('dy_template').innerHTML = '<img src=' + $scope.TemplatesceUrl + ' width="100%">'
        
    };

    //reload
    $scope.reloadPage = function () {
        window.location.reload();
    };

    //table sorting started
    $scope.propertyName = 'Title';
    $scope.reverse = true;
    $scope.sortBy = function (propertyName) {
        $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
        $scope.propertyName = propertyName;
    };
    //table sorting ended
    // pagination started
    $scope.dDownValues = [10, 15, 20, 30];
    $scope.viewby = $scope.dDownValues[0];

    //$scope.viewby = 10;
    $scope.itemsPerPage = $scope.viewby;
    console.log($scope.viewby);

    $scope.setItemsPerPage = function (num) {
        $scope.viewby = num;
        $scope.itemsPerPage = num;
        $scope.currentPage = 1; //reset to first paghe
    };

    console.log($scope.totalItems, '$scope.totalItems');
    $scope.currentPage = 1;
        //pagination ended

}); //controller ended

//service code starts here
dsFactory.factory('catalogueService', function ($http, $q) {
    //here $q is an angularjs service which help us to run asynchronous function and return result when processing done.
    return {
        //getting All pending-Approval data
        getAllData: function () {
            //console.log('inside the getservice');
            return $http.get('/FaceRecCatalog/GetAllCatalog');
        },
        updateStatus: function (data) {
            console.log(data);
            var defer = $q.defer();
            $http({
                url: '/Admin/Update',
                method: 'POST',
                data: { vmApprove: JSON.stringify(data) },
                headers: { 'content-type': 'application/json' }
            }).then(function successCallback(response) {
                console.log(response);
                defer.resolve(response);
            }, function errorCallback(err) {
                console.log(err);
                defer.reject(err);
            });
            return defer.promise;
        }
    }
});

dsFactory.factory("uploadService",
    ["akFileUploaderService", function (akFileUploaderService) {
        var saveTutorial = function (tutorial) {
            console.log(tutorial, 'inside service');
            return akFileUploaderService.saveModel(tutorial, "/FaceRecCatalog/UploadCatalog");
        };
        return {
            saveTutorial: saveTutorial
        };
    }]);


