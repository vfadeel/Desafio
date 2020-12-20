angular.module("app")

    .controller("tituloController", function ($scope, $http) {

        $scope.txtBuscar = "";
        $scope.titulos = [{}];
        $scope.errormodal = "";
        $scope.isView = false;

        $scope.titulo = { IdTitulo: 0,  Numero: "", DevedorNome: "", DevedorCpf: "", JurosPercentual: "", MultaPercentual: "", Parcelas: [] }
        $scope.parcela = { Numero: "", DataVencimento: "", Valor: 0, IdTitulo: "" }


        var init = function () {


            getAllTitulos();

            modalConfig();

            document.getElementById("txtBuscar").focus();

        }

        $scope.onNovoTituloClick = function () {

            $scope.isView = false;

            $("#tituloModal").modal("show");

            limparModal();

        }

        $scope.onParcelaAdicionarClick = function (Parcela) {

            // axios.post(apiEndereco + 'titulo/add', Parcela);

            limparParcelaErrors();

            if (validarParcela(Parcela)) {

                Parcela.DataVencimento = converterDataToISO(Parcela);

                $scope.titulo.Parcelas.push(angular.copy(Parcela));

                limparParcela();

                sumValorOriginal();

                document.getElementById("txtParcelaNumero").focus();

            }

        }

        $scope.onDeleteParcela = function(Parcela){

            var i = 0;

            while(i < $scope.titulo.Parcelas.length){

                if(Parcela === $scope.titulo.Parcelas[i]){
                    $scope.titulo.Parcelas.splice(i);
                }

            }

            document.getElementById("txtParcelaNumero").focus();

        }

        $scope.onTituloAdicionarClick = function (Titulo) {

            if (validarTitulo(Titulo)) {

                $scope.titulo.Numero = Titulo.Numero;
                $scope.titulo.DevedorNome = Titulo.DevedorNome;
                $scope.titulo.DevedorCpf = Titulo.DevedorCpf;
                $scope.titulo.JurosPercentual = Titulo.JurosPercentual;
                $scope.titulo.MultaPercentual = Titulo.MultaPercentual;

                $http.post(apiEndereco + 'titulo/adicionar', $scope.titulo)
                .then(function (response) {
                    
                $scope.success = "Título inserido com sucesso.";
                $("#tituloModal").modal("hide");
                getAllTitulos();
                    
                })
                .catch(function (error) {
                    $scope.errormodal = error.data;
                });
                
            }

        }

        $scope.onTituloViewClick = function(Titulo){

            $http.get(apiEndereco + 'titulo/gettitulo/' + Titulo.IdTitulo)
            .then(function (response) {

                $scope.titulo = response.data;
                
                sumValorOriginal();

                $scope.isView = true;
                
                $("#tituloModal").modal("show");


            })
            .catch(function (error) {
                console.log(error);
                
                console.log(error.data);
                if(error.data != null && error.data.ExceptionMessage != ""){

                    $scope.error = error.data.ExceptionMessage;

                }else
                {
                    $scope.error = "Erro ao se conectar a API. Verifique o arquivo api.js";
                }

            });



        }

        var modalConfig = function () {

            $("#tituloModal").on("shown.bs.modal", function (e) {

                document.getElementById("txtTituloNumero").focus();

            });

        }

        var getAllTitulos = function () {

            $http.get(apiEndereco + 'titulo/gettitulos')
                .then(function (response) {

                    $scope.titulos = response.data;

                })
                .catch(function (error) {

                    if(error.data != null && error.data.message != ""){

                        $scope.error = error.data.Message;

                    }else
                    {
                        $scope.error = "Erro ao se conectar a API. Verifique o arquivo api.js";
                    }

                });

        }
        
        var limparParcela = function () {
            $scope.parcela = { Numero: "", DataVencimento: "", Valor: 0, IdTitulo: "" };
        }

        var limparParcelaErrors = function () {

            $scope.parcelaNumeroError = "";
            $scope.parcelaDataVencimentoError = "";
            $scope.parcelaValorError = "";

        }

        var limparTituloErrors = function () {

            $scope.tituloNumeroError = "";
            $scope.tituloDevedorNomeError = "";
            $scope.tituloDevedorCpfError = "";
            $scope.tituloJurosPercentualError = "";
            $scope.tituloMultaPercentualError = "";

        }

        var limparModal = function(){

            $scope.success = "";
            $scope.error = "";
            $scope.errormodal = "";
           
            $scope.titulo = { IdTitulo: 0, Numero: "", DevedorNome: "", DevedorCpf: "", JurosPercentual: "", MultaPercentual: "", Parcelas: [] }
            $scope.parcela = { Numero: "", DataVencimento: "", Valor: 0, IdTitulo: "" }
            $scope.valorOriginal = 0;
        }

        
        var converterDataToISO = function(Parcela){
            
            var Dia = Parcela.DataVencimento.substring(0, 2);
            var Mes = Parcela.DataVencimento.substring(3, 5);
            var Ano = Parcela.DataVencimento.substring(6, 10);

            return new Date(Ano + "-" + Mes + "-" + Dia + "T00:00");

        }

        var validarParcela = function (Parcela) {

            limparParcelaErrors();

            if (Parcela.Numero == "") {
                $scope.parcelaNumeroError = "Campo obrigatório";
                document.getElementById("txtParcelaNumero").focus();
                return false;
            }

            if (Parcela.Numero.length > 3) {
                $scope.parcelaNumeroError = "Máximo três digitos";
                document.getElementById("txtParcelaNumero").focus();
                return false;
            }

            if (Parcela.DataVencimento.length < 10) {
                $scope.parcelaDataVencimentoError = "Data não permitida.";
                document.getElementById("txtParcelaDataVencimento").focus();
                return false;
            }


            if (Parcela.DataVencimento == "") {
                $scope.parcelaDataVencimentoError = "Campo obrigatório";
                document.getElementById("txtParcelaDataVencimento").focus();
                return false;
            }

            if (Parcela.Valor < 0) {
                $scope.parcelaValorError = "Valor deve ser maior ou igual a zero.";
                document.getElementById("txtParcelaValor").focus();
                return false;
            }

            if(existeParcelaComMesmoNumero(Parcela)){
                $scope.parcelaNumeroError = "Número de parcela já cadastrado.";
                document.getElementById("txtParcelaNumero").focus();
                return false;
            }

            return true;
        }


        var validarTitulo = function (Titulo) {

            limparTituloErrors();

            if (Titulo.Numero == "") {
                $scope.tituloNumeroError = "Campo obrigatório";
                document.getElementById("txtTituloNumero").focus();
                return false;
            }

            if (Titulo.Numero.length > 6) {
                $scope.tituloNumeroError = "Máximo 6 digitos";
                document.getElementById("txtTituloNumero").focus();
                return false;
            }

            if (Titulo.DevedorNome == "") {
                $scope.tituloDevedorNomeError = "Campo obrigatório";
                document.getElementById("txtDevedorNome").focus();
                return false;
            }

            if (Titulo.DevedorNome.length > 80) {
                $scope.tituloDevedorNomeError = "Máximo de 80 caracteres";
                document.getElementById("txtDevedorNome").focus();
                return false;
            }

            if (onlyNumber(Titulo.DevedorCpf) == "") {
                $scope.tituloDevedorCpfError = "Campo obrigatório";
                document.getElementById("txtDevedorCpf").focus();
                return false;
            }

            if (onlyNumber(Titulo.DevedorCpf).length < 11) {
                $scope.tituloDevedorCpfError = "CPF incorreto.";
                document.getElementById("txtDevedorCpf").focus();
                return false;
            }

            if (onlyNumber(Titulo.DevedorCpf).length > 11) {
                $scope.tituloDevedorCpfError = "Máximo de 11 números.";
                document.getElementById("txtDevedorCpf").focus();
                return false;
            }

            if(Titulo.Parcelas.length == 0)
            {
                $scope.errormodal = "Título deve ter uma ou mais parcelas.";
                document.getElementById("txtParcelaNumero").focus();
                return false;
            }

            return true;
        }

        function onlyNumber(value) {

            var i = 0, out = "";

            while (i < value.length) {
                if (value.toString()[i] == Number(value.toString()[i])) {
                    out += value.toString()[i];

                }

                i++;
            }

            return out;

        }
        
        function existeParcelaComMesmoNumero(Parcela) {

            var _out = false;

            $scope.titulo.Parcelas.forEach(parcela => {
                
                if(parcela.Numero == Parcela.Numero){
                    _out = true;
                }

            });

            return _out;

        }

        function sumValorOriginal() {

            $scope.valorOriginal = 0;

            $scope.titulo.Parcelas.forEach(parcela => {
                $scope.valorOriginal += parcela.Valor;
            });

        }

        init();

    });
