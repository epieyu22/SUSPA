(function() {
  'use strict';
  angular
    .module('app.core')
    .factory('NotifySvc', NotifySvc);

  NotifySvc.$inject = ['ngNotify'];
    /* @ngInject */

  function NotifySvc(ngNotify) {
    var service = {
        success: success,
        error: error,
        failed: failed,
        successFilePayrollElec: successFilePayrollElec
    };
    return service;
    ////////////////

    function success(msg) {
      if(!msg){
        var msg = 'Cambios guardados satisfactorimente';
      }
      ngNotify.set(msg, 'success');
    }

    function successFilePayrollElec(msg) {
      if (!msg) {
        var msg = 'Archivo generado exitosamente';
      }
      ngNotify.set(msg, 'success');
    }

    function error(msg) {
      if(!msg){
        var msg = '<h5>Ha ocurrido un error inesperado.</h5>'
        msg += '<p>Intente de nuevo más tarde, si el error persiste comuníquese con <a href="mailto:soporte@systemsltda.com">nosotros</a></p>';
      }
      ngNotify.set(msg, {
        type: 'error',
        html: true,
        sticky: true});
    }

    function failed(msg) {
      if (!msg) {
        var msg = '<h5>El procedimiento ha fallado</h5>'
        msg += '<p>Intente de nuevo más tarde, si el error persiste comuníquese con <a href="mailto:soporte@systemsltda.com">nosotros</a></p>';
      }
      ngNotify.set(msg, {
        type: 'error',
        html: true,
        sticky: true
      });
    }
  }
})();
