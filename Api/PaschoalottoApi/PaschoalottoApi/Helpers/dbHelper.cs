using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PaschoalottoApi.Helpers
{
    public class dbHelper
    {

        private string connectionString { get; set; }
        private SqlConnection sqlConnection { get; set; }
        private SqlTransaction sqlTransaction { get; set; }
        private bool isOpenedHere { get; set; }


        public dbHelper(ConHelper conHelper = null)
        {
            this.connectionString = ConfigurationManager.ConnectionStrings["defaultConnectionString"].ConnectionString;

            this.isOpenedHere = conHelper == null ? true : false;

            this.sqlConnection = conHelper == null ? new SqlConnection(connectionString) : conHelper.Connection;

            this.sqlTransaction = conHelper == null ? null : conHelper.Transaction;
        }

        #region ExecuteNonQuery
        /// <summary>
        /// Executa um comando sql que não seja uma consulta. Retorna a quantidade de linhas afetadas
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string queryString, SqlParameter[] _parameters = null)
        {

            int linhasAfetadas = 0;

            try
            {

                SqlCommand sqlCommand = this.GenerateSqlCommand(queryString, _parameters);

                this.OpenConnection();

                linhasAfetadas = sqlCommand.ExecuteNonQuery();

                this.CloseConnection();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return linhasAfetadas;

        }
        #endregion


        #region ExecuteReader
        /// <summary>
        /// Execute uma consulta sql. Retorna um datatable com o resultado.
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public DataTable ExecuteReader(string queryString, SqlParameter[] _Parameters = null)
        {

            DataTable _Out = new DataTable();


            try
            {

                SqlCommand command = this.GenerateSqlCommand(queryString, _Parameters);

                this.OpenConnection();

                SqlDataReader _DataReader = command.ExecuteReader();

                _Out.Load(_DataReader);

                this.CloseConnection();

            }
            catch (Exception)
            {

                throw;


            }

            return _Out;

        }
        #endregion



        #region Métodos privados


        /// <summary>
        /// 
        /// </summary>
        private void OpenConnection()
        {

            if (sqlConnection.State == ConnectionState.Closed)
            {
                this.sqlConnection.Open();

                isOpenedHere = true;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        private void CloseConnection()
        {

            if (sqlConnection.State == ConnectionState.Open && isOpenedHere == true)
            {

                this.sqlConnection.Close();

            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryString"></param>
        /// <param name="_parameters"></param>
        /// <returns></returns>
        private SqlCommand GenerateSqlCommand(string queryString, SqlParameter[] _parameters = null)
        {

            SqlCommand command = new SqlCommand(queryString, this.sqlConnection);

            command.Transaction = this.sqlTransaction;

            if (_parameters != null)
            {

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddRange(_parameters);
            }

            return command;
        }

        #endregion


    }

}