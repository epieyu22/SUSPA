(function () {
  'use strict';

  angular
    .module('app.dashboardBi')
    .factory('DashboardBiSvc', DashboardBiSvc);

  DashboardBiSvc.$inject = ['$http', '$q'];
  function DashboardBiSvc($http, $q) {
    var service = {
     /* IngresarSolicitud: IngresarSolicitud,*/
    };

    return service;

    ////////////////
    //function IngresarSolicitud(viewmodel) {
    //  var deferred = $q.defer();
    //  var url = ROOTURL + 'API/SolPersonal/Reporte/' + viewmodel.Empresa.trim() + '/IngresarSolicitud';
    //  $http
    //    .post(url, viewmodel)
    //    .success(function (data) {
    //      deferred.resolve(data);
    //    })
    //    .error(function () {
    //    });
    //  return deferred.promise;
    //}
  }
})();
