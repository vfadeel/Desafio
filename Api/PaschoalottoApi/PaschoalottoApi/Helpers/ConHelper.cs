using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PaschoalottoApi.Helpers
{
    public class ConHelper
    {

        public SqlConnection Connection { get; set; }
        public SqlTransaction Transaction { get; set; }

        public ConHelper()
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["defaultConnectionString"].ConnectionString;

            this.Connection = new SqlConnection(ConnectionString);
        }

        public void Open()
        {
            if(!this.IsConnectionOpened())
                this.Connection.Open();
        }

        public void Close()
        {
            if (this.IsConnectionOpened())
                this.Connection.Close();
        }

        public void BeginTransaction()
        {
            try
            {

                if (this.Transaction == null)
                {
                    this.Transaction = this.Connection.BeginTransaction();
                }
                else
                {
                    throw new Exception("Transaction solicitada já está aberta.");
                }

            }
            catch
            {
                throw;
            }
            
        }
        public void RollbackTransaction()
        {
            try
            {
                if (this.Transaction != null)
                {
                    this.Transaction.Rollback();
                }
                else
                {
                    throw new Exception("Transaction solicitada não existe.");
                }

            }
            catch
            {
                throw;
            }

        }
        public void CommitTransaction()
        {
            try
            {
                if (this.Transaction != null)
                {
                    this.Transaction.Commit();
                }
                else
                {
                    throw new Exception("Transaction solicitada não existe.");
                }

            }
            catch
            {
                throw;
            }

        }

        private bool IsConnectionOpened()
        {
            bool _Out = false;

            if (this.Connection.State == System.Data.ConnectionState.Open)
            {
                _Out = true;
            }

            return _Out;
        }

    }
}