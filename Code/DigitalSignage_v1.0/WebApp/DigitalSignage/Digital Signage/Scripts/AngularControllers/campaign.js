dsControllers.controller('CampaignController', function ($scope, $rootScope, campaignService, dateFilter, $filter, cfpLoadingBar) {
    $rootScope.loader = true;
    $rootScope.parentBgHome = true;
    console.log('Welcome to PlayList');
    $scope.addNewCamp = false;
    $scope.viewCamp = false;
    $scope.publishCamp = true;
    $scope.EditContentDiv = false;
    $scope.viewExpired = false;
    $scope.subTitle = "- Publish PlayList";
    $scope.example = {
        value: ''
    };

    $scope.cloneMultiIds = [];
    $rootScope.cBValue = [];
    $scope.jsonData = {
        CampaignName: '',
        subGroupName: '',
        stationName: '',
        deviceName: '',
        DisplayId: '',
        MultiSceneIds: '',
        StartDate: '',
        EndDate: '',
        StartTimeVal: '',
        EndTimeVal: '',
        // Frequency: '',
        Interval: '',
        Type: ''
    };
    //$scope.jsonDataSub = {
    //    CampaignName: '',
    //    SubGroupName: '',
    //    DisplayId: '',
    //    MultiSceneIds: '',
    //    StartDate: '',
    //    EndDate: '',
    //    StartTimeVal: '',
    //    EndTimeVal: '',
    //    // Frequency: '',
    //    Interval: ''

    //};
    
    $scope.editPublishData = {
        CampaignId: '',
        DisplayId: '',
        DisplayStationName: '',
        startDate: '',
        EndDate: '',
        OffsetTime: '',
        Zone: ''
    };


    //Breadcrumb starts
    $scope.cUrl = window.location.href;
    var res = $scope.cUrl.split("/");
    var subst = res.pop();
    $rootScope.state = subst.replace(/#/g, "");
    console.log($rootScope.state);
    //Breadcrumb ends

    $scope.Frequency = ["Daily", "Weekly", "Monthly"];
    $scope.Intervals = ["3", "5", "10", "30", "60"];
    $scope.templateImage = '';
    $scope.templateContent = '';

    $scope.start = function () {
        cfpLoadingBar.start();
    };
    $scope.start();
    $scope.complete = function () {
        cfpLoadingBar.complete();
    }

    // creating ckeckBoxes array
    $scope.indexOfCkeckBox = "";
    $scope.cleckBoxData = function (ckeckBoxId) {
        console.log(ckeckBoxId);
        $rootScope.cBoxValue = ckeckBoxId;
        if (document.getElementById(ckeckBoxId).checked) {
            $scope.indexOfCkeckBox = ckeckBoxId;
            $rootScope.cBValue.push(ckeckBoxId);
            console.log($scope.indexOfCkeckBox, 'checkbox', $rootScope.cBValue);
        } else {
            $rootScope.cBValue.splice($rootScope.cBValue.indexOf(ckeckBoxId), 1);
            console.log($rootScope.cBValue);
        }
        //else if ($scope.indexOfCkeckBox.indexOf(ckeckBoxId)!=-1)
        //{
        //    var index = $scope.indexOfCkeckBox.indexOf(ckeckBoxId);
        //    $scope.indexOfCkeckBox.splice(index, 1);
        //    //console.log($scope.indexOfCkeckBox);
        //}
    };

    //adding New Campaign
    $scope.aNCampaign = function () {
        $scope.msg = "";
        $scope.deviceNameSet = "";
        $scope.GroupName = "";
        $scope.deviceNameRadio = "";
        $scope.AddPlayListActive = "activeLinkSideMenu";
        $scope.ViewPlayListActive = "";
        $scope.ExpirePlayListActive = "";
        $scope.PublishPlayListActive = "";
        $scope.ReloadActive = "";
        $scope.viewExpired = false;
        $scope.addNewCamp = true;

        $scope.EditContentDiv = true;
        $scope.viewCamp = false;
        $scope.publishCamp = false;
        $scope.cloneIndicator = false;
        $scope.subTitle = "- Add Playlist";
        $rootScope.substate = " Add Playlist";
        $scope.currentPage = 1;
        
        $scope.jsonData = {
            CampaignName: '',
            DisplayId: '',
            MultiSceneIds: '',
            StartDate: '',
            EndDate: '',
            StartTimeVal: '',
            EndTimeVal: '',
            Interval: ''
        }
        
        $scope.stationNamesValue = '';
        $scope.frequencyValue = '';
        $scope.Interval = '';
        $scope.jsonData.Interval = $scope.Intervals[0];
        $scope.getScenesData(); //calling scenes data service
        console.log($rootScope.cBValue, '$rootScope.cBValue-----', $rootScope.cBValue.length);
        if ($rootScope.cBValue.length > 0) {
            for (var id in $rootScope.cBValue) {
                console.log($rootScope.cBValue[id]);

                document.getElementById($rootScope.cBValue[id]).checked = false;
            }
            $rootScope.cBValue.length = 0;
        }
    };

    $scope.vCampaign = function () {
        $scope.AddPlayListActive = "";
        $scope.ViewPlayListActive = "activeLinkSideMenu";
        $scope.ExpirePlayListActive = "";
        $scope.PublishPlayListActive = "";
        $scope.ReloadActive = "";
        $scope.EditContentDiv = false;
        $scope.addNewCamp = false;

        $scope.viewCamp = true;
        $scope.viewExpired = false;
        $scope.publishCamp = false;
        $scope.subTitle = "- View Playlist";
        $rootScope.substate = "View Playlist";
        $scope.currentPage = 1;
    };
    $scope.viewExpiredList = function () {
        $scope.AddPlayListActive = "";
        $scope.ViewPlayListActive = "";
        $scope.ExpirePlayListActive = "activeLinkSideMenu";
        $scope.PublishPlayListActive = "";
        $scope.ReloadActive = "";
        $scope.viewExpired = true;
        $scope.EditContentDiv = false;
        $scope.addNewCamp = false;

        $scope.viewCamp = false;
        $scope.publishCamp = false;
        $scope.subTitle = "- Expired Playlist";
        $rootScope.substate = "Expired Playlist";
        $scope.currentPage = 1;
    }
    $scope.pCampaign = function () {
        $scope.deviceNameSet = "";
        $scope.GroupName = "";
        $scope.deviceNameRadio = "";
        $scope.AddPlayListActive = "";
        $scope.ViewPlayListActive = "";
        $scope.ExpirePlayListActive = "";
        $scope.PublishPlayListActive = "activeLinkSideMenu";
        $scope.ReloadActive = "";
        $scope.viewExpired = false;

        $scope.addNewCamp = false;
        $scope.EditContentDiv = false;
        $scope.viewCamp = false;
        $scope.publishCamp = true;
        $scope.subTitle = "- Publish Playlist";
        $rootScope.substate = "Publish Playlist";
        $scope.currentPage = 1;
    };

    //calling getAllExpiredData service
    $scope.getAllExpiredData = function () {
        campaignService.getAllExpiredDataServCal().then(function (res) {
            //$scope.publishData = res.data;

            $scope.expiredPlayList = res.data;
            //        console.log('publishData res', $scope.model, 'publishData length', $scope.pubTotalItems);
        });
    };
    $scope.activatePlayer = function (playerID) {
        campaignService.activatePlayer(playerID).then(function (res) {
            //$scope.publishData = res.data;
            console.log(res);

            $scope.res = res;
            //$scope.scenes = res.data;
            if (res.data == 'Success') {
                $scope.cancMsg = 'Playlist Activated successfully';
                //setTimout msg
                setTimeout(function () {
                    console.log("refreshing");
                    $scope.cancMsg = '';
                    $scope.getAllExpiredData();
                }, 2000);
            } else {
                //           console.log(res.data);

                $scope.cancMsg = res.data;
            }

        });
    }
    // pasing data to template window 
    $scope.templateFunction = function (imgurl, content, sceType, weatherIcPos) {
        console.log(imgurl, content, sceType);
        //$scope.templateImage = imgurl;
        //$scope.templateContent = content;
        ////var myEl = angular.element(document.querySelector('#conId'));
        //myEl.html($scope.templateContent);
        /////////Remil Aug20
        if (sceType != 'VIDEOURL') {
            if (sceType != 'WEBURL') {
                if (sceType != 'VIDEO') {
                    document.getElementById('dy_template').innerHTML = "";
                    $scope.TemplatesceUrl = "";
                    $scope.showcompModal1 = !$scope.showcompModal1;
                }
            }
        };
        $scope.TemplatesceUrl = imgurl;
        $scope.weatherIcPos = weatherIcPos;


        $scope.showIcon = false;
        if ($scope.weatherIcPos == "TopRight") {
            $scope.showIcon = true;
            $scope.weatherIconModal = "weatherIconModalShowTR";
        }
        else if ($scope.weatherIcPos == "TopLeft") {
            $scope.showIcon = true;
            $scope.weatherIconModal = "weatherIconModalShowTL";
        }
        else if ($scope.weatherIcPos == "BottomRight") {
            $scope.showIcon = true;
            $scope.weatherIconModal = "weatherIconModalShowBR";
        }
        else if ($scope.weatherIcPos == "BottomLeft") {
            $scope.showIcon = true;
            $scope.weatherIconModal = "weatherIconModalShowBL";
        }
        else if ($scope.weatherIcPos == "Nothing") {
            $scope.showIcon = false;
            $scope.weatherIconModal = "weatherIconModalHide";
        }
        else {
            $scope.weatherIconModal = "weatherIconModalHide";
        }
        document.getElementById('dy_template').innerHTML = ' <div class=' + $scope.weatherIconModal + ' > <img src="../Images/weather.png" /></div ><img src=' + $scope.TemplatesceUrl + ' width="100%">'

        if (sceType == "WEBURL" || sceType == 'VIDEOURL' || sceType == 'VIDEO') {
            window.open($scope.TemplatesceUrl, "", "width=800,height=500");
        };
    };

    $scope.cancFun = function () {
        window.location.reload();
    };

    //cloning campaign starts here
    $scope.cloneCampaign = function (data) {
        $scope.msg = "";
        console.log("*******************************************");
        $scope.deviceNameSet = "";
        $scope.GroupName = "";
        $scope.deviceNameRadio = "";
        $scope.viewExpired = false;
        console.log(data);
        $scope.StationName = data;
        $scope.aNCampaign(); //calling add new campagin function
        $scope.frequencySelected = data.Frequency;
        $scope.subTitle = "- Copy Playlist";
        $scope.cloneIndicator = true;
        $scope.cloneMultiIds = [];
        $scope.cloneMultiIds = data.MultiSceneIds;
        var mIds = data.MultiSceneIds.split(",");
        var frequencysel = data.DaysOfWeek.split(",");
        console.log(frequencysel);
        if (frequencysel.length != 0 && $scope.frequencySelected == "Weekly") {
            console.log("--------------------- executing frequencysel()");
            setTimeout(function () {
                for (f = 0; f < frequencysel.length; f++) {
                    console.log(document.getElementById(frequencysel[f]));
                    document.getElementById(frequencysel[f]).checked = true;
                }
            })
        }
        setTimeout(function () {
            for (var id in mIds) {
                console.log(document.getElementById(mIds[id]));
                document.getElementById(mIds[id]).checked = true;
                $scope.indexOfCkeckBox = data.SceneId;
            }
            document.getElementById(mIds[0]).checked = true;


        }, 700);
        var cbID = [];
        cbID = $scope.cloneMultiIds.split(",");

        Array.prototype.push.apply($rootScope.cBValue, cbID);
        $rootScope.cBValue = $rootScope.cBValue.map(Number);
        //  console.log($rootScope.cBValue, '____________');
        if (data.Type == "Sub Group") {
            $scope.jsonData.stationNameSub = data.DisplayName;
            console.log($scope.SubGroupName);

        }
        else if (data.Type == "Devices") {
            $scope.deviceNameRadio = data.DisplayName;
            $scope.jsonData.stationNameDev = data.DisplayName;
            $scope.selectGroup = "Devices";
        }
        else {

            $scope.jsonData.stationName = data.DisplayName;
        }
        $scope.GroupName = data.DisplayName;

        $scope.jsonData.stationName = data.DisplayName;
        $scope.jsonData.CampaignName = data.CampaignName + "_Copy";
        $scope.jsonData.MultiSceneIds = data.MultiSceneIds;

        var sdate = new Date(data.StartDateAndTime);
        $scope.jsonData.StartDate = sdate;
        var edate = new Date(data.EndDateAndTime);
        $scope.jsonData.EndDate = edate;
        var zone = sdate.toString().match(/\(([A-Za-z\s].*)\)/)[1];
        var obj = {
            "date": sdate, "timezone_type": 3, "timezone": zone
        };
        var obj1 = {
            "date": edate, "timezone_type": 3, "timezone": zone
        };
        if (data.Type == "Parent Group") {
            $scope.selectGroup = "Parent Group";
        }
        else if (data.Type == "Sub Group") {
            $scope.selectGroup = "Sub Group";
            $scope.jsonData.Type = data.Type;
        }

        $scope.jsonData.StartTimeVal = new Date(obj.date);
        $scope.jsonData.EndTimeVal = new Date(obj1.date);
        $scope.jsonData.Interval = $scope.Intervals[0];
        $scope.jsonData.Copy = "Y";
        //   console.log(zone, $scope.jsonData);
    };
    //cloning campaign ends here

    $scope.checkSub = function (data) {
        $scope.GroupName = data;
    }
    $scope.checkDevice = function (data) {

        $scope.deviceNameSet = data;
    }
    //save data function
    $scope.SaveFunction = function (selectGroup, stationID, startDatevalue, endDatevalue, startTimevalue, endTimevalue, Interval, frequencySelected) {
        console.log("**********************************************************************");
        var todayDate = new Date();
        todayDate.setHours(0, 0, 0, 0);
        $scope.msg = "";
        console.log($scope.jsonData.StartDate);
        console.log($scope.jsonData.EndDate);
        if (($scope.jsonData.EndDate == null || $scope.jsonData.EndDate == undefined) || ($scope.jsonData.StartDate == null || $scope.jsonData.StartDate == undefined) || (selectGroup == "" || selectGroup == undefined) || (frequencySelected == "" || frequencySelected == undefined)) {
            $scope.msg = "Fields should not be empty !";
        }
        else {
            var stDtD = new Date($scope.jsonData.StartDate);
            var edDtD = new Date($scope.jsonData.EndDate);
            var todayDt = new Date(todayDate);

            if (edDtD < todayDate) {
                console.log(edDtD + "   " + todayDate);
                $scope.msg = "Past date is not allowed !";
            }


            $scope.jsonData.Type = selectGroup;
            //  alert(stationID + "     ahfdhafhafdhaf " + $scope.GroupName);
            //   console.log($scope.indexOfCkeckBox, 'stationID', stationID, $rootScope.cBValue);
            var dateValue = document.getElementById('myLocalDate').value;
            var d = new Date(dateValue);
            var dateOffsetValue = d.getTimezoneOffset();
            if (d != "Invalid Date") {
                var zone = d.toString().match(/\(([A-Za-z\s].*)\)/)[1];
                //     console.log(dateValue, 'dateValue', dateOffsetValue, 'zone', zone);
            }
            //  console.log($scope.jsonData.stationName, '$scope.jsonData.stationName', $scope.cloneMultiIds, $scope.cloneIndicator);

            if (selectGroup == "Parent Group") {
                for (var sid in $scope.stationNames) {
                    //      console.log($scope.stationNames[sid].DisplayStationName + "    " + $scope.jsonData.stationName);
                    if ($scope.stationNames[sid].DisplayStationName == $scope.jsonData.stationName) {
                        //          console.log($scope.stationNames[sid].DisplayStationName);
                        $scope.jsonData.DisplayId = $scope.stationNames[sid].DisplayStationid;
                    }
                }
            }
            else if (selectGroup == "Sub Group") {
                for (var sid in $scope.subgroupNames) {
                    console.log($scope.subgroupNames[sid].GroupName + "    " + $scope.jsonData.stationNameSub);
                    if ($scope.subgroupNames[sid].GroupName == $scope.jsonData.stationNameSub) {
                        console.log($scope.subgroupNames[sid].GroupName);
                        $scope.jsonData.DisplayId = $scope.subgroupNames[sid].GroupId;
                    }
                    //$scope.jsonData.DisplayId = 187;
                }
            }
            else {
                console.log($scope.deviceNames);
                for (var sid in $scope.deviceNames) {
                    //if (stationID != undefined) {
                    //    if ($scope.deviceNames[sid].PlayerName == stationID) {
                    //        $scope.jsonData.DisplayId = $scope.deviceNames[sid].PlayerId;
                    //        console.log("********matching*****");
                    //        console.log($scope.jsonData.DisplayId);

                    //    }
                    //}
                    //else {
                    console.log($scope.deviceNames[sid].PlayerId + "   " + $scope.deviceNames[sid].PlayerName + "           " + $scope.jsonData.stationNameDev);
                    if ($scope.deviceNames[sid].PlayerName == $scope.jsonData.stationNameDev){
                            $scope.jsonData.DisplayId = $scope.deviceNames[sid].PlayerId;
                            console.log("********matching*****");
                            console.log($scope.jsonData.DisplayId);
                            break;

                    }
                    //}

                }
            }

            $scope.jsonData.OffsetTime = dateOffsetValue;
            $scope.jsonData.Zone = zone;
            $scope.jsonData.Copy = "N";
            $scope.jsonData.Frequency = frequencySelected;
            if ($scope.cloneIndicator == true) {
                //    console.log('cloneIndicator' );

                $scope.jsonData.Copy = "Y";
                //    console.log($rootScope.cBValue, '$rootScope.cBValue', $rootScope.cBValue.length);
                $scope.jsonData.MultiSceneIds = JSON.stringify($rootScope.cBValue);
                if ($rootScope.cBValue.length == 0) {
                    $scope.msg = "Fields should not be empty !";
                }
            } else {
                $scope.jsonData.MultiSceneIds = JSON.stringify($rootScope.cBValue);
                //    console.log($rootScope.cBValue.length,'length');
                if ($rootScope.cBValue.length == 0) {
                    $scope.msg = "Fields should not be empty !";
                }
            }
            //  console.log($scope.jsonData);
            //Form validation
            var DaysOfWeekSelected = "";
            for (var i in $scope.jsonData) {
                //    console.log($scope.jsonData[i]);

                if ($scope.jsonData[i] == "" || $scope.jsonData[i] == undefined || $scope.jsonData[i] == null) {
                    console.log(i + "      " + $scope.jsonData[i]);
                    //      console.log("Fields should not be empty !")
                    $scope.msg = "Fields should not be empty !";
                }
                if ($scope.jsonData[i] == "Weekly") {
                    var checkedValue = null;
                    var inputElements = document.getElementsByClassName('weekDay');
                    for (var i = 0; inputElements[i]; ++i) {
                        if (inputElements[i].checked) {
                            checkedValue = inputElements[i].value;
                            if (DaysOfWeekSelected == "") {
                                DaysOfWeekSelected = checkedValue;
                            }
                            else {
                                DaysOfWeekSelected = DaysOfWeekSelected + "," + checkedValue;
                            }
                        }
                    }
                    if (DaysOfWeekSelected == "") {
                        if ($scope.msg == "") {
                            $scope.msg = "Please select the days for weekly frequency !";
                        }
                    }
                    $scope.jsonData.DaysOfWeek = DaysOfWeekSelected;

                }
            }
            if ($scope.jsonData.StartDate > $scope.jsonData.EndDate) {
                $scope.msg = "End date shouldn't be less than Start date !";
            } else {

                var sDate = new Date($scope.jsonData.StartDate);
                var stime = sDate.getTime();
                var eDate = new Date($scope.jsonData.EndDate);
                var etime = eDate.getTime();
                //     console.log(sDate, "Dateeeeeeeeee", eDate, "stimmmmmmmmmme", stime, 'etimeeeeeeee', etime)
                if (stime == etime) {
                    //        console.log('Both the startDate and endData are equal', $scope.jsonData.StartTimeVal, $scope.jsonData.EndTimeVal);
                    if ($scope.jsonData.StartTimeVal > $scope.jsonData.EndTimeVal) {
                        $scope.msg = "End Time shouldn't be less than start Time !";
                    }
                }
            }
            if ($scope.msg != "") {
                console.log($scope.msg);
            } else {
                //calling saveCampaign service      
                campaignService.saveCampaign($scope.jsonData).then(function (res) {
                    //        console.log(res.data);
                    $scope.res = res;
                    //$scope.scenes = res.data;
                    if (res.data == 'Success') {
                        $scope.msg = 'Playlist saved successfully';
                        //setTimout msg
                        setTimeout(function () {
                            window.location.reload();
                        }, 2000);
                    } else {
                        //           console.log(res.data);
                       // $scope.getScenesData();
                        $scope.addNewCamp = true;
                        $scope.msg = res.data;
                    }
                });
            }
        }

    };

    //reload started here
    $scope.reloadPage = function () {
        $scope.AddPlayListActive = "";
        $scope.ViewPlayListActive = "";
        $scope.ExpirePlayListActive = "";
        $scope.PublishPlayListActive = "";
        $scope.ReloadActive = "activeLinkSideMenu";
        window.location.reload();
    };
    // reload ended here

    //get stationNames
    campaignService.getstationData().then(function (res) {
        //    console.log(res.data, 'getAllStationNames');
        $scope.stationNames = res.data;
    });
    //get subgroupnames
    campaignService.getsubgroupData().then(function (res) {
        //      console.log(res.data, 'getAllStationNames');
        $scope.subgroupNames = res.data;
    });
    campaignService.getdeviceData().then(function (res) {
        //    console.log(res.data, 'getAllStationNames');
        $scope.deviceNames = res.data;
    });
    //get scenesData

    $scope.getScenesData = function () {
        campaignService.getScenesData().then(function (res) {
            //       console.log("&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&")
            //       console.log(res.data);
            $scope.scenes = res.data;

            $scope.totalItems = $scope.scenes.length;   //for pagination
            console.log($scope.totalItems, 'Add Playlist scenes Table');
            $scope.complete();
            $rootScope.loader = false;
        });
    };
    //calling getAllViewCampaignData service
    $scope.getAllViewCampaignData = function () {
        campaignService.getAllCampaignData().then(function (res) {
            //        console.log('getAllCampaignData res');
            //        console.log(res.data);
            $scope.campaignData = res.data;
            console.log($scope.campaignData);
            $rootScope.loader = false;
            $scope.viewTotalItems = $scope.campaignData.length;   //for pagination
            //        console.log($scope.viewTotalItems, 'ViewCampaignData');
        });
    }
    //calling cancelPublish function started
    $scope.cancPublish = function (cpid) {
        //    console.log(cpid);
        //cancelPublish service
        campaignService.cncPublish(cpid).then(function (res) {
            //        console.log(res);
            $scope.res = res;
            if (res.data == 'Success') {
                $scope.cancMsg = 'Playlist Cancelled Successfully';
                //Alert message timeout
                setTimeout(function () {
                    $scope.cancMsg = '';
                    $scope.getPublishData(); //calling getAllPublish service
                    $scope.getAllViewCampaignData();
                }, 2000);
            } else {
                $scope.cancMsg = res.data;
            }
        });
    }
    
    
    //calling getAllPublishData service
    $scope.getPublishData = function () {
        console.log($rootScope.loader);
        campaignService.getAllPublishData().then(function (res) {
            
            console.log("loader : "+$rootScope.loader);
           
            //$scope.publishData = res.data;
            $scope.model.publishTable = res.data;
            console.log($scope.model.publishTable);
            console.log(typeof $scope.model.publishTable.SceneName);
            for (var i = 1; i < $scope.model.publishTable.length; i++) {
                $scope.model.publishTable[i].SceneName = $scope.model.publishTable[i].SceneName.replace(/,[s]*/g, ", ");

            }
            $scope.pubTotalItems = $scope.model.publishTable.length;   //for pagination
            //        console.log('publishData res', $scope.model, 'publishData length', $scope.pubTotalItems);
            $scope.complete();
            $rootScope.loader = false;
            if ($rootScope.loader == true) {
                $rootScope.loadingText = false;
            }
        });

    };

    //table sorting started
    //$scope.propertyName = 'CampaignName';
    //$scope.reverse = false;
    //$scope.sortBy = function (propertyName) {
    //    $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
    //    $scope.propertyName = propertyName;
    //};
    $scope.propertyName = 'SceneName';
    $scope.reverse = false;
    $scope.sortBy = function (propertyName) {
        $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
        $scope.propertyName = propertyName;
    };
    //table sorting ended

    // pagination started
    $scope.dDownValues = [10, 15, 20, 30];
    $scope.viewby = $scope.dDownValues[0];

    $scope.viewby = 10;
    $scope.itemsPerPage = $scope.viewby;
    //   console.log($scope.viewby);

    $scope.setItemsPerPage = function (num) {
        $scope.viewby = num;
        $scope.itemsPerPage = num;
        $scope.currentPage = 1; //reset to first page
    };

    //   console.log($scope.totalItems, '$scope.totalItems');
    $scope.currentPage = 1;
    //Pagination ended

    //edit Publishtable started here
    $scope.model = {
        publishTable: [],
        selected: {}
    };

    // gets the template to ng-include for a table row / item
    $scope.getTemplate = function (publishData) {
        if (publishData.CampaignId === $scope.model.selected.CampaignId) return 'edit';
        else return 'display';
    };

    $scope.editPublishCampaign = function (publishData) {
        console.log("******************************************");
        $scope.model.selected = angular.copy(publishData);

        console.log($scope.model.selected);
    };

    $scope.saveEditCampaign = function (cid, displayId, displayName, MultiSceneIds, sceneId, stnName, eStartDate, eEndDate) {
        //    console.log(cid, '******* ', displayId, '******* ', displayName, '*******', MultiSceneIds, '*****', sceneId,'****', eStartDate, '*****',eEndDate);
        //    console.log(stnName);
        console.log(cid + "  " + displayId + "  " + displayName + "   " + MultiSceneIds + "    " + sceneId + "   " + stnName + "  " + eStartDate + "  " + eEndDate);

        if (stnName == "" || stnName == undefined || eStartDate == "" || eStartDate == undefined || eEndDate == "" || eEndDate == undefined) {
            $scope.pubMsg = "Fields should not be empty !";
        }
        else {
            var d = new Date(eStartDate);
            var dateOffsetValue = d.getTimezoneOffset();
            var zone = d.toString().match(/\(([A-Za-z\s].*)\)/)[1];
            //    console.log(eStartDate, 'dateValue', dateOffsetValue, 'Zone', zone);

            $scope.editPublishData.CampaignId = cid;
            $scope.editPublishData.DisplayId = displayId;
            $scope.editPublishData.DisplayStationName = stnName;
            $scope.editPublishData.MultiSceneIds = MultiSceneIds;

            $scope.editPublishData.startDate = eStartDate;
            $scope.editPublishData.EndDate = eEndDate;
            $scope.editPublishData.OffsetTime = dateOffsetValue;
            $scope.editPublishData.Zone = zone;

            //   console.log($scope.editPublishData);
            $scope.pubMsg = "";
            for (var i in $scope.editPublishData) {

                //        console.log($scope.editPublishData[i]);
                if ($scope.editPublishData[i] == "" || $scope.editPublishData[i] == undefined) {

                    $scope.pubMsg = "Fields should not be empty !";
                }
            }
            //    console.log($scope.editPublishData, '$scope.editPublishData');

            if ($scope.editPublishData.startDate > $scope.editPublishData.EndDate) {
                $scope.pubMsg = "End date & time shouldn't be less than Start date & time !";
            } else {
                var sDate = new Date($scope.editPublishData.startDate);
                var stime = sDate.getTime();
                var eDate = new Date($scope.editPublishData.EndDate);
                var etime = eDate.getTime();
                //     console.log(sDate, "Dateeeeeeeeee", eDate, "stimmmmmmmmmme", stime, 'etimeeeeeeee', etime)
                if (stime == etime) {
                    //          console.log('Both the startDate and endData are equal', $scope.editPublishData.startDate, $scope.editPublishData.EndDate);
                    var startTime = $scope.editPublishData.startDate.toTimeString()
                    var endTime = $scope.editPublishData.EndDate.toTimeString()
                    //          console.log(startTime,"starttime and endtime",endTime);
                    //if (startTime > endTime) {
                    //    $scope.pubMsg = "End Time shouldn't be less than start Time !";
                    //}
                }
            }
            if ($scope.pubMsg != "") {
                console.log($scope.pubMsg);
            } else {
                //editpublishCampagin service call
                campaignService.editPubCampaign($scope.editPublishData).then(function (res) {
                    console.log(res);
                    $scope.res = res;
                    if (res.data == 'Success') {
                        $scope.pubMsg = 'Playlist Updated Successfully';
                        //Alert message timeout
                        setTimeout(function () {
                            window.location.reload();
                        }, 2000);
                    } else {
                        $scope.pubMsg = res.data;
                    }
                });
            };

        }


    };

    $scope.reset = function () {
        $scope.model.selected = {};
        $scope.pubMsg = "";
    };

    //delete campaign started
    $scope.deleteCampaign = function (cid) {
        $scope.viewExpired = false;
        console.log('Playlist id', cid);
        //delete service call
        campaignService.delCampaign(cid).then(function (res) {
            console.log(res);
            $scope.res = res;
            if (res.data == 'Success') {
                $scope.pubMsg = 'Playlist Deleted Successfully';
                //Alert message timeout
                setTimeout(function () {
                    window.location.reload();
                }, 2000);
            } else {
                $scope.pubMsg = 'Error in Deleting Playlist';
            }
        });

    }

    //calling publish function
    $scope.publishFun = function (pid) {

        console.log(pid);
        //Publish service
        campaignService.pubCampaign(pid).then(function (res) {
            console.log(res);
            $scope.res = res;
            if (res.data == 'Success') {
                $scope.pubMsg = 'Playlist Published Successfully';
                //Alert message timeout
                setTimeout(function () {
                    window.location.reload();
                }, 2000);
            } else {
                $scope.pubMsg = 'Error In Publishing Playlist';
            }
        });
    }
});

//Modal directive started here
dsDirectives.directive('modalCompc', function () {
    return {
        template: '<div class="modal fade" style="z-index:9999;">' +
        '<div class="modal-dialog cusWidth">' +
        '<div class="modal-content">' +
        '<div class="modal-header">' +
        '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>' +
        '<h4 class="modal-title"></h4>' +
        '</div>' +
        '<div class="modal-body" ng-transclude> <div id="dy_template"></div></div>' +
        '</div>' +
        '</div>' +
        '</div>',
        restrict: 'E',
        transclude: true,
        replace: true,
        scope: true,
        link: function postLink(scope, element, attrs) {
            scope.title = attrs.title;

            scope.$watch(attrs.visible, function (value) {
                if (value == true)
                    $(element).modal('show');
                else
                    $(element).modal('hide');
            });

            $(element).on('shown.bs.modal', function () {
                scope.$apply(function () {
                    scope.$parent[attrs.visible] = true;
                });
            });

            $(element).on('hidden.bs.modal', function () {
                scope.$apply(function () {
                    scope.$parent[attrs.visible] = false;
                });
            });
        }
    };
});
//Modal directive ended here



//campaignService factory starts here
dsFactory.factory('campaignService', function ($http, $q) {

    //here $q is an angularjs service which help us to run asynchronous function and return result when processing done.
    return {
        // Save data
        saveCampaign: function (data) {
            console.log(data);
            var defer = $q.defer();
            $http({
                url: '/Campaign/SaveCampaign',
                method: 'POST',
                data: { vmCampgn: JSON.stringify(data) },
                headers: { 'content-type': 'application/json' }
            }).then(function successCallback(response) {
                console.log(response);
                defer.resolve(response);
            }, function errorCallback(err) {
                console.log(err);
                defer.reject(err);
            });
            return defer.promise;
        },

        getScenesData: function () {
            return $http.get('/Campaign/GetApprovedScenes');
        },
        getAllExpiredDataServCal() {
            return $http.get('/Campaign/GetAllExpiredCampaigns');
        },

        //Get All station data
        getstationData: function () {
            return $http.get('/Campaign/GetAllStations');
        },

        getsubgroupData: function () {
            return $http.get('/Campaign/GetAllSubGroupNames');
        },

        getdeviceData: function () {
            return $http.get('/Campaign/GetUnassignedDeviceNames');
        },

        //Get All Campaign data
        getAllCampaignData: function () {
            return $http.get('/Campaign/GetAllCampaigns');

        },

        //Get All Publish data
        getAllPublishData: function () {
            return $http.get('/Campaign/GetCampaignsToPublish');
        },
        activatePlayer: function (data) {
            var defer = $q.defer();
            $http({
                url: '/Campaign/ActivateCampaign',
                method: 'POST',
                data: { campaignId: JSON.stringify(data) },
                headers: { 'content-type': 'application/json' }
            }).then(function successCallback(response) {
                console.log(response);
                defer.resolve(response);
            }, function errorCallback(err) {
                console.log(err);
                defer.reject(err);
            });
            return defer.promise;
        },
        // cancelPublish data
        cncPublish: function (data) {
            console.log(data);
            var defer = $q.defer();
            $http({
                url: '/Campaign/CancelCampaign',
                method: 'POST',
                data: { campaignId: JSON.stringify(data) },
                headers: { 'content-type': 'application/json' }
            }).then(function successCallback(response) {
                console.log(response);
                defer.resolve(response);
            }, function errorCallback(err) {
                console.log(err);
                defer.reject(err);
            });
            return defer.promise;
        },
        // publish data
        pubCampaign: function (data) {
            console.log(data);
            var defer = $q.defer();
            $http({
                url: '/Campaign/PublishCampaign',
                method: 'POST',
                data: { campaignId: JSON.stringify(data) },
                headers: { 'content-type': 'application/json' }
            }).then(function successCallback(response) {
                console.log(response);
                defer.resolve(response);
            }, function errorCallback(err) {
                console.log(err);
                defer.reject(err);
            });
            return defer.promise;
        },
        // publish data
        delCampaign: function (data) {
            console.log(data);
            var defer = $q.defer();
            $http({
                url: '/Campaign/DeleteCampaign',
                method: 'POST',
                data: { campaignId: JSON.stringify(data) },
                headers: { 'content-type': 'application/json' }
            }).then(function successCallback(response) {
                console.log(response);
                defer.resolve(response);
            }, function errorCallback(err) {
                console.log(err);
                defer.reject(err);
            });
            return defer.promise;
        },
        //editPublishCampaign data
        editPubCampaign: function (data) {
            console.log(data);
            var defer = $q.defer();
            $http({
                url: '/Campaign/EditCampaign',
                method: 'POST',
                data: { vmCampgn: JSON.stringify(data) },
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