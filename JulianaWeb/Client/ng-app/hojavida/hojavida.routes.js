(function() {
  'use strict';
  angular.module('app.hojavida').run(appRun);

  appRun.$inject = ['routerHelper'];
     /* @ngInject */
  function appRun(routerHelper) {
      routerHelper.configureStates(getStates());
  }

  function getStates() {
    return [
      {
        state: 'hojavida',
        config: {
          url: '/HojaDeVida',
          views: {
            "":{
              templateUrl: 'Client/ng-app/layout/layout.html',
              controller: 'LayoutCtrl',
              controllerAs: 'vm'
            },
            "MainContent@hojavida": {
              templateUrl: 'Client/ng-app/hojavida/hojavida.html',
              controller: 'HojavidaCtrl',
              controllerAs: 'vm'
            }
          },
          resolve:{
            data: getHojavidaData,
            tables: getHojavidaTables
          }
        }
      },
      {
        state: 'hojavidaaprobar',
        config: {
          url: '/HojavidaAdmin',
          views: {
            "":{
              templateUrl: 'Client/ng-app/layout/layout.html',
              controller: 'LayoutCtrl',
              controllerAs: 'vm'
            },
            "MainContent@hojavidaaprobar": {
              templateUrl: 'Client/ng-app/hojavida/aprobar.html',
              controller: 'HojavidaCtrl',
              controllerAs: 'vm'
            }
          },
          resolve:{
            data: getHojavidaAprobar,
            tables: getHojavidaTables
          }
        }
      },
      {
        state: 'hojavida_aprobar_once',
        config: {
          url: '/Hojavida/:Cedula',
          views: {
            "":{
              templateUrl: 'Client/ng-app/layout/layout.html',
              controller: 'LayoutCtrl',
              controllerAs: 'vm'
            },
            "MainContent@hojavida_aprobar_once": {
              templateUrl: 'Client/ng-app/hojavida/individual.html',
              controller: 'HojavidaCtrl',
              controllerAs: 'vm'
            }
          },
          resolve:{
            data: getHojavida,
            tables: getHojavidaTables
          }
        }
      },
      {
        state: 'hojavida_aprobar_new',
        config: {
          url: 'Hoja-Vida-Nuevo/:Cedula',
          views: {
            "":{
              templateUrl: 'Client/ng-app/layout/layout.html',
              controller: 'LayoutCtrl',
              controllerAs: 'vm'
            },
            "MainContent@hojavida_aprobar_new": {
              templateUrl: 'Client/ng-app/hojavida/nuevo-ingreso.html',
              controller: 'HojavidaCtrl',
              controllerAs: 'vm'
            }
          },
          resolve:{
            data: getHojavida,
            tables: getHojavidaTables
          }
        }
      },
    ];
  }

  getHojavidaData.$inject = ['$q', 'HojavidaSvc']
  function getHojavidaData($q, HojavidaSvc) {
    var deferred = $q.defer();
     HojavidaSvc.getHojavidaData()
      .then(function (data) {
        deferred.resolve(data);
      });
     return deferred.promise;
  }


  getHojavidaAprobar.$inject = ['$q', 'HojavidaSvc']
  function getHojavidaAprobar($q, HojavidaSvc) {
    var deferred = $q.defer();
     HojavidaSvc.getHojavidaAprobar()
      .then(function (data) {
        deferred.resolve(data);
      });
     return deferred.promise;
  }

  getHojavidaTables.$inject = ['$q', 'HojavidaSvc']
  function getHojavidaTables($q, HojavidaSvc) {
    var deferred = $q.defer();
    HojavidaSvc.getHojavidaTables()
     .then(function (data) {
        deferred.resolve(data);
      });
    return deferred.promise;
  }

  getHojavida.$inject = ['$q', '$stateParams', 'HojavidaSvc']
  function getHojavida($q, $stateParams, HojavidaSvc){
    var deferred = $q.defer();
    var Cedula = $stateParams.Cedula;
    HojavidaSvc.getHojavida(Cedula)
     .then(function (data) {
        deferred.resolve(data);
      });
    return deferred.promise;
  }
})();
