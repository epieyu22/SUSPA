(function() {
  'use strict';
  angular.module('app.incapacidades').run(appRun);

  appRun.$inject = ['routerHelper'];
  /* @ngInject */
  function appRun(routerHelper) {
    routerHelper.configureStates(getStates());
  }

  function getStates() {
    return [
      {
        state: 'incapacidades',
        config: {
          url: '/Incapacidades',
          views: {
            '': {
              templateUrl: 'Client/ng-app/layout/layout.html',
              controller: 'LayoutCtrl',
              controllerAs: 'vm'
            },
            'MainContent@incapacidades': {
              templateUrl: 'Client/ng-app/incapacidades/incapacidades.html',
              controller: 'IncapacidadesCtrl',
              controllerAs: 'vm',
              resolve: {
                data: Get_Conceptos_Incapacidades
              }
            }
          }
        }
      },
      {
        state: 'licencias',
        config: {
          url: '/Licencias',
          views: {
            '': {
              templateUrl: 'Client/ng-app/layout/layout.html',
              controller: 'LayoutCtrl',
              controllerAs: 'vm'
            },
            'MainContent@licencias': {
              templateUrl: 'Client/ng-app/incapacidades/licencias.html',
              controller: 'LicenciasCtrl',
              controllerAs: 'LicenciasCtrl',
              resolve: {
                data: Get_Conceptos_Incapacidades2
              }
            }
          }
        }
      }
    ];
  }

  Get_Conceptos_Incapacidades.$inject = ['$q', 'IncapacidadesSvc'];

  function Get_Conceptos_Incapacidades($q, IncapacidadesSvc) {
    var deferred = $q.defer();
    IncapacidadesSvc.Get_Conceptos_Incapacidades().then(function(data) {
      deferred.resolve(data);
    });
    return deferred.promise;
  }

  Get_Conceptos_Incapacidades2.$inject = ['$q', 'IncapacidadesSvc'];

  function Get_Conceptos_Incapacidades2($q, IncapacidadesSvc) {
    var deferred = $q.defer();
    IncapacidadesSvc.Get_Conceptos_Licencias().then(function(data) {
      deferred.resolve(data);
    });
    return deferred.promise;
  }

  Cargar_Historico_Licencias.$inject = ['$q', 'IncapacidadesSvc'];

  function Cargar_Historico_Licencias($q, IncapacidadesSvc) {
    var deferred = $q.defer();
    IncapacidadesSvc.Cargar_Historico_Licencias().then(function(data) {
      deferred.resolve(data);
    });
    return deferred.promise;
  }
})();
