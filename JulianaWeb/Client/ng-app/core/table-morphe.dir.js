(function() {
  'use strict';

  angular
    .module('app.core')
    .directive('tableMorphe', tableMorphe);

  tableMorphe.$inject = ['$timeout'];
  function tableMorphe($timeout) {
    // Usage:
    //
    // Creates:
    //
    var directive = {
        link: link,
        restrict: 'A',
    };
    return directive;
    
    function link(scope, element, attrs) {

      function morphe(){
        var headertext = [];
        var headers = document.querySelectorAll("thead");
        var tablebody = document.querySelectorAll("tbody");

        for (var i = 0; i < headers.length; i++) {
          headertext[i]=[];
          for (var j = 0, headrow; headrow = headers[i].rows[0].cells[j]; j++) {
            var current = headrow;
            headertext[i].push(current.textContent);
          }
        }


        for (var h = 0, tbody; tbody = tablebody[h]; h++) {
          for (var i = 0, row; row = tbody.rows[i]; i++) {
            for (var j = 0, col; col = row.cells[j]; j++) {
              col.setAttribute("data-th", headertext[h][j]);
            }
          }
        }
      }

      $timeout(morphe, 1);
      
    }
  }
})();
