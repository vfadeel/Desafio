using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PaschoalottoApi.Helpers
{
    public class SqlParameterHelper
    {

        List<SqlParameter> _Parameters { get; set; }


        public SqlParameterHelper()
        {

            this._Parameters = new List<SqlParameter>();

        }

        public void Add(string ParameterName, object Value)
        {

            SqlParameter _SqlParameter = new SqlParameter();

            _SqlParameter.ParameterName = ParameterName;
            _SqlParameter.Value = Value;

            this._Parameters.Add(_SqlParameter);

        }

        public SqlParameter[] GetParameters()
        {

            return this._Parameters.ToArray();

        }


    }
}