(function () {
  'use strict';

  angular
    .module('app.solpersonal')
    .factory('SolPersonalSvc', SolPersonalSvc);

  SolPersonalSvc.$inject = ['$http', '$q'];
  function SolPersonalSvc($http, $q) {
    var service = {
      IngresarSolicitud: IngresarSolicitud,
    };

    return service;

    ////////////////
    function IngresarSolicitud(viewmodel) {
      var deferred = $q.defer();
      var url = ROOTURL + 'API/SolPersonal/Reporte/' + viewmodel.Empresa.trim() + '/IngresarSolicitud';
      $http
        .post(url, viewmodel)
        .success(function (data) {
          deferred.resolve(data);
        })
        .error(function () {
        });
      return deferred.promise;
    }
  }
})();
