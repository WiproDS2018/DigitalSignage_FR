
dsControllers.controller('PlayerController', function (entityServiceImg,$scope, $rootScope, playerService, $q, cfpLoadingBar) {
    $('.activeCls').click(function (e) {
        e.preventDefault();
        $('.activeCls').removeClass('active');
        $(this).addClass('active');
    });
    $scope.HomeErrMsg = "";
    $scope.ShowContentDiv = false;
    $scope.getBannerImage = function () {
        playerService.getBannerImageSer().then(function (response) {
            console.log(response);
            if (response.data.ImageUrl == null) {
                $scope.BannerImageURL = "/Images/v2.png";
                $scope.ShowContentDiv = true;
            }
            else {
                $scope.imgObj = {};
                $scope.BannerImageURL = response.data.ImageUrl;
               
                $scope.imgObj.opacity = parseFloat(response.data.Opacity);
                $scope.ShowContentDiv = false; 

            }


        })
    }
    $scope.resetHomeImage = function () {
        playerService.deleteBannerImage().then(function (response) {
            if (response.data == 'Success') {
                $scope.message = "Banner Image reset to default.";
                setTimeout(function () {
                    window.location.reload();
                }, 2000);
            }
            else {
                $scope.message = "Sorry ! Could not save the changes.";
                setTimeout(function () {
                    window.location.reload();
                }, 2000);
            }
        })
    }

    $scope.myFunction = function () {
        var popup = document.getElementById("myPopup");
        popup.classList.toggle("show");
    }
    $scope.Opacity = function () {
        var popup = document.getElementById("myPopup");
        popup.classList.toggle("show");
    }
    $scope.clickedItem1 = function (file) {
        var popup = document.getElementById("myPopup");
        popup.classList.toggle("show");
        document.getElementById('contextMenuId').addEventListener('change', readURL, true);
        function readURL() {
            var file = document.getElementById('contextMenuId').files[0];
            if (file != undefined) {
                var reader = new FileReader();
                reader.onloadend = function (e) {

                    $('#HomeBGImage').attr('src', e.target.result);
                }
                if (file) {
                    reader.readAsDataURL(file);
                }

            }
        }
    }
    $scope.SaveHomePageImg = function (img) {
        console.log(img);
        img.ImageName = img.Attachment.name;
        img.opacity = 1;
        entityServiceImg.saveTutorial(img).then(function (response) {
            if (response.data == 'Success') {
                setTimeout(function () {
                    window.location.reload();
                },2000)
                
            }
            else {
                $scope.HomeErrMsg = "Could not save the changes";
            }
        });

    }
    $scope.password = {};
   
    $scope.resetPassData = function () {
        $scope.password = {};
        $scope.errMsg = "";
    }
    $scope.updatePassword = function (password) {
        $scope.errMsg = "";
        console.log(password);
        //alert(password.User);
        if (password.newPassword == password.confirmPassword) {
            playerService.changePasswordMeth(password).then(function (response) {
                console.log(response);
                if (response.data == 'Success') {
                    $scope.showErrMsg = response.data;
                    $scope.errMsg = "Password changed successfully";
                    setTimeout(function () {
                        window.location.reload();
                    },2000);
                }
                else {
                    $scope.errMsg = response.data;
                }
            })
        }
        else {
            $scope.errMsg = "Password mismatch"
        }
    }
    $rootScope.parentBgHome = true;
    console.log("welcome to device");
    $scope.active = true;
    $scope.chckBox = false;
    $scope.aNPlayer = false;
    $scope.chckBoxMsg = false;
    $rootScope.loader = true;
    $rootScope.cBValue = [];
    //$scope.aNGroup = false;
    $scope.activeElement = "";
    $scope.subTitle = " - View Devices";
    $scope.playersTable = true;
    //$scope.GroupTableVisibility = false;
    $scope.PlayerJoinGroup = {
        PlayerId: '',
        PlayerName: '',
        PlayerSerialNo: '',
        GroupId: '',
        GroupName: ''
    };
    $scope.Player = {
        Playername: '',
        PlayerSerialNo: ''
    };
    $scope.start = function () {
        cfpLoadingBar.start();
    };
    $scope.start();
    $scope.complete = function () {
        cfpLoadingBar.complete();
    }
    $scope.viewPlayers = function () {
        // active class elements
        $scope.subTitle = " - View Devices";
        $scope.ActiveAddDevice = "";
        $scope.ActiveRemoveDevice = "";
        $scope.ActiveViewPlayers = "activeLinkSideMenu";
        $scope.ActiveReloadPage = "";
        // end of actve class elements
        $scope.playersTable = true;
        $scope.aNPlayer = false;
        $scope.chckBoxMsg = false;
        $scope.editMsg = "";
        $scope.delMsg = "";
        playerService.getAllPlayerJoinGroup().then(function (res) {
            console.log('getAllPlayerJoinGroup', res);
            $scope.model.PlayerjoinGroupList = res.data;
            $scope.totalItems = $scope.model.PlayerjoinGroupList.length;   //for pagination
            $scope.complete();
            $rootScope.loader = false;
        });
    }

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
    $scope.vm.myClick = function (ind, pid, psno, pname, Gid, Gname) {
        $rootScope.ind = ind;
        $rootScope.pid = pid;
        $rootScope.psno = psno;
        $rootScope.pname = pname;
        $rootScope.Gname = Gname;
        $rootScope.Gid = Gid
        console.log($rootScope.ind + "**********" + $rootScope.pid + "------------" + $rootScope.psno + "****************" + $rootScope.pname + "gdwuhiwhiwh" + $rootScope.Gid + "   name  :-" + $rootScope.GName);

        $scope.alertText = document.getElementById(ind).checked;
        console.log($scope.alertText);
        $scope.chckBox = $scope.alertText;
        if ($scope.alertText) {
            console.log('checkbox is true');
            $rootScope.cBValue.push($rootScope.pid);
            console.log($rootScope.cBValue);
        } else {
            $rootScope.cBValue.splice($rootScope.cBValue.indexOf($rootScope.pid), 1);
            console.log($rootScope.cBValue);
        }
    };
    //checkbox code ended here

    $scope.cancFun = function () {
        window.location.reload();
    };

    //remove player started here
    $scope.rmData = function () {
        
        // active class elements
        $scope.ActiveAddDevice = "";
        $scope.ActiveRemoveDevice = "activeLinkSideMenu";
        $scope.ActiveViewPlayers = "";
        $scope.ActiveReloadPage = "";
        // end of actve class elements
        $scope.msg = "";
        //$scope.chckBoxMsg = true;
        if ($scope.alertText || $rootScope.cBValue.length > 0) {
            $scope.subTitle = " - Remove Device";
            $scope.chckBoxMsg = false;
            $scope.removeData(); //calling removeData() Function()
        }
        else {
            $scope.chckBoxMsg = true;
        }
    };
    $scope.removeData = function () {
        console.log('inside remove function');
        console.log($rootScope.cBValue);
        console.log($rootScope.ind + "**********" + $rootScope.pid + "------------" + $rootScope.psno + "****************" + $rootScope.pname);
        //calling removeStationData service
        playerService.removePlayerData($rootScope.cBValue).then(function (res) {
            console.log(res);
            $scope.res = res.data;
            console.log(res);
            if (res.data == 'Success') {
                $scope.editMsg = 'Device removed Successfully';
               // Alert message timeout
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
    //remove player ended here

    //reload started here
    $scope.reloadPage = function () {
        // active class elements
        $scope.ActiveAddDevice = "";
        $scope.ActiveRemoveDevice = "";
        $scope.ActiveViewPlayers = "";
        $scope.ActiveReloadPage = "activeLinkSideMenu";
        // end of actve class elements
        window.location.reload();
    };
    // reload ended here

    //adding New Player started here
    $scope.newPlayer = function () {
        $scope.subTitle = " - Add Device";
        // active class elements
        $scope.ActiveAddDevice = "activeLinkSideMenu";
        $scope.ActiveRemoveDevice = "";
        $scope.ActiveViewPlayers = "";
        $scope.ActiveReloadPage = "";
        // end of actve class elements
        $scope.aNPlayer = true;
        $scope.aNGroup = false;
        $scope.playersTable = false;
        $scope.GroupTableVisibility = false;
        $scope.chckBoxMsg = false;
        $rootScope.cBValue = [];
        $scope.subTitle = "- Add Device"
        $rootScope.substate = "Add Device";
    };

    $scope.addPlayer = function () {
        console.log($scope.Player);
        $scope.msg = "";
        //Form Validation
        for (var i in $scope.Player) {
            //console.log($scope.Player[i]);
            if ($scope.Player[i] == "") {
                $scope.msg = "Fields should not be empty !";
            }
        }
        if ($scope.msg != "") {
            console.log($scope.msg);
        } else {
            //calling SaveFormData service
            playerService.SaveFormData($scope.Player).then(function (res) {
                console.log('inside controller');
                console.log(res);
                $scope.res = res;
                if (res.data == 'Success') {
                    //Alert message timeout
                    //document.getElementById("alertMsg").className = "alrtTxtSuccess";
                    $scope.msg = 'Device Added Successfully';
                        setTimeout(function () {
                            window.location.reload();
                           
                        }, 2000);
                }                 
                else {
                    
                    $scope.msg = res.data;
                }
            });
        }
    };
    //adding New player ended here    
    //edit playerTable code starts here
    $scope.model = {
        playerTable: [],
        selected: {},
        PlayerGroupTable: [],
        PlayerjoinGroupList: []
    };

    // gets the template to ng-include for a table row / item
    $scope.getTemplate = function (PlayerJoinGroup) {                                                 //(PlayerJoinGroup) replace by player by prodipta
        if (PlayerJoinGroup.PlayerId === $scope.model.selected.PlayerId) return 'edit';
        else return 'display';
    };

    $scope.editplayer = function (player) {
        console.log(player);
        $scope.model.selected = angular.copy(player);
    };

    $scope.saveplayer = function (idx, sid, sname, sloc, Gid) {
        console.log(idx + "**********" + sid + "----------" + sname + "********" + sloc + "    GId:- " + Gid);
        console.log("Saving player Details");

        $scope.saveplayerData = {
            "PlayerId": sid,
            "PlayerName": sname,
            "PlayerSerialNo": sloc,
            "GroupId": Gid                                                // Gid added by Prodipta
        };
        console.log($scope.saveplayerData, "save device");
        $scope.model.playerTable[idx] = angular.copy($scope.model.selected);
        console.log($scope.model);
        $scope.reset();

        //calling editPlayerTable service
        playerService.editPlayerTable($scope.saveplayerData).then(function (res) {
            console.log(res);
            $scope.res = res.data;
            if (res.data == 'Success') {
                //document.getElementById("Messag").className = "alrtTxtSuccess";
                $scope.editMsg = 'Device details updated successfully';                    
            } else {
                $scope.editMsg = res.data;
            }
            //Alert message timeout
            setTimeout(function () {
                window.location.reload();
            }, 2000);
        });
    };

    $scope.reset = function () {
        $scope.model.selected = {};
    };
    //edit playerTable code ends here

    //getting players data started
    //playerService.getAllPlayerData().then(function (res) {
    //    $scope.model.playerTable = res.data;
    //    console.log(res);
    //    $scope.complete();
    //    $rootScope.loader = false;
    //});
    ////getting player data ended
    //playerService.getAllPlayerGroupData().then(function (res) {
    //    $scope.model.PlayerGroupTable = res.data;
    //    console.log(res);
    //    $scope.complete();
    //    $rootScope.loader = false;
         
    //});
    
    playerService.getAllPlayerJoinGroup().then(function (res) {

        
        console.log('getAllPlayerJoinGroup',res);
        $scope.model.PlayerjoinGroupList = res.data;
        $scope.totalItems = $scope.model.PlayerjoinGroupList.length;
        //for pagination
        $scope.complete();
        $rootScope.loader = false;
    });

    // function to get all Playergroup
    $scope.getAllPlayerGroup = function () {
        $scope.GroupTableVisibility = true;
        $scope.aNPlayer = false;
        $scope.aNGroup = false;
        $scope.playersTable = false;
        playerService.getAllPlayerGroupData().then(function (res) {
            console.log(res);
            $scope.model.PlayerGroupTable = res.data;
            $rootScope.loader = false;
        })
    };
 
    $scope.reset = function () {
        $scope.model.selected = {};
    };
    // Edit PlayerGroup ended by Prodipta
    
    //table sorting started
    $scope.propertyName = 'PlayerName';
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

});


//service code starts here
dsFactory.factory('playerService', function ($http, $q, $rootScope) {
    //here $q is an angularjs service which help us to run asynchronous function and return result when processing done.
    return {
        //save formData
        SaveFormData: function (data) {
            var defer = $q.defer();
            $http({
                url: '/Player/SavePlayer',
                method: 'POST',
                data: { vmplayer: JSON.stringify(data) },
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
        //        
        //getting All Players data
        getAllPlayerData: function () {
            return $http.get('/Player/GetAllPlayers');
        },
        //getting all Players along with Group .joining is there.
        getAllPlayerJoinGroup: function () {
            return $http.get('/Player/GetPlayerJoinGroup');
        },
        //tableEdit started here
        editPlayerTable: function (data) {
            console.log(data)
            var defer = $q.defer();
            $http({
                url: '/Player/EditPlayer',
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

        //remove station started here
        removePlayerData: function (playerID) {
            console.log('inside remove station');
            console.log(playerID)
            var defer = $q.defer();
            $http({
                url: '/Player/DeletePlayer',
                method: 'POST',
                data: { 'playerID': JSON.stringify(playerID) },
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


        deleteBannerImage: function () {
            return $http.get('/Home/DeleteHomeImage');
        },
        /////////////////////////////////////////////////////////////Group Services
        //  getAllPlayerGroupData:
        getAllPlayerGroupData: function () {
            return $http.get('/Player/getAllGroupPlayer');
        },
        getBannerImageSer: function () {
            return $http.get('/Home/getBannerImageSer');
        }, saveBannerImage: function (img) {
            var defer = $q.defer();
            $http({
                url: '/Home/SaveImage',
                method: 'POST',
                data: { 'image': img },
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
        changePasswordMeth: function (pass) {
            var defer = $q.defer();
            $http({
                url: '/User/changePassword',
                method: 'POST',
                data: { 'oldPass': pass.oldPassword, 'newPass': pass.newPassword },
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

        //remove Group
        //removePlayerGroupData: function (GroupId) {
        //    console.log('inside remove station');
        //    console.log(GroupId)
        //    var defer = $q.defer();
        //    $http({
        //        url: '/Player/DeletePlayerGroup',
        //        method: 'POST',
        //        data: { 'GroupId': GroupId },
        //        headers: { 'content-type': 'application/json' }
        //    }).then(function successCallback(response) {
        //        console.log(response);
        //        defer.resolve(response);
        //        $rootScope.loader = false;
        //    }, function errorCallback(err) {
        //        console.log(err);
        //        defer.reject(err);
        //    });
        //    return defer.promise;
        //},
    }
});
dsFactory.factory("entityServiceImg",
    ["akFileUploaderService", function (akFileUploaderService) {
        var saveTutorial = function (tutorial) {
            return akFileUploaderService.saveModel(tutorial, "/Home/SaveImage");
        };
        return {
            saveTutorial: saveTutorial
        };
    }]);


