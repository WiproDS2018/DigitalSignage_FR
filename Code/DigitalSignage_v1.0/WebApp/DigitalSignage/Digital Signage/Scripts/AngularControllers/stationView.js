dsControllers.controller('StationController', function ($window, $scope, $rootScope, stationService, playerGroupService, cfpLoadingBar) {
    $rootScope.loader = true;
    $scope.aNStation = false;
    $scope.assnStation = false;
    $scope.stationTable = false;
    $scope.chckBoxMsg = false;
    $scope.chckBoxMsgassign = false;
    $scope.active = true;
    $rootScope.parentBgHome = true;
    $scope.hideCol = false;
    $scope.statName = "";
    $scope.AllGroupsTable = true;
    $scope.allGroupData = [];
    $scope.groupArray = [];
    $scope.checkedList = [];
    $scope.checkedListSub = [];
    $scope.subTitle = " - Groups";
    $scope.assignMsg = "Select the Device Group !";
    $scope.obj = {
        'available': '',
        'selected': '',
    };
    
    $scope.start = function () {
        cfpLoadingBar.start();
    };
    $scope.start();
    $scope.complete = function () {
        cfpLoadingBar.complete();
    }

    $scope.myUserRole = $('#myUserRole').val();
    if ($scope.myUserRole == "Admin") {
        $scope.adminUserOnly = true;
    }
    else {
        $scope.adminUserOnly = false;
    }
    $scope.clearMsg = function () {
        // All active class variables for ng-class //
        $scope.ActiveDefineGrouping = "activeLinkSideMenu";
        $scope.ActiveCreateParentGroupSub = "";
        $scope.ActiveCreateSubGroupSub = "";
        $scope.ActiveViewParentGroup = "";
        $scope.ActiveViewParentGroupSubAttach = "";
        $scope.ActiveViewParentGroupSubRemove = "";
        $scope.ActiveViewSubGroup = "";
        $scope.ActiveViewSubGroupAttach = "";
        $scope.ActiveViewSubGroupRemove = "";
        // variable for active class ends here 
        $scope.hideSubGroup = true;
        $scope.hideParentGroup = true;


        $scope.editMsg = "";
        $scope.AllGroupsTable = true;
        $scope.assnStationPlrGr = false;
        $scope.GroupTableVisibilityPlrGr = false;
        $scope.stationTable = false;
        $scope.aNStation = false;
        $scope.aNGroupPlrGr = false;
        $scope.assnStation = false;
        $scope.changeckBoxMsg = false;
        $scope.chckBoxMsgassign = false;
        $scope.chckBoxMsg = false;
        $scope.chckBoxMsgassign = false;
        $scope.chckBoxMsgForGrpPlrGr = false;
        $scope.chckBoxforAssignPlayerPlrGr = false;
    }
    $scope.getAllStationsData = function () {
        $scope.AllGroupsTable = false;
        $scope.assnStationPlrGr = false;
        $scope.GroupTableVisibilityPlrGr = false;
        $scope.stationTable = true;
        $scope.aNStation = false;
        $scope.aNGroupPlrGr = false;
        $scope.assnStation = false;
        $scope.changeckBoxMsg = false;
        $scope.chckBoxMsgassign = false;
        $scope.chckBoxMsg = false;
        $scope.chckBoxMsgassign = false;
        $scope.chckBoxMsgForGrpPlrGr = false;
        $scope.chckBoxforAssignPlayerPlrGr = false;
        $scope.hideCol = true;
        stationService.getStationsData().then(function (res) {
            console.log('getting data from server', res)
            $scope.model.stationTable = res.data;
            $scope.totalItems = $scope.model.stationTable.length;   //for pagination
            $scope.stationData = res.data;
            
            
            $rootScope.loader = false;
        });
    }
    $scope.getAllSubGroupData = function () {
        $scope.AllGroupsTable = false;
        $scope.hideCol = false;
        $scope.assnStationPlrGr = false;
        $scope.GroupTableVisibilityPlrGr = true;
        $scope.stationTable = false;
        $scope.aNStation = false;
        $scope.aNGroupPlrGr = false;
        $scope.assnStation = false;
        $scope.changeckBoxMsg = false;
        $scope.chckBoxMsgassign = false;
        $scope.chckBoxMsg = false;
        $scope.chckBoxMsgassign = false;
        $scope.chckBoxMsgForGrpPlrGr = false;
        $scope.chckBoxforAssignPlayerPlrGr = false;
        playerGroupService.getAllPlayerGroupData().then(function (res) {
            $scope.modelPlrGr.PlayerGroupTable = res.data;
            $scope.totalItems = $scope.modelPlrGr.PlayerGroupTable.length;   //for pagination
            $scope.getPlayerGroupData = res.data;
            console.log(res);
            $rootScope.loader = false;
        });
    }
    console.log($scope.obj);
    $scope.getParentGroup = function () {
        $scope.subTitle = " - Parent groups";
        $scope.hideParentGroup = false;
        $scope.hideSubGroup = true;
        // All active class variables for ng-class //
        $scope.ActiveDefineGrouping = "";
        $scope.ActiveCreateParentGroupSub = "";
        $scope.ActiveCreateSubGroupSub = "";
        $scope.ActiveViewParentGroup = "activeLinkSideMenu";
        $scope.ActiveViewParentGroupSubAttach = "";
        $scope.ActiveViewParentGroupSubRemove = "";
        $scope.ActiveViewSubGroup = "";
        $scope.ActiveViewSubGroupAttach = "";
        $scope.ActiveViewSubGroupRemove = "";
        // variable for active class ends here 
        $scope.AllGroupsTable = false;
        $scope.editMsg = "";
        $scope.hideCol = false;
        $scope.assnStationPlrGr = false;
        $scope.GroupTableVisibilityPlrGr = false;
        $scope.stationTable = true;
        $scope.aNStation = false;
        $scope.aNGroupPlrGr = false;
        $scope.assnStation = false;
        $scope.changeckBoxMsg = false;
        $scope.chckBoxMsgassign = false;
        $scope.chckBoxMsg = false;
        $scope.chckBoxMsgassign = false;
        $scope.chckBoxMsgForGrpPlrGr = false;
        $scope.chckBoxforAssignPlayerPlrGr = false;
        $rootScope.satationcBValue = [];
        for (k = 0; k < $scope.checkedList.length; k++) {
            document.getElementById($scope.checkedList[k]).checked = false
        }

    }
    $scope.getSubGroup = function () {
        $scope.alertText2 = false;
        $scope.subTitle = " - Sub groups";
        $scope.hideSubGroup = false;
        $scope.hideParentGroup = true;
        // All active class variables for ng-class //
        $scope.ActiveDefineGrouping = "";
        $scope.ActiveCreateParentGroupSub = "";
        $scope.ActiveCreateSubGroupSub = "";
        $scope.ActiveViewParentGroup = "";
        $scope.ActiveViewParentGroupSubAttach = "";
        $scope.ActiveViewParentGroupSubRemove = "";
        $scope.ActiveViewSubGroup = "activeLinkSideMenu";
        $scope.ActiveViewSubGroupAttach = "";
        $scope.ActiveViewSubGroupRemove = "";
        // variable for active class ends here 
        $scope.AllGroupsTable = false;
        $scope.editMsg = "";
        $scope.hideCol = false;
        $scope.assnStationPlrGr = false;
        $scope.GroupTableVisibilityPlrGr = true;
        $scope.stationTable = false;
        $scope.aNStation = false;
        $scope.aNGroupPlrGr = false;
        $scope.assnStation = false;
        $scope.changeckBoxMsg = false;
        $scope.chckBoxMsgassign = false;
        $scope.chckBoxMsg = false;
        $scope.chckBoxMsgassign = false;
        $scope.chckBoxMsgForGrpPlrGr = false;
        $scope.chckBoxforAssignPlayerPlrGr = false;
        $rootScope.cBValuePlrGr = [];
        for (k = 0; k < $scope.checkedListSub.length; k++) {

            document.getElementById($scope.checkedListSub[k]).checked = false
        }

    }
    //$scope.changeViewParent = function () {
    //    $scope.assnStationPlrGr = false;
    //    $scope.GroupTableVisibilityPlrGr = false;
    //    $scope.stationTable = true;
    //    $scope.aNStation = false;
    //    $scope.aNGroupPlrGr = false;
    //    $scope.assnStation = false;
    //    $scope.changeckBoxMsg = false;
    //    $scope.chckBoxMsgassign = false;
    //    $scope.chckBoxMsg = false;
    //    $scope.chckBoxMsgassign = false;
    //    $scope.chckBoxMsgForGrpPlrGr = false;
    //    $scope.chckBoxforAssignPlayerPlrGr = false;

    //}
    //$scope.changeView = function () {
    //    $scope.aNStation = false;
    //    $scope.aNGroupPlrGr = false;
    //    $scope.assnStation = false;
    //    $scope.changeckBoxMsg = false;
    //    $scope.chckBoxMsgassign = false;
    //    $scope.chckBoxMsg = false;
    //    $scope.stationTable = false;
    //    $scope.assnStation = false;
    //    $scope.changeckBoxMsg = false;
    //    $scope.chckBoxMsgassign = false;
    //    $scope.chckBoxMsgForGrpPlrGr = false;
    //    $scope.chckBoxforAssignPlayerPlrGr = false;
    //    $scope.assnStationPlrGr = false;



    //    if ($scope.GroupTableVisibilityPlrGr == true) {
    //        $scope.GroupTableVisibilityPlrGr = true;
    //        //$scope.stationTable = true;
    //    }
    //    else {
    //        $scope.GroupTableVisibilityPlrGr = true;
    //    }

    //}

    // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@CODE FOR PLAYER GROUP @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@//
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

    $scope.activePlrGr = false;
    $scope.chckBoxPlrGr = false;
    $scope.chckBoxMsgPlrGr = false;
    $rootScope.loader = true;
    $rootScope.cBValuePlrGr = [];
    $scope.aNGroupPlrGr = false;
    $scope.GroupTableVisibilityPlrGr = false;
    $scope.assnStationPlrGr = false;
    $scope.chckBoxforAssignPlayerPlrGr = false;
    $scope.subTitlePlrGr = "- View Device"
    $rootScope.cBValuePlrGr = [];

    $scope.PlayerGroupPlrGr = {    // added for group
        gid: 12,
        groupName: '',
        groupDescription: ''
    };
    $scope.GroupPlrGr = {    // added for group
        groupName: '',
        groupDescription: ''
    };

    $scope.assignMsgPlrGr = "Select the Group for Assign !";
    //Breadcrumb starts
    //$scope.cUrlPlrGr = window.location.href;
    //var resPlrGr = $scope.cUrl.split("/");
    //var substPlrGr = res.pop();
    //$rootScope.state = substPlrGr.replace(/#/g, "");
    //console.log($rootScope.state);
    ////Breadcrumb ends

    //checkbox code started here
    $scope.vmPlrGr = {};
    $scope.vmPGroupPlrGr = {};

    $scope.cancFunPlrGr = function () {
        window.location.reload();
    };

    //reload started here
    $scope.reloadPagePlrGr = function () {
        window.location.reload();
    };

    //edit playerTable code starts here
    $scope.modelPlrGr = {
        selected: {},
        PlayerGroupTable: [],
        PlayerjoinGroupList: []
    };

    $scope.resetPlrGr = function () {
        $scope.modelPlrGr.selected = {};
    };
    //edit playerTable code ends here

    //@@@@@@@@@@@@@@@@ CREATE PLAYER GROUP DIV 
    // create player group
    $scope.newGroup = function () {
        $scope.subTitle = " - Create Sub group";
        $scope.hideSubGroup = true;
        $scope.hideParentGroup = true;
        // All active class variables for ng-class //
        $scope.ActiveDefineGrouping = "";
        $scope.ActiveCreateParentGroupSub = "";
        $scope.ActiveCreateSubGroupSub = "activeLinkSideMenu";
        $scope.ActiveViewParentGroup = "";
        $scope.ActiveViewParentGroupSubAttach = "";
        $scope.ActiveViewParentGroupSubRemove = "";
        $scope.ActiveViewSubGroup = "";
        $scope.ActiveViewSubGroupAttach = "";
        $scope.ActiveViewSubGroupRemove = "";
        // variable for active class ends here 
        $scope.AllGroupsTable = false;
        console.log($scope.GroupPlrGr);
        $scope.aNGroupPlrGr = true;
        $scope.aNPlayerPlrGr = false;
        $scope.playersTablePlrGr = false;
        $scope.GroupTableVisibilityPlrGr = false;
        $scope.assnStationPlrGr = false;
        $scope.aNStation = false;
        $scope.assnStation = false;
        $scope.stationTable = false;
        $scope.chckBoxMsg = false;
        $scope.chckBoxMsgassign = false;
        $scope.chckBoxMsgForGrpPlrGr = false;
        $scope.chckBoxforAssignPlayerPlrGr = false;
        $rootScope.cBValuePlrGr = [];
        $scope.subTitlePlrGr = "- Create DeviceGroup";
        $scope.GroupPlrGr = {};
        $scope.msgPlrGr = "";
    };
    $scope.addGroup = function () {
        console.log($scope.GroupPlrGr, $scope.PlayerGroupPlrGr);
        $scope.msgPlrGr = "";
        //Form Validation
        //for (var i in $scope.GroupPlrGr) {
        //    console.log(i);
        //    //console.log($scope.Player[i]);
        //    if ($scope.GroupPlrGr[i] == "") {
        //        $scope.msgPlrGr = "Fields should not be empty  !! !";
        //    }
        //}
        if ($scope.GroupPlrGr.groupName == "" || $scope.GroupPlrGr.groupName == undefined) {
            $scope.msgPlrGr = "Group Name should not be empty  !! !";
        }
        if ($scope.msgPlrGr != "") {
            console.log($scope.msgPlrGr);
        } else {
            //calling SaveFormDataGroup service
            console.log($scope.GroupPlrGr);
            playerGroupService.SaveFormDataGroup($scope.GroupPlrGr).then(function (res) {
                console.log('inside controller');
                console.log(res);
                $scope.resPlrGr = res;
                if (res.data == 'Success') {
                    $scope.msgPlrGr = 'Sub group Added Successfully';
                    // Alert message timeout
                    setTimeout(function () {
                        $scope.getAllGroupPlayer();
                        $scope.msgPlrGr = "";
                        $scope.aNGroupPlrGr = false;
                        $scope.assnStationPlrGr = false;
                        $scope.GroupTableVisibilityPlrGr = true;
                        //window.location.reload();
                    }, 2000);

                } else {
                    $scope.msgPlrGr = res.data;
                }

            });
        }
    };
    // adding player group code end here
    //@@@@@@@@@@@@@@@@@@@@ CREATE PLAYER GROUP DIV ENDS HERE @@@@@@@@@@@

    // view device group
    $scope.getAllPlayerGroup = function () {
        $scope.AllGroupsTable = false;
        $rootScope.cBValuePlrGr = [];
        $scope.GroupTableVisibilityPlrGr = true;
        $scope.aNPlayerPlrGr = false;
        $scope.aNGroupPlrGr = false;
        $scope.playersTablePlrGr = false;
        $scope.assnStationPlrGr = false;
        $scope.aNStation = false;
        $scope.assnStation = false;
        $scope.stationTable = false;
        $scope.active = true;
        playerGroupService.getAllPlayerGroupData().then(function (res) {
            console.log(res);
            $scope.modelPlrGr.PlayerGroupTable = res.data;
            for (r = 0; r < res.data.length; r++) {
                var p = {};
                p.groupName = res.data[r].DisplayStationName;
                p.groupDetails = res.data[r].DisplayStationLocation;
                p.type = "Sub Group";
                $scope.allGroupData.push(p);
                //$scope.complete();
                //$rootScope.loader = false;
            }

            $rootScope.loader = false;
        });
        //window.location.reload();
    };

    $scope.vmPGroupPlrGr.myClick = function (ind, Gid, Gname, Gdescription, groupData) {
        $scope.alertText2 = false;
        $scope.chckBoxforAssignPlayerPlrGr = false;
        $rootScope.ind = ind;
        $rootScope.GroupIdd = Gid;
        $rootScope.Gname = Gname;
        $rootScope.Gdescription = Gdescription;
        console.log(groupData);
        console.log($rootScope.ind + "**********" + $rootScope.GroupIdd + "------------" + $rootScope.Gname + "****************" + $rootScope.Gdescription);
        $scope.alertText2 = document.getElementById($rootScope.GroupIdd).checked;
        console.log($scope.alertText2);

        if ($scope.alertText2) {
            $scope.chckBoxMsgForGrpPlrGr = false;
            console.log('checkbox is true');
            $scope.checkedListSub.push($rootScope.GroupIdd);
            $rootScope.cBValuePlrGr.push($rootScope.GroupIdd);
            console.log($rootScope.cBValuePlrGr);
        } else {
            $rootScope.cBValuePlrGr.splice($rootScope.cBValuePlrGr.indexOf($rootScope.GroupIdd), 1);
            console.log($rootScope.cBValuePlrGr);
        }
    };

    // Edit PlayerGroup started by Prodipta
    $scope.getTemplateForGroup = function (PlayerGroup) {
        if (PlayerGroup.GroupId === $scope.modelPlrGr.selected.GroupId) return 'editGroup';
        else return 'displayGroup';
    };
    $scope.editPlayerGroup = function (PlayerGroup) {
        $scope.modelPlrGr.selected = angular.copy(PlayerGroup);
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
        $scope.modelPlrGr.PlayerGroupTable[idx] = angular.copy($scope.modelPlrGr.selected);
        console.log($scope.modelPlrGr);
        $scope.resetPlrGr();

        //calling editPlayerGroupTable service
        playerGroupService.editPlayerGroupTable($scope.savePlayerGroupData).then(function (res) {
            console.log(res);
            $scope.res = res;
            if (res.data == 'Success') {
                $scope.editMsg = 'Device Group Details Edited Successlly';
                //Alert message timeout
                setTimeout(function () {
                    $scope.editMsg = "";
                    $scope.GroupTableVisibilityPlrGr = true;
                    $rootScope.cBValuePlrGr = [];
                    $scope.getAllSubGroupData();
                }, 2000);
            } else {
                $scope.editMsg = res.data;
            }

        });
    };

    $scope.getAllGroupPlayer = function () {
        playerGroupService.getAllPlayerGroupData().then(function (res) {
            $scope.modelPlrGr.PlayerGroupTable = res.data;
            $scope.totalItems = $scope.modelPlrGr.PlayerGroupTable.length;   //for pagination
            $scope.getPlayerGroupData = res.data;
            $scope.complete();
            $rootScope.loader = false;
        });
    }

    
    $scope.modelsPlrGr = {
        selected: null,
        lists: { "Available": [], "Selected": [] }
    };

    //assignPlayer started here

  
    $scope.assignPlayerPlrGr = function () {
        // All active class variables for ng-class //
        $scope.ActiveDefineGrouping = "";
        $scope.ActiveCreateParentGroupSub = "";
        $scope.ActiveCreateSubGroupSub = "";
        $scope.ActiveViewParentGroup = "";
        $scope.ActiveViewParentGroupSubAttach = "";
        $scope.ActiveViewParentGroupSubRemove = "";
        $scope.ActiveViewSubGroup = "";
        $scope.ActiveViewSubGroupAttach = "activeLinkSideMenu";
        $scope.ActiveViewSubGroupRemove = "";
        // variable for active class ends here 
        $scope.AllGroupsTable = false;
        $scope.chckBoxMsg = false;
        $scope.chckBoxMsgassign = false;
        $scope.GroupTableVisibilityPlrGr = true;
        $scope.aNStation = false;
        $scope.aNGroupPlrGr = false;
        $scope.assnStation = false;
        $scope.stationTable = false;
        $scope.aNGroupPlrGr = false;
        $scope.chckBoxMsgForGrpPlrGr = false;
        console.log("inside Assign Device func ", $scope.alertText1, $rootScope.cBValuePlrGr.length);
        $scope.chckBoxforAssignPlayerPlrGr = true;
        console.log($scope.alertText1);
        console.log($rootScope.cBValuePlrGr.length);
        if ($rootScope.cBValuePlrGr.length < 2 && $rootScope.cBValuePlrGr.length > 0) {
            console.log('checkbox length is less than one');
            $scope.chckBoxforAssignPlayerPlrGr = false;
            $scope.assignPlayerGroup(); // calling the assignPlayergroup()
            $scope.alertText1 = false;
        } else {
            $scope.assignMsgPlrGr = "Please Select one checkbox !";
        }
    };
    //calling getPlayersData service
    $scope.assignPlayerGroup = function () {
        console.log('asdfasdfasdf', $scope.alertText1);
        $scope.chckBoxforAssignPlayerPlrGr = true;
        $scope.editMsg = "";
        $scope.GroupTableVisibilityPlrGr = false;
        if ($scope.alertText1 == false || $rootScope.cBValuePlrGr.length > 0) {
            $scope.chckBoxforAssignPlayerPlrGr = false;
            $scope.getAssignPlayersListPlrGr(); //calling removeData() Function()        
        }
    };

    $scope.getAssignPlayersListPlrGr = function () {
       // console.log('inside assignPlayer', $rootScope.cBValuePlrGr, $rootScope.GroupIdd, $scope.getPlayerGroupData);
        var gId = $rootScope.cBValuePlrGr[0];
        console.log(gId, 'gid');

        $scope.getPlayerGroupData.forEach(function (data) {
            console.log(" printing length  "+data.length)
            console.log(data);
            if (data.GroupId == gId) {
                $scope.grpName = data.GroupName;
                console.log($scope.grpName);
            }
        });

        $scope.assnStationPlrGr = true;
        $scope.subTitle = "- Assign Device";
        //calling getAvailablePlayersData service
        playerGroupService.getPlayersData(gId).then(function (res) {
            console.log('getting all available Device data from service');
            //$scope.models.lists.Available.push(res.data);
            $scope.modelsPlrGr.lists.Available = res.data;
            // $scope.models.lists.Selected.push(res.data[0]);
        });

        //calling getAssignedPlayersData service
        playerGroupService.getAssignedPlayersData($rootScope.GroupIdd).then(function (res) {
            console.log('getting all assigned Device data from service');
            $scope.modelsPlrGr.lists.Selected = res.data;
        });
    }

    $scope.ddRestorePlrGr = function () {
        $scope.getAssignPlayersListPlrGr();
    }
    $scope.ddSavePlrGr = function () {
        //debugger;
        //Get selected players list
        var selectedLists = $scope.modelsPlrGr.lists.Selected;
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
            
            setTimeout(function () {
                $rootScope.cBValuePlrGr = [];
                $scope.getAllGroupPlayer();
                $scope.editMsg = "";
                $scope.assnStationPlrGr = false;
                $scope.GroupTableVisibilityPlrGr = true;
            }, 2000);
            //Alert message timeout
            //setTimeout(function () {

            //    window.location.reload();
            //}, 2000);
        });
    };

    $scope.rmGroupData = function () {
        // All active class variables for ng-class //
        $scope.ActiveDefineGrouping = "";
        $scope.ActiveCreateParentGroupSub = "";
        $scope.ActiveCreateSubGroupSub = "";
        $scope.ActiveViewParentGroup = "";
        $scope.ActiveViewParentGroupSubAttach = "";
        $scope.ActiveViewParentGroupSubRemove = "";
        $scope.ActiveViewSubGroup = "";
        $scope.ActiveViewSubGroupAttach = "";
        $scope.ActiveViewSubGroupRemove = "activeLinkSideMenu";
        // variable for active class ends here 

        $scope.AllGroupsTable = false;
        $scope.chckBoxMsgForGrpPlrGr = true;
        $scope.chckBoxforAssignPlayerPlrGr = false;
        console.log($scope.alertText2);
        if ($scope.alertText2 == true || $rootScope.cBValuePlrGr.length > 0) {
            //if ($scope.assnStationPlrGr == false) {
            console.log("selected somethinh")
                $scope.chckBoxMsgForGrpPlrGr = false;
                $scope.chckBoxMsgForGrp = false;
                $scope.removeGroupData(); //calling removeGroupData() Function()
                $scope.checkedListSub = [];
                $scope.alertText2 = false;
            //};
        }
        else {
            console.log("did not selected ")
            $scope.chckBoxMsgForGrpPlrGr = true;
        }




    };
    $scope.removeGroupData = function () {
        console.log('inside remove function of group', $rootScope.cBValuePlrGr);
        //calling removeStationData service
        playerGroupService.removePlayerGroupData($rootScope.cBValuePlrGr).then(function (res) {
            console.log(res);
            $scope.res = res;
            if (res.data == 'Success') {
                $scope.editMsg = 'Sub Group removed Successfully';
                //Alert message timeout
                $scope.GroupTableVisibilityPlrGr = true;
                $scope.assignMsgPlrGr = "";
                setTimeout(function () {
                    $scope.editMsg = "";
                    $scope.GroupTableVisibilityPlrGr = true;
                    $rootScope.cBValuePlrGr = [];
                    $scope.getAllSubGroupData();
                    
                }, 2000);
            } else {
                $scope.editMsg = "Error while deleting the record";
            }
        });
    };

    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ END OF CODE FOR PLAYER GROUP @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@//

   

    $scope.Station = {
        DisplayStationName: '',
        DisplayStationLocation: ''
    };

    $scope.cancelAdd = function () {
        $scope.statName = "";
        $scope.cancFun();
    };
    $scope.cancFun = function () {
        window.location.reload();
    };
    //Breadcrumb starts
    $scope.cUrl = window.location.href;
    var res = $scope.cUrl.split("/");
    var subst = res.pop();
    $rootScope.state = subst.replace(/#/g, "");
    console.log($rootScope.state);
    //Breadcrumb ends

    //Adding New Parent Group code started here
    $scope.aNewStation = function () {
        $scope.subTitle = " - Create Parent group";
        $scope.hideSubGroup = true;
        $scope.hideParentGroup = true;
        // All active class variables for ng-class //
        $scope.ActiveDefineGrouping = "";
        $scope.ActiveCreateParentGroupSub = "activeLinkSideMenu";
        $scope.ActiveCreateSubGroupSub = "";
        $scope.ActiveViewParentGroup = "";
        $scope.ActiveViewParentGroupSubAttach = "";
        $scope.ActiveViewParentGroupSubRemove = "";
        $scope.ActiveViewSubGroup = "";
        $scope.ActiveViewSubGroupAttach = "";
        $scope.ActiveViewSubGroupRemove = "";
        // variable for active class ends here 
        $scope.AllGroupsTable = false;
        $scope.assnStationPlrGr = false;
        $scope.aNStation = true;
        $scope.aNGroupPlrGr = false;
        $scope.chckBoxMsgassign = false;
        $scope.GroupTableVisibilityPlrGr = false ;
        $scope.assnStation = false;
        $scope.stationTable = false;
        $scope.alertText = false;
       // $scope.subTitle = "- Parent Group";
        $rootScope.substate = "Add Location";
        $scope.msg = "";
        $scope.Station = {};
    };

    $scope.addStationData = function () {
        console.log('welcome to locations');
        $scope.msg = "";
        console.log($scope.Station);
        //for (var i in $scope.Station) {
        //    console.log($scope.Station[i]);
        //    if ($scope.Station[i] == "") {
        //        $scope.msg = "Fields should not be empty !";
        //    }
        //}

        if ($scope.Station.DisplayStationName == "" || $scope.Station.DisplayStationName == undefined) {
            $scope.msg = "Fields should not be empty !";
        }
        if ($scope.msg != "") {
            console.log($scope.msg);
        } else {
            //calling addNewStation service
            stationService.addNewStation($scope.Station).then(function (res) {
                console.log(res.data);
                $scope.res = res;
                if (res.data == 'Success') {
                    $scope.msg = 'Parent group Added Successfully !';
                    //Alert message timeout
                    setTimeout(function () {
                        $scope.editMsg = "";
                        $scope.getAllStationsData();
                           
                        }, 2000);
                } else {
                    $scope.msg = res.data;
                }
              
            });
        }
    };
    //Adding New station code ended here

    //checkbox code started here
    $scope.vm = {};
    $rootScope.satationcBValue = [];
    $scope.vm.myClick = function (sid, ind, sname, sloc) {
        console.log("selected ");
        console.log($scope.satationcBValue);
        $rootScope.sid = sid;
        $rootScope.ind = ind;
        $rootScope.sname = sname;
        $rootScope.sloc = sloc;
        console.log($rootScope.ind + "**********" + $rootScope.sid + "----" + $rootScope.sname + "------------" + $rootScope.sloc);
        $scope.alertText = document.getElementById(ind).checked;
        console.log($scope.alertText);
        if ($scope.alertText) {
            console.log('checkbox is true');
            $scope.checkedList.push(ind);
            $rootScope.satationcBValue.push($rootScope.sid);
            $scope.chckBoxMsgassign = $scope.chckBoxMsg = false;
            console.log($rootScope.satationcBValue);
        } else {
            $rootScope.satationcBValue.splice($rootScope.satationcBValue.indexOf($rootScope.sid), 1);
            console.log($rootScope.satationcBValue);
            if ($rootScope.satationcBValue > 0) {
                $scope.alertText = true;
            }

        }
        $scope.chckBox = $scope.alertText;
    };
    //checkbox code ended here

    //remove station started here
    $scope.rmData = function () {
        // All active class variables for ng-class //
        $scope.ActiveDefineGrouping = "";
        $scope.ActiveCreateParentGroupSub = "";
        $scope.ActiveCreateSubGroupSub = "";
        $scope.ActiveViewParentGroup = "";
        $scope.ActiveViewParentGroupSubAttach = "";
        $scope.ActiveViewParentGroupSubRemove = "activeLinkSideMenu";
        $scope.ActiveViewSubGroup = "";
        $scope.ActiveViewSubGroupAttach = "";
        $scope.ActiveViewSubGroupRemove = "";
        // variable for active class ends here 
        $scope.AllGroupsTable = false;
        $scope.assnStationPlrGr = false;
        $scope.chckBoxMsgassign = false;
        console.log("&&&&&&&&&&&&&&&&&&");
        console.log($scope.satationcBValue);
        console.log($scope.alertText, $scope.chckBox, $rootScope.satationcBValue.length);
        $scope.chckBoxMsg = true;
        if ($rootScope.satationcBValue.length > 0) {
            $scope.alertText = true;
            if ($scope.alertText) {
                $scope.chckBoxMsg = false;
                $scope.removeData(); //calling removeData() Function()
                 $scope.chckBox = false;
                console.log($scope.alertText, $scope.chckBox);
            }
            else {
                $scope.chckBoxMsg = true;
            }
        }
    };
    $scope.removeData = function () {
        $scope.AllGroupsTable = false;
        $scope.assnStationPlrGr = false;
        console.log('inside remove function', $scope.alertText);
        console.log($rootScope.satationcBValue);
        console.log($rootScope.ind + "**********" + $rootScope.sid + "----" + $rootScope.sname + "------------" + $rootScope.sloc);
        $scope.alertText = false;
        //calling removeStationData service
        stationService.removeStationData($rootScope.satationcBValue).then(function (res) {
            console.log(res);
            $scope.res = res;
            if (res.data == 'Success') {
                $scope.editMsg = 'Parent group Removed Successfully !';
                //Alert message timeout
                setTimeout(function () {
                    $rootScope.satationcBValue = [];
                        //window.location.reload();
                          $scope.editMsg = "";
                          $scope.getAllStationsData();
                        
                    }, 2000);
            } else {
                $scope.editMsg = res.data;
            }
           
        });
    };
    //remove station ended here

    // refresh started
    $scope.reloadPage = function () {
        window.location.reload();
    };
    // refresh ended

    //edit table started here
    $scope.model = {
        stationTable: [],
        selected: {}
    };

    // gets the template to ng-include for a table row / item
    $scope.getTemplate = function (station) {
        if (station.DisplayStationid === $scope.model.selected.DisplayStationid) return 'edit';
        else return 'display';
    };

    $scope.editStation = function (station) {
        console.log(station,'editstation')
        $scope.model.selected = angular.copy(station);
    };

    $scope.saveStation = function (idx, sid, sname, sloc) {
        console.log(idx + "**********" + sid + "----------" + sname + "********" + sloc);
        console.log("Saving location Details");
        $scope.saveStationData = {
            "DisplayStationid": sid,
            "DisplayStationName": sname,
            "DisplayStationLocation": sloc
        };
        console.log($scope.saveStationData);
        $scope.model.stationTable[sid] = angular.copy($scope.model.selected);
        console.log($scope.model);
        $scope.reset();

        //calling editStationTable service
        stationService.editStationTable($scope.saveStationData).then(function (res) {
            console.log(res);
            $scope.res = res;
            if (res.data == 'Success') {
                $scope.editMsg = 'Parent Group Details Edited Successfully !';
                //Alert message timeout
                    setTimeout(function () {
                        window.location.reload();
                    }, 2000);
            } else {
                $scope.editMsg = res.data;
                setTimeout(function () {
                        window.location.reload();
                    }, 2000);
            }
           
        });
    };

    $scope.reset = function () {
        $scope.model.selected = {};
    };

    $scope.viewAllStations = function () {
        $scope.AllGroupsTable = false;
        $rootScope.satationcBValue = [];
        $scope.aNStation = false;
        $scope.assnStation = false;
        $scope.stationTable = true;
        $scope.chckBoxMsg = false;
        $scope.chckBoxMsgassign = false;
        $scope.activePlrGr = false;
        $scope.chckBoxPlrGr = false;
        $scope.chckBoxMsgPlrGr = false;
        $scope.active = true;
        $scope.assnStationPlrGr = false;
        stationService.getStationsData().then(function (res) {
           
            console.log('getting data from server', res)
            $scope.model.stationTable = res.data;
            $scope.totalItems = $scope.model.stationTable.length;   //for pagination
            $scope.stationData = res.data;

            $scope.complete();
            $rootScope.loader = false;

        });
    }
    //station table code starts here
    stationService.getStationsData().then(function (res) {
        console.log('getting data from server',res)
        $scope.model.stationTable = res.data;

        for (r = 0; r < res.data.length; r++) {
            var p = {};
            p.groupName = res.data[r].DisplayStationName;
            p.groupDetails = res.data[r].DisplayStationLocation;
            p.type = "Parent Group";
            $scope.allGroupData.push(p);
        }
        $scope.totalItems = $scope.model.stationTable.length;   //for pagination
        $scope.stationData = res.data;
        $rootScope.loader = false;
    });
    playerGroupService.getAllPlayerGroupData().then(function (res) {
        console.log(res);
        for (r = 0; r < res.data.length; r++) {
            var p = {};
            p.groupName = res.data[r].GroupName;
            p.groupDetails = res.data[r].GroupDescription;
            p.type = "Sub Group";
            $scope.allGroupData.push(p);
        }
        console.log('l_test');
         $scope.complete();
        $rootScope.loader = false;
    });
    //station table code ended here
    
    $scope.copyStation = function (station) {
        console.log(station, 'station');
        var copy = station;
        if (copy.DisplayStationLocation) {
            copy.DisplayStationid = 1234;
            copy.DisplayStationLocation = copy.DisplayStationLocation + '- copy';
            copy.DisplayStationName = copy.DisplayStationName + ' - copy';
        }
        console.log(copy, 'copy');
        $scope.model.stationTable.push(copy);
        console.log($scope.model.stationTable);
    };
    
    //drag and drop code starts here

    $scope.models = {
        selected: null,
        lists: { "Available": [], "Selected": [] }
    };

    //calling getPlayersData service
    $scope.assignPlayer = function () {
        // All active class variables for ng-class //
        $scope.ActiveDefineGrouping = "";
        $scope.ActiveCreateParentGroupSub = "";
        $scope.ActiveCreateSubGroupSub = "";
        $scope.ActiveViewParentGroup = "";
        $scope.ActiveViewParentGroupSubAttach = "activeLinkSideMenu";
        $scope.ActiveViewParentGroupSubRemove = "";
        $scope.ActiveViewSubGroup = "";
        $scope.ActiveViewSubGroupAttach = "";
        $scope.ActiveViewSubGroupRemove = "";
        // variable for active class ends here 
        $scope.editMsg = '';
        $scope.chckBoxMsgassign = true;
        $scope.chckBoxMsg = false;
        if ($rootScope.satationcBValue.length < 2 && $rootScope.satationcBValue.length > 0) {
            if ($scope.alertText) {
                $scope.chckBoxMsgassign = false;
                $scope.getAssignPlayersList(); //calling removeData() Function()
            }
        } else {
            $scope.assignMsg = "Select One Checkbox !"
        }
    };

    $scope.getAssignPlayersList = function () {
        $scope.AllGroupsTable = false;
        console.log('inside assignPlayer', $rootScope.satationcBValue, $rootScope.sid);
        $scope.editMsg = "";
        $scope.assnStation = true;
        $scope.aNGroupPlrGr = false;
        $scope.GroupTableVisibilityPlrGr = false;
        $scope.assnStationPlrGr = false;
        $scope.aNStation = false;
        $scope.stationTable = false;
        $scope.alertText = false;
        $scope.subTitle = "- Attach Devices/Device Group";
        $rootScope.substate = "Assign Device";

        var sId = $rootScope.satationcBValue[0];
        console.log(sId, 'sId');
        //calling getAvailablePlayersByGroup service
        stationService.getAssignedPlayersByGroup().then(function (res) {
            console.log('Inside getAssignedPlayersByGroup', res);
            $scope.stationModel.lists.Available = res.data;

        });
        $scope.stationData.forEach(function (data) {
            //console.log(data);
            if (data.DisplayStationid == sId) {
                $scope.stnName = data.DisplayStationName;
                console.log($scope.stnName);
            }
        });

        //calling getAvailablePlayersData service
        //stationService.getPlayersData(sId).then(function (res) {
        //    console.log('getting all available players data from service', res.data);
        //    //$scope.models.lists.Available.push(res.data);
        //    //$scope.models.lists.Available = res.data;
        //    // $scope.models.lists.Selected.push(res.data[0]);
        //});

        //calling getAssignedPlayersData service
        //stationService.getAssignedPlayersData(sId).then(function (res) {
        //    console.log('getting all assigned players data from service');
        //    console.log(res.data);
        //    $scope.models.lists.Selected = res.data;
        //});

        //new DragandDrop Assigned data
        stationService.getSelectedPlayersByGroup(sId).then(function (res) {
            console.log('getting all Selectedplayers data from service');
            console.log(res.data);
            $scope.stationModel.lists.Assigned = res.data;

        });
    };


    $scope.resetAssignPlayerLists = function () {
        if ($scope.models.lists.Available.length == 0) {
            $scope.models.lists.Available = [];
            $scope.models.lists.Available.push("");
        }

        if ($scope.models.lists.Selected.length == 0) {
            $scope.models.lists.Selected = [];
            $scope.models.lists.Selected.push("");
        }
    };

    // Model to JSON for demo purpose
    $scope.$watch('models', function (model) {
        $scope.modelAsJson = angular.toJson(model, true);
        console.log($scope.modelAsJson);
        $scope.resetAssignPlayerLists();
        }, true);

    //Reset button to Assign Players
    $scope.ddRestore = function restore() {
        $scope.getAssignPlayersList();
    };
    //drag and drop code ended here

    //new Dragand drop started
    $scope.stationModel = {
        selected: null,
        lists: {
            "Available": [],
            "Assigned": []
        }
    };

    //calling getAvailablePlayersByGroup service
    stationService.getAssignedPlayersByGroup().then(function (res) {
        console.log('Inside getAssignedPlayersByGroup', res);
        $scope.stationModel.lists.Available = res.data;
        
    });

    $scope.dropSuccessHandler = function ($event, index, array, id) {
        array.splice(index, 1);
        console.log(index, array, id, array.length);
        console.log($scope.tempJson, '+++++++++++++++++++', $scope.tempJson1);       
    };

    $scope.ddSave = function () {
        var test = $scope.stationModel.lists.Assigned;
        console.log(test, 'test');
        for (var i = 0; i < test.length; i++) {
            if (test[i].Indicator == true) {
                console.log(test[i].Players, 'child array');
                for (var j = 0; j < test[i].Players.length; j++) {
                    $scope.groupArray.push(test[i].Players[j].PlayerId);
                }
                console.log($scope.groupArray);
            } else {
                if (test[i].Indicator == false) {
                    $scope.groupArray.push(test[i].GroupId);
                }
            }
        }
        console.log($scope.groupArray);
        var selectedLists = $scope.groupArray;
        console.log(selectedLists);
        var length = selectedLists.length;
        //Get selected station Id
        var selectedStationId = $rootScope.sid;

        var playerIdValues = "";
        var i;
        for (i = 0; i < length; i++) {
            playerIdValues += (selectedLists[i] != "")
                ? selectedLists[i] + ((i < length - 1) ? ", " : "") : "";
        }
        console.log(playerIdValues, "-----------------", selectedStationId);
        stationService.SaveAssignPlayerStation(playerIdValues, selectedStationId).then(function (res) {
            console.log(res);
            $scope.res = res;
            if (res.data == 'sucess') {
                $scope.editMsg = 'Changes updated successfully !';
            } else {
                $scope.editMsg = 'Error In Assigning Device !';
            }
            //Alert message timeout
                setTimeout(function () {
                    window.location.reload();
                }, 2000);
        });
    };

    $scope.onDrop = function ($event, $data, array,id) {
        array.push($data);
        console.log($data, $data.GroupId, array, id);
    };

    // initiate an array to hold all active tabs
    $scope.activeTabs = [];

    // check if the tab is active
    $scope.isOpenTab = function (tab) {
        // check if this tab is already in the activeTabs array
        return $scope.activeTabs.indexOf(tab.GroupId);
    };

    // function to 'open' a tab
    $scope.openTab = function (tab) {
        if (tab.Players)
            $scope.arrw= true;
            tab.opened = !tab.opened;
        return tab.opened;
    };

    // Model to JSON for demo purpose
    $scope.$watch('stationModel', function (model) {
        $scope.modelAsJson = angular.toJson(model, true);
        //console.log(model);
    }, true);
    //new dragand drop ended

    //table sorting started
    $scope.propertyName = 'DisplayStationName';
    $scope.reverse = false;
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


//Service started here :
dsFactory.factory('stationService', function ($http, $q) {
    //here $q is an angularjs service which help us to run asynchronous function and return result when processing done.
    return {
        // Save data
        addNewStation: function (data) {
            var defer = $q.defer();
            $http({
                url: '/DisplayStation/AddStation',
                method: 'POST',
                data: { displayStation: JSON.stringify(data) },
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

        //Get all Stations started
        getStationsData: function () {
            return $http.get('/DisplayStation/GetAllStations');
        },

        //Get all Players started
        getPlayersData: function (stationID) {
            return $http.get('/DisplayStation/GetAvailablePlayers');
        },

        //Get all Assigned Players started
        getAssignedPlayersData: function (stationID) {
            return $http.get('/DisplayStation/GetAssignedPlayers', { params: { stationId: stationID } });
        },

        //Available players by Group draganddrop  
        getAssignedPlayersByGroup: function (stationID) {
            return $http.get('/DisplayStation/GetAvailablePlayersByGroup');
        },
        //selected players by Group draganddrop
        getSelectedPlayersByGroup: function (stationID) {
            return $http.get('/DisplayStation/GetAssignedPlayersByGroup', { params: { stationId: stationID } });
        },


        //tableEdit started here
        editStationTable: function (data) {
            console.log(data)
            var defer = $q.defer();
            $http({
                url: '/DisplayStation/UpdateStation',
                method: 'POST',
                data: JSON.stringify(data),
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

        //remove station started here
        removeStationData: function (stationID) {
            console.log('inside remove station');
            console.log(stationID)
            var defer = $q.defer();
            $http({
                url: '/DisplayStation/DeleteStation',
                method: 'POST',
                data: { 'stationID': JSON.stringify(stationID) },
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

        // Save Assign player data
        SaveAssignPlayerStation: function (playerIds, stationId) {
            var defer = $q.defer();
            $http({
                url: '/DisplayStation/AssignPlayer',
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
        }
    }
});

