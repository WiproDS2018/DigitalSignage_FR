
dsControllers.controller('PlayerGroupController', function ($scope, $rootScope, playerGroupService, $q) {
    $rootScope.loader = true;
    $rootScope.parentBgHome = true;
    console.log("welcome to deviceGroup");
    $scope.active = true;
    $scope.chckBox = false;
    $scope.chckBoxMsg = false;
    $rootScope.loader = true;
    $rootScope.cBValue = [];
    $scope.aNGroup = false;
    $scope.GroupTableVisibility = true;
    $scope.assnStation = false;
    $scope.chckBoxforAssignPlayer = false;
    $scope.subTitle = "- View Device"
    $rootScope.cBValue = [];
  
    $scope.PlayerGroup = {    // added for group
        gid: 12,
        groupName: '',
        groupDescription: ''
    };
    $scope.Group = {    // added for group
        groupName: '',
        groupDescription: ''
    };

    $scope.assignMsg = "Select the Group for Assign !";
    //Breadcrumb starts
    $scope.cUrl = window.location.href;
    var res = $scope.cUrl.split("/");
    var subst = res.pop();
    $rootScope.state = subst.replace(/#/g, "");
    console.log($rootScope.state);
    //Breadcrumb ends

    //checkbox code started here
    $scope.vm = {};
    $scope.vmPGroup = {};
 
    $scope.cancFun = function () {
        window.location.reload();
    };

    //reload started here
    $scope.reloadPage = function () {
        window.location.reload();
    };
  
    //edit playerTable code starts here
    $scope.model = {
        selected: {},
        PlayerGroupTable: [],
        PlayerjoinGroupList: []
    };
    
    $scope.reset = function () {
        $scope.model.selected = {};
    };
    //edit playerTable code ends here
       
    //getting player data ended
    $scope.getAllGroupPlayer = function () {
        playerGroupService.getAllPlayerGroupData().then(function (res) {
            $scope.model.PlayerGroupTable = res.data;
            $scope.totalItems = $scope.model.PlayerGroupTable.length;   //for pagination
            $scope.getPlayerGroupData = res.data;
            console.log(res);
            $rootScope.loader = false;
        });
    }
  
    // adding group by prodipta /////////////////////////////////////////////////////////////////////////////////////////
    $scope.newGroup = function () {
        console.log($scope.Group);
        $scope.aNGroup = true;
        $scope.aNPlayer = false;
        $scope.playersTable = false;
        $scope.GroupTableVisibility = false;
        $scope.assnStation = false;
        $rootScope.cBValue = [];
        $scope.subTitle = "- Add DeviceGroup"
    };
    $scope.addGroup = function () {
        console.log($scope.Group, $scope.PlayerGroup);
        $scope.msg = "";
        //Form Validation
        for (var i in $scope.Group) {
            console.log(i);
            //console.log($scope.Player[i]);
            if ($scope.Group[i] == "") {
                $scope.msg = "Fields should not be empty  !! !";
            }
        }
        if ($scope.msg != "") {
            console.log($scope.msg);
        } else {
            //calling SaveFormDataGroup service
            console.log($scope.Group);
            playerGroupService.SaveFormDataGroup($scope.Group).then(function (res) {
                console.log('inside controller');
                console.log(res);
                $scope.res = res;
                if (res.data == 'Success') {
                    $scope.msg = 'Group Added Successfully';
                    // Alert message timeout
                    setTimeout(function () {
                        window.location.reload();
                    }, 2000);

                } else {
                    $scope.msg = res.data;
                }
                
            });
        }
    };
    // adding player group code end hare

    // function to get all Playergroup
    $scope.getAllPlayerGroup = function () {
        $scope.GroupTableVisibility = true;
        $scope.aNPlayer = false;
        $scope.aNGroup = false;
        $scope.playersTable = false;
        $scope.assnStation = false;
        playerGroupService.getAllPlayerGroupData().then(function (res) {
            console.log(res);
            $scope.model.PlayerGroupTable = res.data;
            $rootScope.loader = false;
        });
        window.location.reload();
    };
    //vmPGroup check box code started by prodipta
    $scope.vmPGroup.myClick = function (ind, Gid, Gname, Gdescription,groupData) {
        $rootScope.ind = ind;
        $rootScope.GroupIdd = Gid;
        $rootScope.Gname = Gname;
        $rootScope.Gdescription = Gdescription;
        console.log(groupData);
        console.log($rootScope.ind + "**********" + $rootScope.GroupIdd + "------------" + $rootScope.Gname + "****************" + $rootScope.Gdescription);
        $scope.alertText1 = document.getElementById($rootScope.GroupIdd).checked;
        console.log($scope.alertText1);
        if ($scope.alertText1) {
            console.log('checkbox is true');
            $rootScope.cBValue.push($rootScope.GroupIdd);
            console.log($rootScope.cBValue);
        } else {
            $rootScope.cBValue.splice($rootScope.cBValue.indexOf($rootScope.GroupIdd), 1);
            console.log($rootScope.cBValue);
        }
    };
    // vmPGroup code ended

    // Edit PlayerGroup started by Prodipta
    $scope.getTemplateForGroup = function (PlayerGroup) {
        if (PlayerGroup.GroupId === $scope.model.selected.GroupId) return 'editGroup';
        else return 'displayGroup';
    };

    $scope.editPlayerGroup = function (PlayerGroup) {
        $scope.model.selected = angular.copy(PlayerGroup);
    };

    $scope.saveplayerGroup = function (idx, Gid, Gname, Gdescription) {
        console.log(idx + "**********" + Gid + "----------" + Gname + "********" + Gdescription);
        console.log("Saving device group Details");
        $scope.savePlayerGroupData = {
            "GroupId": Gid,
            "GroupName": Gname,
            "GroupDescription": Gdescription
        };
        console.log($scope.savePlayerGroupData);
        $scope.model.PlayerGroupTable[idx] = angular.copy($scope.model.selected);
        console.log($scope.model);
        $scope.reset();

        //calling editPlayerGroupTable service
        playerGroupService.editPlayerGroupTable($scope.savePlayerGroupData).then(function (res) {
            console.log(res);
            $scope.res = res;
            if (res.data == 'Success') {
                $scope.editMsg = 'Device Group Details Edited Successlly';
                //Alert message timeout
                    setTimeout(function () {
                        window.location.reload();
                    }, 3000);
            } else {
                $scope.editMsg = res.data;
            }
           
        });
    };

    $scope.reset = function () {
        $scope.model.selected = {};
    };
    // Edit PlayerGroup ended by Prodipta

    // for removing the Group by prodipta
    $scope.rmGroupData = function () {
        $scope.chckBoxMsgForGrp = true;
        console.log($scope.alertText1);
        if ($scope.alertText1 || $rootScope.cBValue.length > 0) {
            if ($scope.assnStation == false){
                $scope.chckBoxMsgForGrp = false;
                $scope.removeGroupData(); //calling removeGroupData() Function()
                $scope.alertText1 = false;
            };
        }
    };
    $scope.removeGroupData = function () {
        console.log('inside remove function of group', $rootScope.cBValue);
        //calling removeStationData service
        playerGroupService.removePlayerGroupData($rootScope.cBValue).then(function (res) {
            console.log(res);
            $scope.res = res;
            if (res.data == 'Success') {
                $scope.editMsg = 'Device Group removed Successfully';
                //Alert message timeout
                setTimeout(function () {
                    window.location.reload();
                }, 2000);
            } else {
                $scope.editMsg = res.data;
            }            
        });
    };

    //drag and drop code starts here
    $scope.models = {
        selected: null,
        lists: { "Available": [], "Selected": [] }
    };

    //assignPlayer started here
    $scope.assignPlayer = function () {
        console.log("inside Assign Device func ", $scope.alertText1, $rootScope.cBValue.length);
        $scope.chckBoxforAssignPlayer = true;
        console.log($scope.alertText1);
        if ($rootScope.cBValue.length < 2 && $rootScope.cBValue.length > 0) {
            console.log('checkbox length is less than one');            
            $scope.chckBoxforAssignPlayer = false;
            $scope.assignPlayerGroup(); // calling the assignPlayergroup()
            $scope.alertText1 = false;
        } else {
            $scope.assignMsg = "Please Select one checkbox !";
        }
    };

    //calling getPlayersData service
    $scope.assignPlayerGroup = function () {
        console.log('asdfasdfasdf', $scope.alertText1);
        $scope.chckBoxMsgassign = true;
        $scope.editMsg = "";
        $scope.GroupTableVisibility = false;
        if ($scope.alertText1 || $rootScope.cBValue.length > 0) {
            $scope.chckBoxMsgassign = false;
            $scope.getAssignPlayersList(); //calling removeData() Function()        
        }
    };
    $scope.getAssignPlayersList = function () {
        console.log('inside assignPlayer', $rootScope.cBValue, $rootScope.GroupIdd, $scope.getPlayerGroupData);
        var gId = $rootScope.cBValue[0];
        console.log(gId, 'gid');

        $scope.getPlayerGroupData.forEach(function (data) {
            console.log(data);
            if (data.GroupId == gId) {
                $scope.grpName = data.GroupName;
                console.log($scope.grpName);
            }
        });

        $scope.assnStation = true;
        $scope.subTitle = "- Assign Device";
        //calling getAvailablePlayersData service
        playerGroupService.getPlayersData(gId).then(function (res) {
            console.log('getting all available Device data from service');
            //$scope.models.lists.Available.push(res.data);
            $scope.models.lists.Available = res.data;
            // $scope.models.lists.Selected.push(res.data[0]);
        });

        //calling getAssignedPlayersData service
        playerGroupService.getAssignedPlayersData($rootScope.GroupIdd).then(function (res) {
            console.log('getting all assigned Device data from service');
            $scope.models.lists.Selected = res.data;
        });
    }


    $scope.resetAssignPlayerLists = function () {
        if ($scope.models.lists.Available.length == 0) {
            $scope.models.lists.Available = [];
            $scope.models.lists.Available.push("");
        }

        if ($scope.models.lists.Selected.length == 0) {
            $scope.models.lists.Selected = [];
            $scope.models.lists.Selected.push("");
        }
    }

    // Model to JSON for demo purpose
    $scope.$watch('models', function (model) {
        $scope.modelAsJson = angular.toJson(model, true);
        console.log($scope.modelAsJson);
        //$scope.resetAssignPlayerLists();
        //console.log("Available length: " + $scope.models.lists.Available.length);
        //console.log("Selected length: " + $scope.models.lists.Selected.length);
        //debugger;
    }, true);

    $scope.ddSave = function () {
        //debugger;
        //Get selected players list
        var selectedLists = $scope.models.lists.Selected;
        var length = selectedLists.length;
        //Get selected station Id
        var selectedStationId = $rootScope.GroupIdd;

        var playerIdValues = "";
        var i;
        for (i = 0; i < length; i++) {
            playerIdValues += (selectedLists[i] != "")
                ? selectedLists[i].PlayerId + ((i < length - 1) ? ", " : "") : "";
        }
        console.log(playerIdValues);
        playerGroupService.SaveAssignPlayerStation(playerIdValues, selectedStationId).then(function (res) {
            console.log(res);
            $scope.res = res;
            if (res.data == 'sucess') {
                $scope.editMsg = 'Changes updated successfully !';
            } else {
                $scope.editMsg = res.data;
            }
            //Alert message timeout
                setTimeout(function () {
                    window.location.reload();
                }, 2000);
        });
    };

    //Reset button to Assign Players
    $scope.ddRestore = function restore() {
        $scope.getAssignPlayersList();
    };

    //table sorting started
    $scope.propertyName = 'GroupName';
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

});   //controller ended


//service code starts here
dsFactory.factory('playerGroupService', function ($http, $q, $rootScope) {
    //here $q is an angularjs service which help us to run asynchronous function and return result when processing done.
    return {
        // for saving the master group 
        SaveFormDataGroup: function (data) {
            var defer = $q.defer();
            $http({
                url: '/Player/SavePlayerGroup',
                method: 'POST',
                data: { vmPlayergroup: data },
                headers: { 'content-type': 'application/json' }
            }).then(function successCallback(response) {
                console.log(response);
                defer.resolve(response);
                $rootScope.loader = false;
            }, function errorCallback(err) {
                console.log(err);
                defer.reject(err);
            });
            return defer.promise;
        },
        //  getAllPlayerGroupData:
        getAllPlayerGroupData: function () {
            return $http.get('/Player/getAllGroupPlayer');
        },
        //PlayerGrouptable Edit started here by prodipta
        editPlayerGroupTable: function (data) {
            console.log(data)
            var defer = $q.defer();
            $http({
                url: '/Player/EditGroup',
                method: 'POST',
                data: JSON.stringify(data),
                headers: { 'content-type': 'application/json' }
            }).then(function successCallback(response) {
                console.log(response);
                defer.resolve(response);
                $rootScope.loader = false;
            }, function errorCallback(err) {
                console.log(err);
                defer.reject(err);
            });
            return defer.promise;
        },
        //PlayerGrouptable Edit started here by prodipta

        //Get all Assigned Players started
        getAssignedPlayersData: function (stationID) {
            return $http.get('/Player/GetGrouppedPlayers', { params: { groupid: stationID } });
        },

        //Get all Players started
        getPlayersData: function () {
            return $http.get('/Player/GetUnGrouppedPlayers');
        },

        // Save Assign player data
        SaveAssignPlayerStation: function (playerIds, stationId) {
            var defer = $q.defer();
            $http({
                url: '/Player/SavePlayerToGroup',
                method: 'POST',
                data: { playerIds: JSON.stringify(playerIds), stationId: JSON.stringify(stationId) },
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
        //remove Group
        removePlayerGroupData: function (GroupId) {
            console.log('inside remove station', GroupId);
            var defer = $q.defer();
            $http({
                url: '/Player/DeletePlayerGroup',
                method: 'POST',
                data: { 'GroupIds': JSON.stringify(GroupId) },
                headers: { 'content-type': 'application/json' }
            }).then(function successCallback(response) {
                console.log(response);
                defer.resolve(response);
                $rootScope.loader = false;
            }, function errorCallback(err) {
                console.log(err);
                defer.reject(err);
            });
            return defer.promise;
        },
    }
});


