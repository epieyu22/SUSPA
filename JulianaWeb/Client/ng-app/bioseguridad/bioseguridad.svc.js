(function () {
  'use strict';

  angular
    .module('app.bioseguridad')
    .factory('BioseguirdadSvc', BioseguirdadSvc);

  BioseguirdadSvc.$inject = ['$http', '$q', '$resource', 'AuthSvc', 'NotifySvc'];
  function BioseguirdadSvc($http, $q, $resource, AuthSvc, NotifySvc) {
    
    var service = {
      AddRegistroPreguntas: AddRegistroPreguntas
    };

    return service;
    
    function AddRegistroPreguntas(data) {
      var deferred = $q.defer();
      var Cedula = AuthSvc.data.UserName;
      var url = ROOTURL + 'API/Bioseguridad/' + Cedula + '/AddRespuestas' ;
      $http
        .post(url, data)
        .success(function (data) {
          $state.go(
            $state.current,
            {},
            {
              reload: true
            }
          );
          NotifySvc.success();
        
        })
        .error(function (status, error) {
          NotifySvc.errorPrueba(Message, error);
          deferred.reject(status, error);
        });
      return deferred.promise;
    }

  }
})();
