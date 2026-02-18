(function() {
  'use strict';

  angular.module('app.incapacidades').controller('LicenciasCtrl', LicenciasCtrl);

  LicenciasCtrl.$inject = ['$http', 'ngDialog', 'AuthSvc', "SolicitudesSvc", "$state"];
  function LicenciasCtrl($http, ngDialog, AuthSvc, SolicitudesSvc, $state) {
    var LicenciasCtrl = this;
    LicenciasCtrl.loading = true;
    LicenciasCtrl.data = {};

    LicenciasCtrl.cargarDatos = function() {
      LicenciasCtrl.loading = true;
      AuthSvc.getEmpleado().then(function(empleado) {
        var Empresa = AuthSvc.data.DBName;
        var url = ROOTURL + 'API/Licencias/' + Empresa.trim() + '/' + empleado.Cod_Empleado;
        $http.get(url).success(function(data) {
          LicenciasCtrl.loading = false;
          LicenciasCtrl.data = data;
        });
      });
    };

    LicenciasCtrl.Solicitar_Nueva_Licencia = function() {
      ngDialog.open({
        template: 'Client/ng-app/incapacidades/nueva_solicitud_licencia.html',
        className: 'ngdialog-theme-plain',
        appendClassName: 'ngdialog-systems-theme',
        controller: 'IncapacidadesDialogCtrl',
        data: LicenciasCtrl.data
      });
    };

    LicenciasCtrl.Confirm_Del_Solicitud = function Confirm_Del_Solicitud(Cod_Solicitud) {
      ngDialog
        .openConfirm({
          template:
            '\
        <div class="ngdialog-body">\
            <div class="row"><div class="col-md-12"><h4>Eliminar solicitud</h4></div>    \
                <div class="col-md-12 my-2">¿Está seguro de eliminar esta solicitud pendiente de aprobación? Si elimina esta solicitud, ya no podra ser aprobada</div></div> \
            <div class="d-flex mt-3">\
              <button class="btn btn-outline-secondary" ng-click="closeThisDialog()">Cancelar</button>\
              <button class="btn btn-primary ml-auto" ng-click="confirm()">Eliminar</button>\
            <div>\
        </div>',
          plain: true,
          className: "ngdialog-theme-plain",
          appendClassName: "ngdialog-systems-theme"
        })
        .then(
          function () {
            SolicitudesSvc.Verificar_Solicitud_Rechazable(Cod_Solicitud, AuthSvc.data.DBName).then(function (data) {

              if (data.status == "OK")
                SolicitudesSvc.Delete_Solicitud(Cod_Solicitud).then(function () {
                });
            })

          },
          function () {

          }
        );
    }

    LicenciasCtrl.Confirm_PD_Solicitud = function Confirm_PD_Solicitud(Cod_Solicitud) {
      ngDialog
        .openConfirm({
          template:
            '\
        <div class="ngdialog-body">\
            <div class="row"><div class="col-md-12"><h4>Pedi desaprobación</h4></div>    \
                <div class="col-md-12 m-y-2">¿Está seguro de Pedir la <b>desaprobación</b> de esta solicitud ya aprobada? <br/> Se informara al aprobador para que confirme esta acción.</div></div> \
            <div class="text-right">\
              <button class="btn btn-outline-danger" ng-click="closeThisDialog()">Cancelar</button>\
              <button class="btn btn-primary" ng-click="confirm()">Pedir Desaprobación</button>\
            <div>\
        </div>',
          plain: true,
          className: "ngdialog-theme-plain",
          appendClassName: "ngdialog-systems-theme"
        })
        .then(
          function () {
            SolicitudesSvc.Verificar_Solicitud_Rechazable(Cod_Solicitud, AuthSvc.data.DBName).then(function (data) {

              if (data.status == "OK")
                SolicitudesSvc.PD_solicitud(Cod_Solicitud).then(function () {
                  $state.go(
                    $state.current,
                    {},
                    {
                      reload: true
                    }
                  );
                });
            })

          },
          function () {

          }
        );
    }

    activate();

    ////////////////

    function activate() {
      LicenciasCtrl.cargarDatos();
    }
  }
})();
