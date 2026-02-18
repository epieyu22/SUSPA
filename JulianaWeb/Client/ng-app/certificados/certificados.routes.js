(function() {
  'use strict';
  angular.module('app.certificados').run(appRun);

  appRun.$inject = ['routerHelper'];
     /* @ngInject */
  function appRun(routerHelper) {
      routerHelper.configureStates(getStates());
  }

  function getStates() {
    return [
      {
        state: 'compropago',
        config: {
          url: '/Compropago',
          views: {
            "":{
              templateUrl: 'Client/ng-app/layout/layout.html',
              controller: 'LayoutCtrl',
              controllerAs: 'vm'
            },
            "MainContent@compropago": {
              templateUrl: 'Client/ng-app/certificados/compropago.html',
              controller: 'CompropagoCtrl',
              controllerAs: 'vm'
            }
          },
          resolve:{
            config: getCompropagoConfig
          }
        }
      },
      {
        state: 'compropago.admin',
        config: {
          url: '/Admin',
          views: {
            "":{
              templateUrl: 'Client/ng-app/layout/layout.html',
              controller: 'LayoutCtrl',
              controllerAs: 'vm'
            },
            "MainContent@compropago": {
              templateUrl: 'Client/ng-app/certificados/admin/admin-compropago.html',
              controller: 'AdminCertlaboralCtrl',
              controllerAs: 'vm'
            }
          },
          resolve:{
            data: getDefaultCertLabConfig
          }
        }
      },
      {
        state: 'compropago.config',
        config: {
          url: '/Config',
          views: {
            "":{
              templateUrl: 'Client/ng-app/layout/layout.html',
              controller: 'LayoutCtrl',
              controllerAs: 'vm'
            },
            "MainContent@compropago": {
              templateUrl: 'Client/ng-app/certificados/admin/config-compropago.html',
              controller: 'AdminCertlaboralCtrl',
              controllerAs: 'vm'
            }
          },
          resolve:{
            data: getDefaultCertLabConfig
          }
        }
      },
      {
        state: 'certlaboral',
        config: {
          url: '/Certlaboral',
          views: {
            "":{
              templateUrl: 'Client/ng-app/layout/layout.html',
              controller: 'LayoutCtrl',
              controllerAs: 'vm'
            },
            "MainContent@certlaboral": {
              templateUrl: 'Client/ng-app/certificados/certlaboral.html',
              controller: 'CertlaboralCtrl',
              controllerAs: 'vm'
            }
          },
          resolve:{
            config: getCertLabConfig
          }
        }
      },
      {
        state: 'certlaboralbFiltro',
        config: {
          url: '/Certlaboral/Filtro',
          views: {
            "":{
              templateUrl: 'Client/ng-app/layout/layout.html',
              controller: 'LayoutCtrl',
              controllerAs: 'vm'
            },
            "MainContent@certlaboralbFiltro": {
              templateUrl: 'Client/ng-app/certificados/certlab-filtro.html',
              controller: 'CompropagoCtrl',
              controllerAs: 'vm'
            }
          },
          resolve:{
            config: getCompropagoConfig
          }
        }
      },
      {
        state: 'certlaboral_admin',
        config: {
          url: '/CertLaboralAdmin',
          views: {
            "":{
              templateUrl: 'Client/ng-app/layout/layout.html',
              controller: 'LayoutCtrl',
              controllerAs: 'vm'
            },
            "MainContent@certlaboral_admin": {
              templateUrl: 'Client/ng-app/certificados/admin/admin-certlaboral.html',
              controller: 'AdminCertlaboralCtrl',
              controllerAs: 'vm'
            }
          },
          resolve:{
            data: getDefaultCertLabConfig
          }
        }
      },
      {
        state: 'certlaboral_config',
        config: {
          url: '/CertlabConfig',
          views: {
            "":{
              templateUrl: 'Client/ng-app/layout/layout.html',
              controller: 'LayoutCtrl',
              controllerAs: 'vm'
            },
            "MainContent@certlaboral_config": {
              templateUrl: 'Client/ng-app/certificados/admin/config-certlaboral.html',
              controller: 'AdminCertlaboralCtrl',
              controllerAs: 'vm'
            }
          },
          resolve:{
            data: getDefaultCertLabConfig
          }
        }
      },
      {
        state: 'retefuente',
        config: {
          url: '/Retefuente',
          views: {
            "":{
              templateUrl: 'Client/ng-app/layout/layout.html',
              controller: 'LayoutCtrl',
              controllerAs: 'vm'
            },
            "MainContent@retefuente": {
              templateUrl: 'Client/ng-app/certificados/retefuente.html',
              controller: 'RetefuenteCtrl',
              controllerAs: 'vm'
            }
          },
          resolve:{
            config: getRetefuenteConfig
          }
        }
      },
      {
        state: 'retefuenteAdmin',
        config: {
          url: '/RetefuenteAdmin',
          views: {
            "":{
              templateUrl: 'Client/ng-app/layout/layout.html',
              controller: 'LayoutCtrl',
              controllerAs: 'vm'
            },
            "MainContent@retefuenteAdmin": {
              templateUrl: 'Client/ng-app/certificados/admin/admin-retefuente.html',
              controller: 'RetefuenteAdminCtrl',
              controllerAs: 'vm'
            }
          },
          resolve:{
            empresas: Get_Empresas_Usuarios
          }
        }
      },
      {
        state: 'retefuente.config',
        config: {
          url: '/Config',
          views: {
            "":{
              templateUrl: 'Client/ng-app/layout/layout.html',
              controller: 'LayoutCtrl',
              controllerAs: 'vm'
            },
            "MainContent@retefuente": {
              templateUrl: 'Client/ng-app/certificados/admin/config-retefuente.html',
              controller: 'AdminCertlaboralCtrl',
              controllerAs: 'vm'
            }
          },
          resolve:{
            data: getDefaultCertLabConfig
          }
        }
      },
      {
        state: 'sri',
        config: {
          url: '/SRI',
          views: {
            "":{
              templateUrl: 'Client/ng-app/layout/layout.html',
              controller: 'LayoutCtrl',
              controllerAs: 'vm'
            },
            "MainContent@sri": {
              templateUrl: 'Client/ng-app/certificados/SRI/sri.html',
              controller: 'SRICtrl',
              controllerAs: 'vm'
            }
          }
        }
      }
    ];
  }

  getCompropagoConfig.$inject = ['$q', 'CertificadosSvc'];
  function getCompropagoConfig($q, CertificadosSvc) {
    var deferred = $q.defer();
    var Empresa = "NOBOSZ";
    CertificadosSvc.getCompropagoConfig(Empresa)
      .then(function(data){
        deferred.resolve(data);
      })
     return deferred.promise;
   }

   getRetefuenteConfig.$inject = ['$q', 'CertificadosSvc']
   function getRetefuenteConfig($q, CertificadosSvc) {
    var deferred = $q.defer();
     CertificadosSvc.getRetefuenteConfig()
      .then(function(data){
        deferred.resolve(data);
      })
     return deferred.promise;
   }

   getCertLabConfig.$inject = ['$q', 'CertificadosSvc'];
   function getCertLabConfig($q, CertificadosSvc){
    var deferred = $q.defer();
    CertificadosSvc.getCertLabConfig()
      .then(function(data){
        deferred.resolve(data);
      })
     return deferred.promise;
   }
   
   getDefaultCertLabConfig.$inject = ['$q', 'CertificadosSvc'];
   function getDefaultCertLabConfig($q, CertificadosSvc){
    var deferred = $q.defer();
    CertificadosSvc.getEmpresas()
      .then(function(data){
        deferred.resolve(data);
      })
     return deferred.promise;
   }


   Get_Empresas_Usuarios.$inject = ['$q', 'Certifica2Svc'];
   function Get_Empresas_Usuarios($q, Certifica2Svc){
    var deferred = $q.defer();
    Certifica2Svc.Get_Empresas_Usuarios()
      .then(function(data){
        deferred.resolve(data);
      })
     return deferred.promise;
   }

})();
