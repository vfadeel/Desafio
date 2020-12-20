using PaschoalottoApi.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaschoalottoApi.Bll
{
    public class Parcela
    {

        public decimal GetValorAtualizado(tpParcela _tpParcela, tpTitulo _tpTitulo)
        {

            decimal _Out = _tpParcela.Valor;

            _Out += this.GetMultaValor(_tpParcela, _tpTitulo.MultaPercentual);

            _Out += this.GetJurosValor(_tpParcela, _tpTitulo.JurosPercentual);

            return _Out;
        }

        public decimal GetDiasEmAtraso(tpParcela _tpParcela)
        {
            decimal _Out = 0;
            
            if (_tpParcela.DataVencimento < DateTime.Now.Date)
            {
                _Out = DateTime.Now.Date.Subtract(_tpParcela.DataVencimento.Date).Days;
            }

            return _Out;

        }

        private decimal GetMultaValor(tpParcela _tpParcela, decimal MultaPercentual)
        {

            decimal _Out = 0;

            if (_tpParcela.DataVencimento.Date < DateTime.Now.Date && MultaPercentual > 0)
            {
                _Out += _tpParcela.Valor * (MultaPercentual / 100);
            }

            return _Out;
        }

        private decimal GetJurosValor(tpParcela _tpParcela, decimal JurosPercentual)
        {

            decimal _Out = 0;

            if (JurosPercentual > 0)
            {
                _Out = ((JurosPercentual / 30) / 100) * this.GetDiasEmAtraso(_tpParcela) * _tpParcela.Valor;
            }

            return _Out;
        }

    }
}