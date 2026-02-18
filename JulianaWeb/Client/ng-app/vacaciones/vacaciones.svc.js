(function() {
'use strict';

angular
  .module('app.vacaciones')
  .factory('VacacionesSVC', VacacionesSVC);

VacacionesSVC.$inject = ['$http', '$q'];
function VacacionesSVC($http, $q) {
  var service = {
    Get_Reporte_Vacaciones: Get_Reporte_Vacaciones,
    Solicitudes_2_Excel: Solicitudes_2_Excel, 
    Reporte_2_Excel: Reporte_2_Excel,
    Mail_Solicitudes_Pendientes: Mail_Solicitudes_Pendientes
  };
  
  return service;

  ////////////////
  function Get_Reporte_Vacaciones(viewmodel) {
    
    var deferred = $q.defer();
    var url = ROOTURL + 'API/Vacaciones/Reporte/'+viewmodel.Empresa;
    $http
      .post(url, viewmodel)
      .success(function (data) {
          deferred.resolve(data);
      })
      .error(function(){
      });
    return deferred.promise;
  }
  
  function Solicitudes_2_Excel(viewmodel) { 
    var deferred = $q.defer();
    var url = ROOTURL + 'Excel/Solicitudes/';
    $http.post(url, viewmodel,  { responseType: 'arraybuffer' })
      .success(function (data) {
        var file = new Blob([data], {type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'});
          saveAs(file, "Solicitudes.xlsx");
          deferred.resolve();
      })
      .error(function (data){deferred.reject(data)});
    return deferred.promise;
  }


  function Reporte_2_Excel(viewmodel) { 
    var deferred = $q.defer();
    var url = ROOTURL + 'Excel/ReporteVacaciones/';
    $http.post(url, viewmodel,  { responseType: 'arraybuffer' })
      .success(function (data) {
        var file = new Blob([data], {type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'});
          saveAs(file, "ReporteVacaciones.xlsx");
          deferred.resolve();
      })
      .error(function (data){deferred.reject(data)});
    return deferred.promise;
  }


  function Mail_Solicitudes_Pendientes(viewmodel){
    var deferred = $q.defer();
    var url = ROOTURL + 'API/Solicitudes/Mail_Solicitudes_Pendientes/'+viewmodel.Empresa;
    $http
      .get(url)
      .success(function (data) {
          deferred.resolve(data);
      })
      .error(function(){
      });
    return deferred.promise;
  }

}
})();
