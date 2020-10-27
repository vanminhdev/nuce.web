//var mainModule = angular.module('frApp', ["ui.bootstrap", "cgBusy","ngClipboard"]); // ngAnimate

// angular cbBusy loading indicator
var mainModule = angular.module('frApp', ["ui.bootstrap", "cgBusy"]).value('cgBusyDefaults', {
    message: 'Loading...',
    backdrop: true,
    //templateUrl: 'my_custom_template.html',
    delay: 0, // khoảng thời gian trễ để show loading khi bắt đầu promise
    minDuration: 500 // khoảng thời gian trễ sau khi promise được hoàn thành để hủy show loading
    //wrapperClass: 'my-class my-class2'
});

mainModule.run(['$rootScope', '$location', function ($rootScope, $location) {
    $rootScope.$on('$locationChangeSuccess', function (event, newUrl, oldUrl) { // $locationChangeStart
        event.preventDefault();
        if (newUrl.indexOf("survey/list") != -1) {
            var isReloaded = localStorage.IsReloaded + "";
            if (isReloaded == false || isReloaded == "false") {
                localStorage.IsReloaded = true;
                location.reload();
            }
        }
    });
    $rootScope.$watch(function () { return $location.absUrl(); }, function (newLocation, oldLocation) {
    
    });

}]);



