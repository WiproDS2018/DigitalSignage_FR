dsControllers.controller('dashBoardController', function ($window,$scope, $rootScope, settingService, $q, $uibModal, $http, myService) {
    //reload started here
    $rootScope.parentBgHome = true;
    $scope.goToAdmin = function () {
        $window.location.href = "/Home/Settings";
    }
    window.onload = function () {
        $http({
            method: 'POST',
            url: '/CampaignHistory/GetDashBoardDetails',
            dataType: 'json',
        }).then(function (response) {
            console.log("for pie", response.data);
            var gData = [];
            var data = response.data || [];
            data.forEach(function (obj) {

                gData.push({ y: obj.Yvalue, label: obj.Label });

            });
            console.log("==gData==", gData);
            var chart = new CanvasJS.Chart("chartContainer", {
                animationEnabled: true,
                theme: "light2",
                title: {
                    text: "Playlist Status"
                },
                data: [{
                    type: "pie",
                    startAngle: 240,
                    yValueFormatString: "##0\''",
                    indexLabel: "{label} {y}",
                    dataPoints: gData
                }]
            });

            chart.render();
        });

        $http({
            method: 'POST',
            url: '/CampaignHistory/GetDashBoardDetailsBarChart',
            dataType: 'json',
        }).then(function (response) {
            console.log(response.data);
            var gData = [];
            console.log("initail data", gData)
            var data = response.data || [];
            data.forEach(function (obj) {

                gData.push({ y: obj.Yvalue, label: obj.Label });

            });

            var chart = new CanvasJS.Chart("chartContainer1", {
                animationEnabled: true,
                theme: "light2", // "light1", "light2", "dark1", "dark2"
                title: {
                    text: "Content Status"
                },
                axisY: {
                    title: "Units"
                },
                data: [{
                    type: "column",
                    showInLegend: false,
                    dataPoints: gData
                }]
            });
            console.log("bar chart gData", gData);
            chart.render();

        });


        $http({
            method: 'POST',
            url: '/CampaignHistory/GetDashBoardDetailsDevice',
            dataType: 'json',
        }).then(function (response) {
            console.log(response.data);
            var gData = [];
            console.log("initail data", gData)
            var data = response.data || [];
            data.forEach(function (obj) {

                gData.push({ y: obj.Yvalue, label: obj.Label });

            });

            var chart = new CanvasJS.Chart("chartContainer2", {
                animationEnabled: true,
                theme: "light2", // "light1", "light2", "dark1", "dark2"
                title: {
                    text: "Device Status"
                },
                axisY: {
                    title: "Units"
                },
                data: [{
                    type: "pie",
                    showInLegend: false,
                    dataPoints: gData
                }]
            });
            console.log("bar chart gData", gData);
            chart.render();

        });


        $http({
            method: 'POST',
            url: '/CampaignHistory/GetDeviceLocationDash',
            dataType: 'json',
        }).then(function (response) {
            console.log(response.data);
            var gData = [];

            console.log("initail data", gData)
            var data = response.data || [];
            data.forEach(function (obj) {

                gData.push({ y: obj.Yvalue, label: obj.Label, deviceId: obj.DeviceList });

            });
            dataArray = [];
            console.log("******************");
            console.log(gData);
            console.log(gData[0].deviceId[0]);
            var colors = ["#d62f83", "#ff506e", "#6cb254", "#77dcb8", "#e4a173", "#99004d", "#e8d8be", "#eabe87", "#d5ab96",  "#ff4b4b"];
                for (var i = 0; i < colors.length; i++) {
                    var dataPoint = [];
                    
                    for (var j = 0; j < gData.length; j++) {
                        console.log(gData[j].deviceId)
                        if (gData[j].y <= (i)) {
                            dataPoint.push({ y: 0, label: gData[j].label, id: "NA" });
                        }
                        else {
                            dataPoint.push({ y: 1, label: gData[j].label, id: gData[j].deviceId[i].DeviceName});
                        }
                    }
                    var obj = {
                        type: "stackedColumn",
                        showInLegend: false,
                        color: colors[i],
                        dataPoints: dataPoint
                    };
                    dataArray.push(obj);
                }
                console.log(dataArray)
            var chart = new CanvasJS.Chart("chartContainer3", {
                animationEnabled: true,
                theme: "light2",
                title: {
                    text: "Device Group Details",
                    //fontFamily: "arial black",
                    fontColor: "#695A42"
                },
                axisX: {
                    title: "Device Groups"
                },

                axisY: {
                    //valueFormatString: "$#0bn",
                    title: "Device Count",
                    //gridColor: "#B6B1A8",
                    tickColor: "#B6B1A8",
                    valueFormatString:  "0"
                },
                toolTip: {
                    shared: true,
                    content: toolTipContent
                },

                data: dataArray
            });
            chart.render();

            function toolTipContent(e) {
                var str = "";
                for (var i = e.entries.length-1; i >= 0; i--) {
                    if (e.entries[i].dataPoint.id == "NA") {
                        continue;
                    }
                    else {
                        var str1 = "<span  style= \"color:" + e.entries[i].dataSeries.color + "\"> " + "Device Name" + "</span>: <strong>" + e.entries[i].dataPoint.id + "</strong><br/>";

                        str = str.concat(str1);
                    }
                    
                }

                return str;
            }

        });
        $scope.reloadPage = function () {
            window.location.reload();
        };
    }
});