using PaschoalottoApi.Interfaces;
using System;
using System.Collections.Generic;
using PaschoalottoApi.Dto;
using System.Data.SqlClient;
using PaschoalottoApi.Helpers;

namespace PaschoalottoApi.Dal
{
    public class dbParcela : IDal
    {

        protected override IDto GetDto()
        {
            return new tpParcela();
        }


        protected override void LoadObjectProperties(IDto _Dto, bool LoadCascade, ConHelper _ConHelper = null)
        {


        }

        protected override void GetErrorList()
        {

            base.ErrorList = new Dictionary<string, string>();

            base.ErrorList.Add("pkparcela", "Identificador já cadastrado.");
            base.ErrorList.Add("unparcelanumero", "Numero da Parcela já cadastrado para o título.");
            base.ErrorList.Add("ckparcelavalor", "Valor deve ser preenchido.");
            base.ErrorList.Add("ckparcelanumero", "Número deve ser preenchido");

        }



    }
}