
dsControllers.controller('settingController', function ($window, $scope, $rootScope, settingService, $q) {
    $('.activeCls').click(function (e) {
        e.preventDefault();
        $('.activeCls').removeClass('active');
        $(this).addClass('active');
    });
    $rootScope.loader = true;
    $rootScope.parentBgHome = true;
    console.log('Welcome to Admin & User');
    $scope.userTab = true;
    $scope.adminTab = false;
    $scope.errorMsg = false;
    $scope.successMsgSpan = false;
    $scope.errorMsgSpan = false;
    $scope.subTitle = "- Update User Role";
    $scope.addNewUserDiv = false;
    //Breadcrumb starts
    $scope.cUrl = window.location.href;
    var res = $scope.cUrl.split("/");
    var subst = res.pop();
    $rootScope.state = subst.replace(/#/g, "");
    console.log($rootScope.state);
    //Breadcrumb ends
    $scope.resetPassword = function (resetPass) {
        $scope.errMsg = "";
        if ((resetPass.UserName == "") || (resetPass.UserName == undefined) || (resetPass.Email == "") || (resetPass.Email == undefined)) {
            $scope.errMsg = "Fields should not be empty.";
        }
        else {
            
            var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
            var validate = re.test(String(resetPass.Email).toLowerCase());
            if (validate == false) {
                $scope.errMsg = "Enter proper EmailID!!";
                $scope.showErrMsg = "";
            }
            else {
                settingService.resetUserPassword(resetPass).then(function (response) {
                    if (response.data == 'Success') {
                        $scope.showErrMsg = 'Success';
                        $scope.errMsg = "Password reset for the user " + resetPass.UserName + " is successful.";
                        setTimeout(function(){
                            window.location.reload();
                        }, 2000);
                    }
                    else {
                        $scope.errMsg = response.data;
                    }

                })
            }
        }
        
    }
    $scope.addNewUser = function () {
        $scope.subTitle = "Add User";
        $scope.successMsgSpan = false;
        $scope.errorMsgSpan = false;
        $scope.userTab = false;
        $scope.adminTab = false;
        $scope.addNewUserDiv = true;
       
    }
    $scope.submitNewUser = function (newUserData) {
        $scope.errorMsg = "";
        $scope.errorMsgSpan = true;
        $scope.successMsgSpan = true;
        console.log(newUserData);
        if (newUserData == null || newUserData == undefined) {
            $scope.errorMsg = "Fields should not be empty!!";
            $scope.errorMsgSpan = true;
        }
        for (var i in newUserData) {
            if (newUserData[i] == "" || newUserData[i] == undefined) {
                $scope.errorMsg = "Fields should not be empty!!";
                $scope.errorMsgSpan = true;
            }
        }
        
        if ($scope.errorMsg == "") {
            if (newUserData.Password != newUserData.confirmPassword) {
                $scope.errorMsg = "Password does not match";
                $scope.errorMsgSpan = true;
            }
            else if (newUserData.email == "" || newUserData.email == undefined) {
                var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
                var validate = re.test(String(newUserData.email).toLowerCase());
                $scope.errorMsg = "Enter proper EmailID!!";
                $scope.errorMsgSpan = true;
            }
            else {
                $scope.newUserData = newUserData;
                console.log(newUserData);
                settingService.saveNewUserData($scope.newUserData).then(function (response) {
                    console.log(response.data);
                    if (response.data == "User registered successfully.") {
                        $scope.successMsgSpan = true;
                        $scope.successMsg = "User registered successfully !"
                        setTimeout(function () {
                            window.location.reload();
                        }, 1000);
                    }
                    else {
                        $scope.errorMsg = response.data;
                        $scope.errorMsgSpan = true;
                    }
                });
            }
        }
        
        }

    $scope.reportPage = function () {
        $window.location.href = "/Home/Report";
    }
    $scope.dashboardPage = function () {
        $window.location.href = "/Home/Dashboard";
    }
    $scope.reportChange = function () {
        $scope.subTitle = " - Report";
    }
    $scope.user = function () {
        $scope.subTitle = " - Users";
        $scope.addNewUserDiv = false;
        $scope.userTab = true;
        $scope.adminTab = false;
        $scope.successMsgSpan = false;
        $scope.errorMsgSpan = false;
        $scope.subTitle = "- Update User Role";
        $rootScope.substate = "Users";
        $scope.propertyName = 'UserName';
    };

    $scope.admin = function () {
        $scope.addNewUserDiv = false;
        $scope.userTab = false;
        $scope.adminTab = true;
        $scope.successMsgSpan = false;
        $scope.errorMsgSpan = false;
        $scope.subTitle = "- Admin";
        $rootScope.substate = "Admin";
        $scope.propertyName = 'SceneName';
    };

    //Admin code starts here
    //calling getAllData()service 
    $scope.getAllAdminData = function ()    {
        settingService.getAllData().then(function (res) {
            $scope.pendApproval = res.data;
            console.log($scope.pendApproval);
            $scope.totalItems = $scope.pendApproval.length;   //for pagination
            console.log($scope.pendApproval, 'Admin table length', $scope.totalItems);
            $rootScope.loader = false;
        });
    };

    //Template view started
    $scope.templateFunction = function (TemplatesceUrl, sceType, weatherIcPos) {
        console.log(TemplatesceUrl);     

        if (sceType != 'VIDEOURL') {
            if (sceType != 'WEBURL') {
                if (sceType != 'VIDEO') {
                    document.getElementById('dy_template').innerHTML = "";
                    $scope.TemplatesceUrl = "";
                    $scope.showcompModal = !$scope.showcompModal;
                }
            }
        };
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

        $scope.TemplatesceUrl = TemplatesceUrl;
        document.getElementById('dy_template').innerHTML = '<div class=' + $scope.weatherIconModal + '><img   src="../Images/weather.png"/></div><img src=' + $scope.TemplatesceUrl + ' width="100%">'

        if (sceType == "WEBURL" || sceType == 'VIDEOURL' || sceType == 'VIDEO') {
            window.open($scope.TemplatesceUrl, "", "width=800,height=500");
        };
    };

    //approval function starts here
    $scope.approveFun = function (aid, aSceneName, aMan, aStatus) {
        $scope.successMsgSpan = false;
        $scope.errorMsgSpan = false;
        $scope.addNewUserDiv = false;
        console.log('inside approval function');
        console.log(aid + "----------" + aSceneName + "**********" + aMan + "&&&&&&&&&&" + aStatus);
        $scope.updateData = {
            'SceneId': aid,
            'Status': 'APR'
        };
        console.log($scope.updateData);
        //calling updateStatus service
        settingService.updateStatus($scope.updateData).then(function (res) {
            console.log(res);
            $scope.res = res;
            if (res.data == 'Success') {
                $scope.adminMsg = 'Template Approved Successfully';
            } else {
                $scope.adminMsg = 'Error In Approving Template';
            }
            //Alert message timeout
            setTimeout(function () {
                $scope.adminMsg = '';
                $scope.getAllAdminData() //calling getAllAdminData func
            }, 2000);
        });
    };

    //rejection function starts here
    $scope.rejectFun = function (rid, rSceneName, rMan, rStatus) {
        $scope.successMsgSpan = false;
        $scope.errorMsgSpan = false;
        $scope.addNewUserDiv = false;
        console.log('inside reject function');
        console.log(rid + "----------" + rSceneName + "**********" + rMan + "&&&&&&&&&&" + rStatus);
        $scope.updateData = {
            'SceneId': rid,
            'Status': 'REJ'
        };
        console.log($scope.updateData);
        //calling updateStatus service
        settingService.updateStatus($scope.updateData).then(function (res) {
            console.log(res);
            $scope.res = res;
            if (res.data == 'Success') {
                $scope.adminMsg = 'Template Rejected Successfully';
            } else {
                $scope.adminMsg = 'Error In Rejecting Template';
            }
            //Alert message timeout
            setTimeout(function () {
                $scope.adminMsg = '';
                $scope.getAllAdminData() //calling getAllAdminData func
             }, 2000);
        });
    };

    //reload started here
    $scope.reloadPage = function () {
        window.location.reload();
    };
    // reload ended here

    //table sorting started
    $scope.propertyName = 'UserName';
    $scope.reverse = false;
    $scope.addNewUserDiv = false;
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
    //Admin code ends here

    //User code starts here
    //calling getuseData service
    settingService.getuserData().then(function (res) {
        console.log(res);
        $scope.userData = res.data;
        $scope.totalItems = $scope.userData.length;   //for pagination
    });

    //calling Save user function
    $scope.Save = function (idvalue, userRole) {
        $scope.successMsgSpan = false;
        $scope.errorMsgSpan = false;
        if (idvalue == "" || idvalue == null || idvalue == undefined || userRole == "" || userRole == null || userRole == undefined) {
            $scope.msg = "fields should not be empty !!";
        }
        else {
            $scope.userSaveData = {
                userId: idvalue,
                roleId: userRole.RoleId
            };
            settingService.saveuserData($scope.userSaveData).then(function (res) {
                console.log(res);
                $scope.res = res;
                if (res.data == 'Success') {
                    $scope.msg = 'User Details Updated Successfully';
                } else {
                    $scope.msg = 'Error In Updating User Details';
                }
                //Alert message timeout
                setTimeout(function () {
                    window.location.reload();
                }, 2000);
            });
        }
        
    }
    //User code ends hee
}); //controller ended

//service code starts here
dsFactory.factory('settingService', function ($http, $q) {
    //here $q is an angularjs service which help us to run asynchronous function and return result when processing done.
    return {
        //getting All pending-Approval data
        getAllData: function () {
            //console.log('inside the getservice');
            return $http.get('/Admin/GetAllPendingApprovals');
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
        },
        // User Save data
        saveuserData: function (data) {
            console.log(data);
            var defer = $q.defer();
            $http({
                url: '/User/UpdateUserRole',
                method: 'POST',
                data: data,
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
        saveNewUserData: function (data) {
            var defer = $q.defer();
            $http({
                url: '/User/SaveUser',
                method: 'POST',
                data: { newUser: JSON.stringify(data) },
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
        //GEt AllUser Data
        getuserData: function () {
            return $http.get('/User/GetAllUserWithRoles');
        },
        resetUserPassword: function (resetData) {
            console.log("&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&")
            console.log(resetData);
            var defer = $q.defer();
            $http({
                url: '/User/ResetUserPassword',
                method: 'POST',
                data: { 'resetData': JSON.stringify(resetData) },
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


