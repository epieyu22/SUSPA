(function() {
  'use strict';
  angular
    .module('app.admin')
    .controller('AprobacionesCtrl', AprobacionesCtrl)
    .controller('AprobacionesDialogCtrl', AprobacionesDialogCtrl)
    ;
  
  AprobacionesCtrl.$inject = ['ngDialog', 'AdminSvc', 'empresas', '$rootScope', 'NgTableParams'];
  /* @ngInject */
  function AprobacionesCtrl(ngDialog, AdminSvc, empresas, $rootScope, NgTableParams) {
    var vm = this;
    vm.viewmodel = {};
    vm.data = {};
    vm.report = {};
    vm.data.aprobadores = [];
    vm.Get_Aprobadores_Empresa = Get_Aprobadores_Empresa;
    vm.Open_New_Aprobacion_Dialog = Open_New_Aprobacion_Dialog;
    vm.Open_Edit_Aprobacion_Dialog = Open_Edit_Aprobacion_Dialog;
    vm.Confirm_Del_Solicitud = Confirm_Del_Solicitud;
    vm.Reporte_Aprobadores_Excel = Reporte_Aprobadores_Excel;


    activate();

    ////////////////
    function activate() {
      vm.data.empresas = empresas;
      vm.viewmodel.DBName = vm.data.empresas.length > 0 ? vm.data.empresas[0].BaseDatos : null;
      if (empresas.length) vm.report.Empresa = empresas[0].BaseDatos.trim();
      vm.report.Filtro = 'None';
      Get_Filter_Tables_Data();
      initTablaAprobadores();
    }

    function Reporte_Aprobadores_Excel() {
      $rootScope.generating = true;
      AdminSvc.Reporte_Aprobadores_Excel(vm.report).then(function () {
        $rootScope.generating = false;
      });
    }

    let pageOldValue;
    let countOldValue;
    let empresaOldValue;
    let empleadoOldValue;
    let docEmpleadoOldValue;
    let carIdEmpleadoOldValue;
    let aprobadorOldValue;
    let docAprobadorOldValue;
    let carIdAprobadorOldValue;
    function initTablaAprobadores() {
      vm.aprobadoresGrid = new NgTableParams(
        {
          page: 1,
          count: 10
        },
        {
          total: 0,
          getData: function (params) {
            const page = params.page();
            const count = params.count();
            const empresa = vm.viewmodel.DBName?.trim();
            const filters = params.filter();
            const docEmpleado = filters.DocEmpleado ?? "";
            const carIdEmpleado = filters.CarIdEmpleado ?? "";
            const empleado = filters.Empleado ?? "";
            const docAprobador = filters.DocAprobador ?? "";
            const carIdAprobador = filters.CarIdAprobador ?? "";
            const aprobador = filters.Aprobador ?? "";

            if (page !== pageOldValue || count !== countOldValue || empresa !== empresaOldValue
              || docEmpleado !== docEmpleadoOldValue || carIdEmpleado !== carIdEmpleadoOldValue || empleado !== empleadoOldValue
              || docAprobador !== docAprobadorOldValue || carIdAprobador !== carIdAprobadorOldValue || aprobador !== aprobadorOldValue) {
              vm.loading = true;
              pageOldValue = page;
              countOldValue = count;
              empresaOldValue = empresa;
              docEmpleadoOldValue = docEmpleado;
              carIdEmpleadoOldValue = carIdEmpleado;
              empleadoOldValue = empleado;
              docAprobadorOldValue = docAprobador;
              carIdAprobadorOldValue = carIdAprobador;
              aprobadorOldValue = aprobador;

              return AdminSvc.Get_Aprobadores_Empresa(empresa, page, count, docEmpleado, carIdEmpleado, empleado, docAprobador, carIdAprobador, aprobador)
                .then(function (data) {
                  vm.data.aprobadores = data.Items;
                  params.total(data.TotalRows);
                  return data.Items;
                })
                .finally(function () {
                  vm.loading = false;
                });
            } else {
              vm.loading = false;
              return vm.data.aprobadores;
            }
          }
        }
      );
    }
    function Get_Aprobadores_Empresa() {
      vm.loading = true;
      vm.aprobadoresGrid.reload();
      Get_Filter_Tables_Data();
    }

    function Get_Filter_Tables_Data(){
      var Empresa = vm.viewmodel.DBName.trim();
      AdminSvc
        .Get_Filter_Tables_Data(Empresa)
        .then(function(data){
          vm.Filter_Tables = data;
          vm.loading = false;
        });
    }

    function Open_New_Aprobacion_Dialog(){
      var Empresa = vm.viewmodel.DBName.trim();;
      ngDialog.open({
        template: 'Client/ng-app/admin/aprobaciones/aprobaciones-dialog.html',
        className: 'ngdialog-theme-plain',
        appendClassName : 'ngdialog-systems-theme',
        controller: 'AprobacionesDialogCtrl',
        data: {tables: vm.Filter_Tables, Empresa: Empresa}
      });
    }

    function Open_Edit_Aprobacion_Dialog(aprobador){
      // console.log(aprobacion)
      var Empresa = vm.viewmodel.DBName.trim();;
      ngDialog.open({
        template: 'Client/ng-app/admin/aprobaciones/aprobaciones-dialog.html',
        className: 'ngdialog-theme-plain',
        appendClassName : 'ngdialog-systems-theme',
        controller: 'AprobacionesDialogCtrl',
        data: {tables: vm.Filter_Tables, Empresa: Empresa, edit: true, aprobador: aprobador}
      });
    }


    function Confirm_Del_Solicitud(aprobador) {
      var Empresa = vm.viewmodel.DBName.trim();;
      ngDialog.openConfirm({
        template: '\
        <div class="ngdialog-body">\
            <div class="row"><div class="col-md-12"><h4>¿Eliminar este registro?</h4></div>    \
                <div class="col-md-12 m-y-2">¿Está seguro de eliminar este registro de configuración?</div></div> \
            <div class="text-right">\
              <button class="btn btn-outline-danger" ng-click="closeThisDialog(0)">Cancelar</button>\
              <button class="btn btn-primary" ng-click="confirm(1)">Eliminar</button>\
            <div>\
        </div>',
        plain: true,
        className: 'ngdialog-theme-plain',
        appendClassName: 'ngdialog-systems-theme',
        data: { aprobador: aprobador}

        })
        .then(function confirm(){
          AdminSvc
            .Delete_Aprobador(Empresa,
              {
                Cod_Empleado: aprobador.Cod_Empleado,
                Cod_Filtro: aprobador.Cod_Filtro

              });
          
      }, function(){
        console.log("No borrar")
      });
    }
    function confirm(viewmodel) {
      
    }
  }

  AprobacionesDialogCtrl.$inject = ['$scope', 'AdminSvc'];
  function AprobacionesDialogCtrl($scope, AdminSvc){
    
    $scope.data = $scope.ngDialogData;
    $scope.viewmodel = {};

    // Objecto usado para enviar los datos que deben ser actualizados y borrados
    // Si, esta mal hecho, pero esto lo hago muyrapido :P
    $scope.updateVm = {};
    $scope.updateVm.upsert = [];
    $scope.updateVm.delete = [];
    $scope.viewmodel.Cod_Aprobadores = [];
    activate()

    function activate(){
      if($scope.data.edit){
        $scope.viewmodel.Filtro = $scope.data.aprobador.Filtro;
        $scope.viewmodel.Cod_Filtro = $scope.data.aprobador.Cod_Filtro;
        $scope.viewmodel.Cod_Filtro = $scope.data.aprobador.Cod_Filtro;
        $scope.viewmodel.Tipo_Aprobacion = $scope.data.aprobador.Tipo_Aprobacion;
        Get_Aprobacion_Config();
      }
    }

    //Eliminar aprobadores
    $scope.confirmDelete = confirmDelete;
    function confirmDelete(data) {      
      AdminSvc
        .IdiomasHojaVida.delete(
          {
            Cod_HojaVida: vm.data.Cod_HojaVida,
            Cod_Idioma_Hojavida: data.Cod_Idioma_Hojavida
          },
          function (data) {
            vm.IDIOMAS_HOJAVIDA.splice(vm.IDIOMAS_HOJAVIDA.indexOf(data), 1);
          });
    }

    $scope.range = function(n) {
        return new Array(n);
    };

    $scope.Get_Aprobacion_Config = Get_Aprobacion_Config;
    function Get_Aprobacion_Config(){
      if($scope.viewmodel.Cod_Filtro && $scope.viewmodel.Tipo_Aprobacion){
        $scope.loading = true;
        AdminSvc
          .Get_Aprobacion_Config($scope.data.Empresa, $scope.viewmodel.Filtro, $scope.viewmodel.Cod_Filtro, $scope.viewmodel.Tipo_Aprobacion)
          .then(function(data){
            $scope.data.aprobadores = data;
            for (var i = 0; i < data.length; i++) {
              var d = data[i];
              $scope.viewmodel.Cod_Aprobadores.push(d.Cod_Empleado)              
            }
            $scope.loading = false;
          })
      }
    }



    $scope.Add_Aprobadores_Empresa = function (){
      AdminSvc
        .Add_Aprobadores_Empresa($scope.data.Empresa, $scope.viewmodel);
    }



    $scope.Add_Nivel = Add_Nivel;
    function Add_Nivel(){
      var newAprobador = {};
      newAprobador.Nivel = $scope.data.aprobadores.length;
      newAprobador.Filtro = $scope.data.aprobadores[0].Filtro;
      newAprobador.Cod_Filtro = $scope.data.aprobadores[0].Cod_Filtro;
      newAprobador.Tipo_Aprobacion = $scope.data.aprobadores[0].Tipo_Aprobacion;
      $scope.data.aprobadores.push(newAprobador)
    }

    $scope.Delete_Nivel = Delete_Nivel;
    // APROBAODRES nivel
    function Delete_Nivel(nivel){
      if(nivel.Cod_Aprobador){
        $scope.updateVm.delete.push($scope.data.aprobadores.pop())
        console.log($scope.updateVm)
      }else{
        $scope.data.aprobadores.pop()
      }
    }


  }


})();

