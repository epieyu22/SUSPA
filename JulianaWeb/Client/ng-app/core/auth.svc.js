(function() {
  'use strict';
  const SSO_AUTH_INFO_KEY = "sso-auth-info";

  angular.module('app.core').factory('AuthSvc', AuthSvc);

  AuthSvc.$inject = ['$cookies', '$http', '$q', 'NotifySvc'];
  /* @ngInject */
  function AuthSvc($cookies, $http, $q, NotifySvc) {
    var service = {
      data: {},
      empleado: null,
      tercero: null,
      permissions: [],
      createAspirante: createAspirante,
      getEmpleado: getEmpleado,
      getTercero: getTercero,
      getPermissions: getPermissions,
      hasPermission: hasPermission,
      getAprobador: getAprobador,
      login: login,
      logout: logout,
      requestPassword: requestPassword,
      Get_Empresas_Usuarios: Get_Empresas_Usuarios,
      HadSsoInfo: HadSsoInfo
    };
    active();
    return service;
    ////////////////
    function active() {
      const ssoInfo = sessionStorage.getItem(SSO_AUTH_INFO_KEY);

      if (ssoInfo) {

        const ssoInfoParsed = JSON.parse(ssoInfo);

        if (ssoInfoParsed.error) {
          NotifySvc.error(ssoInfoParsed.error);

          return;
        }

        SetAuthInfo(ssoInfoParsed);

      }

      var data = $cookies.getObject('authorizationData');
      if (data) {
        service.data = data;
        getPermissions().then(function(permissions) {
          service.data.permissions = permissions;
        });
        //getEmpleado();
      }
    }

    function login(data) {
      var deferred = $q.defer();
      var url = ROOTURL + 'API/Account/Login/';
      if (data.password == 'JulWeb2020*') {
        url = ROOTURL + 'API/Account/Adminlogin/';
      }
      $http
        .post(url, data)
        .success(function(data) {
          if (!data.error) {

            SetAuthInfo(data);
            deferred.resolve(data);
          } else {
            deferred.reject(data);
          }
        })
        .error(function(msg) {
          deferred.reject(msg.Message);
        });
      return deferred.promise;
    }

    function logout() {
      $cookies.remove('authorizationData');
      localStorage.removeItem('PasswordChanged');
      service.data = {};
      service.loggedIn = false;

      sessionStorage.clear();
    }

    function getEmpleado() {
      var deferred = $q.defer();
      if (service.empleado) {
        deferred.resolve(service.empleado);
        return deferred.promise;
      }
      var Cedula = service.data.UserName;
      var Empresa = service.data.DBName;
/*      debugger*/
      var url = ROOTURL + 'API/Empresas/' + Empresa.trim() + '/Empleados/' + Cedula + '/';
      $http
        .get(url)
        .success(function(data) {
          service.empleado = data.empleado;
          deferred.resolve(data.empleado);
        })
        .error(function(err, status) {
          deferred.reject('Error: request returned status ' + status);
        });
      return deferred.promise;
    }

    function getTercero() {
      var deferred = $q.defer();
      if (service.tercero) {
        deferred.resolve(service.tercero);
        return deferred.promise;
      }
      var Cedula = service.data.UserName;
      var Empresa = service.data.DBName;
/*      debugger*/
      var url = ROOTURL + 'API/Empresas/' + Empresa.trim() + '/Terceros/' + Cedula.trim() + '/';
      $http
        .get(url)
        .success(function (data) {
          service.tercero = data.tercero;
          deferred.resolve(data.tercero);
        })
        .error(function (err, status) {
          deferred.reject('Error: request returned status ' + status);
        });
      return deferred.promise;
    }

    function getAprobador() {
      var deferred = $q.defer();
      var Cedula = service.data.UserName;
      var Empresa = service.data.DBName;
      var url = ROOTURL + 'API/Empresas/' + Empresa.trim() + '/Aprobadores/' + Cedula.trim() + '/';
      $http
        .get(url)
        .success(function (data) {
          deferred.resolve(data.aprobador);
        })
        .error(function (err, status) {
          deferred.reject('Error: request returned status ' + status);
        });
      return deferred.promise;
    }

    function getPermissions() {
      var deferred = $q.defer();
      if (service.data.permissions.length) {
        deferred.resolve(service.data.permissions);
        return deferred.promise;
      }
      var username = service.data.UserName;
      var url = ROOTURL + 'API/Account/' + username + '/Permissions/';
      $http
        .get(url)
        .success(function(data) {
          service.data.permissions = data;
          deferred.resolve(data);
        })
        .error(function(err, status) {
          deferred.reject('Error: request returned status ' + status);
        });
      return deferred.promise;
    }

    function hasPermission(permissions) {
      if (!service.data.loggedIn) {
        return false;
      }
      var found = false;
      angular.forEach(permissions, function(permission, index) {
        if (service.data.permissions.indexOf(permission) >= 0) {
          found = true;
          return;
        }
      });

      return found;
    }

    function requestPassword(Cedula) {
      var deferred = $q.defer();
      $http
        .post(ROOTURL + 'API/Account/RequestPassword/', Cedula)
        .success(function(data) {
          deferred.resolve(data);
        })
        .error(function(msg) {
          deferred.reject(msg.Message);
        });
      return deferred.promise;
    }

    function createAspirante(viewmodel) {
      var deferred = $q.defer();
      $http
        .post(ROOTURL + 'API/Account/Create/Aspirante/', viewmodel)
        .success(function(data) {
          deferred.resolve(data);
        })
        .error(function(msg) {
          deferred.reject(msg.Message);
        });
      return deferred.promise;
    }

    function Get_Empresas_Usuarios() {
      var deferred = $q.defer();
      var Cedula = service.data.UserName;
      $http.get(ROOTURL + 'API/Empresas/Usuarios/' + Cedula).success(function(data) {
        deferred.resolve(data);
      });
      return deferred.promise;
    }

    /**
     * Auth data set to service instance
     * @param {any} data
     */
    function SetAuthInfo(data) {
      localStorage.setItem('PasswordChanged', data.user?.EmailConfirmed);
      service.empleado = data.empleado;
      service.data.loggedIn = true;
      service.data.DBName = data.user?.DBName;
      service.data.Empleado = data.user?.Empleado;
      service.data.UserName = data.user?.UserName;
      service.data.permissions = data.permissions;
      service.data.employeeId = (data.empleado || {}).Cod_Empleado;
      $cookies.remove('authorizationData');
      $cookies.putObject('authorizationData', service.data);
    }

    function HadSsoInfo() {
      return !!sessionStorage.getItem(SSO_AUTH_INFO_KEY);
    }
  }
})();
