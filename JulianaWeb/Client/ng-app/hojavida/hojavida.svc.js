(function() {
  'use strict';
  angular
    .module('app.hojavida')
    .factory('HojavidaSvc', HojavidaSvc);

  HojavidaSvc.$inject = ['$http', '$q', '$resource', 'AuthSvc', '$state'];
  /* @ngInject */
  function HojavidaSvc($http, $q, $resource, AuthSvc, $state) {

    //No se que metodos utilizo el otro programador pero ahi vamos
    /*
     * IMPORTANTE: Los metodos PUT y DELETE no son aceptados por el servidor actual. Para dichos metodos se tiene que usar POST y GET hasta
     * resolver el problema 
     */


    //Variables de agregación
    var referencias = $resource(
      ROOTURL + 'API/HojaVida/:Empresa/:Cod_HojaVida/Referencias/:id',
      { Empresa: AuthSvc.data.DBName, id: '@Cod_Referencia_Personal' },
      {
        update: { method: 'POST' }
      }
    );

    //Variables de eliminacion
    var idiomasDelete = $resource(
      ROOTURL + 'API/HojaVida/:Empresa/:Cod_HojaVida/IdiomasHojaVida/:id/Delete',
      { Empresa: AuthSvc.data.DBName, id: '@Cod_Idioma_Hojavida' },
      {
        update: { method: 'POST' }
      }
    );
    var referenciasDelete = $resource(
      ROOTURL + 'API/HojaVida/:Empresa/:Cod_HojaVida/Referencias/:id/Delete',
      { Empresa: AuthSvc.data.DBName, id: '@Cod_Referencia_Personal' },
      {
        update: { method: 'POST' }
      }
    );
    
    var formacionDelete = $resource(
      ROOTURL + 'API/HojaVida/:Empresa/FormacionAcademica/:id/Delete',
      { Empresa: AuthSvc.data.DBName, id: '@Cod_FormAcademica'  },
      {
        update: { method: 'POST' }
      }
    );

    var experienciaDelete = $resource(
      ROOTURL + 'API/HojaVida/:Empresa/Experiencia/:id/Delete/Exp',
      { Empresa: AuthSvc.data.DBName, id: '@Cod_ExpLaboral' },
      {
        update: { method: 'POST' }
      }
    );
      
    


    //Variables para obtener datos
   
    var IdiomasHojaVida = $resource(
      ROOTURL + 'API/HojaVida/:Empresa/:Cod_HojaVida/IdiomasHojaVida/:Cod_Idioma_Hojavida',
      { Empresa: AuthSvc.data.DBName, id: '@Cod_Idioma_Hojavida' },
      {
        update: { method: 'GET' }
      }
    );
    var ExpLabroal = $resource(
      ROOTURL + 'API/HojaVida/:Empresa/:Cod_HojaVida/ExpLaboral/:Cod_ExpLaboral',
      { Empresa: AuthSvc.data.DBName, id: '@Cod_ExpLaboral' },
      {
        update: { method: 'GET' }
      }
    );
    var ForAcademica = $resource(
      
      ROOTURL + 'API/HojaVida/:Empresa/:Cod_HojaVida/GetFormacademica',
      { Empresa: AuthSvc.data.DBName, Cod_HojaVida: '@Cod_HojaVida' },
      {
        update: { method: 'GET' }
      }
    );
    
    var service = {
      //Invocacion de servicios get
      getHojavidaData: getHojavidaData,
      getHojavidaTables: getHojavidaTables,
      getHojavidaAprobar: getHojavidaAprobar,
      getFormacademica: getFormacademica,
      referencias: referencias,
      ForAcademica: ForAcademica,
      ExpLabroal: ExpLabroal,
      //Invocacion de servicios delete
      formacionDelete: formacionDelete,
      referenciasDelete: referenciasDelete,
      idiomasDelete: idiomasDelete,
      experienciaDelete: experienciaDelete,
      //Invocacion de servicios update
      Update_Hojavida: Update_Hojavida,

      //Invocacion de servicios add 
      
      IdiomasHojaVida: IdiomasHojaVida,
      
      getHojavida:getHojavida,
      aprobarAspirante: aprobarAspirante,
      changeImage: changeImage,
      rechazarAspirante: rechazarAspirante,
      AddFormAcademica: AddFormAcademica
    };

    return service;

    function getHojavidaData() {
      var deferred = $q.defer();
      var Cedula = AuthSvc.data.UserName;
      var Empresa = AuthSvc.data.DBName;
      var url = ROOTURL + 'API/HojaVida/' + Empresa.trim() + '/' + Cedula;
      $http.get(url)
        .success(function(data){
          deferred.resolve(data);
        });
      return deferred.promise;
    }

    function getHojavidaAprobar() {
      var deferred = $q.defer();
      var Empresa = AuthSvc.data.DBName;
      var url = ROOTURL + 'API/HojaVida/' + Empresa.trim() + '/Aspirantes';
      $http.get(url)
        .success(function(data){
          deferred.resolve(data);
        });
      return deferred.promise;
    }

    function getHojavidaTables(){
      var deferred = $q.defer();
      var Empresa = AuthSvc.data.DBName;
      var url = ROOTURL + 'API/HojaVida/' + Empresa;
      $http.get(url)
        .success(function(data){
          deferred.resolve(data);
        });
      return deferred.promise;
    }

    function Update_Hojavida(Cod_HojaVida, data) {
      var deferred = $q.defer();
      var Empresa = AuthSvc.data.DBName;
      var url = ROOTURL + 'API/HojaVida/' + Empresa.trim() + '/' + Cod_HojaVida;
      $http.post(url, data)
        .success(function(){
          deferred.resolve();
        })
        .error(function(){ deferred.reject(); });
      return deferred.promise;
    }


   

    function getHojavida(Cedula){
      var deferred = $q.defer();
      var Empresa = AuthSvc.data.DBName;
      var url = ROOTURL + 'API/HojaVida/' + Empresa.trim() + '/' + Cedula;
      $http.get(url)
        .success(function(data){
          deferred.resolve(data);
        });
      return deferred.promise;
    }

    function getFormacademica(Cod_HojaVida){
      var deferred = $q.defer();
      var Empresa = AuthSvc.data.DBName;
      var url = ROOTURL + 'API/HojaVida/' + Empresa.trim() + '/' + Cod_HojaVida + '/Formacademica';
      $http.get(url)
        .success(function(data){
          deferred.resolve(data);
        });
      return deferred.promise;
    }

    function AddFormAcademica(data, file) {
      var deferred = $q.defer();
      debugger
      var fd = new FormData();
      fd.append('file', file);
      fd.append('Cod_HojaVida', file.Cod_HojaVida);
      fd.append('Cod_Institucion', file.Cod_Institucion);
      fd.append('Cod_Titulo', file.Cod_Titulo);
      fd.append('Cod_Nivel', file.Cod_Nivel);
      fd.append('Inicio', file.Inicio);
      fd.append('Salida', file.Salida);
      fd.append('Estado', file.Estado);
      fd.append('Otra_Institucion', file.Otra_Institucion);
      fd.append('Otro_Titulo', file.Otro_Titulo);
      var Empresa = AuthSvc.data.DBName;
      var url = ROOTURL + 'API/HojaVida/' + Empresa.trim() + '/' + file.Cod_HojaVida  +'/FormaAcademica';
      $http.post(url, fd, {headers: {'Content-Type': undefined, transformRequest: angular.identity,}})
        .success(function (response) {          
          $state.go($state.current, {}, { reload: true });
        })
        .error(function (error) {
          deferred.reject(error);
        });
      return deferred.promise;
    }

    function aprobarAspirante(data) {
      var deferred = $q.defer();
      var Empresa = AuthSvc.data.DBName;
      var url = ROOTURL + 'API/HojaVida/' + Empresa.trim() + '/Aspirante/Aprobar';
      $http.post(url, data)
        .success(function(data){
          deferred.resolve(data);
        })
        .error(function(msg){
          deferred.reject(msg);
        });
      return deferred.promise;
    }

    function rechazarAspirante(Cod_HojaVida) {
      var deferred = $q.defer();
      var Empresa = AuthSvc.data.DBName;
      var url = ROOTURL + 'API/HojaVida/' + Empresa.trim() + '/Aspirante/' + Cod_HojaVida;
      $http.delete(url)
        .success(function(data){
          deferred.resolve();
        })
        .error(function(msg){
          deferred.reject(msg);
        });
      return deferred.promise;
    }

    function changeImage(Cod_HojaVida, image){
      var deferred = $q.defer();
      var fd = new FormData();
      fd.append('file', image);
      var Empresa = AuthSvc.data.DBName;
      var url = ROOTURL + 'API/HojaVida/' + Empresa.trim() + '/'+ Cod_HojaVida + '/ChangeProfilePicture';
      $http.post(url, fd, {headers: {'Content-Type': undefined, transformRequest: angular.identity,}})
        .success(function (response) {
          deferred.resolve();
        })
        .error(function (error) {
          deferred.reject();
        });
      return deferred.promise;
    }

  }
})();
