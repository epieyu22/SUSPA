(function() {
  'use strict';
  angular.module('app.solicitudes').run(appRun);

  appRun.$inject = ['routerHelper'];
  /* @ngInject */
  function appRun(routerHelper) {
    routerHelper.configureStates(getStates());
  }

  function getStates() {
    return [
      {
        state: 'solicitudes',
        config: {
          url: '/Solicitudes',
          views: {
            '': {
              templateUrl: 'Client/ng-app/layout/layout.html',
              controller: 'LayoutCtrl',
              controllerAs: 'vm'
            },
            'MainContent@solicitudes': {
              templateUrl: 'Client/ng-app/solicitudes/solicitudes.html',
              controller: 'SolicitudesCtrl',
              controllerAs: 'vm'
            }
          },
          resolve: {
            data: getSolicitudes
          }
        }
      },
      {
        state: 'calendario',
        config: {
          url: '/Vacaciones/Calendario',
          views: {
            '': {
              templateUrl: 'Client/ng-app/layout/layout.html',
              controller: 'LayoutCtrl',
              controllerAs: 'vm'
            },
            'MainContent@calendario': {
              templateUrl: 'Client/ng-app/solicitudes/solicitudes.admin.html',
              controller: 'CalendarioVacacionesCtrl',
              controllerAs: 'vm'
            }
          },
          resolve: {
            data: Get_All_Solicitudes
          }
        }
      },
      {
        state: 'solicitud',
        config: {
          url: '/Solicitud/{Cod_Solicitud}',
          views: {
            '': {
              templateUrl: 'Client/ng-app/layout/layout.html',
              controller: 'LayoutCtrl',
              controllerAs: 'vm'
            },
            'MainContent@solicitud': {
              templateUrl: 'Client/ng-app/solicitudes/solicitud.html',
              controller: 'SolicitudesCtrl',
              controllerAs: 'vm'
            }
          },
          resolve: {
            data: Get_Solicitud_Data
          }
        }
      },
      {
        state: 'solicitudEmpresa',
        config: {
          url: '/Solicitud/{Empresa}/{Cod_Solicitud}',
          views: {
            '': {
              templateUrl: 'Client/ng-app/layout/layout.html',
              controller: 'LayoutCtrl',
              controllerAs: 'vm'
            },
            'MainContent@solicitudEmpresa': {
              templateUrl: 'Client/ng-app/solicitudes/solicitud.html',
              controller: 'SolicitudEmpresaCtrl',
              controllerAs: 'vm'
            }
          },
          resolve: {
            data: SolicitudEmpresaData
          }
        }
      },
      {
        state: 'solicitudLicencia',
        config: {
          url: '/Solicitud/{Empresa}/{Cod_Solicitud}',
          views: {
            '': {
              templateUrl: 'Client/ng-app/layout/layout.html',
              controller: 'LayoutCtrl',
              controllerAs: 'vm'
            },
            'MainContent@solicitudEmpresa': {
              templateUrl: 'Client/ng-app/solicitudes/solicitud.html',
              controller: 'SolicitudEmpresaCtrl',
              controllerAs: 'vm'
            }
          },
          resolve: {
            data: SolicitudEmpresaData
          }
        }
      },
      {
        state: 'solicitudes.aprobador',
        config: {
          url: '/Aprobador',
          views: {
            '': {
              templateUrl: 'Client/ng-app/layout/layout.html',
              controller: 'LayoutCtrl',
              controllerAs: 'vm'
            },
            'MainContent@solicitudes': {
              templateUrl: 'Client/ng-app/solicitudes/aprobador/list.html',
              controller: 'AprobadoresCtrl',
              controllerAs: 'vm'
            }
          },
          resolve: {
            Empresas: Get_Empresas_Usuarios
          }
        }
      },
      {
        state: 'cesantias',
        config: {
          url: '/Cesantias',
          views: {
            '': {
              templateUrl: 'Client/ng-app/layout/layout.html',
              controller: 'LayoutCtrl',
              controllerAs: 'vm'
            },
            'MainContent@cesantias': {
              templateUrl: 'Client/ng-app/cesantias/cesantias.html',
              controller: 'CesantiasCtrl',
              controllerAs: 'vm'
            }
          },
          resolve: {
            data: getCesantiasData
          }
        }
      }
    ];
  }

  getSolicitudes.$inject = ['$q', 'SolicitudesSvc'];
  function getSolicitudes($q, SolicitudesSvc) {
    var deferred = $q.defer();
    SolicitudesSvc.getSolicitudes().then(function(data) {
      deferred.resolve(data);
    });
    return deferred.promise;
  }

  getSolicitudesAprobador.$inject = ['$q', 'SolicitudesSvc'];
  function getSolicitudesAprobador($q, SolicitudesSvc) {
    var deferred = $q.defer();
    SolicitudesSvc.getSolicitudesAprobador().then(function(data) {
      deferred.resolve(data);
    });
    return deferred.promise;
  }

  getCesantiasData.$inject = ['$q', 'SolicitudesSvc'];
  function getCesantiasData($q, SolicitudesSvc) {
    var deferred = $q.defer();
    SolicitudesSvc.getCesantiasData().then(function(data) {
      deferred.resolve(data);
    });
    return deferred.promise;
  }

  Get_Solicitud_Data.$inject = ['$q', '$stateParams', 'SolicitudesSvc'];
  function Get_Solicitud_Data($q, $stateParams, SolicitudesSvc) {
    var deferred = $q.defer();
    SolicitudesSvc.Get_Solicitud_Data($stateParams.Cod_Solicitud).then(function(data) {
      deferred.resolve(data);
    });
    return deferred.promise;
  }

  SolicitudEmpresaData.$inject = ['$q', '$stateParams', 'SolicitudesSvc'];
  function SolicitudEmpresaData($q, $stateParams, SolicitudesSvc) {
    var deferred = $q.defer();
    SolicitudesSvc.SolicitudEmpresaData($stateParams.Empresa, $stateParams.Cod_Solicitud).then(function(data) {
      deferred.resolve(data);
    });
    return deferred.promise;
  }

  Get_All_Solicitudes.$inject = ['$q', 'SolicitudesSvc'];
  function Get_All_Solicitudes($q, SolicitudesSvc) {
    var deferred = $q.defer();
    SolicitudesSvc.Get_All_Solicitudes().then(function(data) {
      deferred.resolve(data);
    });
    return deferred.promise;
  }

  Get_Empresas_Usuarios.$inject = ['$q', 'Certifica2Svc'];
  function Get_Empresas_Usuarios($q, Certifica2Svc) {
    var deferred = $q.defer();
    Certifica2Svc.Get_Empresas_Usuarios().then(function(data) {
      deferred.resolve(data);
    });
    return deferred.promise;
  }
})();
