mainModule.factory("sessionStorageSvc", ["$window", "$q", function ($window, $q) {
    function getStorageKey(key) {
        var value;
        try {
            value = angular.fromJson($window.sessionStorage[key]);
        }
        catch(e) {
            value = null;
        }
        return value;
    }

    function setStorageKey(key, value) {        
        try {
            $window.sessionStorage[key] = angular.toJson(value);
        }
        catch (e) {
            $window.sessionStorage[key] = undefined;
        }
    }


    return {
        get: function (key, getRequestPromise) {
            var deferred = $q.defer();

            var staticData = getStorageKey(key);
            if (staticData)
                deferred.resolve(staticData);            
            else {
                getRequestPromise().then(function (response) {
                    if (response.data && response.data.Success) {
                            staticData = response.data.Value;
                            setStorageKey(key, staticData);
                            deferred.resolve(staticData);
                        }
                        else {
                            deferred.reject(null);
                        }
                    }, function (error) {
                        deferred.reject(error);
                });
            }                

            return deferred.promise;
        },

        remove: function (key) {
            delete $window.sessionStorage.removeItem(key);
        }
    }
}]);