
dsControllers.controller('ScenesController', function (settingService, $scope, $rootScope, $filter, $http, $q, $location, sceneService, entityService, DTOptionsBuilder, DTColumnBuilder, $compile, $sce, cfpLoadingBar) {
    $('.activeCls').click(function (e) {
        e.preventDefault();
        $('.activeCls').removeClass('active');
        $(this).addClass('active');
    });
    $scope.vm = {};

    $rootScope.parentBgHome = true;
    $rootScope.loader = true;
    $scope.vm.dtInstance = {};
    $scope.vm.dtOptions = { paging: false };
    $scope.vm.dtOptions = DTOptionsBuilder.newOptions()
        .withOption('order', [0, 'asc']);

    $scope.userRole = $('#myHiddenValue1').val();
    if ($scope.userRole == "Admin") {

        $scope.authUser = true;
    }
    else {
        $scope.authUser = false;
    }
    $scope.start = function () {
        cfpLoadingBar.start();
    };
    $scope.start();
    $scope.complete = function () {
        cfpLoadingBar.complete();
    }
    $scope.active = true;
    $scope.upload = false;
    $scope.template = false;
    $scope.temp5 = false;
    $scope.temp6 = false;
    $scope.mainTemp = false;
    $scope.animationTemp = false;
    $scope.chckBoxMsg = false;
    $scope.customTemplate = false;
    $scope.trackingScenes = false;
    $rootScope.cBValue = [];

    $scope.save = false;
    //Breadcrumb starts
    $scope.cUrl = window.location.href;
    var res = $scope.cUrl.split("/");
    var subst = res.pop();
    $rootScope.state = subst.replace(/#/g, "");
    console.log($rootScope.state);
    //Breadcrumb ends

    $scope.browserURL = $location.protocol() + '://' + window.location.host;
    console.log($scope.browserURL, '$scope.browserURL');


    $scope.admin = function () {
        $scope.userTab = false;
        $scope.active = false;
        $scope.upload = false;
        $scope.chckBoxMsg = false;
        $scope.customTemplate = false;
        $scope.template = false;
        $scope.trackingScenes = false;
        $scope.adminTab = true;
        $scope.subTitle = "- Approvals";
        $rootScope.substate = "Admin";
        $scope.propertyName = 'SceneName';

        $scope.getAllAdminData();
    };

    //Admin code starts here
    //calling getAllData()service 
    $scope.getAllAdminData = function () {
        $rootScope.loader = true;
        settingService.getAllData().then(function (res) {
            $scope.pendApproval = res.data;
            $scope.totalItems = $scope.pendApproval.length;   //for pagination
            console.log($scope.pendApproval, 'Admin table length', $scope.totalItems);
            $scope.allItems = $scope.pendApproval;

            $scope.complete();
            $rootScope.loader = false;
        });
    };
    $scope.search = function (searchText) {
        console.log("**************")
        console.log($scope.allItems);
        $scope.searchText = searchText;
        console.log($scope.searchText);
        $scope.filteredList = sceneService.searched($scope.allItems, $scope.searchText);
        if ($scope.searchText == '') {
            $scope.filteredList = $scope.allItems;
        }
        $scope.pagination();
    }
    $scope.pagination = function () {
        $scope.ItemsByPage = sceneService.paged($scope.filteredList, $scope.itemsPerPage);
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


    //Get scenes started
    $scope.scenesData = [];
    $scope.getScenesData = function () {
        console.log("I was called !!");
        //code for pushing data from Template in the UI to back end
        console.log("I was called to get scenes data");

        function handleError(response, data) {
            if (!angular.isObject(response.data) || !response.data.message) {
                return ($q.reject("An unknown error occurred."));
            }
            return ($q.reject(response.data.message));

        }
        $rootScope.loader = true;
        function handleSuccess(response) {
            console.log(response);
            $scope.ApproverName = [];
            $scope.ApproverId = [];
            for (var i = 0; i < response.data.length; i++) {
                $scope.ApproverName = [];
                $scope.ApproverId = [];
                for (var j = 0; j < response.data[i].ApproverList.length; j++) {
                    $scope.ApproverName.push(response.data[i].ApproverList[j].UserName);
                    $scope.ApproverId.push(response.data[i].ApproverList[j].UserId);
                }
                response.data[i].ApproverName = $scope.ApproverName;
                response.data[i].ApproverId = $scope.ApproverId;
                //console.log($scope.ApproverId, $scope.ApproverName);
            }
            $scope.scenesData = response.data;

            console.log($scope.scenesData, 'response');
            $scope.totalItems = $scope.scenesData.length;   //for pagination

            $scope.complete();
            $rootScope.loader = false;
            return (response.data);


        }
        var upl = $http({
            method: 'GET',
            url: '/Scene/GetAllScenes', // /api/upload
            headers: { 'Content-Type': 'application/json' }
        })
        return upl.then(handleSuccess, handleError);
    }
    $scope.getScenesData(); //calling getscene function

    $scope.status = false;
    $scope.status1 = false;
    $scope.htmlContent = "";

    //preview code starts here
    $scope.generateTemplate = function (modalContent, sceType, sceUrl, weatherIcPos) {
        //alert(weatherIcPos);
        //generate Template code
        console.log(modalContent + "   " + sceType + "   " + sceUrl);
        if (sceType != 'VIDEOURL') {
            if (sceType != 'WEBURL') {
                if (sceType != 'VIDEO') {
                    document.getElementById('dy_template').innerHTML = "";
                    $scope.TemplatesceUrl = "";
                    $scope.showcompModal = !$scope.showcompModal;
                }
            }
        };
        /////Change Aug20
        $scope.TemplatesceUrl = sceUrl;
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
        document.getElementById('dy_template').innerHTML = '<div class=' + $scope.weatherIconModal + '><img   src="../Images/weather.png"/></div><img src=' + $scope.TemplatesceUrl + ' class="previewTemplate" width="100%">'
        //if (sceType != "IMAGE-UPLOAD") {
        //    if (sceType != "IMAGE-TEMPLATE") {
        //        document.getElementById("dy_template").className = "textStyle";
        //        document.getElementById('dy_template').innerHTML = 'Preview Unavailable';
        //    }
        //}
        //if (sceType == 'VIDEO') {
        //    console.log(sceType, 'sceType ');
        //    document.getElementById('dy_template').innerHTML = '<iframe src=' + $scope.TemplatesceUrl + ' height ="100%">' + '</iframe>'
        //    //<iframe width="420" height="345" src="https://digitalsignagestore.blob.core.windows.net/signageimagecontainer/video"></iframe>
        //}
        if (sceType == "WEBURL" || sceType == 'VIDEOURL' || sceType == 'VIDEO') {
            //document.getElementById('dy_template').innerHTML = '<img src=' + $scope.TemplatesceUrl + '  width="100%">'
            console.log('window.open')
            window.open($scope.TemplatesceUrl, "", "width=800,height=500");
        };
    };

    $scope.generateTemplate1 = function (arg) {
        console.log(arg + 'inside generateTemplate function');
        $scope.showcompModal = !$scope.showcompModal;
        document.getElementById('dy_template').innerHTML = arg;
    };

    $scope.sceneName = []
    $scope.saveSceneData = {
        "SceneId": "",
        "SceneName": "Sene1",
        "sceneContent": "<h1>HTML CONTENT HERE FOR VIEWING</h1>"
    };

    //switching from upload, scene
    $scope.uFun = function () {
        $scope.uploadMsg = "";
        $scope.chckBoxMsg = false;
        $scope.active = false;
        $scope.template = false;
        $scope.adminTab = false;
        $scope.upload = true;
        $scope.customTemplate = false;
        $scope.trackingScenes = false;
        $scope.subTitle = "- Upload";
        $rootScope.ChildOfSubstate = "Upload";
        $scope.tutorial = {};
        document.getElementById('attachment').value = '';
        console.log($scope.active, $scope.upload);
    };
    $scope.temFun = function () {
        $scope.uploadMsg = "";
        $scope.chckBoxMsg = false;
        console.log('template');
        $scope.mainTemp = true;
        $scope.adminTab = false;
        $scope.active = false;
        $scope.upload = false;
        $scope.template = true;
        $scope.subTemplate = false;
        $scope.customTemplate = false;
        $scope.trackingScenes = false;
        $scope.subTitle = "- Predefined Template";
        $rootScope.ChildOfSubstate = "Custom Template";
    };
    $scope.cusTempFun = function () {
        $scope.chckBoxMsg = false;
        console.log('Inside Custom Template');
        $scope.customTemplate = true;
        $scope.template = false;
        $scope.adminTab = false;
        $scope.upload = false;
        $scope.active = false;
        $scope.trackingScenes = false;
        $scope.subTitle = "- Custom Template";
    };
    $scope.newScene = function () {
        $scope.uploadMsg = "";
        $scope.chckBoxMsg = false;
        console.log('Adding Content');
        $scope.active = true;
        $scope.upload = false;
        $scope.adminTab = false;
        $scope.customTemplate = false;
        $scope.template = false;
        $scope.trackingScenes = false;
        $scope.uploadTitle = " ";
        $scope.subTitle = "- Add Content";
    };
    $scope.viewScenes = function () {
        $scope.chckBoxMsg = false;
        $scope.active = true;
        $scope.upload = false;
        $scope.adminTab = false;
        $scope.customTemplate = false;
        $scope.template = false;
        $scope.trackingScenes = false;
        $scope.subTitle = "- Submit Content";
    };
    //checkbox function starts
    $scope.sceneFun = function (sid, sun, ind) {
        $scope.chckBoxMsg = false;
        console.log(sid + "***************", sun, ind);
        $scope.cboxVal = "cbox-" + ind;
        $scope.alertText = document.getElementById($scope.cboxVal).checked;
        console.log($scope.alertText);
        $scope.chckBox = $scope.alertText;

        $scope.selectedval = "sout-" + ind;
        $scope.selectedVal1 = (document.getElementById($scope.selectedval).value).split(':')[1];
        console.log($scope.selectedVal1);
        $scope.dataCheck($scope.selectedVal1, sun); //calling datacheck function
        $rootScope.sid = sid;
        //$rootScope.suid = sun[ind].UserId;
        console.log($rootScope.sid + "-------" + $rootScope.suid);
    };

    //datacheck function started
    $scope.dataCheck = function (uname, udata) {
        console.log('inside data check');
        console.log(uname, udata);
        udata.forEach(function (data) {
            console.log(data);
            if (data.UserName == uname) {
                console.log(data.UserId);
                $rootScope.suid = data.UserId;
            }
        });
    };

    //submit function starts
    $scope.scenesApproval = function (sceneId, userId) {
        console.log("scenes got approved");
        $scope.remdata = {
            'sceneid': sceneId,
            'userid': userId
        };
        console.log($scope.remdata);
        //Validation
        $scope.msg = "";
        for (var i in $scope.remdata) {
            console.log($scope.remdata[i]);
            if ($scope.remdata[i] == "" || $scope.remdata[i] == undefined) {
                $scope.msg = "Select Content Manager !";
            }
        }
        if ($scope.msg != "") {
            console.log($scope.msg);
        } else {
            //submit savedservice service call started
            sceneService.submitSavedService($scope.remdata).then(function (res) {
                console.log('getting data from server');
                console.log(res);
                $scope.res = res;
                if (res.data == 'Success') {
                    $scope.msg = 'Template Submitted Successfully !';
                } else {
                    $scope.msg = 'Error In Submitting Template !';
                }
                //Alert message timeout
                setTimeout(function () {
                    $scope.msg = '';
                    $scope.getScenesData();
                }, 2000);
            });
        }
    };

    $scope.trackFun = function (sceneId, ind) {
        console.log(sceneId, ind, "newtrackfun");
        $rootScope.sid = sceneId;
        $scope.alertText = document.getElementById(ind).checked;
        console.log($scope.alertText);
        if ($scope.alertText) {
            console.log('checkbox is true');
            $rootScope.cBValue.push($rootScope.sid);
            console.log($rootScope.cBValue);
        } else {
            $rootScope.cBValue.splice($rootScope.cBValue.indexOf($rootScope.sid), 1);
            console.log($rootScope.cBValue);
        }
    };

    //remove function starts
    $scope.rmData = function () {
        $scope.chckBoxMsg = true;
        if ($scope.alertText || $rootScope.cBValue.length > 0) {
            $scope.chckBoxMsg = false;
            $scope.removeFun(); //calling removeData() Function()
        }
    };

    $scope.removeFun = function () {
        console.log('inside remove function');
        console.log($rootScope.sid);
        //removeSavedService service call started
        sceneService.removeSavedService($rootScope.cBValue).then(function (res) {
            console.log('getting data from server');
            console.log(res);
            $scope.res = res;
            if (res.data == 'Success') {
                $scope.msg = 'Template Removed Successfully !';
                //Alert message timeout
                setTimeout(function () {
                    window.location.reload();
                }, 2000);
            } else {
                $scope.msg = res.data;
                setTimeout(function () {
                    window.location.reload();
                }, 2000);
            }

        });
    };
    $scope.changeImgNew = function (param1, param2) {
        $scope.uploadMsg = "";
        $scope.templateMsg = "";

        document.getElementById(param1).addEventListener('change', readURL, true);
        function readURL() {
            $scope.templateMsg = "";
            var file = document.getElementById(param1).files[0];
            if (file != undefined) {
                if (file.size > 300000) {
                    $scope.res = {};
                    $scope.res.data = "";
                    $scope.templateMsg = "Image size is greater than 300KB.Cannot save the template";

                }
                var reader = new FileReader();
                reader.onloadend = function (e) {
                    var srcVal = "#" + param2;
                    $(srcVal).attr('src', e.target.result);
                }
                if (file) {
                    reader.readAsDataURL(file);
                }
            }


        };
    };
    //image rendering in pre-defined templates started
    $scope.changeImg = function (param1, param2) {
        $scope.uploadMsg = "";
        $scope.templateMsg = "";
        console.log(param1, param2)
        document.getElementById(param1).addEventListener('change', readURL, true);
        function readURL() {
            console.log('asdfasdfasdf');
            var file = document.getElementById(param1).files[0];
            if (file.size > 300000) {
                $scope.res = {};
                $scope.res.data = "";
                $scope.templateMsg = "Image size is greater than 300KB.Cannot save the template";

            }
            var reader = new FileReader();
            reader.onloadend = function () {
                document.getElementById(param2).style.backgroundImage = "url(" + reader.result + ")";
            }
            if (file) {
                reader.readAsDataURL(file);
            }
        };
    };

    //image rendering in pre-defined templates ended

    //TrackScenes function started
    $scope.trackScenes = function () {
        $scope.msg = "";
        $scope.chckBoxMsg = false;
        $scope.trackingScenes = true;
        $scope.active = false;
        $scope.adminTab = false;
        $scope.upload = false;
        $scope.customTemplate = false;
        $scope.subTemplate = false;
        $scope.template = false;
        $scope.subTitle = "- Track Contents";

        $rootScope.loader = true;
        sceneService.getAllTrackedScenes().then(function (res) {
            $scope.trackScenesData = res.data;
            $scope.totalItems = $scope.trackScenesData.length;   //for pagination  
            $scope.complete();
            $rootScope.loader = false;
        });

    };

    //reload started here
    $scope.reloadPage = function () {
        $scope.getScenesData(); //calling getAllScenes service
    };
    $scope.refreshPage = function () {
        window.location.reload();
    };
    // reload ended here

    //table sorting started
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

    //upload code starts from here
    $scope.uploadDropdown = [
        {
            "id": 1,
            "value": "Image"
        },
        //{
        //    "id": 2,
        //    "value": "High resolution Image(16:9)"
        //},
        {
            "id": 3,
            "value": "Video"
        },
        {
            "id": 4,
            "value": "Web URL"
        },
        {
            "id": 5,
            "value": "Video URL (Blob)"
        }
    ];
    $scope.uploadValue = $scope.uploadDropdown[0].id;
    console.log($scope.uploadValue);

    var imgGAccess;
    function SatrtTheFun(arg) {
        imgGAccess = arg;


        var divWidth = document.getElementById('container1').offsetWidth;
        var divHeight = document.getElementById('container1').offsetHeight;
        var img = arg;

        console.log(img.height());
        console.log(img.width());
        var expectedHeight = 99.5;
        var expectedWidth = 99.5;
        var exHeight = expectedHeight + '%';
        var exWidth = expectedWidth + '%';

        var mar = '4%';
        console.log("************");
        console.log(img.height() + "    " + divHeight);
        if (img.height() > divHeight) {
            console.log('ImG Great', img.clientHeight);
            $("img").css({
                "width": "100%",
                "height": exHeight
            });
            //  img.style.height = expectedHeight + '%';
            //  img.style.width = 'auto';
        } else if (img.width() > divWidth) {
            console.log('Img Less');
            $("img").css({
                "height": "auto",
                "width": exWidth
            });
            //  img.style.width = expectedWidth + '%';
            // img.style.height = 'auto';
        }
        // $(setTimeout(() => {
        //     var a = innerHtmlData
        // }, 1000))



    }

    function readURL(input) {

        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#blah').attr('src', e.target.result);
            }
            reader.readAsDataURL(input.files[0]);
        }
    }
    var innerHtmlData;
    $("#attachment").change(function () {
        $('#blah').attr('src', '');
        if ($scope.uploadValue == 1) {
            $scope.start();
            $rootScope.loader = true;
            readURL(this);

            $(setTimeout(() => {
                /*---------------*/

                SatrtTheFun($('#blah'));

                innerHtmlData = document.getElementById('mas').innerHTML;
                $scope.complete();
                $rootScope.loader = false;
                // $("img").css({"max-width":"90%" });
                /*---------------*/
            }, 1000))
        }


    });
    $scope.displayPreview = false;
    $scope.saveTutorial = function (tutorial) {
        $scope.uploadMsg = "";
        tutorial.SceneType = $scope.uploadValue;
        console.log(tutorial, $scope.uploadValue);
        //Form validation
        $scope.uploadMsg = "";
        if (($scope.uploadValue == 1) && (tutorial.attachment == null || tutorial.attachment == undefined)) {
            $scope.uploadMsg = "Please fill all the fields";
        }
        //if (($scope.uploadValue == 1) && (tutorial.attachment.size > 1000000)) {
        //    $scope.uploadMsg = 'File size should not be greater than 1MB';
        //}
        //if (($scope.uploadValue == 2) && (tutorial.attachment.size < 1000000)){
        //        $scope.uploadMsg = 'Please choose option "Image less than 1MB" option for images less than 1MB';
        //}
        else {
            if (tutorial == undefined || tutorial == null || tutorial == "") {
                console.log('inside upload form validation');
                $scope.uploadMsg = 'Fields should not be empty';
            }
            if (Object.keys(tutorial).length < 4) {
                console.log(Object.keys(tutorial).length);
                $scope.uploadMsg = 'Fields should not be empty';
            }

            if ($scope.uploadValue != 4) {
                if ($scope.uploadValue != 5) {
                    console.log(tutorial.attachment.size, 'file size in bytes');
                    tutorial.UploadUrl = '';
                    if (tutorial.attachment.size > 10000000) {
                        $scope.uploadMsg = 'File size should not be greater than 10MB';
                    }
                }
            }

            if ($scope.uploadValue == 4) {
                var validURL = tutorial.UploadUrl.includes("http", 0);
                console.log(validURL, 'validURL');
                if (!validURL) {
                    $scope.uploadMsg = 'Enter a Valid URL with "http:// or https://"';
                }
                if (tutorial.description == "") {
                    $scope.uploadMsg = 'Fields should not be empty';
                }
            };
            if ($scope.uploadValue == 5) {
                var validURL = tutorial.UploadUrl.includes("http", 0);
                console.log(validURL, 'validURL');
                if (!validURL) {
                    $scope.uploadMsg = 'Enter a Valid URL with "http:// or https://"';
                }
                if (tutorial.description == "") {
                    $scope.uploadMsg = 'Fields should not be empty';
                }
            };

            //console.log(tutorial.attachment.type, 'type');
            //var uploadStringValue = tutorial.attachment.type;
            //var uploadType = uploadStringValue.substring(0, uploadStringValue.lastIndexOf('/'));

            if ($scope.uploadValue == 1) {
                var uploadStringValue = tutorial.attachment.type;

                var uploadType = uploadStringValue.substring(0, uploadStringValue.lastIndexOf('/'));
                console.log(uploadStringValue, 'uploadStringValue', uploadType);
                if (uploadType != 'image') {
                    $scope.uploadMsg = 'Images only can be uploaded here';
                }
            }
            if ($scope.uploadValue == 3) {
                var uploadStringValue = tutorial.attachment.type;
                var uploadType = uploadStringValue.substring(0, uploadStringValue.lastIndexOf('/'));
                console.log(uploadStringValue, 'uploadStringValue', uploadType);
                if (uploadType != 'video') {
                    $scope.uploadMsg = 'Videos only can be uploaded here';
                }
            }
        }


        if ($scope.uploadMsg != "") {
            console.log($scope.uploadMsg);
        }
        else {
            //calling upload service

            if (uploadType == 'image' && $scope.uploadValue == 1) {
                readURL(this);
                $scope.ratio_containerV = "ratio_containerView";
                $(setTimeout(() => {

                    SatrtTheFun($('#blah'));
                    innerHtmlData = document.getElementById('mas').innerHTML;
                    // $("img").css({"max-width":"90%" });

                }, 1000));

                //tutorial.TemplateType = "Time and Weather Template";
                //tutorial.templateId = "";
                //tutorial.SceneContent = tutorial.description;
                //tutorial.IsActive - true
                //tutorial.IsPrimaryApproved = false;
                //tutorial.SceneUrl = "";
                //tutorial.Comments = "";
                //calling upload service
                // var imgValue = document.getElementById("container1").innerHTML;
                //html2canvas($('#container1'), {
                //    onrendered: function (canvas) {

                //        theCanvas = canvas;
                //        //document.body.appendChild(canvas);
                //        // Convert and download as image 
                //        //Canvas2Image.saveAsPNG(canvas);
                //        var base64 = getBase64Image(canvas); //calling the getBase64Image function

                //        tutorial.imgString = base64;
                //        console.log(tutorial);
                //        sceneService.savedSceneService(tutorial).then(function (res) {
                //            console.log(res);
                //            $scope.res = res;
                //            if (res.data == 'Success') {
                //                $scope.uploadMsg = 'Uploaded Successfully !';
                //                //Alert message timeout
                //                setTimeout(function () {
                //                    window.location.reload();
                //                }, 2000);
                //            } else {
                //                $scope.uploadMsg = res.data;
                //                setTimeout(function () {
                //                    window.location.reload();
                //                }, 2000);
                //            }
                //        });

                //    }
                //});
            }
            //else {
            entityService.saveTutorial(tutorial).then(function (res) {
                console.log(res);
                $scope.res = res;
                if (res.data == 'Success') {
                    $scope.uploadMsg = 'Uploaded Successfully !';
                    //Alert message timeout
                    setTimeout(function () {
                        window.location.reload();
                    }, 2000);
                } else {
                    console.log(res.data);
                    $scope.uploadMsg = res.data;
                    setTimeout(function () {
                        window.location.reload();
                    }, 2000);

                }
            });
            //}

        }
    };
    //upload code ends from here

    //template code starts here
    $scope.thumbList = [
        //{
        //    "id": "1",
        //    "imgUrl": "../../Images/thumb3.jpg",
        //    "content": "Sit amet, dignissim nibh, accumsan et vulputate consequat, a ultrices sit ves. Pharo, per mattis esswaztor",
        //    "salary": "$320,800",
        //    "start_date": "2011/04/25",
        //    "office": "Edinburgh",
        //    "extn": "5421",
        //    "type": "Time and Weather Template"
        //},
        //{
        //    "id": "2",
        //    "imgUrl": "../../Images/thumb4.jpg",
        //    "content": "Sit amet, dignissim nibh, accumsan et vulputate consequat, a ultrices sit ves. Pharo, per mattis esswaztor",
        //    "salary": "$320,800",
        //    "start_date": "2011/04/25",
        //    "office": "Edinburgh",
        //    "extn": "5421",
        //    "type": "Food"
        //},
        //{
        //    "id": "3",
        //    "imgUrl": "../../Images/thumb5.jpg",
        //    "content": "Sit amet, dignissim nibh, accumsan et vulputate consequat, a ultrices sit ves. Pharo, per mattis esswaztor",
        //    "salary": "$320,800",
        //    "start_date": "2011/04/25",
        //    "office": "Edinburgh",
        //    "extn": "5421",
        //    "type": "Food"
        //},
        //{
        //    "id": "4",
        //    "imgUrl": "../../Images/thumb6.jpg",
        //    "content": "Sit amet, dignissim nibh, accumsan et vulputate consequat, a ultrices sit ves. Pharo, per mattis esswaztor",
        //    "salary": "$320,800",
        //    "start_date": "2011/04/25",
        //    "office": "Edinburgh",
        //    "extn": "5421",
        //    "type": "Food"
        //},
        {
            "id": "5",
            "imgUrl": "../../Images/thumb1Temp.png",
            "content": "Sit amet, dignissim nibh, accumsan et vulputate consequat, a ultrices sit ves. Pharo, per mattis esswaztor",
            "salary": "$320,800",
            "start_date": "2011/04/25",
            "office": "Edinburgh",
            "extn": "5421",
            "type": "16:9 Templates"
        },
        //{
        //    "id": "6",
        //    "imgUrl": "../../Images/thumb2.png",
        //    "content": "Sit amet, dignissim nibh, accumsan et vulputate consequat, a ultrices sit ves. Pharo, per mattis esswaztor",
        //    "salary": "$320,800",
        //    "start_date": "2011/04/25",
        //    "office": "Edinburgh",
        //    "extn": "5421",
        //    "type": "16:9 Templates"
        //},
        //{
        //    "id": "7",
        //    "imgUrl": "../../Images/thumb7.jpg",
        //    "content": "Sit amet, dignissim nibh, accumsan et vulputate consequat, a ultrices sit ves. Pharo, per mattis esswaztor",
        //    "salary": "$320,800",
        //    "start_date": "2011/04/25",
        //    "office": "Edinburgh",
        //    "extn": "5421",
        //    "type": "Food"
        //}, {
        //    "id": "8",
        //    "imgUrl": "../../Images/thumb8.jpg",
        //    "content": "Sit amet, dignissim nibh, accumsan et vulputate consequat, a ultrices sit ves. Pharo, per mattis esswaztor",
        //    "salary": "$320,800",
        //    "start_date": "2011/04/25",
        //    "office": "Edinburgh",
        //    "extn": "5421",
        //    "type": "Food"
        //}, {
        //    "id": "9",
        //    "imgUrl": "../../Images/thumb9.jpg",
        //    "content": "Sit amet, dignissim nibh, accumsan et vulputate consequat, a ultrices sit ves. Pharo, per mattis esswaztor",
        //    "salary": "$320,800",
        //    "start_date": "2011/04/25",
        //    "office": "Edinburgh",
        //    "extn": "5421",
        //    "type": "Food"
        //},
        //{
        //    "id": "10",
        //    "imgUrl": "../../Images/thumb10.jpg",
        //    "content": "Sit amet, dignissim nibh, accumsan et vulputate consequat, a ultrices sit ves. Pharo, per mattis esswaztor",
        //    "salary": "$320,800",
        //    "start_date": "2011/04/25",
        //    "office": "Edinburgh",
        //    "extn": "5421",
        //    "type": "Food"
        //},
        {
            "id": "12",
            //"imgUrl": "../../Images/school_thumb1.png",
            "imgUrl": "../../Images/thumb2Temp.png",
            "content": "Sit amet, dignissim nibh, accumsan et vulputate consequat, a ultrices sit ves. Pharo, per mattis esswaztor.",
            "salary": "$320,800",
            "start_date": "2011/04/25",
            "office": "Edinburgh",
            "extn": "5421",
            "type": "Time and Weather Template"
        },
        //{
        //    "id": "13",
        //    "imgUrl": "../../Images/school_thumb2.png",
        //    "content": "A school could be the happy memory of your life or deliver the useful knowledge for your study.",
        //    "salary": "$320,800",
        //    "start_date": "2011/04/25",
        //    "office": "Edinburgh",
        //    "extn": "5421",
        //    "type": "School"
        //},
        {
            "id": "14",
            "imgUrl": "../../Images/school_thumb3.png",
            "content": "A school could be the happy memory of your life or deliver the useful knowledge for your study.",
            "salary": "$320,800",
            "start_date": "2011/04/25",
            "office": "Edinburgh",
            "extn": "5421",
            "type": "School"
        },
        //{
        //    "id": "15",
        //    "imgUrl": "../../Images/coffee_thumb3.png",
        //    "content": "Coffee shop, accumsan et vulputate consequat, a ultrices sit ves. Pharo, per mattis esswaztor",
        //    "salary": "$320,800",
        //    "start_date": "2011/04/25",
        //    "office": "Edinburgh",
        //    "extn": "5421",
        //    "type": "Coffee"
        //},
        {
            "id": "16",
            "imgUrl": "../../Images/coffee_thumb2.png",
            "content": "Coffee shop, accumsan et vulputate consequat, a ultrices sit ves. Pharo, per mattis esswaztor",
            "salary": "$320,800",
            "start_date": "2011/04/25",
            "office": "Edinburgh",
            "extn": "5421",
            "type": "Coffee"
        },
        {
            "id": "17",
            "imgUrl": "../../Images/coffee_thumb1.png",
            "content": "Coffee shop, accumsan et vulputate consequat, a ultrices sit ves. Pharo, per mattis esswaztor",
            "salary": "$320,800",
            "start_date": "2011/04/25",
            "office": "Edinburgh",
            "extn": "5421",
            "type": "Coffee"
        },

        {
            "id": "11",
            "imgUrl": "../../Images/new_thumb.png",
            "content": "Sit amet, dignissim nibh, accumsan et vulputate consequat, a ultrices sit ves. Pharo, per mattis esswaztor",
            "salary": "$320,800",
            "start_date": "2011/04/25",
            "office": "Edinburgh",
            "extn": "5421",
            "type": "Verticle Templates"
        },
        {
            "id": "18",
            "imgUrl": "../../Images/school_thumb4.png",
            "content": "Sit amet, dignissim nibh, accumsan et vulputate consequat, a ultrices sit ves. Pharo, per mattis esswaztor",
            "salary": "$320,800",
            "start_date": "2011/04/25",
            "office": "Edinburgh",
            "extn": "5421",
            "type": "School"
        },
        {
            "id": "19",
            "imgUrl": "../../Images/coffee_thumb4.png",
            "content": "Coffee shop,  accumsan et vulputate consequat, a ultrices sit ves. Pharo, per mattis esswaztor",
            "salary": "$320,800",
            "start_date": "2011/04/25",
            "office": "Edinburgh",
            "extn": "5421",
            "type": "Coffee"
        }

    ];


    //newTemplate5
    $scope.conTemp = {};
    $scope.conTemp.conTemp1 = "NewYork Classic";
    $scope.conTemp.conTemp2 = "Burger";
    $scope.conTemp.conTemp3 = "Week 5";
    $scope.conTemp.conTemp4 = "7th July - 2nd Sep 2017";
    $scope.conTemp.conTemp5 = "Iceberg 190";
    $scope.conTemp.conTemp6 = "Lorem ipsum dolor sit amet, dignissim nibh, accumsan et ipsum sit amet, diguyg sim vull";
    $scope.conTemp.conTemp7 = "100% Iceberg Lettuce.";
    $scope.conTemp.conTemp8 = "190";
    $scope.conTemp.tempName = "";

    //newTemplate6
    $scope.conTemp1 = {};
    $scope.conTemp1.conTemp1 = "Digital ";
    $scope.conTemp1.conTemp2 = "Signage";
    $scope.conTemp1.conTemp3 = "Digital Display";
    $scope.conTemp1.conTemp4 = "Digital Signage is platform that connects seamlessly with IoT ";
    $scope.conTemp1.conTemp5 = "Electronics";
    $scope.conTemp1.conTemp6 = "90";
    $scope.conTemp1.tempName = "";

    //newTemplate3
    $scope.conTemp3 = {};
    $scope.conTemp3.Heading = "Marriot's Cozumel Resort and Spa";
    $scope.conTemp3.content1 = "The point of using Lorem Ipsum is that it has a more-or-less normal distribution of letters.";
    $scope.conTemp3.content2 = "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout.";
    $scope.conTemp3.content3 = "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout.";
    $scope.conTemp3.tempName = "";

    //newTemplate 3v
    $scope.conTemp3v = {};
    $scope.conTemp3v.Heading = "Marriot's Cozumel Resort and Spa";
    $scope.conTemp3v.content1 = "The point of using Lorem Ipsum is that it has a more-or-less normal distribution of letters.";
    $scope.conTemp3v.content2 = " It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout.";
    $scope.conTemp3v.content3 = "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout.";
    $scope.conTemp3v.tempName = "";

    //newTemplate 4
    $scope.conTemp4 = {};
    $scope.conTemp4.Heading = "3rd Street Pier ";
    $scope.conTemp4.content1 = "It is a long established fact that a reader will be distracted by the readable content of a page when looking at.";
    $scope.conTemp4.content2 = "It is a long established fact that a reader will be distracted by the readable content of a page when looking at.";
    $scope.conTemp4.content3 = "The point of using Lorem Ipsum is that it has a more-or-less normal distribution of letters.";
    $scope.conTemp4.tempName = "";

    //newTemplate 4v
    $scope.conTemp4v = {};
    $scope.conTemp4v.Heading = "3rd Street Pier ";
    $scope.conTemp4v.content1 = "It is a long established fact that a reader will be distracted by the readable content of a page when looking at.";
    $scope.conTemp4v.content2 = "It is a long established fact that a reader will be distracted by the readable content of a page when looking at.";
    $scope.conTemp4v.content3 = "The point of using Lorem Ipsum is that it has a more-or-less normal distribution of letters.";
    $scope.conTemp4v.tempName = "";

    //newTemplate 5
    $scope.conTemp5 = {};
    $scope.conTemp5.Heading = "Sanctuary Cosmetics ";
    $scope.conTemp5.content1 = "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout.";
    $scope.conTemp5.content2 = "Normal distribution of letters, as opposed to using content here...";
    $scope.conTemp5.content3 = "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout.";
    $scope.conTemp5.tempName = "";

    //newTemplate 5v
    $scope.conTemp5v = {};
    $scope.conTemp5v.Heading = "Sanctuary Cosmetics ";
    $scope.conTemp5v.content1 = "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout.";
    $scope.conTemp5v.content2 = "Normal distribution of letters, as opposed to using content here...";
    $scope.conTemp5v.content3 = "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout.";
    $scope.conTemp5v.tempName = "";

    //newTemplate 6
    $scope.conTemp6 = {};
    $scope.conTemp6.Heading = "Alaska Airlines ";
    $scope.conTemp6.content1 = "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout.";
    $scope.conTemp6.content2 = "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. ";
    $scope.conTemp6.tempName = "";

    //newTemplate 6v
    $scope.conTemp6v = {};
    $scope.conTemp6v.Heading = "Alaska Airlines ";
    $scope.conTemp6v.content1 = "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout.";
    $scope.conTemp6v.content2 = "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. ";
    $scope.conTemp6.tempName = "";

    // newTemplate 7
    $scope.conTemp7 = {};
    $scope.conTemp7.Discount = "30%";
    $scope.conTemp7.discountHeading = "Discount";
    $scope.conTemp7.name = "DF";
    $scope.conTemp7.content1 = "Source of Omega-3 Fatty Acids";
    $scope.conTemp7.content2 = "Digital Fish Fry";
    $scope.conTemp7.content3 = "18";
    $scope.conTemp7.content4 = "Lorem ipsum dolor sit amet, dignissim nibh, $5/- et vulputate."
    $scope.conTemp7.offerDate = "Offer till Feb14th";
    $scope.conTemp7.content5 = "Decrease the risk of Depression, ADHD, Alzheimer’s Disease, Dementia, and Diabetes";
    $scope.conTemp7.grpList1 = "MON $3";
    $scope.conTemp7.grpListCont1 = "Lorem ipsum dolor sit amet, dignissim nibh, accumsan et";
    $scope.conTemp7.grpList2 = "TUE $4";
    $scope.conTemp7.grpListCont2 = "Lorem ipsum dolor sit amet, dignissim nibh, accumsan et";
    $scope.conTemp7.grpList3 = "WED $3.5";
    $scope.conTemp7.grpListCont3 = "Lorem ipsum dolor sit amet, dignissim nibh, accumsan et";
    $scope.conTemp7.grpList4 = "THU $4";
    $scope.conTemp7.grpListCont4 = "Lorem ipsum dolor sit amet, dignissim nibh, accumsan et";
    $scope.conTemp7.hotelHead = "Hotel Fisher";
    $scope.conTemp7.hotelHead1 = "Lorem ipsum dolor";
    $scope.conTemp7.code = "0011 - 55 - 2000";

    $scope.conTemp7.tempName = "";

    // @@@@@@@@@@@@@@@@@@@@NEW SCHOOL TEMPLATE @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    $scope.content12 = {};
    $scope.content12.tempName = "";
    $scope.content12.DateText = "April 12th 2018";
    $scope.content12.DateTextYear = "2018";
    $scope.content12.b2School = "Today";
    $scope.content12.SchoolName = "United States";
    $scope.content12.shortDesc1 = "859 Washington consequat, ";
    $scope.content12.shortDesc2 = "accumsan et vulputate consequat";
    $scope.content12.Collect = "Morning fog, partly cloudy...";
    $scope.content12.BGImage = "~/Images/school1.png";

    $scope.content13 = {};
    $scope.content12.tempName = "";
    $scope.content13.new_admission = "New Admission at";
    $scope.content13.SchoolName = "Conti Baby School";
    $scope.content13.fromDate = "14";
    $scope.content13.fromDate1 = "April";
    $scope.content13.toDate = "25";
    $scope.content13.toDate1 = "May";
    $scope.content13.phoneNum = "1002-5555-887";
    $scope.content13.addrs1 = "859 Washington Street";
    $scope.content13.addrs2 = "(Corner of west 13th street";
    $scope.content13.addrs3 = "New York, Ny, 10014";

    $scope.content14 = {};
    $scope.content14.tempName = "";
    $scope.content14.discript = "Education is the most powerful weapon which you can use to change the world.";

    $scope.content15 = {};
    $scope.content15.taste = "Taste";
    $scope.content15.freshness = "the Freshness";
    $scope.content15.speacial = "We have replaced their regular coffee with";
    $scope.content15.speacial1 = "Special";
    $scope.content15.special2 = ". Let's see if they notice!";
    $scope.content15.tempName = "";

    $scope.content16 = {};
    $scope.content16.getRich = "Get Rich quick.";
    $scope.content16.good_c_block = "Good coffee is like friendship";
    $scope.content16.good_c_block2 = "73 Sacramento, CA, USA.,";
    $scope.content16.tempName = "";

    $scope.content17 = {};
    $scope.content17.rotate = "Featured Taste";
    $scope.content17.perfect1 = "perfect";
    $scope.content17.coffee1 = "Coffee";
    $scope.content17.real_block = "The real coffee experts.";
    $scope.content17.real_block2 = "Florida, U.S.A.";
    $scope.content17.tempName = "";

    $scope.content18 = {};
    $scope.content18.sc4_usa = "USA";
    $scope.content18.sc4_excell = "School of excellence";
    $scope.content18.sc4_We_wel = "We welcome you to back again";
    $scope.content18.sc4_Educat = "Education is the movement from darkness to light.";
    $scope.content18.sc4_white_bg = "on June 1st 2018";
    $scope.content18.tempName = "";

    $scope.content19 = {};
    $scope.content19.cof4_cday = "Coffee Day";
    $scope.content19.cof4_law = "Where the laws of nature apply.";
    $scope.content19.cof4_add = "Coffee Day, Seattle, WA, 98104, USA";

    $scope.newTempFun = function (idval) {
        console.log('inside subTemplates function');
        $rootScope.Template = 'Template' + idval;
        $rootScope.sceneTempId = idval;

        console.log($rootScope.sceneTempId);
        $scope.subTemplate = true;
        $scope.mainTemp = false;
        $scope.animationTemp = true;
    };
    //Back to More Template
    $scope.backTemp = function () {
        console.log('Back to Templates');
        $rootScope.Template = '';
        $scope.subTemplate = true;
        $scope.mainTemp = true;
        $scope.animationTemp = false;
    };

    //saving Template
    $scope.TemplateForm1 = true;
    //$scope.saveTemp = function (contentTemplate) {
    //    console.log('inside template publish');                
    //    //console.log($scope.conTemp);
    //    //console.log($scope.conTemp1);
    //    $scope.tcontent = document.getElementById(contentTemplate).innerHTML;
    //    console.log($scope.tcontent);
    //    $scope.svalue ='scene'+ contentTemplate.slice(-1);
    //    console.log($scope.svalue);

    //    if (contentTemplate == 'scene2') {
    //        console.log('scene1 Template Name', $scope.conTemp.tempName);
    //        $scope.templateName = $scope.conTemp.tempName;
    //    } else {
    //        console.log('scene1 Template Name', $scope.conTemp1.tempName);
    //        $scope.templateName = $scope.conTemp1.tempName;
    //    }
    //    console.log($scope.templateName);
    //    $scope.postData = {};
    //    $scope.postData.templateId = $rootScope.sceneTempId
    //    $scope.postData.Status = "";
    //    $scope.postData.SceneContent = $scope.tcontent;
    //    $scope.postData.SceneType = "template";
    //    $scope.postData.IsActive = true;
    //    $scope.postData.IsPrimaryApproved = false;
    //    $scope.postData.SceneUrl = "";
    //    $scope.postData.Comments = "";
    //    $scope.postData.SceneName = $scope.templateName;
    //    console.log($scope.postData);
    //    if ($scope.postData.SceneName == "") {
    //        $scope.templateMsg = "Template Name should not be empty"
    //    } else {
    //        //savedSceneservice service call started
    //        sceneService.savedSceneService($scope.postData).then(function (res) {
    //            console.log(res);
    //            if (res.data == 'Success') {
    //                $scope.templateMsg = 'Template Saved Successfully !';
    //                setTimeout(function () {
    //                    //$("#templateSuccess").alert('close');
    //                    $scope.templateMsg = '';
    //                    $scope.getScenesData(); //calling getAllScenes service
    //                    $scope.temp5 = false;
    //                    $scope.temp6 = false;
    //                    $scope.animationTemp = false;
    //                    $scope.active = true;
    //                }, 2000);
    //            } else {
    //                $scope.templateMsg = res.data;
    //            }
    //            //Alert message timeout                            
    //        });
    //    }
    //};
    //template code ends here     

    //converting the DOM content to image
    $scope.generateImage = function (chartObj, weatherIcPos) {
        console.log(chartObj);

        $scope.showWeatherIcon = false;
        $scope.tempData = {};
        if (weatherIcPos == null || weatherIcPos == undefined) {
            $scope.tempData.IconPosition = "";
            $scope.tempData.TemplateType = "";

        }
        else {
            $scope.tempData.IconPosition = weatherIcPos;
            $scope.tempData.TemplateType = "Time and Weather Template";
        }
        $scope.tempData.SceneName = "";
        if (chartObj == 'template3') {
            $scope.templateName = $scope.conTemp3.tempName;
        }
        $scope.tempData.SceneName = $scope.templateName;

        if ($scope.tempData.SceneName == "") {
            $scope.templateMsg = "Template Name should not be empty"
        } else {
            var tcontent = document.getElementById(chartObj).innerHTML;
            html2canvas($('#template3'), {
                onrendered: function (canvas) {
                    theCanvas = canvas;

                    document.body.appendChild(canvas);
                    // Convert and download as image 
                    //Canvas2Image.saveAsPNG(canvas);
                    var myImg = document.querySelector("#template3");
                    var currWidth = myImg.clientWidth;
                    var currHeight = myImg.clientHeight;
                    console.log(currWidth + "   " + currHeight);
                    $("#img-out").append(canvas);
                    var base64 = getBase64Image(canvas); //calling the getBase64Image function
                    console.log(base64);

                    $scope.tempData.templateId = $rootScope.sceneTempId;
                    $scope.tempData.imgString = base64;
                    $scope.tempData.SceneContent = tcontent;
                    $scope.tempData.SceneType = "template";
                    $scope.tempData.IsActive = true;
                    $scope.tempData.IsPrimaryApproved = false;
                    $scope.tempData.SceneUrl = "";
                    $scope.tempData.Comments = "";

                    if ($scope.tempData.SceneName == "") {
                        $scope.templateMsg = "Template Name should not be empty"
                    } else {
                        //savedSceneservice service call started
                        sceneService.savedSceneService($scope.tempData).then(function (res) {
                            console.log(res);
                            $scope.res = res;
                            if (res.data == 'Success') {
                                $scope.templateMsg = 'Template Saved Successfully !';
                                setTimeout(function () {
                                    window.location.reload();
                                }, 2000)
                            } else {
                                $scope.templateMsg = res.data;
                            }
                        });
                    } // else ended                     
                }
            });
        }//main else
    };

    $scope.generateImage1 = function (chartObj, weatherIcPos) {

        console.log(chartObj);
        $scope.showWeatherIcon = false;
        $scope.tempData = {};
        if (weatherIcPos == null || weatherIcPos == undefined) {
            $scope.tempData.IconPosition = "";
            $scope.tempData.TemplateType = "";

        }
        else {
            $scope.tempData.IconPosition = weatherIcPos;
            $scope.tempData.TemplateType = "Time and Weather Template";
        }
        $scope.tempData.SceneName = "";
        if (chartObj == 'template3v') {
            $scope.templateName = $scope.conTemp3v.tempName;
            console.log('inside template3v');
        }
        $scope.tempData.SceneName = $scope.templateName;

        if ($scope.tempData.SceneName == "") {
            $scope.templateMsg = "Template Name should not be empty"
        } else {
            var tcontent = document.getElementById(chartObj).innerHTML;

            html2canvas($('#template3v'), {
                onrendered: function (canvas) {
                    theCanvas = canvas;
                    document.body.appendChild(canvas);
                    // Convert and download as image 
                    //Canvas2Image.saveAsPNG(canvas);
                    $("#img-out").append(canvas);
                    var base64 = getBase64Image(canvas); //calling the getBase64Image function
                    console.log(base64);

                    $scope.tempData.templateId = $rootScope.sceneTempId
                    $scope.tempData.imgString = base64;
                    $scope.tempData.SceneContent = tcontent;
                    $scope.tempData.SceneType = "template";
                    $scope.tempData.IsActive = true;
                    $scope.tempData.IsPrimaryApproved = false;
                    $scope.tempData.SceneUrl = "";
                    $scope.tempData.Comments = "";
                    console.log($scope.tempData);

                    if ($scope.tempData.SceneName == "") {
                        $scope.templateMsg = "Template Name should not be empty"
                    } else {
                        //savedSceneservice service call started
                        sceneService.savedSceneService($scope.tempData).then(function (res) {
                            console.log(res);
                            $scope.res = res;
                            if (res.data == 'Success') {
                                $scope.templateMsg = 'Template Saved Successfully !';
                            } else {
                                $scope.templateMsg = 'Error In Saving Template !';
                            }
                            //Alert message timeout
                            setTimeout(function () {
                                window.location.reload();
                            }, 2000);
                        });
                    }
                }
            });
        } //main else
    };

    $scope.generateImage4 = function (chartObj) {
        console.log(chartObj);
        $scope.tempData = {};
        $scope.tempData.IconPosition = "";
        $scope.tempData.TemplateType = "";
        $scope.tempData.SceneName = "";
        if (chartObj == 'template4') {
            $scope.templateName = $scope.conTemp4.tempName;
        }
        $scope.tempData.SceneName = $scope.templateName;

        if ($scope.tempData.SceneName == "") {
            $scope.templateMsg = "Template Name should not be empty"
        } else {
            var tcontent = document.getElementById(chartObj).innerHTML;
            html2canvas($('#template4'), {
                onrendered: function (canvas) {
                    theCanvas = canvas;
                    document.body.appendChild(canvas);
                    // Convert and download as image 
                    //Canvas2Image.saveAsPNG(canvas);
                    $("#img-out").append(canvas);
                    var base64 = getBase64Image(canvas); //calling the getBase64Image function
                    console.log(base64);

                    $scope.tempData.templateId = $rootScope.sceneTempId
                    $scope.tempData.imgString = base64;
                    $scope.tempData.SceneContent = tcontent;
                    $scope.tempData.SceneType = "template";
                    $scope.tempData.IsActive = true;
                    $scope.tempData.IsPrimaryApproved = false;
                    $scope.tempData.SceneUrl = "";
                    $scope.tempData.Comments = "";
                    console.log($scope.tempData);

                    if ($scope.tempData.SceneName == "") {
                        $scope.templateMsg = "Template Name should not be empty"
                    } else {
                        //savedSceneservice service call started
                        sceneService.savedSceneService($scope.tempData).then(function (res) {
                            console.log(res);
                            $scope.res = res;
                            if (res.data == 'Success') {
                                $scope.templateMsg = 'Template Saved Successfully !';
                                setTimeout(function () {
                                    window.location.reload();
                                }, 2000);
                            } else {
                                $scope.templateMsg = res.data;
                            }
                        });
                    } // else ended                     
                }
            });
        }//main else
    };


    $scope.generateImage2 = function (chartObj) {
        console.log(chartObj, 'chartobj***');
        $scope.tempData = {};
        $scope.tempData.IconPosition = "";
        $scope.tempData.TemplateType = "";
        $scope.tempData.SceneName = "";
        if (chartObj == 'template2') {
            $scope.templateName = $scope.conTemp.tempName;
            console.log($scope.templateName);
        }
        $scope.tempData.SceneName = $scope.templateName;

        if ($scope.tempData.SceneName == "") {
            $scope.templateMsg = "Template Name should not be empty"
        } else {

            $scope.ovr_newYorkClass = "ovr_newYork";
            $scope.ovr_tempImg2Class = "ovr_tempImg2";
            $scope.ovr_mainTemplate2Class = "ovr_mainTemplate2";
            $scope.ovr_txt1aClass = "ovr_txt1a";
            $scope.ovr_mainImg2Class = "ovr_mainImg2_img";
            $scope.ovr_newYork_ClassicClass = "ovr_newYork_Classic";
            $scope.ovr_Week5Class = "ovr_Week5";
            $scope.ovr_7thjulyClass = "ovr_7thjuly";
            $scope.ovr_txt2aClass = "ovr_txt2a";
            $scope.ovr_IcebergClass = "ovr_Iceberg";
            $scope.ovr_IcebergPClass = "ovr_IcebergP";
            $scope.ovr_txt3aClass = "ovr_txt3a";
            $scope.ovr_IcebergLetClass = "ovr_IcebergLet";
            $scope.ovr_IcebergLet190Class = "ovr_IcebergLet190";
            $scope.ovr_txt1aaClass = "ovr_txt1aa";
            $scope.ovr_mainImg2Class = "ovr_mainImg2";

            var tcontent = document.getElementById(chartObj).innerHTML;
            html2canvas($('#template2'), {
                onrendered: function (canvas) {
                    theCanvas = canvas;
                    document.body.appendChild(canvas);
                    // Convert and download as image 
                    //Canvas2Image.saveAsPNG(canvas);
                    $("#img-out").append(canvas);
                    var base64 = getBase64Image(canvas); //calling the getBase64Image function
                    console.log(base64);

                    $scope.tempData.templateId = $rootScope.sceneTempId
                    $scope.tempData.imgString = base64;
                    $scope.tempData.SceneContent = tcontent;
                    $scope.tempData.SceneType = "template";
                    $scope.tempData.IsActive = true;
                    $scope.tempData.IsPrimaryApproved = false;
                    $scope.tempData.SceneUrl = "";
                    $scope.tempData.Comments = "";
                    console.log($scope.tempData);

                    if ($scope.tempData.SceneName == "") {
                        $scope.templateMsg = "Template Name should not be empty"
                    } else {
                        //savedSceneservice service call started
                        sceneService.savedSceneService($scope.tempData).then(function (res) {
                            console.log(res);
                            $scope.res = res;
                            if (res.data == 'Success') {
                                $scope.templateMsg = 'Template Saved Successfully !';
                                setTimeout(function () {
                                    window.location.reload();
                                }, 2000)
                            } else {
                                $scope.templateMsg = res.data;
                            }
                        });
                    } // else ended                     
                }
            });
        }//main else
    };

    $scope.generateImage3 = function (chartObj) {
        if ($scope.templateMsg == "Image size is greater than 300KB.Cannot save the template") {
            $scope.templateMsg = "Image size is greater than 300KB.Cannot save the template";
        }
        else {
            console.log(chartObj);
            $scope.tempData = {};
            $scope.tempData.IconPosition = "";
            $scope.tempData.TemplateType = "";
            $scope.tempData.SceneName = "";
            if (chartObj == 'template1') {
                $scope.templateName = $scope.conTemp1.tempName;
            }
            $scope.tempData.SceneName = $scope.templateName;

            if ($scope.tempData.SceneName == "") {
                $scope.templateMsg = "Template Name should not be empty"
            } else {

                $scope.ovr_tempImg_pr2Class = "ovr_tempImg_pr2";
                $scope.ovr_txt1_pr2Class = "ovr_txt1_pr2";
                $scope.ovr_txt1_h3_pr2Class = "ovr_txt1_h3_pr2";
                $scope.ovr_txt1_h2_pr2Class = "ovr_txt1_h2_pr2";
                $scope.ovr_txt2_pr2Class = "ovr_txt2_pr2";
                $scope.ovr_txt2_h4_pr2Class = "ovr_txt2_h4_pr2";
                $scope.ovr_txt2_p_pr2Class = "ovr_txt2_p_pr2";
                $scope.ovr_txt2_h3_pr2Class = "ovr_txt2_h3_pr2";
                $scope.ovr_txt2_h2_pr2Class = "ovr_txt2_h2_pr2";
                $scope.ovr_mainImg_pr2Class = "ovr_mainImg_pr2";
                $scope.ovr_mainTemplate_pr2Class = "ovr_mainTemplate_pr2";

                var tcontent = document.getElementById(chartObj).innerHTML;
                html2canvas($('#template1'), {
                    onrendered: function (canvas) {
                        theCanvas = canvas;
                        document.body.appendChild(canvas);
                        // Convert and download as image 
                        //Canvas2Image.saveAsPNG(canvas);
                        $("#img-out").append(canvas);
                        var base64 = getBase64Image(canvas); //calling the getBase64Image function
                        console.log(base64);

                        $scope.tempData.templateId = $rootScope.sceneTempId
                        $scope.tempData.imgString = base64;
                        $scope.tempData.SceneContent = tcontent;
                        $scope.tempData.SceneType = "template";
                        $scope.tempData.IsActive = true;
                        $scope.tempData.IsPrimaryApproved = false;
                        $scope.tempData.SceneUrl = "";
                        $scope.tempData.Comments = "";
                        console.log($scope.tempData);

                        if ($scope.tempData.SceneName == "") {
                            $scope.templateMsg = "Template Name should not be empty"
                        } else {
                            //savedSceneservice service call started
                            console.log("entering");
                            sceneService.savedSceneService($scope.tempData).then(function (res) {
                                console.log(res);
                                $scope.res = res;
                                if (res.data == 'Success') {
                                    $scope.templateMsg = 'Template Saved Successfully !';
                                    setTimeout(function () {
                                        window.location.reload();
                                    }, 2000)
                                } else {
                                    $scope.templateMsg = res.data;
                                    setTimeout(function () {
                                        window.location.reload();
                                    }, 2000);
                                }
                            });
                        } // else ended                     
                    }
                });
            }//main else
        }

    };

    $scope.displayWeatherIcon = function (icon) {
        $scope.showWeatherIcon = true;
        $scope.weatherIconClass = "weatherIcon";
        if (icon == 'TopRight') {
            $scope.weatherIconClass = "weatherIconTR";
        }
        else if (icon == 'TopLeft') {
            $scope.weatherIconClass = "weatherIconTL";
        }
        else if (icon == 'BottomRight') {
            $scope.weatherIconClass = "weatherIconBR";
        }
        else if (icon == 'BottomLeft') {
            $scope.weatherIconClass = "weatherIconBL";
        }

    }
    //$scope.templateList = angular.copy($scope.thumbList);
    $scope.templateList = $scope.thumbList;
    $scope.templateType = "";
    $scope.changeType = function (type) {
        $scope.templateList = $filter('typeFilter')($scope.thumbList, $scope.templateType);
    }

    $scope.templateTypes = ['16:9 Templates', 'Time and Weather Template', 'School', 'Coffee'];
    $scope.temp11_block = "t_temp_block";
    $scope.temp11_t_leaf_bg = "t_leaf_bg";
    $scope.temp11_t_icon_bg = "t_icon_bg";
    $scope.temp11_t_thirty_per = "t_thirty_per";
    $scope.temp11_t_span_line = "t_span_line";
    $scope.temp11_t_double_line_mas = "t_double_line_mas";
    $scope.temp11_t_d_line = "t_d_line";
    $scope.temp11_t_df2_mas = "t_df2_mas";
    $scope.temp11_t_omega = "t_omega";
    $scope.temp11_t_fish_img = "t_fish_img";
    $scope.temp11_t_orange_cur_mas = "t_orange_cur_mas";
    $scope.temp11_t_orange_bg1 = "t_orange_bg1";
    $scope.temp11_blk1 = "blk1";
    $scope.temp11_t_f_img1 = "t_f_img1";
    $scope.temp11_blk2 = "blk2";
    $scope.temp11_t_white_bg1 = "t_white_bg1";
    $scope.temp11_t_grey_bg1 = "t_grey_bg1";
    $scope.temp11_t_green_bg1 = "t_green_bg1";
    $scope.temp11_days = "days";
    $scope.temp11_one = "one";
    $scope.temp11_two = "two";
    $scope.temp11_three = "three";
    $scope.temp11_four = "four";
    $scope.temp11_t_hotelName = "t_hotelName";

    $scope.generateImage11 = function (chartObj) {
        console.log(chartObj);

        $scope.tempData = {};
        // alert($scope.conTemp7.tempName);
        $scope.tempData.SceneName = "";
        if (chartObj == 'Template11') {
            $scope.templateName = $scope.conTemp7.tempName;
            $scope.tempData.SceneName = $scope.conTemp7.tempName;
            // alert($scope.tempData.SceneName+ "      sdfsdf     " +$scope.conTemp7.tempName)
        }

        //  $scope.tempData.SceneName = $scope.templateName;

        //  alert($scope.tempData.SceneName+"   ****");
        if ($scope.tempData.SceneName == "") {
            $scope.templateMsg = "Template Name should not be empty"
        } else {

            $scope.temp11_block = "temp_block";
            $scope.temp11_t_leaf_bg = "leaf_bg";
            $scope.temp11_t_icon_bg = "icon_bg";
            $scope.temp11_t_thirty_per = "thirty_per";
            $scope.temp11_t_span_line = "span_line";
            $scope.temp11_t_double_line_mas = "double_line_mas";
            $scope.temp11_t_d_line = "d_line";
            $scope.temp11_t_df2_mas = "df2_mas";
            $scope.temp11_t_omega = "omega";
            $scope.temp11_t_fish_img = "fish_img";
            $scope.temp11_t_orange_cur_mas = "orange_cur_mas";
            $scope.temp11_t_orange_bg1 = "orange_bg1";
            $scope.temp11_blk1 = "blk1";
            $scope.temp11_t_f_img1 = "f_img1";
            $scope.temp11_blk2 = "blk2";
            $scope.temp11_t_white_bg1 = "white_bg1";
            $scope.temp11_t_grey_bg1 = "grey_bg1";
            $scope.temp11_t_green_bg1 = "green_bg1";
            $scope.temp11_days = "days";
            $scope.temp11_one = "one";
            $scope.temp11_two = "two";
            $scope.temp11_three = "three";
            $scope.temp11_four = "four";
            $scope.temp11_t_hotelName = "hotelName";
            $scope.ovr_mainTemplate_pr2Class = "ovr_mainTemplate_pr2";
            // alert(chartObj);
            var tcontent = document.getElementById('template11').innerHTML;
            html2canvas($('#template11'), {
                onrendered: function (canvas) {
                    theCanvas = canvas;
                    document.body.appendChild(canvas);
                    // Convert and download as image 
                    //Canvas2Image.saveAsPNG(canvas);
                    $("#img-out").append(canvas);
                    var base64 = getBase64Image(canvas); //calling the getBase64Image function

                    $scope.tempData.templateId = $rootScope.sceneTempId
                    $scope.tempData.imgString = base64;
                    $scope.tempData.SceneContent = tcontent;
                    $scope.tempData.SceneType = "template";
                    $scope.tempData.IsActive = true;
                    $scope.tempData.IsPrimaryApproved = false;
                    $scope.tempData.SceneUrl = "";
                    $scope.tempData.Comments = "";
                    console.log($scope.tempData);
                    // alert("*********       " + $scope.tempData.SceneName);
                    if ($scope.tempData.SceneName == "") {
                        //   alert("templlate name is empty");
                        $scope.templateMsg = "Template Name should not be empty"
                    } else {
                        //savedSceneservice service call started
                        sceneService.savedSceneService($scope.tempData).then(function (res) {
                            console.log(res);
                            $scope.res = res;
                            console.log("*****************************************");
                            console.log(res);
                            if (res.data == 'Success') {
                                $scope.templateMsg = 'Template Saved Successfully !';
                                setTimeout(function () {
                                    window.location.reload();
                                }, 2000)
                            } else {
                                $scope.templateMsg = res.data;
                            }
                        });
                    } // else ended                     
                }
            });
        }//main else
    };

    $scope.generateImage4v = function (chartObj) {
        console.log(chartObj);
        $scope.tempData = {};
        $scope.tempData.SceneName = "";
        if (chartObj == 'template4v') {
            $scope.templateName = $scope.conTemp4v.tempName;
        }
        $scope.tempData.SceneName = $scope.templateName;

        if ($scope.tempData.SceneName == "") {
            $scope.templateMsg = "Template Name should not be empty"
        } else {
            var tcontent = document.getElementById(chartObj).innerHTML;
            html2canvas($('#template4v'), {
                onrendered: function (canvas) {
                    theCanvas = canvas;
                    document.body.appendChild(canvas);
                    $("#img-out").append(canvas);
                    var base64 = getBase64Image(canvas); //calling the getBase64Image function
                    console.log(base64);

                    $scope.tempData.templateId = $rootScope.sceneTempId
                    $scope.tempData.imgString = base64;
                    $scope.tempData.SceneContent = tcontent;
                    $scope.tempData.SceneType = "template";
                    $scope.tempData.IsActive = true;
                    $scope.tempData.IsPrimaryApproved = false;
                    $scope.tempData.SceneUrl = "";
                    $scope.tempData.Comments = "";
                    console.log($scope.tempData);

                    if ($scope.tempData.SceneName == "") {
                        $scope.templateMsg = "Template Name should not be empty"
                    } else {
                        //savedSceneservice service call started
                        sceneService.savedSceneService($scope.tempData).then(function (res) {
                            console.log(res);
                            $scope.res = res;
                            if (res.data == 'Success') {
                                $scope.templateMsg = 'Template Saved Successfully !';
                                setTimeout(function () {
                                    window.location.reload();
                                }, 2000)
                            } else {
                                $scope.templateMsg = res.data;
                            }
                        });
                    } // else ended                     
                }
            });
        }//main else
    };

    $scope.generateImage5 = function (chartObj) {
        console.log(chartObj);
        $scope.tempData = {};
        $scope.tempData.SceneName = "";
        if (chartObj == 'newTemp5') {
            $scope.templateName = $scope.conTemp5.tempName;
        }
        $scope.tempData.SceneName = $scope.templateName;

        if ($scope.tempData.SceneName == "") {
            $scope.templateMsg = "Template Name should not be empty"
        } else {
            var tcontent = document.getElementById(chartObj).innerHTML;
            html2canvas($('#newTemp5'), {
                onrendered: function (canvas) {
                    theCanvas = canvas;
                    document.body.appendChild(canvas);
                    // Convert and download as image 
                    //Canvas2Image.saveAsPNG(canvas);
                    $("#img-out").append(canvas);
                    var base64 = getBase64Image(canvas); //calling the getBase64Image function
                    console.log(base64);

                    $scope.tempData.templateId = $rootScope.sceneTempId
                    $scope.tempData.imgString = base64;
                    $scope.tempData.SceneContent = tcontent;
                    $scope.tempData.SceneType = "template";
                    $scope.tempData.IsActive = true;
                    $scope.tempData.IsPrimaryApproved = false;
                    $scope.tempData.SceneUrl = "";
                    $scope.tempData.Comments = "";
                    console.log($scope.tempData);

                    if ($scope.tempData.SceneName == "") {
                        $scope.templateMsg = "Template Name should not be empty"
                    } else {
                        //savedSceneservice service call started
                        sceneService.savedSceneService($scope.tempData).then(function (res) {
                            console.log(res);
                            $scope.res = res;
                            if (res.data == 'Success') {
                                $scope.templateMsg = 'Template Saved Successfully !';
                                setTimeout(function () {
                                    window.location.reload();
                                }, 2000)
                            } else {
                                $scope.templateMsg = res.data;
                                setTimeout(function () {
                                    window.location.reload();
                                }, 2000)
                            }
                        });
                    } // else ended                     
                }
            });
        }//main else
    };

    $scope.generateImage5v = function (chartObj) {
        console.log(chartObj);
        $scope.tempData = {};
        $scope.tempData.SceneName = "";
        if (chartObj == 'newTemp5v') {
            $scope.templateName = $scope.conTemp5v.tempName;
        }
        $scope.tempData.SceneName = $scope.templateName;

        if ($scope.tempData.SceneName == "") {
            $scope.templateMsg = "Template Name should not be empty"
        } else {
            var tcontent = document.getElementById(chartObj).innerHTML;
            html2canvas($('#newTemp5v'), {
                onrendered: function (canvas) {
                    theCanvas = canvas;
                    document.body.appendChild(canvas);
                    // Convert and download as image 
                    //Canvas2Image.saveAsPNG(canvas);
                    $("#img-out").append(canvas);
                    var base64 = getBase64Image(canvas); //calling the getBase64Image function
                    console.log(base64);

                    $scope.tempData.templateId = $rootScope.sceneTempId
                    $scope.tempData.imgString = base64;
                    $scope.tempData.SceneContent = tcontent;
                    $scope.tempData.SceneType = "template";
                    $scope.tempData.IsActive = true;
                    $scope.tempData.IsPrimaryApproved = false;
                    $scope.tempData.SceneUrl = "";
                    $scope.tempData.Comments = "";
                    console.log($scope.tempData);

                    if ($scope.tempData.SceneName == "") {
                        $scope.templateMsg = "Template Name should not be empty"
                    } else {
                        //savedSceneservice service call started
                        sceneService.savedSceneService($scope.tempData).then(function (res) {
                            console.log(res);
                            $scope.res = res;
                            if (res.data == 'Success') {
                                $scope.templateMsg = 'Template Saved Successfully !';
                                setTimeout(function () {
                                    window.location.reload();
                                }, 2000)
                            } else {
                                $scope.templateMsg = res.data;
                            }
                        });
                    } // else ended                     
                }
            });
        }//main else
    };

    $scope.generateImage6 = function (chartObj) {
        console.log(chartObj);
        $scope.tempData = {};
        $scope.tempData.SceneName = "";
        if (chartObj == 'newTemp6') {
            $scope.templateName = $scope.conTemp6.tempName;
        }
        $scope.tempData.SceneName = $scope.templateName;

        if ($scope.tempData.SceneName == "") {
            $scope.templateMsg = "Template Name should not be empty"
        } else {
            var tcontent = document.getElementById(chartObj).innerHTML;
            html2canvas($('#newTemp6'), {
                onrendered: function (canvas) {
                    theCanvas = canvas;
                    document.body.appendChild(canvas);
                    // Convert and download as image 
                    //Canvas2Image.saveAsPNG(canvas);
                    $("#img-out").append(canvas);
                    var base64 = getBase64Image(canvas); //calling the getBase64Image function
                    console.log(base64);

                    $scope.tempData.templateId = $rootScope.sceneTempId
                    $scope.tempData.imgString = base64;
                    $scope.tempData.SceneContent = tcontent;
                    $scope.tempData.SceneType = "template";
                    $scope.tempData.IsActive = true;
                    $scope.tempData.IsPrimaryApproved = false;
                    $scope.tempData.SceneUrl = "";
                    $scope.tempData.Comments = "";
                    console.log($scope.tempData);

                    if ($scope.tempData.SceneName == "") {
                        $scope.templateMsg = "Template Name should not be empty"
                    } else {
                        //savedSceneservice service call started
                        sceneService.savedSceneService($scope.tempData).then(function (res) {
                            console.log(res);
                            $scope.res = res;
                            if (res.data == 'Success') {
                                $scope.templateMsg = 'Template Saved Successfully !';
                                setTimeout(function () {
                                    window.location.reload();
                                }, 2000)
                            } else {
                                $scope.templateMsg = res.data;
                            }
                        });
                    } // else ended                     
                }
            });
        }//main else
    };

    $scope.generateImage6v = function (chartObj) {
        console.log(chartObj);
        $scope.tempData = {};
        $scope.tempData.SceneName = "";
        if (chartObj == 'newTemp6v') {
            $scope.templateName = $scope.conTemp6v.tempName;
        }
        $scope.tempData.SceneName = $scope.templateName;

        if ($scope.tempData.SceneName == "" || $scope.tempData.SceneName == undefined) {
            $scope.templateMsg = "Template Name should not be empty"
        } else {
            var tcontent = document.getElementById(chartObj).innerHTML;
            html2canvas($('#newTemp6v'), {
                onrendered: function (canvas) {
                    theCanvas = canvas;
                    document.body.appendChild(canvas);
                    // Convert and download as image 
                    //Canvas2Image.saveAsPNG(canvas);
                    $("#img-out").append(canvas);
                    var base64 = getBase64Image(canvas); //calling the getBase64Image function
                    console.log(base64);

                    $scope.tempData.templateId = $rootScope.sceneTempId
                    $scope.tempData.imgString = base64;
                    $scope.tempData.SceneContent = tcontent;
                    $scope.tempData.SceneType = "template";
                    $scope.tempData.IsActive = true;
                    $scope.tempData.IsPrimaryApproved = false;
                    $scope.tempData.SceneUrl = "";
                    $scope.tempData.Comments = "";
                    console.log($scope.tempData);

                    if ($scope.tempData.SceneName == "") {
                        $scope.templateMsg = "Template Name should not be empty"
                    } else {
                        //savedSceneservice service call started
                        sceneService.savedSceneService($scope.tempData).then(function (res) {
                            console.log(res);
                            $scope.res = res;
                            if (res.data == 'Success') {
                                $scope.templateMsg = 'Template Saved Successfully !';
                                setTimeout(function () {
                                    window.location.reload();
                                }, 2000)
                            } else {
                                $scope.templateMsg = res.data;
                            }
                        });
                    } // else ended                     
                }
            });
        }//main else
    };

    $scope.school_bg1 = "school_bg1";
    $scope.school_img2 = "school_img2";
    $scope.school_txt_mas = "school_txt_mas";
    $scope.june_txt = "june_txt";
    $scope.b2School = "b2School";
    $scope.shortDesc = "shortDesc";
    $scope.collect = "collect";

    $scope.generateImage12 = function (chartObj, weatherIcPos) {
        if ($scope.templateMsg == "Image size is greater than 300KB.Cannot save the template") {
            $scope.templateMsg = "Image size is greater than 300KB.Cannot save the template";
        }
        else {
            $scope.uploadMsg = "";
            console.log(chartObj);
            $scope.showWeatherIcon = false;
            $scope.tempData = {};
            if (weatherIcPos == null || weatherIcPos == undefined) {
                $scope.tempData.IconPosition = "";
                $scope.tempData.TemplateType = "";

            }
            else {
                $scope.tempData.IconPosition = weatherIcPos;
                $scope.tempData.TemplateType = "Time and Weather Template";
            }
            $scope.tempData.SceneName = "";
            if (chartObj == 'newTemp12') {
                $scope.tempData.SceneName = $scope.content12.tempName;
            }
            //$scope.tempData.SceneName = $scope.templateName;

            if ($scope.tempData.SceneName == "" || $scope.tempData.SceneName == undefined) {
                $scope.templateMsg = "Template Name should not be empty";
            } else {
                $scope.getTemplateInpBG12 = "getTemplateInpBG12Large";
                $scope.school_bg1 = "school_bg1Large";
                $scope.school_img2 = "school_img2Large";
                $scope.school_txt_mas = "school_txt_masLarge";
                $scope.june_txt = "june_txtLarge";
                $scope.b2School = "b2SchoolLarge";
                $scope.shortDesc = "shortDescLarge";
                $scope.collect = "collectLarge";
                $scope.ovr_tempImg2Class = "ovr_tempImg2New";
                $scope.ovr_mainTemplate2Class = "ovr_mainTemplate2New";
                var tcontent = document.getElementById(chartObj).innerHTML;
                html2canvas($('#newTemp12'), {
                    onrendered: function (canvas) {
                        theCanvas = canvas;
                        document.body.appendChild(canvas);
                        // Convert and download as image 
                        $("#img-out").append(canvas);
                        var base64 = getBase64Image(canvas); //calling the getBase64Image function

                        $scope.tempData.templateId = $rootScope.sceneTempId;
                        $scope.tempData.imgString = base64;
                        $scope.tempData.SceneContent = tcontent;
                        $scope.tempData.SceneType = "template";
                        $scope.tempData.IsActive = true;
                        $scope.tempData.IsPrimaryApproved = false;
                        $scope.tempData.SceneUrl = "";
                        $scope.tempData.Comments = "";

                        if ($scope.tempData.SceneName == "") {
                            $scope.templateMsg = "Template Name should not be empty"
                        } else {
                            //savedSceneservice service call started
                            sceneService.savedSceneService($scope.tempData).then(function (res) {
                                console.log(res);
                                $scope.res = res;
                                if (res.data == 'Success') {
                                    $scope.templateMsg = 'Template Saved Successfully !';
                                    setTimeout(function () {
                                        window.location.reload();
                                    }, 2000);
                                } else {
                                    $scope.templateMsg = res.data;
                                    setTimeout(function () {
                                        window.location.reload();
                                    }, 2000)
                                }
                            });
                        } // else ended                     
                    }
                });
            }//main else
        }

    };
    $scope.leftcard = "leftcard";
    $scope.generateImage14 = function (chartObj) {
        if ($scope.templateMsg == "Image size is greater than 300KB.Cannot save the template") {
            $scope.templateMsg = "Image size is greater than 300KB.Cannot save the template";
        }
        else {
            $scope.uploadMsg = "";
            console.log(chartObj);
            $scope.tempData = {};
            $scope.tempData.SceneName = "";
            if (chartObj == 'newTemp13') {
                $scope.tempData.SceneName = $scope.content13.tempName;
            }
            if (chartObj == 'newTemp14') {

                $scope.tempData.SceneName = $scope.content14.tempName;
            }
            if (chartObj == 'newTemp15') {
                $scope.tempData.SceneName = $scope.content15.tempName;
            }
            if (chartObj == 'newTemp15') {
                $scope.tempData.SceneName = $scope.content15.tempName;
            }
            if (chartObj == 'newTemp16') {
                $scope.tempData.SceneName = $scope.content16.tempName;
            }
            if (chartObj == 'newTemp17') {
                $scope.tempData.SceneName = $scope.content17.tempName;
            }
            if (chartObj == 'newTemp18') {
                $scope.tempData.SceneName = $scope.content18.tempName;
            }
            if (chartObj == 'newTemp19') {
                $scope.tempData.SceneName = $scope.content19.tempName;
            }
            //$scope.tempData.SceneName = $scope.templateName;

            if ($scope.tempData.SceneName == "" || $scope.tempData.SceneName == undefined) {
                $scope.templateMsg = "Template Name should not be empty";
            } else {
                if (chartObj == 'newTemp13') {
                    $scope.school_bg2 = "school_bg2Large";
                    $scope.school2_img2 = "school2_img2Large";
                    $scope.school2_txt_mas = "school2_txt_masLarge";
                    $scope.new_admission = "new_admissionLarge";
                    $scope.from_to_mas = "from_to_masLarge";
                    $scope.fromDate_mas = "fromDate_masLarge";
                    $scope.fromDate = "fromDateLarge";
                    $scope.fromDate1 = "fromDate1Large";
                    $scope.toDate_mas = "toDate_masLarge";
                    $scope.toDate = "toDateLarge";
                    $scope.toDate1 = "toDate1Large";
                    $scope.btm_block = "btm_blockLarge";
                    $scope.phoneNum = "phoneNumLarge";
                    $scope.add1 = "add1Large";
                    $scope.infoCard_mas = "infoCard_masLarge";
                    $scope.ovr_tempImg2Class = "ovr_tempImg2New";
                    $scope.ovr_mainTemplate2Class = "ovr_mainTemplate2New";

                }
                if (chartObj == 'newTemp14') {
                    $scope.school_bg3 = "school_bg3Large";
                    $scope.school3_img2 = "school3_img2Large";
                    $scope.s3_weare_mas = "s3_weare_masLarge";
                    $scope.s3_weare = "s3_weareLarge";
                    $scope.s3_back = "s3_backLarge";
                    $scope.bg_gradiBag = "bg_gradiBagLarge";
                    $scope.ovr_tempImg2Class = "ovr_tempImg2New";
                    $scope.ovr_mainTemplate2Class = "ovr_mainTemplate2New";
                }
                if (chartObj == 'newTemp15') {
                    $scope.coffee3_bg = "coffee3_bgLarge";
                    $scope.taste = "tasteLarge";
                    $scope.freshness = "freshnessLarge";
                    $scope.special1 = "special1Large";
                    $scope.coffee3_img2 = "coffee3_img2Large";
                    $scope.ovr_tempImg2Class = "ovr_tempImg2New";
                    $scope.ovr_mainTemplate2Class = "ovr_mainTemplate2New";

                }
                if (chartObj == 'newTemp16') {
                    $scope.coffee2_bg = "coffee2_bgLarge";
                    $scope.coffee2_img2 = "coffee2_img2Large";
                    $scope.getRich_block = "getRich_blockLarge";
                    $scope.getRich_block_mas = "getRich_block_masLarge";
                    $scope.getRich = "getRichLarge";
                    $scope.good_c_block = "good_c_blockLarge";
                    $scope.good_c_block2 = "good_c_block2Large";
                    $scope.ovr_tempImg2Class = "ovr_tempImg2New";
                    $scope.ovr_mainTemplate2Class = "ovr_mainTemplate2New";
                }
                if (chartObj == 'newTemp17') {
                    $scope.coffee1_bg3 = "coffee1_bg3Large";
                    $scope.coffee1_img2 = "coffee1_img2Large";
                    $scope.coffee1_txt_mas = "coffee1_txt_masLarge";
                    $scope.rotate = "rotateLarge";
                    $scope.leftcard = "leftcardLarge";
                    $scope.perfect_block = "perfect_blockLarge";
                    $scope.per_cof_block = "per_cof_blockLarge";
                    $scope.real_block = "real_blockLarge";
                    $scope.real_block2 = "real_block2Large";
                    $scope.ovr_tempImg2Class = "ovr_tempImg2New";
                    $scope.ovr_mainTemplate2Class = "ovr_mainTemplate2New";
                }
                if (chartObj == 'newTemp18') {
                    $scope.school_bg4 = "school_bg4Large";
                    $scope.red_left_bg = "red_left_bgLarge";
                    $scope.sc4_usa = "sc4_usaLarge";
                    $scope.sc4_excell = "sc4_excellLarge";
                    $scope.sc4_We_wel = "sc4_We_welLarge";
                    $scope.sc4_Educat = "sc4_EducatLarge";
                    $scope.sc4_white_bg = "sc4_white_bgLarge";
                    $scope.school4_img2 = "school4_img2Large";
                    $scope.ovr_tempImg2Class = "ovr_tempImg2New";
                    $scope.ovr_mainTemplate2Class = "ovr_mainTemplate2New";

                }
                if (chartObj == 'newTemp19') {
                    $scope.coffee4_bg = "coffee4_bgLarge";
                    $scope.cof4_cday = "cof4_cdayLarge";
                    $scope.cof4_law = "cof4_lawLarge";
                    $scope.cof4_add = "cof4_addLarge";
                    $scope.coffee4_img2 = "coffee4_img2Large";
                    $scope.ovr_tempImg2Class = "ovr_tempImg2New";
                    $scope.ovr_mainTemplate2Class = "ovr_mainTemplate2New";

                }
                var tcontent = document.getElementById(chartObj).innerHTML;
                var idchar = '#' + chartObj;
                console.log(idchar);
                html2canvas($(idchar), {
                    onrendered: function (canvas) {
                        theCanvas = canvas;
                        document.body.appendChild(canvas);
                        // Convert and download as image 
                        //Canvas2Image.saveAsPNG(canvas);
                        $("#img-out").append(canvas);
                        var base64 = getBase64Image(canvas); //calling the getBase64Image function
                        console.log(base64);

                        $scope.tempData.templateId = $rootScope.sceneTempId;
                        $scope.tempData.imgString = base64;
                        $scope.tempData.SceneContent = tcontent;
                        $scope.tempData.SceneType = "template";
                        $scope.tempData.IsActive = true;
                        $scope.tempData.IsPrimaryApproved = false;
                        $scope.tempData.SceneUrl = "";
                        $scope.tempData.Comments = "";

                        if ($scope.tempData.SceneName == "") {
                            $scope.templateMsg = "Template Name should not be empty"
                        } else {
                            //savedSceneservice service call started
                            sceneService.savedSceneService($scope.tempData).then(function (res) {
                                console.log(res);
                                $scope.res = res;
                                if (res.data == 'Success') {
                                    $scope.templateMsg = 'Template Saved Successfully !';
                                    setTimeout(function () {
                                        window.location.reload();
                                    }, 2000)
                                } else {
                                    $scope.templateMsg = res.data;
                                    setTimeout(function () {
                                        window.location.reload();
                                    }, 2000);
                                }
                            });
                        } // else ended                     
                    }
                });
            }//main else
        }

    };


    //converting image to Base64
    function getBase64Image(img) {
        console.log(img);
        var canvas = document.createElement("canvas");
        console.log(canvas);
        canvas.width = img.width;
        canvas.height = img.height;
        var ctx = canvas.getContext("2d");
        ctx.drawImage(img, 0, 0);
        var dataURL = canvas.toDataURL("image/png");
        return dataURL.replace(/^data:image\/(png|jpg);base64,/, "");
    };

    //converting the DOM content to image ended

    $scope.childInfo = function (user, event) {
        console.log(user, 'Hi 555')
        var scope = $scope.$new(true);
        scope.user = user;

        var link = angular.element(event.currentTarget),
            icon = link.find('.glyphicon'),
            tr = link.parent().parent(),
            table = $scope.vm.dtInstance.DataTable,
            row = table.row(tr);
        //
        if (row.child.isShown()) {
            icon.removeClass('glyphicon-minus-sign').addClass('glyphicon-plus-sign');
            row.child.hide();
            tr.removeClass('shown');
        } else {
            icon.removeClass('glyphicon-plus-sign').addClass('glyphicon-minus-sign');
            row.child($compile('<div tmpl class="clearfix"></div>')(scope)).show();
            tr.addClass('shown');
        }
    };

    //customTemplate started
    $scope.customTemplateinnerHTML = function () {
        $scope.tempData = {};
        $scope.tempData.SceneName = "";
        $scope.custName = document.getElementById('cusTempName').value;
        console.log($scope.custName);

        if ($scope.tempData.SceneName == "") {
            $scope.cusTempMsg = "Template Name should not be empty";
            $scope.tempData.SceneName = $scope.custName;
        };

        if ($scope.tempData.SceneName == "") {
            $scope.cusTempMsg = "Template Name should not be empty";
        } else {
            var tcontent = $("#gjs .gjs-frame").contents().find("body").html();
            html2canvas($("#gjs .gjs-frame").contents().find("body"), {
                onrendered: function (canvas) {
                    theCanvas = canvas;
                    document.body.appendChild(canvas);
                    // Convert and download as image 
                    //Canvas2Image.saveAsPNG(canvas);
                    $("#img-out").append(canvas);
                    var base64 = getBase64Image(canvas); //calling the getBase64Image function
                    //console.log(base64);

                    $scope.tempData.templateId = 123345;
                    $scope.tempData.imgString = base64;
                    $scope.tempData.SceneContent = tcontent;
                    $scope.tempData.SceneType = "template";
                    $scope.tempData.IsActive = true;
                    $scope.tempData.IsPrimaryApproved = false;
                    $scope.tempData.SceneName = $scope.custName;
                    $scope.tempData.SceneUrl = "";
                    $scope.tempData.Comments = "";
                    console.log($scope.tempData);

                    if ($scope.tempData.SceneName == "" || $scope.tempData.SceneName == null) {
                        $scope.cusTempMsg = "Template Name should not be empty"
                    } else {
                        //savedSceneservice service call started
                        sceneService.savedSceneService($scope.tempData).then(function (res) {
                            console.log(res);
                            $scope.res = res;
                            if (res.data == 'Success') {
                                $scope.cusTempMsg = 'Template Saved Successfully !';
                                setTimeout(function () {
                                    window.location.reload();
                                }, 2000)
                            } else {
                                $scope.cusTempMsg = res.data;
                            }
                        });
                    } // else ended                     
                }
            });
        }//main else
    };


});  //Controller ended

