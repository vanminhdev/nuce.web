
mainModule.factory("localStorageSvc", ["$window", "$q", function ($window, $q) {
    return {

        setKey: function (_key, _value) {
            if (window.localStorage) {
                //Local Storage to add Data  
                localStorage.setItem(_key, _value);
            }
        },

        getKey: function (_key) {
            //Get data from Local Storage  
            var item = angular.fromJson(localStorage.getItem(_key)); //  angular.fromJson(localStorage.getItem("employees")); 
            return item ? item : null;
        },

        setItem: function (_key, _value) {
            if (window.localStorage) {
                //Local Storage to add Data  
                localStorage.setItem(_key, angular.toJson(_value));
            }
        },

        getItem: function (_key) {
            //Get data from Local Storage  
            var item = angular.fromJson(localStorage.getItem(_key));
            return item ? item : null;
        }

    };



}]);