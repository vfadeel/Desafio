using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaschoalottoApi.Interfaces
{
    public abstract class IDto
    {

        public string GetModelName()
        {

            return this.GetType().Name.ToString().Remove(0, 2);

        }

        //Métodos abstratos
        public abstract void isValid();

    }
}