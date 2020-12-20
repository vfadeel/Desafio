using PaschoalottoApi.Bll;
using PaschoalottoApi.Dal;
using PaschoalottoApi.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaschoalottoApi.Models
{
    public class TitulosViewModel
    {

        public int IdTitulo { get; set; }
        public string Numero { get; set; }
        public string DevedorNome { get; set; }
        public decimal QuantidadeParcelas { get; set; }
        public decimal ValorOriginal { get; set; }
        public int DiasEmAtraso { get; set; }
        public decimal ValorAtualizado { get; set; }


        public List<TitulosViewModel> Mount(List<tpTitulo> _lstTitulos)
        {

            List<TitulosViewModel> _Out = new List<TitulosViewModel>();

            Titulo _Titulo = new Titulo();
            
            foreach (tpTitulo _tpTitulo in _lstTitulos)
            {
                TitulosViewModel _ViewModel = new TitulosViewModel();

                _ViewModel.IdTitulo = _tpTitulo.IdTitulo;
                _ViewModel.Numero = _tpTitulo.Numero;
                _ViewModel.DevedorNome = _tpTitulo.DevedorNome;
                _ViewModel.ValorOriginal = _tpTitulo.ValorOriginal;
                _ViewModel.QuantidadeParcelas = _Titulo.GetQuantidadeParcelas(_tpTitulo);
                _ViewModel.DiasEmAtraso = _Titulo.GetDiasEmAtraso(_tpTitulo);
                _ViewModel.ValorAtualizado = _Titulo.GetValorAtualizado(_tpTitulo);

                _Out.Add(_ViewModel);
            }

            return _Out;
        }

    }
}