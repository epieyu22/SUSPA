(function () {
  'use strict';

  angular
    .module('app.facElectronica')
    .controller('facElectronicaCtrl', facElectronicaCtrl);

  facElectronicaCtrl.$inject = ['$rootScope','ngDialog', 'NgTableParams', 'data', 'NotifySvc', 'facElectronicaSvc', 'Empresas', '$scope'];
  function facElectronicaCtrl($rootScope, ngDialog, NgTableParams, data, NotifySvc, facElectronicaSvc, Empresas, $scope) {
    var vm = this;
    vm.loading = false;
    vm.tableSelectAll = false;
    vm.responses = [];
    vm.log = log;
    vm.totales = null;
    vm.employees = [];
    vm.errors = [];
    vm.validations = {};

    //Vector de meses   
    vm.MESES = [{
      name: 'Enero',
      value: '01'
    },
    {
      name: 'Febrero',
      value: '02'
    },
    {
      name: 'Marzo',
      value: '03'
    },
    {
      name: 'Abril',
      value: '04'
    },
    {
      name: 'Mayo',
      value: '05'
    },
    {
      name: 'Junio',
      value: '06'
    },
    {
      name: 'Julio',
      value: '07'
    },
    {
      name: 'Agosto',
      value: '08'
    },
    {
      name: 'Septiembre',
      value: '09'
    },
    {
      name: 'Octubre',
      value: '10'
    },
    {
      name: 'Noviembre',
      value: '11'
    },
    {
      name: 'Diciembre',
      value: '12'
    },
    ]


    activate();

    $scope.filteredTodos = []
      , $scope.currentPage = 1
      , $scope.numPerPage = 5
      , $scope.maxSize = 10;

    function log() {
      if (vm.tableSelectAll) {
        vm.viewmodel.empleados = vm.employees;
        for (let empleado of vm.viewmodel.empleados) {
          empleado['selected'] = true;
        }
      }
      else {
        for (let empleado of vm.viewmodel.empleados) {
          empleado['selected'] = false;
        }
        vm.viewmodel.empleados = [];
      }
    }

    $scope.makeTodos = function () {
      $scope.todos = [];
      for (i = 1; i <= 1000; i++) {
        $scope.todos.push({ text: "todo " + i, done: false });
      }
    };

    $scope.setPage = function (pageNo) {
      $scope.currentPage = pageNo;
    };

    $scope.pageChanged = function () {
      $log.log('Page changed to: ' + $scope.currentPage);
    };
    $scope.makeTodos();

    $scope.$watch("currentPage + numPerPage", function () {
      var begin = (($scope.currentPage - 1) * $scope.numPerPage)
        , end = begin + $scope.numPerPage;

      $scope.filteredTodos = $scope.todos.slice(begin, end);
    });
    ////////////////

    function activate() {
      vm.data = data;
      vm.Empresas = Empresas;
      initGrids();

    }
    //Inicializacion de los grid para estilos de bootstrap
    function initGrids() {
      vm.facturacionGrid = new NgTableParams({}, { dataset: vm.data.employees });
    }
    vm.LoadEmployes = LoadEmployes;
    function LoadEmployes() {
      vm.loading = true;
      var Empresa = vm.viewmodel.Empresa.trim();      
      vm.viewmodel.empleados = [];      
      facElectronicaSvc.LoadEmployees(vm.viewmodel)
        .then(function (data) {
          vm.employees = data.employees;
          vm.totales = data.totales;
          vm.facturacionGrid = new NgTableParams(
          {},
          {
            dataset: data.employees = data.employees.map(function (employees) {              
              for (var i = 0; i < data.reponses.length; i++) {                
                if (employees.Cod_Empleado == data.reponses[i].Number) {                  
                  employees.responses = data.reponses[i];
                  break;
                }
              }
              return employees;
            })
          }
        );
          vm.totales['employees'] = data.employees.length;
      });
    }

    vm.LoadBilling = LoadBilling;
    function LoadBilling() {
      vm.errors = [];
      vm.validations = {};
      vm.loading = true;
      $rootScope.generatingPayroll = true;
      facElectronicaSvc
        .IngresarSolicitud(vm.viewmodel)
        .then(function () {
          vm.loading = false;
          $rootScope.generatingPayroll = false;
          NotifySvc.successFilePayrollElec();
        })
        .catch(function (fail) {
          if (fail?.validations)
            vm.validations = fail.validations;
          if (fail?.errors?.length)
            vm.errors = fail.errors;
          $rootScope.generatingPayroll = false;
          NotifySvc.error();
        });
    }
  }
})();
