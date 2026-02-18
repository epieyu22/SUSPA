
(function () {
  'use strict';

  angular
    .module('app.turnos')
    .controller('TurnosCtrl', TurnosCtrl);

  TurnosCtrl.$inject = ['$state', 'data', 'TurnosSvc', '$scope', 'NotifySvc'];
  function TurnosCtrl( $state, data, TurnosSvc, $scope, NotifySvc) {

    var vm = this;
    vm.Add_Data_Turnos = Add_Data_Turnos;

    $scope.viewmodel = {};
    $scope.viewmodel.Cod_Empleados = [];
    vm.data = {};

    activate();
    
    function activate() {
      vm.data = data;
      
    }

    $scope.formatDate = function(date){
          var dateOut = new Date(date);
          return dateOut;
    };

    function Add_Data_Turnos() {     
      TurnosSvc.AddDataTurnos(vm.viewmodel)
        .then(function () {

        });
    }

    $scope.GetDataPorDepartamento = GetDataPorDepartamento;
    function GetDataPorDepartamento() {
      debugger
      TurnosSvc
        .GetDataPorDepartamento(vm.viewmodel.Cod_Depto)
        .then(function (data) {
          $scope.DptosEmpleado = data;
          console.log($scope.DptosEmpleado);
        }, function () {
          NotifySvc.error();
        });
    }

    $scope.limpiarFiltros = limpiarFiltros;
    function limpiarFiltros() {
      debugger
      if (document.getElementById("example-time-input") != null) {
        vm.viewmodel.Hora_Ent = "00:00";
        vm.viewmodel.Hora_Sal = "00:00";
      } else {
        vm.viewmodel.Turno_Real = "";
      }
    }

    $scope.ValidateDate = ValidateDate;
    function ValidateDate() {
      
      var fechainicial = vm.viewmodel.Fecha_Turno;
      var fechafinal = vm.viewmodel.Fecha_Turno_Hasta;
      vm.viewmodel.validador = vm.viewmodel.validador;
      if (Date.parse(fechafinal) < Date.parse(fechainicial)) {
        vm.viewmodel.Fecha_Turno = "";
        vm.viewmodel.Fecha_Turno_Hasta = "";
        alert("La fecha final debe ser mayor a la fecha inicial");
        vm.viewmodel.validador = 1;
      } else {
        vm.viewmodel.validador = 0;

      }
    }

    $scope.disable = function () {
      if (model.questions.length > 1) {
        return true;
      }
      else {
        return false;
      }
    };

   
  }
})();
