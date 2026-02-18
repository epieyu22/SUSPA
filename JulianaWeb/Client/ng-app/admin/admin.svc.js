(function() {
'use strict';

  angular
    .module('app.admin')
    .factory('AdminSvc', AdminSvc);

  AdminSvc.$inject = ['$http', '$state', '$q', 'AuthSvc', 'NotifySvc','$resource'];
  function AdminSvc($http, $state, $q, AuthSvc, NotifySvc, $resource) {


    var service = {
      Get_Permission:          Get_Permission,
      Get_Roles:               Get_Roles,
      Get_Users:               Get_Users,
      Assign_Permissions:      Assign_Permissions,
      Assing_Roles_to_Users:   Assing_Roles_to_Users,
      Change_Password:         Change_Password,
      Create_Role:             Create_Role,
      Get_Aprobadores_Empresa: Get_Aprobadores_Empresa,
      Add_Aprobadores_Empresa: Add_Aprobadores_Empresa,
      Get_Filter_Tables_Data:  Get_Filter_Tables_Data,
      Get_Aprobacion_Config: Get_Aprobacion_Config,
      Delete_Aprobador: Delete_Aprobador,
      getVersion,
      Reporte_Aprobadores_Excel
    };
    
    return service;

    ////////////////   

    function Get_Permission(){
      var url = ROOTURL + 'API/Account/Permission';
      return _Simple_Get(url);
    }

    function Get_Roles() { 
      var url = ROOTURL + 'API/Account/Roles';
      return _Simple_Get(url);
    }

    function Get_Users(){
      var url = ROOTURL + 'API/Account';
      return _Simple_Get(url);
    }

    function Assign_Permissions(roleId, roles){
      var url = ROOTURL + 'API/Account/Roles/'+roleId+'/Permissions';
      $http
        .post(url, roles)
        .success(function (data) {
            NotifySvc.success();
        })
        .error(function(){
          NotifySvc.error();
        });
    }

    function Assing_Roles_to_Users(viewmodel){
      var deferred = $q.defer();
      var url = ROOTURL + 'API/Account/Roles/Assign';
      $http
        .post(url, viewmodel)
        .success(function (data) {
            NotifySvc.success();
            deferred.resolve(data);
        })
        .error(function(){
          NotifySvc.error();
        });
      return deferred.promise;
    }

    function Change_Password(viewmodel){
      var url = ROOTURL + 'API/Account/'+AuthSvc.data.UserName+'/ChangePassword';
      $http
        .post(url, viewmodel)
        .success(function (data) {
          if(data.error){
            NotifySvc.error(data.error);
          }else{
            NotifySvc.success();
            $state.go('dashboard');
          }
        })
        .error(function(){
          NotifySvc.error();
        });
    }


    function Create_Role(Rolename){
      var url = ROOTURL + 'API/Account/Roles';
      $http
        .post(url, Rolename)
        .success(function (data) {
          if(data.error){
            NotifySvc.error(data.error);
          }else{
            NotifySvc.success();
            $state.go($state.current
            , {}, {reload: true});
          }
        })
        .error(function(){
          NotifySvc.error();
        });
    }


    function Get_Aprobadores_Empresa(empresa, page, count, docEmpleado, carIdEmpleado, empleado, docAprobador, carIdAprobador, aprobador) {
      var deferred = $q.defer();
      var url = ROOTURL + 'API/Admin/Aprobaciones/' + empresa + '/Aprobadores?page=' + page + '&count=' + count
        + '&docEmpleado=' + docEmpleado + '&carIdEmpleado=' + carIdEmpleado + '&empleado=' + empleado
        + '&docAprobador=' + docAprobador + '&carIdAprobador=' + carIdAprobador + '&aprobador=' + aprobador;
      $http
        .get(url)
        .success(function (data) {
            deferred.resolve(data);
        })
        .error(function(){
          NotifySvc.error();
        });
      return deferred.promise;
    }



    function Add_Aprobadores_Empresa(Empresa, data){
      var deferred = $q.defer();
      var url = ROOTURL + 'API/Admin/Aprobaciones/'+Empresa+'/Aprobadores';
      $http
        .post(url, data)
        .success(function () {
            NotifySvc.success();
            $state.go($state.current, {}, {reload: true});
        })
        .error(function(){
          NotifySvc.error();
        });
      return deferred.promise;
    }


    function Delete_Aprobador(Empresa, data) {
      
      var deferred = $q.defer();
      var url = ROOTURL + 'API/Admin/Aprobaciones/'+Empresa+'/Retirar';
      $http
        .post(url, data)
        .success(function () {
          NotifySvc.success();
          $state.go($state.current, {}, { reload: true });
        })
        .error(function () {
          NotifySvc.error();
        });
      return deferred.promise;
    }


    function Get_Filter_Tables_Data(Empresa){
      var deferred = $q.defer();
      var url = ROOTURL + 'API/Admin/FilterTables/'+Empresa;
      $http
        .get(url)
        .success(function (data) {
            deferred.resolve(data);
        })
        .error(function(){
          NotifySvc.error();
        });
      return deferred.promise;
    }


    function Get_Aprobacion_Config(Empresa, Filtro, Cod_Filtro, Tipo_Aprobacion){
      var deferred = $q.defer();
      var url = ROOTURL + 'API/Admin/Aprobaciones/' + Empresa.trim() + '/Config/' + Tipo_Aprobacion + '/' + Filtro + '/' + Cod_Filtro;
      $http
        .get(url)
        .success(function (data) {
            deferred.resolve(data);
        })
        .error(function(){
          NotifySvc.error();
        });
      return deferred.promise;
    }

    function getVersion() {
      var deferred = $q.defer();
      var url = `${ROOTURL}API/Admin/version`;
      $http
        .get(url)
        .success(function (data) {
          deferred.resolve(data);
        })
        .error(function () {
          NotifySvc.error();
        });
      return deferred.promise;
    }

    function Reporte_Aprobadores_Excel(viewmodel) {
      var deferred = $q.defer();
      var url = ROOTURL + 'Excel/Aprobadores/';
      $http.post(url, viewmodel, { responseType: 'arraybuffer' })
        .success(function (data) {
          var file = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
          saveAs(file, "ReporteAprobadores.xlsx");
          deferred.resolve();
        })
        .error(function (data) { deferred.reject(data) });
      return deferred.promise;
    }

    /////////////////////
    // Private Methods //
    /////////////////////
    

    function _Simple_Get(url){
      var deferred = $q.defer();
      $http
        .get(url)
        .success(function(data){
          deferred.resolve(data);
        })
        .error(function(){
          deferred.reject();
        });
      return deferred.promise;
    }


  }
})();
