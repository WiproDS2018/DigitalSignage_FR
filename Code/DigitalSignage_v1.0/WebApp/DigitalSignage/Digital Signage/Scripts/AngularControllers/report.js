
dsControllers.controller('reportController', function ($window,$scope, $rootScope, settingService, $q, $http, myService) {
    $('.activeCls').click(function (e) {
        e.preventDefault();
        $('.activeCls').removeClass('active');
        $(this).addClass('active');
    });
    $rootScope.parentBgHome = true;
    // pagination started
    $scope.reportData = [];
    $scope.getStaionName = "";
    $scope.ModalCntrlValue = {};

    $scope.getStaion = function (stName) {
        $scope.getStaionName = stName;
    }
    $scope.goToAdmin = function () {
        $window.location.href = "/Home/Settings";
    }
    $scope.downloadReport = function () {
        html2canvas(document.getElementById('exportthis'), {
            onrendered: function (canvas) {
                var data = canvas.toDataURL();
                var docDefinition = {
                    content: [{
                        image: data,
                        width: 500,
                    }]
                };
                pdfMake.createPdf(docDefinition).download("test.pdf");
            }
        });
    }
    $scope.showReport = function (cat, ctgry, strDt, endDt) {
        $scope.showReportFlag = 0;
        $scope.errorEndDt = 0;
        $scope.errorStrDt = 0;
        var month = strDt.getUTCMonth() + 1; //months from 1-12
        var day = strDt.getUTCDate();
        var year = strDt.getUTCFullYear();

        var stdate = day + "-" + month + "-" + year;

        var edDate = endDt.getUTCDate();
        var edMonth = endDt.getUTCMonth() + 1;
        var edYear = endDt.getUTCFullYear();
        $scope.StationName = ctgry;
        var endDate = endDate + "-" + edMonth + "-" + edYear;
        console.log("*************************");
        var today1 = new Date();
        console.log("This is today date    " + today1);
        console.log(strDt, endDt);
        if (today1 < strDt) {
            $scope.errorStrDt = 1;
        }
        else if (strDt > endDt) {
            $scope.errorEndDt = 1;
        }
        else {
            $scope.errorEndDt = 0;
            $scope.errorStrDt = 0;
            ctgry = ctgry + "";
            console.log("*****************************************************" + strDt.getTimezoneOffset());
            console.log("**************************************************" + strDt.toString().match(/\(([A-Za-z\s].*)\)/)[1])
            console.log()
            $http({

                method: 'POST',

                url: '/CampaignHistory/GetCampaignHistory',
                dataType: 'json',
                data: {
                    criteria: cat, id: ctgry, startDate: strDt, endDate: endDt, offsetTime: strDt.getTimezoneOffset(), zone: strDt.toString().match(/\(([A-Za-z\s].*)\)/)[1]
                }

            }).then(function (response) {
                $scope.reportData = response.data;
                $scope.totalItems = response.data.length;
                $scope.viewby = 10;
                $scope.totalItems = $scope.reportData.length;
                $scope.currentPage = 1;
                $scope.itemsPerPage = 5;
                $scope.maxSize = 5;

                $scope.pageChanged = function () {
                    console.log('Page changed to: ' + $scope.currentPage);
                };
                $scope.itemsPerPage = 5;


            });

            // var endDt = dateFormat(endDt, "dd-mm-yy");
            if ((ctgry == null) || (strDt == null) || (endDt == null)) {
                $scope.showReportFlag = 0;
                $scope.reportData = [];

            }
            else {
                $scope.showReportFlag = 1;
            }
        }

    }
    $scope.downloadPdf = function () {
        console.log("asfdhga");
        html2canvas(document.getElementById('reportModalView'), {
            onrendered: function (canvas) {
                var data = canvas.toDataURL();
                var docDefinition = {
                    content: [{
                        image: data,
                        width: 500,
                    }]
                };
                pdfMake.createPdf(docDefinition).download("report.pdf");
            }
        });
    }
    
    $scope.selectedCategory = function (category) {
        
        $scope.errorEndDt = 0;
        $scope.errorEndDt1 = 0;
        $scope.errorStrDt = 0;
        $scope.errorStrDt1 = 0;
        $scope.stationStrDt = null;
        $scope.stationEndDt = null;
        $scope.campaignStrDt = null;
        $scope.campaignEndDt = null;
        $scope.playerStrDt = null;
        $scope.playerEndDt = null;
        $scope.showReportFlag = 0;
        if (category == "Station") {
            $scope.radioBtnSelected = category;
            $http.get('/CampaignHistory/GetAllStations').then(function (response) {
                $scope.stations = response.data;

                $scope.dDownValues = response.data;

                $scope.viewbystationDrop = $scope.dDownValues;
                console.log("******************************************");

                console.log(typeof response.data + "     " + $scope.viewbystationDrop);
                $scope.reportData = response.data;

            });

            
        }
        if (category == "Campaign") {
            $scope.radioBtnSelected = category;
            $http.get('/CampaignHistory/GetAllCampaigns').then(function (response) {
                $scope.stations = response.data;


                $scope.dDownValues = response.data;
                console.log(response.data);
                $scope.viewbycampaign = $scope.dDownValues;
                $scope.reportData = response.data;
            });
        }
        if (category == "Player") {
          
            $scope.radioBtnSelected = category;
            
            $http.get('/CampaignHistory/GetAllPlayers').then(function (response) {
                $scope.players = response.data;
                $scope.dDownValues = response.data;

                $scope.viewbyPlayers = $scope.dDownValues;
                $scope.reportData = response.data;

            });
        }
    }
    $scope.propertyName = 'DisplayName';
    $scope.reverse = false;
    $scope.sortBy = function (propertyName) {
        $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
        $scope.propertyName = propertyName;
    };
  
    $scope.reloadPage = function () {
        window.location.reload();
    };
   
    
});
dsControllers.service('myService', function () {
    StationNm = "";
    ServiceStDt = "";
    ServiceEndDate = "";
    ServiceReportData = [];
})



