using PaschoalottoApi.Dal;
using PaschoalottoApi.Dto;
using PaschoalottoApi.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace PaschoalottoApi.Bll
{
    public class Titulo
    {

        public decimal GetQuantidadeParcelas(tpTitulo _tpTitulo)
        {
            return _tpTitulo.Parcelas == null ? 0 : _tpTitulo.Parcelas.Count;
        }


        public int GetDiasEmAtraso(tpTitulo _tpTitulo)
        {
            int _Out = 0;

            if (_tpTitulo.Parcelas != null && _tpTitulo.Parcelas.Count > 0)
            {
                DateTime DataVencimentoMaisAntiga = this.GetDataVencimentoMaisAntiga(_tpTitulo);

                if (DataVencimentoMaisAntiga < DateTime.Now.Date)
                {
                    _Out = DateTime.Now.Date.Subtract(DataVencimentoMaisAntiga.Date).Days;
                }

            }
            return _Out;

        }

        public DateTime GetDataVencimentoMaisAntiga(tpTitulo _tpTitulo)
        {

            return _tpTitulo.Parcelas.Min(parcela => parcela.DataVencimento);

        }

        public decimal GetValorAtualizado(tpTitulo _tpTitulo)
        {

            decimal _Out = 0;

            foreach (tpParcela _tpParcela in _tpTitulo.Parcelas)
            {
                _Out += (new Parcela()).GetValorAtualizado(_tpParcela, _tpTitulo);
            }

            return _Out;
                
        }


        public int Insert(tpTitulo _tpTitulo)
        {

            ConHelper _ConHelper = new ConHelper();

            _ConHelper.Open();
            _ConHelper.BeginTransaction();

            int IdTitulo = 0;

            try
            {

                this.ValidateInsert(_tpTitulo, _ConHelper);

                this.FillCalculatedValues(_tpTitulo);

                _tpTitulo.IdTitulo = (new dbTitulo()).Insert(_tpTitulo, _ConHelper);

                this.InserirParcelasDoTitulo(_tpTitulo, _ConHelper);

                _ConHelper.CommitTransaction();

            }
            catch (Exception e)
            {
                _ConHelper.RollbackTransaction();

                throw;
            }
            finally
            {
                _ConHelper.Close();
            }

            return IdTitulo;
        }


        private void ValidateInsert(tpTitulo _tpTitulo, ConHelper _ConHelper)
        {

            if (_tpTitulo.Parcelas == null || _tpTitulo.Parcelas.Count == 0)
            {
                throw new Exception("Um título deve ter uma ou mais parcelas.");
            }

            if (this.TituloJaExiste(_tpTitulo, _ConHelper))
            {
                throw new Exception("Título com mesmo número já cadastrado.");
            }

        }

        private void FillCalculatedValues(tpTitulo _tpTitulo)
        {

            _tpTitulo.ValorOriginal = Decimal.Round(_tpTitulo.Parcelas.Sum(parcela => parcela.Valor), 2);

        }

        private void InserirParcelasDoTitulo(tpTitulo _tpTitulo, ConHelper _ConHelper)
        {

            foreach (tpParcela _tpParcela in _tpTitulo.Parcelas)
            {
                _tpParcela.IdTitulo = _tpTitulo.IdTitulo;

                (new dbParcela()).Insert(_tpParcela, _ConHelper);
            }

        }

        public bool TituloJaExiste(tpTitulo _tpTitulo, ConHelper _ConHelper = null)
        {
            bool _Out = false;

            tpTitulo _tpTituloComCodigo = (tpTitulo)(new dbTitulo()).Select(string.Format("Numero = {0}", _tpTitulo.Numero), false, _ConHelper);

            if (_tpTituloComCodigo.IdTitulo != 0)
            {
                _Out = true;
            }

            return _Out;
        }
    }
}