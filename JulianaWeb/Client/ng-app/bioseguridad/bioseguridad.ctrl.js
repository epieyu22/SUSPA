(function () {
  'use strict';

  angular
    .module('app.bioseguridad')
    .controller('BioseguirdadCtrl', BioseguirdadCtrl);

  BioseguirdadCtrl.$inject = ['ngDialog', '$state', 'data', 'NotifySvc', 'BioseguirdadSvc', 'Empresas','$scope'];
  function BioseguirdadCtrl(ngDialog, $state, data, NotifySvc, BioseguirdadSvc, Empresas, $scope) {
    // Variables locales
    var vm = this;
    
    vm.bioseguirdad = {};
    if (vm.bioseguirdad)
    // Forms view model
    vm.Guardar_reporte = Guardar_reporte;

    activate();
        
    function activate() {
      vm.data = data;
      vm.Empresas = Empresas;
    }
   
    function Guardar_reporte() {
      BioseguirdadSvc.AddRegistroPreguntas(vm.bioseguirdad)
        .then(function () {
          NotifySvc.success();
          $state.go($state.current, {}, { reload: true });
        }, function () {
          NotifySvc.error();
        });
    }
  }
})();
