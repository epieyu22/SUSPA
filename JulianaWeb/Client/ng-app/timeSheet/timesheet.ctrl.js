(function () {
  'use strict';

  angular
    .module('app.timesheet')
    .controller('TimeSheetCtrl', TimeSheetCtrl);

  TimeSheetCtrl.$inject = ['ngDialog', '$state', 'data', 'NotifySvc', 'TimeSheetSvc', 'Empresas','$scope'];
  function TimeSheetCtrl(ngDialog, $state, data, NotifySvc, TimeSheetSvc, Empresas, $scope) {
    // Variables locales
    var vm = this;
    vm.horas = [
      { value: "1", name: "1" },
      { value: "2", name: "2" },
      { value: "3", name: "3" },
      { value: "4", name: "4" },
      { value: "5", name: "5" },
      { value: "6", name: "6" },
      { value: "7", name: "7" },
      { value: "8", name: "8" },
      { value: "9", name: "9" },
      { value: "10", name: "10" },
      { value: "11", name: "11" },
      { value: "12", name: "12" }
    ];
    $scope.fechaActual = new Date();

    vm.timesheet = {};

    // Forms view model
    vm.add_registro_horas = add_registro_horas;

    activate();

    
    function activate() {
      vm.data = data;
      vm.Empresas = Empresas;
    }
   

    function add_registro_horas() {
      TimeSheetSvc.AddRegistroHoras(vm.timesheet)
        .then(function () {
          NotifySvc.success();
          $state.go($state.current, {}, { reload: true });
        }, function () {
          NotifySvc.error();
        });
    }
  }
})();
