(function () {
  "use strict";

  angular
    .module("app.vacaciones")
    .controller("VacacionesCtrl", VacacionesCtrl)
    .controller("VacacionesDialogCtrl", VacacionesDialogCtrl);

  VacacionesCtrl.$inject = ["ngDialog", "NgTableParams", "AuthSvc", "$state", "data", "DialogSvc", "SolicitudesSvc"];
  function VacacionesCtrl(ngDialog, NgTableParams, AuthSvc, $state, data, DialogSvc, SolicitudesSvc) {
    var vm = this;

    vm.openSolicitudDialog = openSolicitudDialog;
    vm.Confirm_Del_Solicitud = Confirm_Del_Solicitud;
    vm.selectedItems = [];
    vm.DiasAnticipados = 0;

    vm.AlertaVacaciones = SETTINGS.AlertaVacaciones;
    vm.MostrarAlertaVacaciones = SETTINGS.MostrarAlertaVacaciones === "true";

    vm.Math = Math;

    activate();
    function activate() {
      vm.data = data;
      initGrids();
      if (vm.data.diasDisponibles < 0) {
        vm.DiasAnticipados = vm.data.diasDisponibles * -1;
      }
    }

    function initGrids() {
      //debugger
      vm.historicoGrid = new NgTableParams({}, { dataset: vm.data.historico });
      vm.solPendientesGrid = new NgTableParams({}, { dataset: vm.data.pendientes });
      vm.solAprobadasGrid = new NgTableParams({}, { dataset: vm.data.aprobadas });
    }

    function openSolicitudDialog() {
      ngDialog.open({
        template: "Client/ng-app/vacaciones/new-solicitud.dialog.html",
        className: "ngdialog-theme-plain",
        appendClassName: "ngdialog-systems-theme",
        controller: "VacacionesDialogCtrl",
        data: angular.copy(vm.data)
      });
    }

    function Confirm_Del_Solicitud(Cod_Solicitud) {
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
              
              if(data.status == "OK") 
                SolicitudesSvc.Delete_Solicitud(Cod_Solicitud).then(function () {
                  $state.reload();
                });
            })
            
          },
          function () {
            
          }
        );
    }

    vm.Confirm_PD_Solicitud = Confirm_PD_Solicitud;

    function Confirm_PD_Solicitud(Cod_Solicitud) {
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
              
              if(data.status == "OK")
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
  }

  VacacionesDialogCtrl.$inject = ["$scope", "$http", "$state", "ngDialog", "SolicitudesSvc", "AuthSvc", "NotifySvc"];

  function VacacionesDialogCtrl($scope, $http, $state, ngDialog, SolicitudesSvc, AuthSvc, NotifySvc) {
    var HOLIDAYS = [];
    var f = new Date();
    var DiasxMes = AuthSvc.empleado.DiasVacAno / 12;

    $scope.data = angular.copy($scope.ngDialogData);

    $scope.viewmodel = {
      Cantidad : 1
    };
    $scope.viewmodel.Modo_Vacaciones = "T";
    $scope.closeDialog = closeDialog;
    $scope.checkDate = checkDate;
    $scope.sendSolicitudVacaciones = sendSolicitudVacaciones;

    activate();

    function activate() {
      $scope.proyeccion = 0;
      $scope.DiasAnticipados = 0;
      $scope.DiasDisponibles = 0;

      
      $scope.DiasDisponibles = parseFloat(parseFloat($scope.data.diasDisponibles).toFixed(2));
      if ($scope.DiasDisponibles < 0) {
        $scope.DiasAnticipados = $scope.DiasDisponibles * -1;
      }

      if ($scope.data.aprobadores.length) {
        $scope.viewmodel.Cod_Aprobador = $scope.data.aprobadores[0].Cod_Tercero;
      }

      $scope.Fec_Actual = new Date();
      var Fec_Desde = new Date($scope.Fec_Actual.getFullYear(), $scope.Fec_Actual.getMonth(), 1);
      if ($scope.Fec_Actual.getDate() > $scope.data.Dia_Cierre_Novedades) {
        Fec_Desde = new Date($scope.Fec_Actual.getFullYear(), $scope.Fec_Actual.getMonth() + 1, 1);
        $scope.viewmodel.Fec_Salida = Fec_Desde;
      }

      $scope.datetimeInput = {
        culture: "es-CO",
        min: Fec_Desde,
        height: "38px",
        width: "100%"
      };

      SolicitudesSvc.getFestivos().then(function (data) {
        for (var i = data.length - 1; i >= 0; i--) {
          var date = new Date(data[i]);
          date.setHours(date.getHours() + date.getTimezoneOffset() / 60);
          HOLIDAYS.push(moment(date).format("YYYYMMDD"));
        }
      });
    }

    $scope.closeDialog = closeDialog;

    function closeDialog() {
      ngDialog.close();
    }

    function checkDate() {
      if (!$scope.viewmodel.Fec_Salida) return;
      
      var DiasxMes = AuthSvc.empleado.DiasVacAno / 12;
      var fec_sal = $scope.viewmodel.Fec_Salida;
      var DiasMaximosAno = DiasxMes * 12;
      var primerDiaMes = new Date($scope.Fec_Actual.getFullYear(), $scope.Fec_Actual.getMonth(), 1);
      var mesActual = moment(primerDiaMes);
      var Fec_Desde = moment($scope.viewmodel.Fec_Salida);
      var Fec_Hasta = addWeekdays(Fec_Desde, $scope.viewmodel.Cantidad);
      var DiffMeses = parseInt(Fec_Desde.diff(mesActual, "months"));
      var DiffMeses = DiffMeses;

      $scope.proyeccion = DiasxMes * DiffMeses;

      $scope.pruebaDiffMeses = function () {
        $scope.greeting = 'Hello ' + $scope.username + '!';
      };


      if ($scope.DiasDisponibles < 0) {
        $scope.DiasAnticipados = $scope.DiasDisponibles * -1;
      }
      //--Proyeccion de dias--//
      if ($scope.proyeccion > 0) {
        $scope.DiasDisponibles = parseFloat(parseFloat($scope.data.diasDisponibles).toFixed(2));
        $scope.DiasDisponibles += $scope.proyeccion;
      } else {
        $scope.DiasDisponibles = parseFloat(parseFloat($scope.data.diasDisponibles).toFixed(2));
      }
      

      if ($scope.viewmodel.Cantidad > $scope.DiasDisponibles)
      {
        $scope.viewmodel.Cantidad = Math.max(0, parseInt($scope.DiasDisponibles));
      }

      if (DiffMeses > 12) {
        $scope.viewmodel.Fec_Salida = null;
        $scope.viewmodel.Fec_Llegada = null;
        $scope.viewmodel.Cantidad = 0;
        $scope.DiasDisponibles = parseFloat(parseFloat($scope.data.diasDisponibles).toFixed(2));
        alert("La fecha de salida seleccionada es superior a la fecha máxima permitida para hacer una solicitud de vacaciones");        
      }

      if($scope.DiasAnticipados > 0){
        var diasmaximosAnticipados = DiasMaximosAno - $scope.DiasAnticipados;
        if($scope.viewmodel.Cantidad > diasmaximosAnticipados){
          $scope.viewmodel.Fec_Salida = null;
          $scope.viewmodel.Fec_Llegada = null;
          $scope.viewmodel.Cantidad = 0;
          alert("La cantidad de días solicitados supera el máximo permitido de días anticipados. " + diasmaximosAnticipados + " días.");
        }
      }

      $scope.isHoliday = SolicitudesSvc.checkDate($scope.viewmodel.Fec_Salida);
      if (HOLIDAYS.indexOf(Fec_Desde.format("YYYYMMDD")) !== -1) $scope.isHoliday = true;
      if ($scope.isHoliday) {
        $scope.viewmodel.Fec_Salida = null;
        $scope.viewmodel.Fec_Llegada = null;
        $scope.viewmodel.Cantidad = 0;
        $scope.DiasDisponibles = parseFloat(parseFloat($scope.data.diasDisponibles).toFixed(2));
        return;
      }

      if ($scope.Fec_Actual.getDate() > $scope.data.Dia_Cierre_Novedades && $scope.viewmodel.Fec_Salida.getMonth() === $scope.Fec_Actual.getMonth()) {
        $scope.Cierre_Novedades = true;
        $scope.viewmodel.Fec_Salida = null;
        $scope.viewmodel.Cantidad = 0;
        $scope.DiasDisponibles = parseFloat(parseFloat($scope.data.diasDisponibles).toFixed(2));
      } else {
        $scope.Cierre_Novedades = false;
      }

      $scope.viewmodel.Fec_Llegada = Fec_Hasta.toDate();

    }

    function addWeekdays(date, days) {
      date = moment(date);
      while (days > 0) {
        date = date.add(1, "days");
        if (date.isoWeekday() !== 6 && date.isoWeekday() !== 7) {
          if (HOLIDAYS.indexOf(date.format("YYYYMMDD")) === -1) {
            days--;
          }
        }
      }
      return date;
    }

    $scope.CantidadMinimaDiasSolisitados = function () {
      return 1;
    };

    $scope.checkCantidadDiasSolisitados = function () {
      $scope.viewmodel.Cantidad = parseInt($scope.viewmodel.Cantidad);
      if (isNaN($scope.viewmodel.Cantidad)) $scope.viewmodel.Cantidad = null;
      if ($scope.viewmodel.Cantidad > $scope.DiasDisponibles) {
        $scope.viewmodel.Cantidad = parseInt($scope.DiasDisponibles);
      } else if ($scope.viewmodel.Cantidad <= 0) {
        $scope.viewmodel.Cantidad = null;
      }
      checkDate();
      return;
    };

    function sendSolicitudVacaciones() {
      $scope.sending = true;
      var Cod_Empleado = AuthSvc.empleado.Cod_Empleado;
      var Empresa = AuthSvc.data.DBName;
      var url = ROOTURL + "API/Solicitudes/" + Empresa.trim() + "/" + Cod_Empleado + "/";
      $scope.viewmodel.Tipo_Solicitud = "V";
      $scope.viewmodel.Cod_Empleado = Cod_Empleado;
      $http
        .post(url, $scope.viewmodel)
        .success(function (data) {
          if (data.alerta) {
            ngDialog.close();
            $scope.mostrarAlerta(data.alerta);
          } else {
            $state.reload();
          }
        })
        .error(function (error, status) {
          ngDialog.close();
          NotifySvc.error(error.Message);
        });
    }

    $scope.mostrarAlerta = function (alerta) {
      ngDialog
        .openConfirm({
          template:
            '\
        <div class="ngdialog-body">\
            <div class="row"><div class="col-md-12"><h4>Solicitud Enviada Con Exito</h4></div>    \
                <div class="col-md-12 m-t-2"> \
                <p>' +
            alerta +
            '.</p>\
                </div></div> \
            <div class="text-right">\
              <button class="btn btn-primary" ng-click="confirm()">Aceptar</button>\
            <div>\
        </div>',
          plain: true,
          className: "ngdialog-theme-plain",
          appendClassName: "ngdialog-systems-theme"
        })
        .then(function () {
          $state.reload();
        });
    };

    $scope.sendSolicitudCesantias = sendSolicitudCesantias;

    function sendSolicitudCesantias() {
      SolicitudesSvc.sendSolicitudCesantias($scope.viewmodel);
      ngDialog.close();
    }
  }
})();
