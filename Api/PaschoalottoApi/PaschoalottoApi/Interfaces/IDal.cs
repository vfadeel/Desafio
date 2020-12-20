using PaschoalottoApi.Attributes;
using PaschoalottoApi.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;

namespace PaschoalottoApi.Interfaces
{
    public abstract class IDal
    {

        //Propriedades
        private SqlParameter[] Parameters { get; set; }
        private IDto Dto { get; set; }
        public Dictionary<string, string> ErrorList { get; set; }


        //Métodos abstratos
        protected abstract IDto GetDto();
        protected abstract void GetErrorList();
        protected abstract void LoadObjectProperties(IDto _Dto, bool LoadCascade, ConHelper _ConHelper = null);


        //Métodos
        public int Insert(IDto _Dto, ConHelper _ConHelper)
        {

            int _Out = 0;

            try
            {

                this.Dto = _Dto;

                this.Dto.isValid();

                this.ModelToParameters();

                (new dbHelper(_ConHelper)).ExecuteNonQuery(this.GetProcedureInsert(), this.Parameters);

                _Out = (int)this.Parameters[0].Value;

            }
            catch (Exception e)
            {

                throw new Exception(TraduzirException(e.Message));

            }

            return _Out;
        }


        public int Update(IDto _Dto, bool LoadCascade = false, ConHelper _ConHelper = null)
        {

            int linhasAlteradas = 0;

            try
            {

                this.Dto = _Dto;

                this.Dto.isValid();

                this.ModelToParameters();

                linhasAlteradas = (new dbHelper(_ConHelper)).ExecuteNonQuery(this.GetProcedureUpdate(), this.Parameters);

            }
            catch (Exception e)
            {

                throw new Exception(TraduzirException(e.Message));

            }

            return linhasAlteradas;

        }


        public int Delete(IDto _Dto, bool LoadCascade = false, ConHelper _ConHelper = null)
        {

            try
            {

                this.Dto = _Dto;

                PropertyInfo _PropertyInfo = _Dto.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(PrimaryKeyAttribute)) == true).First();

                SqlParameter _Parameter = new SqlParameter();

                _Parameter.ParameterName = "@" + _PropertyInfo.Name;
                _Parameter.Value = _PropertyInfo.GetValue(_Dto);

                (new dbHelper(_ConHelper)).ExecuteNonQuery(this.GetProcedureDelete(), new SqlParameter[] { _Parameter });

            }
            catch (Exception e)
            {

                throw new Exception(TraduzirException(e.Message));

            }

            return 0;
        }


        public IDto Select(string QueryString, bool LoadCascade = false, ConHelper _ConHelper = null)
        {

            IDto _Out = this.GetDto();

            try
            {

                DataTable _Result = (new dbHelper(_ConHelper)).ExecuteReader("SELECT TOP(1)* FROM " + _Out.GetModelName() + " WHERE " + QueryString);

                if (_Result.Rows.Count > 0)
                {
                    _Out = this.Fill(_Result.Rows[0]);
                }

                if (LoadCascade)
                {

                    this.LoadObjectProperties(_Out, LoadCascade, _ConHelper);

                }


            }
            catch (Exception e)
            {

                throw new Exception(TraduzirException(e.Message));

            }

            return _Out;

        }


        public List<T> SelectMany<T>(string QueryString, bool LoadCascade = false, ConHelper _ConHelper = null)
            where T : IDto
        {

            IDto _Dto = (IDto)this.GetDto();

            List<T> _Out = new List<T>();

            try
            {

                string WhereBuilder = string.IsNullOrWhiteSpace(QueryString) ? "" : " WHERE " + QueryString;

                DataTable _Result = (new dbHelper(_ConHelper)).ExecuteReader("SELECT * FROM " + _Dto.GetModelName() + WhereBuilder);

                foreach (DataRow _Row in _Result.Rows)
                {

                    _Dto = (T)this.Fill(_Row);

                    if (LoadCascade)
                    {
                        this.LoadObjectProperties(_Dto, LoadCascade, _ConHelper);
                    }

                    _Out.Add((T)_Dto);


                }

            }
            catch (Exception e)
            {

                throw new Exception(TraduzirException(e.Message));

            }

            return _Out;

        }


        public IDto SelectById(int IdDto, bool LoadCascade = false, ConHelper _ConHelper = null)
        {

            IDto _Out = this.GetDto();

            try
            {

                DataTable _Result = (new dbHelper(_ConHelper)).ExecuteReader(string.Format("SELECT TOP(1)* FROM {0} WHERE Id{0} = '{1}'", _Out.GetModelName(), IdDto.ToString()));

                if (_Result.Rows.Count > 0)
                {
                    _Out = this.Fill(_Result.Rows[0]);
                }

                if (LoadCascade)
                {

                    this.LoadObjectProperties(_Out, LoadCascade, _ConHelper);

                }


            }
            catch (Exception e)
            {

                throw new Exception(TraduzirException(e.Message));

            }

            return _Out;

        }


        #region Métodos Privados

        /// <summary>
        /// 
        /// </summary>
        private void ModelToParameters()
        {

            List<SqlParameter> _lstParameters = new List<SqlParameter>();

            foreach (PropertyInfo _PropertyInfo in this.Dto.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(DatabaseAttribute)) == true))
            {

                SqlParameter _Parameter = new SqlParameter();

                _Parameter.ParameterName = "@" + _PropertyInfo.Name;
                _Parameter.Value = _PropertyInfo.GetValue(Dto);

                if (_PropertyInfo.CustomAttributes.Where(a => a.AttributeType == typeof(PrimaryKeyAttribute)).Count() > 0)
                {

                    _Parameter.Direction = System.Data.ParameterDirection.Output;

                }

                _lstParameters.Add(_Parameter);

            }

            this.Parameters = _lstParameters.ToArray();

        }


        private string GetDtoName()
        {

            return string.Format("stp_{0}Insert", this.Dto.GetType().Name);

        }

        private string GetProcedureInsert()
        {

            return string.Format("stp_{0}Insert", this.Dto.GetType().Name.Remove(0, 2));

        }

        private string GetProcedureUpdate()
        {

            return string.Format("stp_{0}Update", this.Dto.GetType().Name.Remove(0, 2));

        }

        private string GetProcedureDelete()
        {

            return string.Format("stp_{0}Delete", this.Dto.GetType().Name.Remove(0, 2));

        }

        protected string TraduzirException(string ExceptionMessage)
        {

            string _Out = ExceptionMessage;

            this.GetErrorList();

            foreach (KeyValuePair<string, string> ErrorItem in this.ErrorList)
            {

                if (ExceptionMessage.ToLower().Contains(ErrorItem.Key))
                {

                    throw new Exception(ErrorItem.Value);

                }

            }

            return _Out;

        }


        public IDto Fill(DataRow _Row)
        {

            IDto Dto = this.GetDto();

            foreach (DataColumn _Column in _Row.Table.Columns)
            {

                PropertyInfo _Property = Dto.GetType().GetProperty(_Column.ColumnName);

                if (_Row[_Column] != DBNull.Value)
                {
                    _Property.SetValue(Dto, _Row[_Column]);
                }

            }

            return Dto;

        }


        #endregion


    }

}