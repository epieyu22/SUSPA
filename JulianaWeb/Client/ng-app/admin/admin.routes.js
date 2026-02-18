(function() {
  'use strict';
  angular.module('app.admin').run(appRun);

  appRun.$inject = ['routerHelper'];
     /* @ngInject */
  function appRun(routerHelper) {
      routerHelper.configureStates(getStates());
  }

  function getStates() {
    return [
      {
        state: 'password',
        config: {
          url: '/ChangePassword',
          views: {
            "":{
              templateUrl: 'Client/ng-app/layout/layout.html',
              controller: 'LayoutCtrl',
              controllerAs: 'vm'
            },
            "MainContent@password": {
              templateUrl: 'Client/ng-app/admin/password.html',
              controller: 'AdminCtrl',
              controllerAs: 'vm'
            }
          }
        }
      },
      {
        state: 'admin',
        config: {
          url: '/Admin',
          views: {
            "":{
              templateUrl: 'Client/ng-app/layout/layout.html',
              controller: 'LayoutCtrl',
              controllerAs: 'vm'
            },
            "MainContent@admin": {
              templateUrl: 'Client/ng-app/admin/admin.html',
              controller: 'AdminCtrl',
              controllerAs: 'vm'
            }
          }
        }
      },
      {
        state: 'admin.roles',
        config: {
          url: '/Roles',
          views: {
            "":{
              templateUrl: 'Client/ng-app/layout/layout.html',
              controller: 'LayoutCtrl',
              controllerAs: 'vm'
            },
            "MainContent@admin": {
              templateUrl: 'Client/ng-app/admin/roles/roles.html',
              controller: 'RolesCtrl',
              controllerAs: 'vm'
            }
          },
          resolve:{
            data: Get_Roles
          }
        }
      },
      {
        state: 'admin.users',
        config: {
          url: '/Users',
          views: {
            "":{
              templateUrl: 'Client/ng-app/layout/layout.html',
              controller: 'LayoutCtrl',
              controllerAs: 'vm'
            },
            "MainContent@admin": {
              templateUrl: 'Client/ng-app/admin/roles/users.html',
              controller: 'UsersCtrl',
              controllerAs: 'vm'
            }
          }
        }
      },
      {
        state: 'admin.permissions',
        config: {
          url: '/Permissions',
          views: {
            "":{
              templateUrl: 'Client/ng-app/layout/layout.html',
              controller: 'LayoutCtrl',
              controllerAs: 'vm'
            },
            "MainContent@admin": {
              templateUrl: 'Client/ng-app/admin/permissions/list.html',
              controller: 'AdminCtrl',
              controllerAs: 'vm'
            }
          }
        }
      },
      {
        state: 'admin.solicitudes',
        config: {
          url: '/Solicitudes',
          views: {
            "":{
              templateUrl: 'Client/ng-app/layout/layout.html',
              controller: 'LayoutCtrl',
              controllerAs: 'vm'
            },
            "MainContent@admin": {
              templateUrl: 'Client/ng-app/admin/solicitudes.html',
              controller: 'UsersCtrl',
              controllerAs: 'vm'
            }
          }
        }
      },
      {
        state: 'admin.aprobaciones',
        config: {
          url: '/aprobaciones',
          views: {
            "":{
              templateUrl: 'Client/ng-app/layout/layout.html',
              controller: 'LayoutCtrl',
              controllerAs: 'vm'
            },
            "MainContent@admin": {
              templateUrl: 'Client/ng-app/admin/aprobaciones.html',
              controller: 'AprobacionesCtrl',
              controllerAs: 'vm'
            }
          },
          resolve: {
            empresas: Get_Empresas_Usuarios
          }
        }
      }
    ];
  }

  Get_Roles.$inject = ['$q', 'AdminSvc'];
  function Get_Roles($q, AdminSvc){
    var deferred = $q.defer();
     AdminSvc
      .Get_Roles()
      .then(function(data){
        deferred.resolve(data);
      });
     return deferred.promise;
  }

  Get_Empresas_Usuarios.$inject = ['$q', 'AuthSvc'];
  function Get_Empresas_Usuarios($q, AuthSvc){
    var deferred = $q.defer();
     AuthSvc
      .Get_Empresas_Usuarios()
      .then(function(data){
        deferred.resolve(data);
      });
     return deferred.promise;
  }

})();
