using PaschoalottoApi.Attributes;
using PaschoalottoApi.Interfaces;
using System;

namespace PaschoalottoApi.Dto
{
    public class tpParcela : IDto
    {
        [Database]
        [PrimaryKey]
        public int IdParcela { get; set; }
        [Database]
        public int IdTitulo { get; set; }
        [Database]
        public string Numero { get; set; }
        [Database]
        public DateTime DataVencimento { get; set; }
        [Database]
        public decimal Valor { get; set; }


        public override void isValid()
        {

            if (IdTitulo == 0)
            {
                throw new Exception("Título deve ser preenchido.");
            }

            if (string.IsNullOrWhiteSpace(this.Numero))
            {
                throw new Exception("Número deve ser preenchido.");
            }

            if (this.Numero.Length > 3)
            {
                throw new Exception("Número não pode possuir mais que 3 caracteres.");
            }

            if (Valor <= 0)
            {
                throw new Exception("Valor deve ser preenchido.");
            }

        }

    }
}