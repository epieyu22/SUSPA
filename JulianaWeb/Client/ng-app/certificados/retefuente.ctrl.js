(function() {
  'use strict';
  angular
    .module('app.certificados')
    .controller('RetefuenteCtrl', RetefuenteCtrl);

  RetefuenteCtrl.$inject = ['$rootScope', 'AuthSvc','CertificadosSvc', 'NotifySvc', 'config'];
    /* @ngInject */
  function RetefuenteCtrl($rootScope, AuthSvc, CertificadosSvc, NotifySvc, config) {
    var vm = this;
    vm.title = 'RetefuenteCtrl';
    vm.years = [];
    vm.retefuenteViemModel = {}
    vm.retefuenteViemModel.empleados = [];
    vm.generateRetefuente = generateRetefuente;
    activate();
      ////////////////
    function activate() {
      AuthSvc.getEmpleado().then(function(empleado){
      // La aplicación solo mostrara los certificados de retenciones desde el 2015
      // debido a que los templates anteriores, son diferente.
      // Posiblemente halla que modificar algo de aqui para que funcione para el año 2016
        var minYearToShow = 2015;
        var anoIngreso = empleado.Fec_Ingreso.substr(0,4);
        if(anoIngreso > minYearToShow){
          minYearToShow = anoIngreso;
        }
        
        var lastYear = new Date().getFullYear() - 1;
        //if(!config.Mostrar_Ano_Anterior){
        //  lastYear--;
        //}
        
        vm.retefuenteViemModel.anoContable = lastYear;
        while (lastYear >= minYearToShow) {
          vm.years.push(lastYear);
          lastYear--;
        }
        vm.retefuenteViemModel.empleados.push(empleado);
      });
      vm.retefuenteViemModel.ByCedula = config.Unificar_Contratos;
      vm.retefuenteViemModel.redondear = config.Redondear_Renglones;
    }

    function generateRetefuente() {
      $rootScope.generating = true;
      CertificadosSvc.generateRetefuente(vm.retefuenteViemModel)
        .then(function(){
            $rootScope.generating = false;
          },
          function(error){
            $rootScope.generating = false;
            NotifySvc.error();
          })
    }
  }


})();
