(function() {
  'use strict';

  angular.module('app.certificados').controller('RetefuenteAdminCtrl', RetefuenteAdminCtrl);

  RetefuenteAdminCtrl.$inject = ['$rootScope', 'NgTableParams', 'Certifica2Svc', 'empresas'];
  function RetefuenteAdminCtrl($rootScope, NgTableParams, Certifica2Svc, empresas) {
    var vm = this;
    vm.data = {};
    vm.viewmodel = {};
    vm.viewmodel.empleados = [];
    vm.years = [];

    vm.loading = false;

    vm.Get_Config_Retefuente = Get_Config_Retefuente;
    vm.Get_Empleados_Retefuente_Ano = Get_Empleados_Retefuente_Ano;
    vm.Generate_Retefuente = Generate_Retefuente;

    //Seleccionar
    vm.selectedItems = [];
    vm.sucursales = [];
    vm.Deselect_Items = Deselect_Items;
    vm.selectedChange = selectedChange;
    vm.log = log;

    activate();

    ////////////////

    function activate() {
      vm.data.empresas = empresas;
    }
    
    function Get_Config_Retefuente() {

      
      vm.loading = true;      
      if (vm.viewmodel.empleados.length > 0) {        
        vm.viewmodel.empleados.length = 0;
      }
      var Empresa = vm.viewmodel.DBName.trim();
      Certifica2Svc.Get_Config_Retefuente(Empresa).then(function(data) {
        vm.config = data;
        config_viewmodel();
        vm.loading = false;
      });
    }

    function Get_Empleados_Retefuente_Ano() {
      vm.loadingEmpleados = true;
      var Empresa = vm.viewmodel.DBName.trim();
      Certifica2Svc.Get_Empleados_Retefuente_Ano(Empresa, vm.viewmodel.anoContable).then(function(data) {
        vm.selectedItems = [];
        vm.empleadosGird = new NgTableParams({}, { dataset: data.empleados });
        vm.data.empleados = data.empleados;
        vm.loadingEmpleados = false;
        vm.sucursales = data.empleados
          .map(function(e) {
            return e.Cod_Sucursal;
          })
          .filter(function(value, index, self) {
            return self.indexOf(value) === index;
          })
          .map(function(e) {
            return { id: e, title: e };
          });
      });
    }

    function Generate_Retefuente() {
      $rootScope.generating = true;
      Certifica2Svc.Generate_Retefuente(vm.viewmodel).then(
        function() {
          $rootScope.generating = false;
        },
        function() {
          $rootScope.generating = false;
        }
      );
      // console.log(vm.viewmodel.empleados)
    }

    function Deselect_Items(source) {
      vm.selectedItems = source;
      for (var i = vm.selectedItems.length - 1; i >= 0; i--) {
        vm.selectedItems[i].selected = false;
      }
      vm.selectedItems = [];
    }

    function log(source) {
      if (vm.tableSelectAll) {
        vm.selectedItems = source;
        for (var i = vm.selectedItems.length - 1; i >= 0; i--) {
          vm.selectedItems[i].selected = true;
        }
      } else {
        for (var i = vm.selectedItems.length - 1; i >= 0; i--) {
          vm.selectedItems[i].selected = false;
        }
        vm.selectedItems = [];
      }
      console.log(source);
    }

    function selectedChange(source) {
      // Cambiar Source en caso de que sean vario
      if (vm.viewmodel.empleados.length == source.length) {
        vm.tableSelectAll = true;
        $('.select-all').prop('indeterminate', false);
      } else if (vm.viewmodel.empleados.length == 0) {
        vm.tableSelectAll = false;
        $('.select-all').prop('indeterminate', false);
      } else {
        $('.select-all').prop('indeterminate', true);
      }
    }

    function config_viewmodel(data) {
      vm.viewmodel.ByCedula = vm.config.Unificar_Contratos;
      vm.viewmodel.redondear = vm.config.Redondear_Renglones;
      var minYearToShow = 2015;
      var lastYear = new Date().getFullYear() - 1;
      //SE QUITO SOLO EN MISSION PARA QUE NO RESTE DOS VECES (MOMENTANEO)
      //if (!vm.config.Mostrar_Ano_Anterior) {
      //  lastYear--;
      //}
      vm.viewmodel.anoContable = lastYear;
      while (lastYear >= minYearToShow) {
        vm.years.push(lastYear);
        lastYear--;
      }
      Get_Empleados_Retefuente_Ano();
    }
  }
})();
