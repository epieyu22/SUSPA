(function() {
  'use strict';
  angular
    .module('app.hojavida')
    .controller('HojavidaCtrl', HojavidaCtrl)
    .controller('ReferenciasDialogCtrl', ReferenciasDialogCtrl);
  /* @ngInject */

  HojavidaCtrl.$inject = ['$filter', '$state', 'NotifySvc', 'JQXGridSVC', 'ngDialog', 'HojavidaSvc', 'data', 'tables', '$q', '$http'];

  function HojavidaCtrl($filter, $state, NotifySvc, JQXGridSVC, ngDialog, HojavidaSvc, data, tables, $q, $http) {
    var vm = this;    
    var years = [];
    var days = [];
    vm.DAYS = days;
    vm.YEARS = years;
    vm.data = {};

    vm.TIPO_DOCUMENTOS = [
      {id:"C", name: "Cedula"},
      {id:"E", name: "Extranjería"},
      {id:"N", name: "Nit"},
      {id:"P", name: "Pasaporte"},
      {id:"R", name: "Reg. Civil"},
      {id:"T", name: "T. Identidad"},
      {id:"U", name: "Num. Unico Identif."}
    ];
    vm.ESTADOS_CIVILES = [
      {id:"1", name: "Soltero"},
      {id:"2", name: "Casado"},
      {id:"3", name: "Separado"},
      {id:"4", name: "Viudo"},
      {id:"5", name: "Unión Libre"}
    ];
    vm.MONTHS = [
      { value: "01", name: "Enero" },
      { value: "02", name: "Febrero" },
      { value: "03", name: "Marzo" },
      { value: "04", name: "Abril" },
      { value: "05", name: "Mayo" },
      { value: "06", name: "Junio" },
      { value: "07", name: "Julio" },
      { value: "08", name: "Agosto" },
      { value: "09", name: "Septimbre" },
      { value: "10", name: "Octubre" },
      { value: "11", name: "Noviembre" },
      { value: "12", name: "Diciembre" }
    ];
    vm.NIVELES_IDIOMA = [
      { name: 'Básico', value: 'A1' },
      { name: 'Elemental', value: 'A2' },
      { name: 'Pre-intermedio', value: 'B1' },
      { name: 'Intermedio superior', value: 'B2' },
      { name: 'Avanzado', value: 'C1' },
      { name: 'Superior', value: 'C2' },
      { name: 'Nativo', value: 'N' }
    ];
    vm.nivelEstudio = [
      { value: "1", name: "Nivel Técnico Profesional" },
      { value: "2", name: "Nivel Tecnológico" },
      { value: "3", name: "Nivel Profesional" },
      { value: "4", name: "Especialización" },
      { value: "5", name: "Maestría" },
      { value: "6", name: "Doctorados" }
    ];
    vm.MONTHS = [
      {value:"01", name:"Enero"},
      {value:"02", name:"Febrero"},
      {value:"03", name:"Marzo"},
      {value:"04", name:"Abril"},
      {value:"05", name:"Mayo"},
      {value:"06", name:"Junio"},
      {value:"07", name:"Julio"},
      {value:"08", name:"Agosto"},
      {value:"09", name:"Septimbre"},
      {value:"10", name:"Octubre"},
      {value:"11", name:"Noviembre"},
      {value:"12", name:"Diciembre"}
    ];
    vm.sectoresEmpresa = [
      { value: "2", name: "Agricultura / Pesca / Ganadería" },
      { value: "5", name: "Construcción / obras" },
      { value: "7", name: "Educación" },
      { value: "8", name: "Energía" },
      { value: "9", name: "Entretenimiento / Deportes" },
      { value: "15", name: "Fabricación" },
      { value: "10", name: "Finanzas / Banca" },
      { value: "16", name: "Gobierno / No Lucro" },
      { value: "12", name: "Hostelería / Turismo" },
      { value: "3", name: "Informática / Hardware" },
      { value: "4", name: "Informática / Software" },
      { value: "13", name: "Internet" },
      { value: "23", name: "Legal / Asesoría" },
      { value: "18", name: "Materias Primas" },
      { value: "14", name: "Medios de Comunicación" },
      { value: "1", name: "Publicidad / RRPP" },
      { value: "19", name: "RRHH / Personal" },
      { value: "11", name: "Salud / Medicina" },
      { value: "17", name: "Servicios Profesionales" },
      { value: "21", name: "Telecomunicaciones" },
      { value: "22", name: "Transporte" },
      { value: "6", name: "Venta al consumidor" },
      { value: "20", name: "Venta al por mayor" }
    ];
    vm.areasEmpresa = [
      { value: "1", name: "Administración / Oficina" },
      { value: "15", name: "Almacén / Logística / Transporte" },
      { value: "16", name: "Atención a clientes" },
      { value: "17", name: "CallCenter / Telemercadeo" },
      { value: "18", name: "Compras / Comercio Exterior" },
      { value: "19", name: "Construccion y obra" },
      { value: "6", name: "Contabilidad / Finanzas" },
      { value: "5", name: "Dirección / Gerencia" },
      { value: "2", name: "Diseño / Artes gráficas" },
      { value: "7", name: "Docencia" },
      { value: "8", name: "Hostelería / Turismo" },
      { value: "4", name: "Informática / Telecomunicaciones" },
      { value: "9", name: "Ingeniería" },
      { value: "3", name: "Investigación y Calidad" },
      { value: "10", name: "Legal / Asesoría" },
      { value: "20", name: "Mantenimiento y Reparaciones Técnicas" },
      { value: "12", name: "Medicina / Salud" },
      { value: "21", name: "Mercadotécnia / Publicidad / Comunicación" },
      { value: "22", name: "Producción / Operarios / Manufactura" },
      { value: "13", name: "Recursos Humanos" },
      { value: "23", name: "Servicios Generales, Aseo y Seguridad " },
      { value: "11", name: "Ventas" }
    ];
    
    vm.Fec_Nacimiento = {};
    vm.selectedInstitucion = null;
    vm.otraInstitucion = null;
    vm.selectedTitulo = null;
    vm.otroTitulo = null;
    vm.selectedCargo = null;
    vm.otroCargo = null;
    vm.inputTitulo = inputTitulo; // Autocomplete input change handle function
    vm.inputInstitucion = inputInstitucion; // Autocomplete input change handle function
    vm.FormAcademicaVm = {};
    vm.FormAcademicaInicio = {};
    vm.FormAcademicaSalida = {};
    vm.AddFormAcademica = AddFormAcademica;
    vm.FORMACADEMICA = [];    
    vm.inputCargo = inputCargo;
    vm.ExpLaboralInicio = {};
    vm.ExpLaboralSalida = {};
    vm.expLaboralVm = {};
    vm.addExpLaboral = addExpLaboral;
    vm.FormatDesdeHasta = FormatDesdeHasta;
    vm.inputChanged = inputChanged;
    vm.selectProfesionReferencia = selectProfesionReferencia;    
    vm.referenciaVm = {}
    vm.idiomaVm = {};

    vm.EXPLABORAL = [];
    vm.IDIOMAS_HOJAVIDA = [];

    // Forms view model

    // Add methods
    vm.addReferencia = addReferencia;
    vm.addIdiomas = addIdiomas;

    // Removes
    vm.DeleteFormacionAcademica = DeleteFormacionAcademica;
    vm.removeExperienciaLab = removeExperienciaLab;
    vm.Delete_Idioma = Delete_Idioma;
    vm.Delete_Experiencia = Delete_Experiencia;
    vm.Delete_Referencia = Delete_Referencia;
    // Updates

    vm.editReferencia = editReferencia;
    vm.updateReferencia = updateReferencia;
    vm.Update_Hojavida = Update_Hojavida;

    // Dialogs
    vm.Open_Referencias_dialog = Open_Referencias_dialog;
    vm.Open_FormaEdu_dialog = Open_FormaEdu_dialog;

    // Metodos adicionales
    vm.showHojavida = showHojavida;
    vm.showNewIngreso = showNewIngreso;
    vm.aprobarAspirante = aprobarAspirante;
    vm.rechazarAspirante = rechazarAspirante;
    vm.filter = filter;
    vm.filterCiudades = filterCiudades;

    // Ciclo para mostrar los años
    for (var i = 2024; i > 1900; i--) {
      years.push(i);
    }
    for (var i = 1; i <= 31; i++) {
      days.push(i);
    }

    activate();

    //Funciones
    function activate() {
      vm.data = data;
      vm.tables = tables;   

      if(vm.data.Fec_Nacimiento){
        vm.Fec_Nacimiento.ano = parseInt(vm.data.Fec_Nacimiento.substr(0,4));
        vm.Fec_Nacimiento.mes = vm.data.Fec_Nacimiento.substr(4,2);
        vm.Fec_Nacimiento.dia = parseInt(vm.data.Fec_Nacimiento.substr(6,2));
      }
      vm.disabled = vm.data.Estado != 'P' && vm.data.Estado != 'A' ? true : false;
      if(vm.data.Cod_Ciudad){
        var ciudad = filter(vm.tables.ciudades, {Codigo: vm.data.Cod_Ciudad});
        vm.departamento = ciudad.Codigo_Departamento;
        filterCiudades();
      }
      if(vm.data.Cod_HojaVida){
        loadForeingData();
      }else{
        inithojavidaGrid()
      }

    }

    //Cargar foranidades de las tablas
    function loadForeingData() {
        vm.REFERENCIAS_PERSONALES = HojavidaSvc.referencias.query(
          {Cod_HojaVida: vm.data.Cod_HojaVida}
        );
        vm.IDIOMAS_HOJAVIDA = HojavidaSvc.IdiomasHojaVida.query(
          {Cod_HojaVida: vm.data.Cod_HojaVida}
        );
        vm.EXPLABORAL = HojavidaSvc.ExpLabroal.query(
          {Cod_HojaVida: vm.data.Cod_HojaVida}
        );
        vm.FORMACADEMICA = HojavidaSvc.ForAcademica.query(
          { Cod_HojaVida: vm.data.Cod_HojaVida }
        );
    }

    function filter(source, query, array) {
      var result = $filter('filter')(source, query);
      if (array) {
        return result
      } 
        return result[0];      
    }

    function inithojavidaGrid() {
      var hojavidaGridSource = {
        datatype: "json",
        datafields: [
          {name: 'Aspirante'},
          {name: 'Doc_Identidad'},
        ],
        localdata: vm.data.empleados
      };
      var hojavidaPendGridSource = {
        datatype: "json",
        datafields: [
          {name: 'Aspirante'},
          {name: 'Doc_Identidad'},
        ],
        localdata: vm.data.pendientes
      };
      var hojavidaAproGridSource = {
        datatype: "json",
        datafields: [
          {name: 'Aspirante'},
          {name: 'Doc_Identidad'},
        ],
        localdata: vm.data.aprobadas
      };
      var hojavidaGridColumns = [
        {text: 'Aspirante', dataField: 'Aspirante', },
        {text: 'Documento de identidad', dataField: 'Doc_Identidad', width: '200'},
        {
          text: 'Acciones',
          filterable: false,
          width: 100,
          align: 'center',
          cellsrenderer: function (index, datafield, value, defaultvalue, column, rowdata) {
            var button = "<a ng-click='vm.showHojavida("+rowdata.Doc_Identidad+")' href='#'>Ver</a>"
            return "<div class='jqx-grid-cell-middle-align' style='margin-top: 6px;'>"+ button + "</div>";
          }
        }
      ];
      var newHojavidaGridColumns = [
        {text: 'Aspirante', dataField: 'Aspirante', },
        {text: 'Documento de identidad', dataField: 'Doc_Identidad', width: '200'},
        {
          text: 'Acciones',
          filterable: false,
          width: 100,
          align: 'center',
          cellsrenderer: function (index, datafield, value, defaultvalue, column, rowdata) {
            var button = "<a ng-click='vm.showNewIngreso("+rowdata.Doc_Identidad+")' href='#'>Ver</a>"
            return "<div class='jqx-grid-cell-middle-align' style='margin-top: 6px;'>"+ button + "</div>";
          }
        }
      ];
      vm.hojavidaGrid = JQXGridSVC.generateGrid(hojavidaGridSource, hojavidaGridColumns);
      vm.hojavidaPendGrid = JQXGridSVC.generateGrid(hojavidaPendGridSource, newHojavidaGridColumns);
      vm.hojavidaAproGrid = JQXGridSVC.generateGrid(hojavidaAproGridSource, hojavidaGridColumns);
    }

    //Funciones para Referencias personales
    function addReferencia() {
      if(!vm.referenciaVm.Ocupacion){
        vm.referenciaVm.Ocupacion = vm.OcupacionReferencia;
      }
      HojavidaSvc
        .referencias.save(
          {Cod_HojaVida: vm.data.Cod_HojaVida},
          vm.referenciaVm,
          function(data){
            vm.REFERENCIAS_PERSONALES.push(data);
          });
      vm.referenciaVm = {};
      $('#AddReferenciaForm').modal('hide')
    }      

    function editReferencia(data) {
      vm.showRefrenciasForm = true;
      vm.referenciaVm = data;
    }

    function updateReferencia() {
      vm.showRefrenciasForm = false;
      HojavidaSvc.referencias.update(
        {Cod_HojaVida: vm.data.Cod_HojaVida},
        vm.referenciaVm);
      vm.referenciaVm = {};
      $('#AddReferenciaForm').modal('hide');
    }


    //Funciones para Experiencia Laboral
    function addExpLaboral() {

      vm.expLaboralVm.Fec_Ingreso = vm.ExpLaboralInicio.ano + vm.ExpLaboralInicio.mes;
      vm.expLaboralVm.Fec_Retiro = vm.ExpLaboralSalida.ano + vm.ExpLaboralSalida.mes;
      vm.expLaboralVm.Cod_HojaVida = data.Cod_HojaVida;
      vm.otroCargo = vm.expLaboralVm.otroCargo;
      vm.inputCargo = inputCargo;
      vm.ExpLaboralInicio = {};
      vm.ExpLaboralSalida = {};
      
      if (vm.selectedCargo) {
        vm.expLaboralVm.Cargo = vm.selectedCargo.originalObject.Nom_Cargo;
      } else {
        vm.expLaboralVm.Cargo = vm.otroCargo;
      }
      HojavidaSvc
        .ExpLabroal.save(
          { Cod_HojaVida: vm.data.Cod_HojaVida },
          vm.expLaboralVm,
          function (data) {
            vm.EXPLABORAL.push(data);
          });
      vm.expLaboralVm = {};
      $('#AddExpLaboralForm').modal('hide')

      vm.selectedCargo = null;
      vm.otroCargo = null;
    }

    function removeExperienciaLab(data) {
      vm.EXPLABORAL
        .splice(vm.EXPLABORAL.indexOf(data), 1);
      HojavidaSvc.referencias.delete({
        Cod_HojaVida: vm.data.Cod_HojaVida,
        id: data.Cod_ExpLaboral
      });

    }

    

    //Funciones para Idiomas
    function addIdiomas() {
      vm.idiomaVm.Cod_HojaVida = vm.data.Cod_HojaVida

      HojavidaSvc
        .IdiomasHojaVida.save(
          {Cod_HojaVida: vm.data.Cod_HojaVida},
          vm.idiomaVm,
          function(data){
            vm.IDIOMAS_HOJAVIDA.push(data);
          });
      vm.idiomaVm = {};
    }

    

    //Funciones para Formacion Academica
    function AddFormAcademica() {

      if (vm.selectedInstitucion) {
        vm.FormAcademicaVm.Cod_Institucion = vm.selectedInstitucion.originalObject.Cod_Institucion;
      } else {
        vm.FormAcademicaVm.Cod_Institucion = 0;
        vm.FormAcademicaVm.Otra_Institucion = vm.otraInstitucion;
      }
      
      if (vm.selectedTitulo) {
        vm.FormAcademicaVm.Cod_Titulo = vm.selectedTitulo.originalObject.Cod_Profesion;
      } else {
        vm.FormAcademicaVm.Cod_Titulo = 0;
        vm.FormAcademicaVm.Otro_Titulo = vm.otroTitulo;
      }

      vm.FormAcademicaVm.Inicio = FormAcademicaInicio.ano + FormAcademicaInicio.mes;
      if (vm.FormAcademicaVm.Estado != 'C') {
        vm.FormAcademicaVm.Salida = vm.FormAcademicaSalida.ano + vm.FormAcademicaSalida.mes;
      } else {
        vm.FormAcademicaVm.Salida = ""
      }

      vm.FormAcademicaVm.Cod_HojaVida = vm.data.Cod_HojaVida;

      HojavidaSvc.AddFormAcademica(vm.FormAcademicaVm, vm.adjuntosFormAcademica)
        .then(function (data) {
          console.log(data)
          vm.FORMACADEMICA.push(data);
          NotifySvc.success();
        }, function (error) {
          NotifySvc.error(error.Message);
        })


      vm.selectedInstitucion = null;
      vm.otraInstitucion = null;
      vm.selectedTitulo = null;
      vm.otroTitulo = null;
      vm.FormAcademicaInicio = {};
      vm.FormAcademicaSalida = {};
      vm.FormAcademicaVm = {};
      $('#AddEduForma').modal('hide');

    }

    if (typeof String.prototype.trim !== 'function') {
      String.prototype.trim = function () {
        return this.replace(/^\s+|\s+$/g, '');
      }
    }

    function Update_Hojavida(){
      vm.data.Fec_Nacimiento = vm.Fec_Nacimiento.ano + vm.Fec_Nacimiento.mes + vm.Fec_Nacimiento.dia;
      debugger
      vm.data.Tel1 = vm.data.Tel1?.trim();
      vm.data.Tel2 = vm.data.Tel2?.trim();
      vm.data.Celular = vm.data.Celular?.trim();
      HojavidaSvc.changeImage(vm.data.Cod_HojaVida, vm.profileImage)
        .then(function(){}, function(){NotifySvc.error();return})
      HojavidaSvc.Update_Hojavida(vm.data.Cod_HojaVida, vm.data)
        .then(function(){
          NotifySvc.success();
        }, function(){
          NotifySvc.error();
        });
    }

    function filterCiudades() {
      var ciudades = filter(
        vm.tables.ciudades,
        {Codigo_Departamento: vm.departamento},
        true
      );
      vm.ciudades = ciudades;
    }

    function showHojavida(data){
      $state.go('hojavida_aprobar_once', {Cedula: data});
    }

    function showNewIngreso(data){
      $state.go('hojavida_aprobar_new', {Cedula: data});
    }

    function aprobarAspirante() {
      HojavidaSvc.aprobarAspirante(vm.data)
        .then(function (data) {
          $('.modal').modal('hide');
          $state.go('hojavida_aprobar_new');
          NotifySvc.success();
        }, function (msg) {
          NotifySvc.error(msg.Message);
        })
    }

    function rechazarAspirante(){
      HojavidaSvc.rechazarAspirante(vm.data.Cod_HojaVida)
       .then(function (data) {
          $('.modal').modal('hide');
          $state.go('hojavida_aprobar_new');
          NotifySvc.success();
        }, function (msg) {
          NotifySvc.error(msg.Message);
        })
    }

    function  selectProfesionReferencia(data, obj){
      vm.referenciaVm.Ocupacion = data.originalObject.Descripcion
    }

    function inputChanged(data){
      vm.OcupacionReferencia = data;
    }

    function inputInstitucion(data) {
      vm.otraInstitucion = data;
    }

    function inputTitulo(data) {
      vm.otroTitulo = data;
    }

    function inputCargo(data) {      
      vm.otroCargo = data;
    }

    function FormatDesdeHasta(date) {
      var mes = filter(vm.MONTHS, {value: date.substr(4, 2)})
      return mes.name + " " + date.substr(0, 4);
    }

    function Delete_Referencia(referencia) {      
      HojavidaSvc
        .referenciasDelete.save(
          {Cod_HojaVida: vm.data.Cod_HojaVida},
          referencia,
          function(data){
            $state.go($state.current, {}, {reload: true});
          });
    }
    function DeleteFormacionAcademica(formacion) {
      
      HojavidaSvc
        .formacionDelete.save(
          { Cod_HojaVida: vm.data.Cod_HojaVida },
          formacion,
          function () {
            $state.go($state.current, {}, { reload: true });
          });
    }

    function Delete_Idioma(idioma) {      
      HojavidaSvc
        .idiomasDelete.save(
          { Cod_HojaVida: vm.data.Cod_HojaVida },
          idioma,
          function () {
            $state.go($state.current, {}, { reload: true });
          });
    }

    
    function Delete_Experiencia(experiencia) {
      
      HojavidaSvc
        .experienciaDelete.save(
          { Cod_ExpLaboral: experiencia.Cod_ExpLaboral},
          experiencia,
          function (data) {
            $state.go($state.current, {}, { reload: true });
          });
    }

    function Open_FormaEdu_dialog(referencia){
      ngDialog.open({
        template: 'Client/ng-app/hojavida/formaedu-dialog.html',
        className: 'ngdialog-theme-plain',
        appendClassName : 'ngdialog-systems-theme',
        controller: 'ReferenciasDialogCtrl',
        data: {tables: vm.tables,  Cod_HojaVida: vm.data.Cod_HojaVida, referencia: referencia}
      });
    }

    function Open_Referencias_dialog(referencia) {
      ngDialog.open({
        template: 'Client/ng-app/hojavida/referencias-dialog.html',
        className: 'ngdialog-theme-plain',
        appendClassName: 'ngdialog-systems-theme',
        controller: 'ReferenciasDialogCtrl',
        data: { tables: vm.tables, Cod_HojaVida: vm.data.Cod_HojaVida, referencia: referencia }
      });
    }
  }

  //Dialogo de referencias
  ReferenciasDialogCtrl.$inject = ['$scope', '$state', 'ngDialog', 'HojavidaSvc'];
  function ReferenciasDialogCtrl($scope, $state, ngDialog, HojavidaSvc) {

    $scope.data = $scope.ngDialogData;
    $scope.Close = ngDialog.close
    var datos = [];
    var years = []
    $scope.years = years;
    var vm = this;
    
    vm.selectedInstitucion = $scope.selectedInstitucion;
    vm.FormAcademicaVm = [];
    vm.FormAcademicaInicio = {};
    vm.FormAcademicaSalida = {};
    vm.FormAcademicaVm.Estado = "";
    activate();

    for (var i = 2024; i > 1900; i--) {
      years.push(i);
    }

 
    function activate(){
      if($scope.data.referencia){
        $scope.viewmodel = $scope.data.referencia;
        $scope.viewmodel.Cod_Ocupacion = {Descripcion: $scope.viewmodel.Ocupacion};
      }      
    }

    datos = $scope.nivelestudio = [
      { value: "1", name: "Nivel Técnico Profesional" },
      { value: "2", name: "Nivel Tecnológico" },
      { value: "3", name: "Nivel Profesional" },
      { value: "4", name: "Especialización" },
      { value: "5", name: "Maestría" },
      { value: "6", name: "Doctorados" }
    ];

    datos = $scope.MONTHS = [
      { value: "01", name: "Enero" },
      { value: "02", name: "Febrero" },
      { value: "03", name: "Marzo" },
      { value: "04", name: "Abril" },
      { value: "05", name: "Mayo" },
      { value: "06", name: "Junio" },
      { value: "07", name: "Julio" },
      { value: "08", name: "Agosto" },
      { value: "09", name: "Septimbre" },
      { value: "10", name: "Octubre" },
      { value: "11", name: "Noviembre" },
      { value: "12", name: "Diciembre" }
    ];

    $scope.AddFormAcademica = AddFormAcademica;
    //Agregar formación academica
    function AddFormAcademica() {           
      
      $scope.uploadFile = function (files) {
        var fd = new FormData();
        //Take the first selected file
        fd.append("file", files[0]);

        $http.post(uploadUrl, fd, {
          withCredentials: true,
          headers: { 'Content-Type': undefined },
          transformRequest: angular.identity
        }).success("Archivo recibido").error("Archivo no recibido");
      };      
      var Cod_Instucion = 0;
      var Otro_Titulo = "Otro titulo"
      vm.FormAcademicaVm.Otra_Institucion = $scope.selectedInstitucion;
      vm.FormAcademicaVm.Cod_Nivel = $scope.nivelSelected;
      vm.FormAcademicaVm.Estado = $scope.Estado;
      
      vm.FormAcademicaVm.Cod_Institucion = Cod_Instucion;
      if ($scope.Cod_Titulo) {
        vm.FormAcademicaVm.Cod_Titulo = $scope.Cod_Titulo.Cod_Profesion;
      } else {
        vm.FormAcademicaVm.Cod_Titulo = 0;
        vm.FormAcademicaVm.Otro_Titulo = $scope.otroTitulo;
      }
      vm.FormAcademicaVm.Cod_HojaVida = $scope.data.Cod_HojaVida;
      vm.FormAcademicaVm.Inicio = $scope.FormAcademicaInicio.ano + "-" + $scope.FormAcademicaInicio.mes ;
      vm.FormAcademicaVm.Salida = $scope.FormAcademicaSalida.ano + "-" + $scope.FormAcademicaSalida.mes ;
      $scope.FormAcademicaVm = vm.FormAcademicaVm;

      vm.FormAcademicaVm.file = $scope.uploadFile;

      HojavidaSvc.
        AddFormAcademica({ Cod_HojaVida: $scope.FormAcademicaVm.Cod_HojaVida },
          $scope.FormAcademicaVm,
          function (data) {
            $state.go($state.current, {}, { reload: true });
          });
      
      vm.selectedInstitucion = null;
      vm.otraInstitucion = null;
      vm.selectedTitulo = null;
      vm.otroTitulo = null;
      vm.FormAcademicaInicio = {};
      vm.FormAcademicaSalida = {};
      vm.FormAcademicaVm = {};
      $('#AddEduForma').modal('hide');

    }

    $scope.Add_Referencia = Add_Referencia;
    
    function Add_Referencia(){
      $scope.viewmodel.Ocupacion  = $scope.viewmodel.Cod_Ocupacion.Descripcion.trim();
      HojavidaSvc
        .referencias.save(
          {Cod_HojaVida: $scope.data.Cod_HojaVida},
          $scope.viewmodel,
          function(data){
            $state.go($state.current, {}, {reload: true});
          });
    }

   

    $scope.Update_Referencia = Update_Referencia;
    function Update_Referencia(){
      $scope.viewmodel.Ocupacion  = $scope.viewmodel.Cod_Ocupacion.Descripcion.trim();
      HojavidaSvc
        .referencias.update(
          {Cod_HojaVida: $scope.data.Cod_HojaVida},
          $scope.viewmodel,
          function(data){
            $state.go($state.current, {}, {reload: true});
          });
    }
  }
})();