//childTable started
//directive started
dsDirectives.directive('tmpl', testComp);

function testComp($compile) {
    console.log('sss');
    var directive = {};

    directive.restrict = 'A';
    directive.templateUrl = '../../Views/Home/_child.cshtml';
    directive.transclude = true;
    directive.link = function (scope, element, attrs) {
    }
    return directive;
}
//childTable ended

//Modal directive
dsDirectives.directive('modalComp', function () {
    return {
        template: '<div class="modal fade">' +
        '<div class="modal-dialog cusWidth">' +
        '<div class="modal-content">' +
        '<div class="modal-header">' +
        '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>' +
        '<h4 class="modal-title">Image Preview</h4>' +
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
})
    .filter('typeFilter', function () {
        return function (thumblist, type) {
            if (!type) {
                return thumblist;
            }
            if (type == "All") {
                return thumblist;
            }
            var arr = [];
            angular.forEach(thumblist, function (v) {
                if (v.type === type) {
                    arr.push(v);
                }
            })

            return arr;
        }
    })
//service starts here
dsFactory.factory('sceneService', function ($http, $q) {
    return {
        //remove saved service
        removeSavedService: function (data) {
            console.log(data, 'Inside removed service')
            var defer = $q.defer();
            $http({
                url: '/Scene/DeleteScene',
                method: 'POST',
                data: { "sceneids": JSON.stringify(data) },
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

        //Submit Saved service
        submitSavedService: function (data) {
            var defer = $q.defer();
            $http({
                url: '/Scene/SubmitScene',
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

        //Saved Scene service
        savedSceneService: function (data) {
            console.log(data);
            var defer = $q.defer();
            $http({
                url: '/Scene/SaveScene',
                method: 'POST',
                data: { sceneTemplate: JSON.stringify(data) },
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

        //  Tracked Scenes
        getAllTrackedScenes: function () {
            return $http.get('/Scene/TrackScenes');
        },
    }
});
dsFactory.factory("entityService",
    ["akFileUploaderService", function (akFileUploaderService) {
        var saveTutorial = function (tutorial) {
            return akFileUploaderService.saveModel(tutorial, "/Scene/UploadScene");
        };
        return {
            saveTutorial: saveTutorial
        };
    }]);

