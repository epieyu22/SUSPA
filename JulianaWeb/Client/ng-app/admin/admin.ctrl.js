(function () {
  'use strict';
  angular
    .module('app.admin')
    .controller('AdminCtrl', AdminCtrl)
    .controller('RolesCtrl', RolesCtrl)
    .controller('UsersCtrl', UsersCtrl)
    .controller('PermisossDialogCtrl', PermisossDialogCtrl);

  AdminCtrl.$inject = ['AdminSvc'];
  /* @ngInject */
  function AdminCtrl(AdminSvc) {
    var vm = this;
    vm.viewmodel = {};
    vm.Change_Password = Change_Password;
    activate();
    ////////////////
    function activate() {
    }

    function Change_Password() {
      AdminSvc.Change_Password(vm.viewmodel);
    }
  }

  RolesCtrl.$inject = ['AdminSvc', 'ngDialog', 'NgTableParams', 'data'];
  function RolesCtrl(AdminSvc, ngDialog, NgTableParams, data) {
    var vm = this;
    vm.config = {};
    vm.Open_Permisos_Dialog = Open_Permisos_Dialog;
    vm.Open_New_Role_Dialog = Open_New_Role_Dialog;

    activate();

    function activate() {
      vm.grid = new NgTableParams({}, { dataset: data });
      AdminSvc
        .Get_Permission()
        .then(function (roles) {
          vm.config.roles = roles;
        });
    }


    function Create_New_Role() {
      AdminSvc
        .Create_New_Role()
        .then(function () {

        });
    }


    function Open_New_Role_Dialog() {
      ngDialog.open({
        template: 'Client/ng-app/admin/roles/new-role-dialog.html',
        className: 'ngdialog-theme-plain',
        appendClassName: 'ngdialog-systems-theme',
      });
    }

    function Open_Permisos_Dialog(role) {
      ngDialog.open({
        template: 'Client/ng-app/admin/roles/permisos-dialog.html',
        className: 'ngdialog-theme-plain',
        appendClassName: 'ngdialog-systems-theme',
        controller: 'PermisossDialogCtrl',
        data: { config: vm.config, role: role }
      });
    }
  }


  UsersCtrl.$inject = ['AdminSvc', '$scope', 'ngDialog', 'NgTableParams'];
  function UsersCtrl(AdminSvc, $scope, ngDialog, NgTableParams) {
    var vm = this;
    vm.viewmodel = {};
    vm.viewmodel.users = $scope.ngDialogData ? $scope.ngDialogData.users : [];

    vm.Assing_Roles_to_Users = Assing_Roles_to_Users;
    vm.Close_Dialog = Close_Dialog;
    vm.Open_Roles_Dialog = Open_Roles_Dialog;
    activate();

    function activate() {
      vm.loading = true;
      AdminSvc.Get_Users().then(function (data) {
        vm.grid = new NgTableParams({}, { dataset: data.users });
        vm.data = data;
        vm.loading = false;
      });
    }



    function Open_Roles_Dialog() {
      ngDialog.open({
        template: 'Client/ng-app/admin/roles/roles-dialog.html',
        className: 'ngdialog-theme-plain',
        appendClassName: 'ngdialog-systems-theme',
        controller: 'UsersCtrl',
        controllerAs: 'vm',
        data: { users: vm.viewmodel.users }
      });
    }

    function Assing_Roles_to_Users() {
      AdminSvc
        .Assing_Roles_to_Users(vm.viewmodel)
        .then(function () {
          ngDialog.close();
        });
    }

    function Close_Dialog() {
      ngDialog.close();
    }
  }


  PermisossDialogCtrl.$inject = ['$filter', '$scope', 'ngDialog', 'AdminSvc']
  function PermisossDialogCtrl($filter, $scope, ngDialog, AdminSvc) {

    $scope.data = $scope.ngDialogData;
    $scope.selectedItems = [];

    // $scope.Check_Role = Check_Role;
    $scope.Close_Dialog = Close_Dialog;
    $scope.Save_Changes = Save_Changes;

    $scope.Get_Option_Selected = function (id) {
      var result = $filter('filter')($scope.data.config.roles, { Id: id });
      if (result.length) return result[0].selected;
      return false;
    }


    Check_Roles();
    // console.log($scope.data.config.roles)



    $scope.Toggle_Perimission = function (roleId) {
      var result = $filter('filter')($scope.data.config.roles, { Id: roleId });
      var index = $scope.selectedItems.indexOf(result[0]);
      if (!result.length) return;
      if (index == -1) {
        $scope.selectedItems.push(result[0]);
      } else {
        $scope.selectedItems.splice(index, 1);
      }
    }


    function Close_Dialog() {
      ngDialog.close()
    }

    function Check_Roles() {
      angular.forEach($scope.data.config.roles, function (role, key) {
        role.selected = false;
        angular.forEach($scope.data.role.Roles, function (value, key) {
          if (value.Role.Id == role.Id) {
            role.selected = true;
            $scope.selectedItems.push(role)
            return;
          }
        });
      });


      
      $scope.Id_59e8428d = $scope.Get_Option_Selected('59e8428d-8186-4096-9340-82102520bdfc');
      $scope.Id_91dc4721 = $scope.Get_Option_Selected('91dc4721-c079-41c0-8bd4-b248c20e0939');
      $scope.Id_39337578 = $scope.Get_Option_Selected('39337578-119d-4e52-a0ab-3c4e9079fbd0');
      $scope.Id_e5494def = $scope.Get_Option_Selected('e5494def-50dd-4243-86f3-f95d428ea92f');
      $scope.Id_7c1e7880 = $scope.Get_Option_Selected('7c1e7880-947b-4c5b-87da-e3222ba053b4');
      $scope.Id_15f128b4 = $scope.Get_Option_Selected('15f128b4-301c-4918-b2b0-7a1af93761a3');
      $scope.Id_a52483bb = $scope.Get_Option_Selected('a52483bb-5118-42b2-848a-d846a936b35a');
      $scope.Id_2700343d = $scope.Get_Option_Selected('2700343d-e885-4b56-bbe2-f838b1ce4f02');
      $scope.Id_492015e4 = $scope.Get_Option_Selected('492015e4-d5b5-4461-98ea-25f5d0ff8394');
      $scope.Id_7f2ea269 = $scope.Get_Option_Selected('7f2ea269-4074-4530-8371-18e1fca8c5c2');
      $scope.Id_6f57b492 = $scope.Get_Option_Selected('6f57b492-b919-438c-b52e-effe182d2602');
      $scope.Id_69a5152f = $scope.Get_Option_Selected('69a5152f-2420-4056-a179-a78a355d0ee7');
      $scope.Id_0e6a2caf = $scope.Get_Option_Selected('0e6a2caf-6b7f-4976-a165-f902e7bc7c14');
      $scope.Id_abd25157 = $scope.Get_Option_Selected('abd25157-ab95-46ee-89cd-28d3ff910d49');
      $scope.Id_7705535b = $scope.Get_Option_Selected('7705535b-ed70-4259-a9a9-5545712dbee5');
      $scope.Id_e41eed8b = $scope.Get_Option_Selected('e41eed8b-ef28-4002-97a5-c74ffe23dde4');
      $scope.Id_7d0ae81a = $scope.Get_Option_Selected('7d0ae81a-6009-4cfc-8ea4-1104bdf7adcf');
      $scope.Id_976876ab = $scope.Get_Option_Selected('976876ab-94d7-403f-b615-7faa6daa5577');
      $scope.Id_732ca543 = $scope.Get_Option_Selected('732ca543-4d77-4de2-980c-b0d3ed1ecbb8');  
      $scope.Id_7ax53281 = $scope.Get_Option_Selected('93375728-459d-44c2-a0ab-a4gjsd4c5dc');
      $scope.Id_1a334aa1 = $scope.Get_Option_Selected('7337fxd8-459d-62c2-a1ab-a4gjsd4c5dc');
      $scope.Id_5dx00121 = $scope.Get_Option_Selected('7237fxd8-as1d-6343-aas1-a42gjsdacr3c');
      $scope.Id_30007571 = $scope.Get_Option_Selected('30007571-999d-4eau-a12b-3c4e9421fbd');
      $scope.Id_30A5CC78 = $scope.Get_Option_Selected('2c26s6a1-9s25-95sc-1afc-a444ssc5dc');
      $scope.Id_3000AC78 = $scope.Get_Option_Selected('2c26s6a1-9s25-9111-ABCD-a444ssc5dc');
      $scope.Id_3ACM4C28 = $scope.Get_Option_Selected('1habch12-1icv-094d-SNCD-Ahchbnsnm2');
      $scope.Id_3ACM4C29 = $scope.Get_Option_Selected('1habch12-1icv-094d-SNCD-AASCCDF123');
      $scope.Id_PER4C157 = $scope.Get_Option_Selected('abd25157-ab95-4000-ascd-28d3ff910d49');


      //Dashboard
      $scope.Id_32b9b101 = $scope.Get_Option_Selected('32b9b101-8ada-47d6-b072-9911b2945c2b');

      $scope.Id_32d6ee52 = $scope.Get_Option_Selected('b8ce5432-ba3c-4bd3-8424-c7fe4f57373c');
      $scope.Id_b6264a0a = $scope.Get_Option_Selected('d8f576ff-d0fb-415f-ac2d-6752abc77b79');
      $scope.Id_8a843db3 = $scope.Get_Option_Selected('8c791c29-bf3d-4473-8d20-7fc59a1d5920');

    }




    function Save_Changes() {
      // var result = $filter('filter')(vm.data.config.roles, {Name: 'certificado'});
      AdminSvc.Assign_Permissions($scope.data.role.Id, $scope.selectedItems);
      Close_Dialog();
    }
  }

})();

