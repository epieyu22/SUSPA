(function() {
  'use strict';
  angular
    .module('app.core')
    .factory('JQXGridSVC', JQXGridSVC);

  // JQXGridSVC.$inject = [];
  /* @ngInject */
  function JQXGridSVC() {
    var localizationobj = {
      '/': "/",
      ':': ":",
      firstDay: 1,
      days: {
        names: ["Domingo","Lunes","Martes","Miércoles","Jueves","Viernes","Sábado"],
        namesAbbr: ["Dom","Lun","Mar","Mié","Jue","Vie","Sáb"],
        namesShort: ["Do","Lu","Ma","Mi","Ju","Vi","Sá"]
      },
      months: {
        names: ["enero","febrero","marzo","abril","mayo","junio","julio","agosto","septiembre","octubre","noviembre","diciembre",""],
        namesAbbr: ["ene","feb","mar","abr","may","jun","jul","ago","sep","oct","nov","dic",""]
      },
      AM: ["a.m.","a.m.","A.M."],
      PM: ["p.m.","p.m.","P.M."],
      eras: [{"name":"D.C.","start":null,"offset":0}],
      twoDigitYearMax: 2029,
      patterns: {
        d: "dd/MM/yyyy",
        D: "dddd, dd' de 'MMMM' de 'yyyy",
        t: "hh:mm tt",
        T: "hh:mm:ss tt",
        f: "dddd, dd' de 'MMMM' de 'yyyy hh:mm tt",
        F: "dddd, dd' de 'MMMM' de 'yyyy hh:mm:ss tt",
        M: "dd MMMM",
        Y: "MMMM' de 'yyyy"
      },
      percentsymbol: "%",
      currencysymbol: "$",
      currencysymbolposition: "before",
      decimalseparator: ',',
      thousandsseparator: '.',
      pagergotopagestring: "Ir a la Página:",
      pagershowrowsstring: "Filas por páginas:",
      pagerrangestring: " de ",
      pagerpreviousbuttonstring: "Anterior",
      pagernextbuttonstring: "Siugiente",
      groupsheaderstring: "Arrastre una columna y sueltela aquí para agrupar por esa columna",
      sortascendingstring: "Ordenar Ascendente",
      sortdescendingstring: "Ordenar Descendente",
      sortremovestring: "Remover Orden",
      groupbystring: "Agrupar por esta columna",
      groupremovestring: "Remover agrupamiento",
      filterclearstring: "Limpiar",
      filterstring: "Filtrar",
      filtershowrowstring: "Mostrar Filas donde:",
      filtershowrowdatestring: "Mostrar Filas donde la Fecha:",
      filterorconditionstring: "O",
      filterandconditionstring: "Y",
      filterselectallstring: "(Seleccionar Todos)",
      filterchoosestring: "Seleccione:",
      filterstringcomparisonoperators: ['vacio', 'no vacio', 'contiene', 'contiene(buscar coincidencias)',
        'no contiene', 'no contiene(buscar coincidencias)', 'empieza con', 'empieza con(buscar coincidencias)',
        'termina con', 'termina con(buscar coincidencias)', 'igual', 'igual(buscar coincidencias)', 'nulo', 'no nulo'],
      filternumericcomparisonoperators: ['igual', 'no igual', 'menor que', 'menor que o igual', 'mayor que', 'mayor que o igual', 'nulo', 'no nulo'],
      filterdatecomparisonoperators: ['igual', 'no igual', 'menor que', 'menor que o igual', 'mayor que', 'mayor que o igual', 'nulo', 'no nulo'],
      filterbooleancomparisonoperators: ['igual', 'no igual'],
      validationstring: "El valor ingresado no es valido",
      emptydatastring: "No hay datos para mostrar",
      filterselectstring: "Seleccionar Filtro",
      loadtext: "Cargando...",
      clearstring: "Limpiar",
      todaystring: "Hoy"
    };

    var service = {
      generateGrid: generateGrid,
      ExportToXLS: ExportToXLS,
      ExportToPDF: ExportToPDF
    };

    return service;

    ////////////////

    function generateGrid(source, columns, config) {
      var dataAdapter = new $.jqx.dataAdapter(source);
      var options = {
        autoheight: true,
        altrows: true,
        width: '100%',
        source: dataAdapter,
        scrollmode: 'logical',
        selectionmode: 'none',
        enablehover: false,
        columns: columns,
        columnmenuopening: function (menu, datafield, height) {
            menu.height(150);
        },
        groupable: false,
        pageable: true,
        sortable: true,
        showfilterrow: true,
        filterable: true,
        theme: 'metro',
        localization: localizationobj
      }
      return $.extend(options, config);

    }

    function ExportToXLS(grid, filename) {
      $(grid).jqxGrid('exportdata', 'xls', filename);
    }

    function ExportToPDF(grid, filename) {
      $(grid).jqxGrid('exportdata', 'pdf', filename);
    }

  }
})();
