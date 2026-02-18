(function() {
  'use strict';
  
  angular
    .module('app.adlogin')
    .controller('ADLoginCtrl', ADLoginCtrl)
  
  ADLoginCtrl.$inject = ['$http', '$state', 'AuthSvc'];
  /* @ngInject */
  function ADLoginCtrl($http, $state, AuthSvc) {
    var vm = this;
    vm.loading = false;
    vm.RegularLogin = RegularLogin;
    vm.initLogin = initLogin;
    ////////////////
    function initLogin() {
      vm.loading = true;
      GetADData();
    }


    activate();

    function activate(){
      if(AuthSvc.data.loggedIn){
        $state.go('dashboard');
      }
    }

    function RegularLogin(){
      vm.loading = true;
      var url = ROOTURL + 'API/AD/RegularLogin';
      $http
        .post(url, vm.viewmodel)
        .success(function(data){
          if(data.username){
            login(data.username);
          }
          if(data.error){
            vm.data = data;
            vm.loading = false;
            return;
          }
        })
        .error(function(err){
          console.log(err)
          vm.loading = false;
        });
    }

    function GetADData(){
      var url = ROOTURL + 'API/AD/login';
      $http
        .get(url)
        .success(function(data){
          console.log(data)
          if(data.error){
            vm.data = data;
            vm.loading = false;
            return;
          }
          if(data.username){
            login(data.username);
          }
        })
        .error(function(err){
          console.log(err)
          vm.loading = false;
          vm.data = {};
          vm.data.error = 'Ha ocurrido un error';
        });
    }


    function login(username) {
      var viewmodel = {
        username: username,
        password: username
      }
      vm.loading = true;
      AuthSvc.login(viewmodel)
        .then(function (data) {
          $state.go('dashboard');
          vm.loading = false;
        }, function (data) {
          vm.data = data;
          vm.loading = false;
        });
    }


  }

 

})();

