(function() {
  'use strict';
  angular.module('app.solicitudes').controller('SolicitudesCtrl', SolicitudesCtrl);

  SolicitudesCtrl.$inject = ['$rootScope', 'NgTableParams', 'ngDialog', 'SolicitudesSvc', 'AuthSvc', 'data'];
  /* @ngInject */
  async function SolicitudesCtrl($rootScope, NgTableParams, ngDialog, SolicitudesSvc, AuthSvc, data) {
    var vm = this;
    vm.title = 'SolicitudesCtrl';
    vm.data = {};
    vm.isTheAprobador = false;
    vm.Empresa = $stateParams.Empresa;

    vm.HasPermission = SolicitudesSvc.HasPermission;

    vm.Confirm_Aprobar_Solicitud = Confirm_Aprobar_Solicitud;
    vm.Confirm_Rechazar_Solicitud = Confirm_Rechazar_Solicitud;

    await activate();

    ////////////////
    async function activate() {
      vm.data = data;
      Load_Motivos_Rechazo();
      vm.solVacacionesGrid = new NgTableParams({}, {dataset: vm.data.vacaciones});
      vm.solCesantiasGrid = new NgTableParams({}, {dataset: vm.data.cesantias});
      vm.solIncapacidades = new NgTableParams({}, {dataset: vm.data.licencias});
      vm.solCertificados = new NgTableParams({}, {dataset: vm.data.certificados});
    

      try {
        let empleado = await AuthSvc.getEmpleado();
        if (data.solicitud) {
          if (data.solicitud.aprobador) {
            vm.isTheAprobador = empleado.Cedula.trim() == data.solicitud.aprobador.Documento.trim();
          } else if (AuthSvc.UserName === 'Admin') {
            vm.isTheAprobador = true;
          }
        } else {
          vm.isTheAprobador = false;
        }
      }
      catch (error) {
        vm.isTheAprobador = false;
      }

      //if (!vm.isTheAprobador) {
      //  try {
      //    let aprobador = await AuthSvc.getAprobador();

      //  }
      //  catch (error) {
      //    vm.isTheAprobador = false;
      //  }
      //}
    }

    function Load_Motivos_Rechazo() {
      return SolicitudesSvc.Load_Motivos_Rechazo().then(function(data) {
        vm.data.motivos = data;
      });
    }

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
            if (vm.Empresa) {
              SolicitudesSvc.Aprobar_Solicitud_Empresa(vm.Empresa, Cod_Solicitud);
            }
            else {
              SolicitudesSvc.Aprobar_Solicitud(Cod_Solicitud);
            }
            
          },
          function() {}
        );
    }

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
        Empresa: AuthSvc.data.DBName,
        Cod_Solicitud: data.solicitud.Cod_Solicitud
      };
      SolicitudesSvc.Decargar_PDF_Solicitud(ndata).then(function() {
        $rootScope.generating = false;
      });
    }
  }
})();
