using PaschoalottoApi.Attributes;
using PaschoalottoApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaschoalottoApi.Dto
{
    public class tpTitulo : IDto
    {
        [Database]
        [PrimaryKey]
        public int IdTitulo { get; set; }
        [Database]
        public string Numero { get; set; }
        [Database]
        public string DevedorNome { get; set; }
        [Database]
        public string DevedorCpf { get; set; }
        [Database]
        public decimal JurosPercentual { get; set; }
        [Database]
        public decimal MultaPercentual { get; set; }
        [Database]
        public decimal ValorOriginal { get; set; }

        public List<tpParcela> Parcelas { get; set; }

        public override void isValid()
        {

            if (string.IsNullOrWhiteSpace(this.Numero))
            {
                throw new Exception("Número deve ser preenchido.");
            }

            if (this.Numero.Length > 6)
            {
                throw new Exception("Número não pode possuir mais que 6 caracteres.");
            }

            if (string.IsNullOrWhiteSpace(this.DevedorNome))
            {
                throw new Exception("Nome do Devedor deve ser preenchido.");
            }

            if (this.DevedorNome.Length > 80)
            {
                throw new Exception("Nome do Devedor não pode possuir mais que 80 caracteres.");
            }


            if (string.IsNullOrWhiteSpace(this.DevedorCpf))
            {
                throw new Exception("Cpf do Devedor deve ser preenchido.");
            }

            if (this.DevedorNome.Length > 80)
            {
                throw new Exception("Cpf do Devedor não pode possuir mais que 11 caracteres.");
            }

            if (JurosPercentual < 0)
            {
                throw new Exception("Juros não pode ser menor que zero.");
            }

            if (MultaPercentual < 0)
            {
                throw new Exception("Multa não pode ser menor que zero.");
            }
        }

    }
}