(function () {
  'use strict';
  angular.module('app.otrasnov').run(appRun);

  appRun.$inject = ['routerHelper'];
  /* @ngInject */
  function appRun(routerHelper) {
    routerHelper.configureStates(getStates());
  }

  function getStates() {
    return [{
      state: 'otrasnov',
      config: {
        url: '/OtrasNov',
        views: {
          "": {
            templateUrl: 'Client/ng-app/layout/layout.html',
            controller: 'LayoutCtrl',
            controllerAs: 'vm'
          },
          "MainContent@otrasnov": {
            templateUrl: 'Client/ng-app/otrasnov/otrasnov.html',
            controller: 'OtrasnovCtrl',
            controllerAs: 'vm'
          },
        },
        resolve: {
          Empresas: Get_Empresas_Usuarios,
        }
      }
    },
    {
      state: 'otrasnov_upload',
      config: {
        url: '/OtrasNov/Upload',
        views: {
          "": {
            templateUrl: 'Client/ng-app/layout/layout.html',
            controller: 'LayoutCtrl',
            controllerAs: 'vm'
          },
          "MainContent@otrasnov_upload": {
            templateUrl: 'Client/ng-app/otrasnov/otrasnov_upload.html',
            controller: 'OtrasnovCtrl',
            controllerAs: 'vm'
          },
        },
        resolve: {
          Empresas: Get_Empresas_Usuarios,
        }
      }
    },
    {
      state: 'Otrasnov_List',
      config: {
        url: '/OtrasNov/Result',
        params: {
          data: null,
          Empresa: null
        },
        views: {
          "": {
            templateUrl: 'Client/ng-app/otrasnov/list.html',
            controller: 'ListOtrasNovCtrl',
            controllerAs: 'vm'
          },
        }
      }
    }
    ];
  }


  Get_Empresas_Usuarios.$inject = ['$q', 'AuthSvc'];

  function Get_Empresas_Usuarios($q, AuthSvc) {
    var deferred = $q.defer();
    AuthSvc
      .Get_Empresas_Usuarios()
      .then(function (data) {
        deferred.resolve(data);
      });
    return deferred.promise;
  }

})();
