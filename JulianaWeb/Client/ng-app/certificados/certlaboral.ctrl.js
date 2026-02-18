(function () {
  'use strict';
  angular
    .module('app.certificados')
    .controller('CertlaboralCtrl', CertlaboralCtrl)
    .controller('PedidoCertlabDialogCtrl', PedidoCertlabDialogCtrl);

  CertlaboralCtrl.$inject = ['$rootScope', 'ngDialog', 'CertificadosSvc', 'NotifySvc', 'config'];
  /* @ngInject */
  function CertlaboralCtrl($rootScope, ngDialog, CertificadosSvc, NotifySvc, config) {
    var vm = this;
    vm.certlaboralViewModel = {};
    vm.generateCertlab = generateCertlab;
    vm.Search_Certlab = Search_Certlab;


    vm.MostrarAlertaCertlaboral = SETTINGS.MostrarAlertaCertlaboral === 'true';
    vm.AlertaCertlaboral = SETTINGS.AlertaCertlaboral;
    activate();
    ////////////////
    function activate() {
      vm.config = config;
      vm.UseDirigidoCertificado = SETTINGS.UseDirigidoCertificado === "true";
      if (!vm.UseDirigidoCertificado) {
        console.log(vm.UseDirigidoCertificado, SETTINGS.UseDirigidoCertificado, SETTINGS.UseDirigidoCertificado === "true")
        vm.certlaboralViewModel.dirigido = "A Quien Interese";
      }
    }

    function generateCertlab() {
      $rootScope.generating = true;
      CertificadosSvc.generateCertlab(vm.certlaboralViewModel)
        .then(function () {
            $rootScope.generating = false;
          },
          function (error) {
            $rootScope.generating = false;
            NotifySvc.error();
          });
    }


    vm.cambiarPlantilla = cambiarPlantilla;

    function cambiarPlantilla() {
      vm.splantilla = vm.plantillas.find(function (f) {
        return f.Autonum == vm.model.Cod_Plantilla
      })
      console.log(vm.splantilla)
    }

    function Search_Certlab() {
      CertificadosSvc
        .Search_Certlab(vm.searchFile)
        .then(function (data) {
          if (data.length) {
            window.open(ROOTURL + "Static/CertificadosBk/" + data + ".pdf", '_blank');
          } else {
            NotifySvc.error("No se ha encontrado un documento con ese Cod. de Verificación");
          }
        });
    }

    vm.Pedido_Certtificado = Pedido_Certtificado;

    function Pedido_Certtificado() {
      ngDialog.open({
        template: 'Client/ng-app/certificados/pedido-certifido.dialog.html',
        className: 'ngdialog-theme-plain',
        appendClassName: 'ngdialog-systems-theme',
        controller: 'PedidoCertlabDialogCtrl',
      });
    }

  }
})();


PedidoCertlabDialogCtrl.$inject = ['$scope', 'ngDialog', 'Certifica2Svc', 'NotifySvc'];

function PedidoCertlabDialogCtrl($scope, ngDialog, Certifica2Svc, NotifySvc) {
  $scope.Solicitud_Certificdo = function () {
    Certifica2Svc.Solicitud_Certificdo($scope.body)
      .then(function () {
        $scope.closeThisDialog();
        NotifySvc.success('Solicitud enviada con éxito');
      });
  }
}
