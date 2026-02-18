(function() {
  'use strict';

  angular
    .module('app.incapacidades')
    .controller('IncapacidadesCtrl', IncapacidadesCtrl)
    .controller('IncapacidadesDialogCtrl', IncapacidadesDialogCtrl);

  IncapacidadesCtrl.$inject = ['$state', 'ngDialog', 'NgTableParams', 'IncapacidadesSvc', 'data', 'NotifySvc'];

  function IncapacidadesCtrl($state, ngDialog, NgTableParams, IncapacidadesSvc, data, NotifySvc) {
    var vm = this;
    vm.viewmodel = {};
    vm.viewmodel.Cod_Diagnostico = 0;
    vm.viewmodel.Nom_Diagnostico = null;
    vm.Loading_Historico = true;
    vm.Get_Historico_Empleado = Get_Historico_Empleado;
    vm.Open_Inacapacidades_Dialog = Open_Inacapacidades_Dialog;
    vm.New_Incapacidad = New_Incapacidad;

    vm.calcularFechaLLegadaCliente = function(hasta) {
      return moment(hasta)
        .add(-1, 'd')
        .toDate();
    };

    activate();

    ////////////////

    function activate() {
      vm.data = data;
      vm.datetimeInput = {
        culture: 'es-CO',
        min: data.Rango_Fec_Desde,
        max: data.Rango_Fec_Hasta,
        height: '38px',
        width: '100%'
      };

      vm.licenciaInput = {
        culture: 'es-CO',
        min: data.Rango_Fec_Desde,
        max: data.Rango_Fec_Hasta,
        height: '38px',
        width: '100%'
      };

      Get_Historico_Empleado();
    }

    function Get_Historico_Empleado() {
      IncapacidadesSvc.Get_Historico_Empleado().then(function(data) {
        // vm.historicoIncapacidades = new NgTableParams({}, { dataset: data.historicoIncapacidades });
        vm.data.incapacidades = data.incapacidades;
        vm.data.licencias = data.licencias;
        vm.data.historicoLicencias = data.historicoLicencias;
        vm.data.historicoIncapacidades = data.historicoIncapacidades;
        // vm.data.solicitudes = data.solicitudes;
        vm.Loading_Historico = false;
      });
    }

    function Open_Inacapacidades_Dialog() {
      ngDialog.open({
        template: 'Client/ng-app/incapacidades/incapacidades.dialog.html',
        className: 'ngdialog-theme-plain',
        appendClassName: 'ngdialog-systems-theme',
        controller: 'IncapacidadesDialogCtrl',
        data: vm.data
      });
    }

    function New_Incapacidad() {
      vm.viewmodel.Cod_Concepto = vm.viewmodel.Concepto.Cod_Concepto;
      IncapacidadesSvc.New_Incapacidad(vm.viewmodel).then(function(Cod_Icapacidad) {
        if (vm.adjunto) {
          IncapacidadesSvc.changeImage(Cod_Icapacidad, vm.adjunto).then(function() {
            $state.go(
              $state.current,
              {},
              {
                reload: true
              }
            );
          });
        } else {
          $state.go(
            $state.current,
            {},
            {
              reload: true
            }
          );
        }
      });
    }

    vm.Solicitar_Nueva_Licencia = Solicitar_Nueva_Licencia;

    function Solicitar_Nueva_Licencia() {
      ngDialog.open({
        template: 'Client/ng-app/incapacidades/nueva_solicitud_licencia.html',
        className: 'ngdialog-theme-plain',
        appendClassName: 'ngdialog-systems-theme',
        controller: 'IncapacidadesDialogCtrl',
        data: vm.data
      });
    }

    vm.Confirm_Del_Incapacidad = Confirm_Del_Incapacidad;

    function Confirm_Del_Incapacidad(Cod_Novaut) {
      ngDialog
        .openConfirm({
          template:
            '\
        <div class="ngdialog-body">\
            <div class="row"><div class="col-md-12"><h4>Eliminar incapacidad</h4></div>    \
                <div class="col-md-12 my-2">¿Está seguro de eliminar esta incapacidad reportada?</div></div> \
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
            IncapacidadesSvc.DeleteIncapacidad(Cod_Novaut).then(function () {
              $state.go(
                $state.current,
                {},
                {
                  reload: true
                }
              );
            });

          },
          function () {

          }
        );
    }
  }

  IncapacidadesDialogCtrl.$inject = ['$scope', '$filter', '$state', 'ngDialog', 'NotifySvc', 'IncapacidadesSvc', '$rootScope', 'SolicitudesSvc'];

  function IncapacidadesDialogCtrl($scope, $filter, $state, ngDialog, NotifySvc, IncapacidadesSvc, $rootScope, SolicitudesSvc) {
    $rootScope.uploading = false;
    $scope.data = $scope.ngDialogData;
    //  = New_Incapacidad;
    $scope.viewmodel = {};
    $scope.disableDiasInput = false;
    $scope.mostrarDias = true;
    $scope.mostrarHoras = false;

    activate();

    function activate(conceptoSeleccionado) {
      $scope.data = $scope.ngDialogData;
      let parametro = $scope.data?.parametros?.find(p => {
        try {
          let valorJson = JSON.parse(p.Valor);
          return valorJson.Tipo === "INCAPACIDAD_GENERAL";
        } catch (e) {
          return false;
        }
      });
      let parametroJSON = {};
      const periodoCerrado = $scope.data.Periodo_Cerrado;
      let usarPeriodoCerrado = false;
      if (parametro) {
        parametroJSON = JSON.parse(parametro.Valor);
        usarPeriodoCerrado = parametroJSON.Tipo == 'INCAPACIDAD_GENERAL' && parametroJSON.UsarPeriodoCerrado && periodoCerrado;
      }

      const hoy = moment();
      const fechaDesde = moment($scope.data.Rango_Fec_Desde);
      const fechaHasta = moment($scope.data.Rango_Fec_Hasta);
      let datetimeInputMin = fechaDesde.toDate();
      let datetimeInputMax = fechaHasta.toDate();
      let licenciaInputMin = fechaDesde.toDate();
      let licenciaInputMax = fechaHasta.toDate();
      if (!!conceptoSeleccionado) {
        const tipoConcepto = conceptoSeleccionado.Concepto.Tipo_Concepto;
        const nombreConcepto = conceptoSeleccionado.Concepto.Nom_Concepto;
        const esIncapacidad = tipoConcepto == 'I';
        const esLiciencia = tipoConcepto == 'L';
        const esLicenciaDeLuto = esLiciencia && nombreConcepto.includes('LUTO');
        if (esIncapacidad) {
          const haceDosMeses = moment().subtract(3, 'months');
          datetimeInputMax = fechaHasta.isAfter(hoy) ? hoy.clone().toDate() : fechaHasta.clone().toDate();
          datetimeInputMin = haceDosMeses.toDate();
          //if (usarPeriodoCerrado) {}
        }
        else if (esLicenciaDeLuto) {
          licenciaInputMin = fechaDesde.clone().add(-1, 'Y').toDate();
          licenciaInputMax = fechaHasta.isAfter(hoy) ? hoy.clone().toDate() : fechaHasta.clone().toDate();
        }
        else if (esLiciencia) {
          licenciaInputMax = fechaHasta.clone().add(1, 'Y').toDate();
          if (periodoCerrado) {
            licenciaInputMin = fechaDesde.clone().add(1, 'M').toDate();
          }
        }
      }

      $scope.datetimeInput = {
        culture: 'es-CO',
        min: datetimeInputMin,
        max: datetimeInputMax,
        height: '38px',
        width: '100%'
      };

      $scope.licenciaInput = {
        culture: 'es-CO',
        min: licenciaInputMin,
        max: licenciaInputMax,
        height: '38px',
        width: '100%',
        change: function (event) {
          var day = event.args.newValue?.getDay();
          if (day === 0 || day === 6) {
            var component = $('#jqxWidget').jqxDateTimeInput('getInstance');
            setTimeout(function () {
              component.setDate(event.args.oldValue);
            }, 0);
          }
        }
      };
    }

    $scope.New_Incapacidad = function () {
      $scope.viewmodel.Cod_Concepto = $scope.viewmodel.Concepto.Cod_Concepto;
      if ($scope.viewmodel.Concepto?.Tipo_Concepto == 'L'
        && $scope.viewmodel.Concepto?.Cod_Concepto != 260
        && !$scope.viewmodel.Cod_Aprobador) {
        alert('Debe elegir un aprobador para esta licencia.');
        return;
      }
      let parametro = $scope.data?.parametros?.find(x => x.Cod_Parametro == "CONCEPTOS_" + $scope.viewmodel.Cod_Concepto);
      if (parametro) {
        let parametroJSON = JSON.parse(parametro.Valor);
        if (parametroJSON.Tipo == 'INCAPACIDAD_GENERAL') {
          if (parametroJSON.Diagnostico && (!$scope.viewmodel.Diagnostico || $scope.viewmodel.Diagnostico.Codigo <= 0)) {
            alert("Debe elegir un diagnostico");
            return;
          }
          if (parametroJSON.Validar_anexo && !$scope.adjunto) {
            alert("Debe adjuntar el documento de incapacidad");
            return;
          }
        }
      }
      $scope.viewmodel.Cod_Diagnostico = $scope.viewmodel?.Diagnostico?.Codigo || 0;
      if ($scope.viewmodel.Cod_Diagnostico > 0) $scope.viewmodel.Nom_Diagnostico = $scope.viewmodel.Diagnostico.Cod_Alterno + ' - ' + $scope.viewmodel.Diagnostico.Descripcion;

      $rootScope.uploading = true;

      IncapacidadesSvc.New_Incapacidad($scope.viewmodel).then(
        function (data) {
          if ($scope.adjunto) {
            if (data.noIncapacidad) {
              ngDialog.close();
              return NotifySvc.success();
            }
            if (data.Cod_Solicitud) {
              IncapacidadesSvc.changeImageLicenciaDeLuto(data.Cod_Solicitud, $scope.adjunto)
                .then(function () {
                  ngDialog.close();
                  Show_Fec_Llegada(data.Desde, data.Hasta);
                  return NotifySvc.success();
                })
                .catch(function (error) {
                  const mostrarNotificacion = false;
                  SolicitudesSvc.Delete_Solicitud(data.Cod_Solicitud, mostrarNotificacion);
                  NotifySvc.error(error?.Message ?? "Ocurrió un error al intentar cargar el archivo.");
                })
                .finally(() => {
                  $rootScope.uploading = false;
                });
            } else {
              IncapacidadesSvc.changeImage(data.Cod_Incapacidad, $scope.adjunto)
                .then(function () {
                  ngDialog.close();
                  Show_Fec_Llegada(data.Desde, data.Hasta);
                  return NotifySvc.success();
                })
                .catch(function (error) {
                  const mostrarNotificacion = false;
                  IncapacidadesSvc.DeleteIncapacidad(data.Cod_Incapacidad, mostrarNotificacion);
                  NotifySvc.error(error?.Message ?? "Ocurrió un error al intentar cargar el archivo.");
                })
                .finally(() => {
                  $rootScope.uploading = false;
                });
            }
          } else {
            ngDialog.close();
            $rootScope.uploading = false;
            if (data.noIncapacidad) return NotifySvc.success();
            Show_Fec_Llegada(data.Desde, data.Hasta);
          }
        },
        function() {
          ngDialog.close();
          $rootScope.uploading = false;
        }
      );
    };

    $scope.closeDialog = function() {
      ngDialog.close();
      $rootScope.uploading = false;
    };

    $scope.checkDate = function() {
      // $scope.showerror = '';
      if ($scope.viewmodel.Concepto.Tipo_Concepto === 'L') {
        if ($scope.viewmodel.Desde < new Date($scope.data.Rango_Fec_Desde)) {
          $scope.viewmodel.Desde = null;
          $scope.showerror = 'No puedes reportar una Licencia atrasada';
        }
      } else {
        if ($scope.viewmodel.Desde > new Date($scope.data.Rango_Fec_Hasta)) {
          $scope.viewmodel.Desde = null;
          $scope.showerror = 'No puedes reportar una Incapacidad con fecha mayor a la fecha actual';
        } else {
          if ($scope.viewmodel.Desde < new Date($scope.data.Rango_Fec_Desde)) {
            $scope.viewmodel.Desde = null;
            $scope.showerror = 'No puedes reportar una Incapacidad atrasada';
          }
        }
      }
    };

    $scope.changeConcepto = function() {
      if (
        $scope.viewmodel.Concepto.Cod_Concepto == $scope.data.Cod_MiTiempo ||
        $scope.viewmodel.Concepto.Cod_Concepto == $scope.data.Cod_MiCumpleanos
      ) {
        // Mi tiempo, Mi cumpleaños
        $scope.disableDiasInput = true;
        $scope.viewmodel.Dias = 4;
        $scope.mostrarHoras = true;
        $scope.mostrarDias = false;
      } else if (
        $scope.viewmodel.Concepto.Cod_Concepto == $scope.data.Cod_DonaccionSangre ||
        $scope.viewmodel.Concepto.Cod_Concepto == $scope.data.Cod_ExamenIngEduSup
      ) {
        // Donación sangre, Examen Ingreso Edu Sup
        $scope.disableDiasInput = true;
        $scope.viewmodel.Dias = 1;
        $scope.mostrarHoras = false;
        $scope.mostrarDias = true;
      } else if ($scope.viewmodel.Concepto.Cod_Concepto == $scope.data.Cod_LicXMatrimonio) {
        // Licencia por Matrimonio
        $scope.disableDiasInput = true;
        $scope.viewmodel.Dias = 3;
        $scope.mostrarHoras = false;
        $scope.mostrarDias = true;
      } else if ($scope.viewmodel.Concepto.Cod_Concepto == 22) {
        // Licencia maternidad
        $scope.disableDiasInput = false;
        $scope.viewmodel.Dias = 126;
        $scope.mostrarHoras = false;
        $scope.mostrarDias = true;
      } else if ($scope.viewmodel.Concepto.Cod_Concepto == 56) {
        // Licencia Paternidad
        $scope.disableDiasInput = true;
        $scope.viewmodel.Dias = 14;
        $scope.mostrarHoras = false;
        $scope.mostrarDias = true;
      } else {
        // Otros
        $scope.disableDiasInput = false;
        $scope.viewmodel.Dias = null;
        $scope.mostrarHoras = false;
        $scope.mostrarDias = true;
      }
      $scope.viewmodel.Diagnostico = null;

      $scope.calcularFechaLlegada();
      activate($scope.viewmodel);
    };

    $scope.calcularFechaLlegada = function() {
      if (!$scope.viewmodel.Desde || !$scope.viewmodel.Dias) {
        $scope.viewmodel.Hasta = null;
        return;
      }
      var desde = moment($scope.viewmodel.Desde);
      var hasta = desde.add($scope.viewmodel.Dias - 1, 'd');
      $scope.viewmodel.Hasta = hasta.toDate();
    };

    function Show_Fec_Llegada(Desde, Hasta) {
      Desde = $filter('date')(Desde, 'fullDate');
      Hasta = $filter('date')(Hasta, 'fullDate');

      if ($scope.viewmodel.Dias >= 15 && SETTINGS.BLOCKLEAVE && SETTINGS.BlockleaveNoCumplido) {
        ngDialog
          .openConfirm({
            template:
              '\
        <div class="ngdialog-body">\
            <div class="row"><div class="col-md-12"><h4>Incapacidad reportada con exito</h4></div>    \
                <div class="col-md-12 m-t-2"> \
                Esta incapacidad cuenta como cumplimiento de la politica de Block Leave\
                </div></div> \
            <div class="text-right">\
              <button class="btn btn-primary" ng-click="confirm()">Aceptar</button>\
            <div>\
        </div>',
            plain: true,
            className: 'ngdialog-theme-plain',
            appendClassName: 'ngdialog-systems-theme'
          })
          .then(function() {
            $state.go($state.current, {}, {reload: true});
          });
      } else {
        $state.go($state.current, {}, {reload: true});
      }
    }
  }
})();
