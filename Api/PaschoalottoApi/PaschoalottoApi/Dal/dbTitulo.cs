using PaschoalottoApi.Interfaces;
using System;
using System.Collections.Generic;
using PaschoalottoApi.Dto;
using System.Data.SqlClient;
using PaschoalottoApi.Helpers;

namespace PaschoalottoApi.Dal
{
    public class dbTitulo : IDal
    {

        protected override IDto GetDto()
        {
            return new tpTitulo();
        }


        protected override void LoadObjectProperties(IDto _Dto, bool LoadCascade, ConHelper _ConHelper = null)
        {

            tpTitulo _tpTitulo = (tpTitulo)_Dto;

            _tpTitulo.Parcelas = (new dbParcela()).SelectMany<tpParcela>("IdTitulo = " + _tpTitulo.IdTitulo.ToString());

        }


        protected override void GetErrorList()
        {

            base.ErrorList = new Dictionary<string, string>();

            base.ErrorList.Add("pktitulo", "Identificador já cadastrado.");
            base.ErrorList.Add("untitulonumero", "Código de Título já cadastrado.");
            base.ErrorList.Add("cktitulonumero", "Número deve ser preenchido.");
            base.ErrorList.Add("cktitulodevedornome", "Nome do Devedor deve ser preenchido");
            base.ErrorList.Add("cktitulodevedorcpf", "Cpf do Devedor deve ser preenchido");
            base.ErrorList.Add("cktitulojurospercentual", "Percentual do Juros não pode ser menor que zero.");
            base.ErrorList.Add("cktitulomultapercentual", "Percentual da Multa não pode ser menor que zero.");
            base.ErrorList.Add("cktitulovalororiginal", "Valor do Título não pode ser menor que zero.");
            base.ErrorList.Add("fkparcelatitulo", "Identificador de Título não encontrado na tabela de Títulos");

        }



    }
}