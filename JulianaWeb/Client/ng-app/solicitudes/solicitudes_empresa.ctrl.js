(function() {
  'use strict';
  angular.module('app.solicitudes').controller('SolicitudEmpresaCtrl', SolicitudEmpresaCtrl);

  SolicitudEmpresaCtrl.$inject = [
    '$rootScope',
    '$http',
    'NgTableParams',
    '$stateParams',
    'ngDialog',
    'SolicitudesSvc',
    'AuthSvc',
    'data'
  ];

  /* @ngInject */
  function SolicitudEmpresaCtrl(
    $rootScope,
    $http,
    NgTableParams,
    $stateParams,
    ngDialog,
    SolicitudesSvc,
    AuthSvc,
    data
  ) {
    var vm = this;
    vm.loading = true;
    vm.data = {};
    vm.viewmodel = {};
    vm.Empresa = $stateParams.Empresa;
    // vm.data.empresas = Empresas;

    activate();

    function activate() {
      vm.data = data;
      Load_Motivos_Rechazo();

      AuthSvc.getEmpleado().then(function (empleado) {
        if (data.solicitud) {
          if (data.solicitud.aprobador) {
            vm.isTheAprobador = empleado.Cedula.trim() == data.solicitud.aprobador.Documento.trim();
          } else if (AuthSvc.UserName === 'Admin') {
            vm.isTheAprobador = true;
          }
        } else {
          vm.isTheAprobador = false;
        }
      })
        .finally(() => {
          if (!vm.isTheAprobador) {
            AuthSvc.getAprobador().then(function (aprobador) {
              if (aprobador && aprobador.Cod_Filtro === 999) {
                vm.isTheAprobador = true;
              }
            });
          }
        });

    }

    function Load_Motivos_Rechazo() {
      return SolicitudesSvc.Load_Motivos_Rechazo().then(function(data) {
        vm.data.motivos = data;
      });
    }

    vm.Confirm_Aprobar_Solicitud = Confirm_Aprobar_Solicitud;
    function Confirm_Aprobar_Solicitud(Cod_Solicitud) {
      ngDialog
        .openConfirm({
          template:
            '\
        <div class="ngdialog-body">\
            <div class="my-5 text-center">\
              <h4>¿Aprobar Solicitud?</h4>\
            </div>\
            <div>\
              <button class="btn btn-outline-secondary" ng-click="closeThisDialog()">Cancelar</button>\
              <button class="btn btn-primary float-right" ng-click="confirm()">Aceptar</button>\
            <div>\
        </div>',
          plain: true,
          className: 'ngdialog-theme-plain',
          appendClassName: 'ngdialog-systems-theme',
          closeByEscape: false,
          closeByDocument: false
        })
        .then(
          function() {
            
            SolicitudesSvc.Aprobar_Solicitud_Empresa(vm.Empresa, Cod_Solicitud);
          },
          function() {}
        );
    }
    vm.Confirm_Rechazar_Solicitud = Confirm_Rechazar_Solicitud;
    function Confirm_Rechazar_Solicitud(solicitud) {
      ngDialog.open({
        template: 'Client/ng-app/solicitudes/aprobador/rechazar-solicitud-dialog.html',
        className: 'ngdialog-theme-plain',
        appendClassName: 'ngdialog-systems-theme',
        controller: [
          '$scope',
          function($scope) {
            $scope.motivos = vm.data.motivos;
            $scope.data = solicitud;
            $scope.Rechazar_Solicitud = function () {
              
              SolicitudesSvc.Rechazar_Solicitud($scope.data, vm.Empresa);
            };
          }
        ]
      });
    }

    vm.Decargar_PDF_Solicitud = Decargar_PDF_Solicitud;
    function Decargar_PDF_Solicitud() {
      $rootScope.generating = true;
      var ndata = {
        Empresa: vm.Empresa,
        Cod_Solicitud: data.solicitud.Cod_Solicitud
      };
      SolicitudesSvc.Decargar_PDF_Solicitud(ndata).then(function() {
        $rootScope.generating = false;
      });
    }
  }
})();
