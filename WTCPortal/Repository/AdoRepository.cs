using System;
using System.Text;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace WTCPortal.Repository
{
    public abstract class AdoRepository<TEntity> where TEntity : class
    {
        private static SqlConnection _connection;

        public AdoRepository(string connectionStringName)
        {
            StringBuilder errorMessages = new StringBuilder();

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
                _connection = new SqlConnection(connectionString);

                if (_connection.IsAvailable())
                {

                }

            }
            catch (SqlException ex)
            {
                for (int i = 0; i < ex.Errors.Count; i++)
                {
                    errorMessages.Append("Index #" + i + "\n" +
                        "Message: " + ex.Errors[i].Message + "\n" +
                        "LineNumber: " + ex.Errors[i].LineNumber + "\n" +
                        "Source: " + ex.Errors[i].Source + "\n" +
                        "Procedure: " + ex.Errors[i].Procedure + "\n");
                }
                Console.WriteLine(errorMessages.ToString());
            }
            catch (Exception)
            {

            }
        }

        public virtual TEntity PopulateRecord(SqlDataReader reader)
        {
            return null;
        }

        // for insert/update/delete
        public int ExecuteCommand(SqlCommand command)
        {
            StringBuilder errorMessages = new StringBuilder();
            int result = 0;
            command.Connection = _connection;
            command.CommandType = CommandType.StoredProcedure;
            _connection.Open();
            try
            {
                result = command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                for (int i = 0; i < ex.Errors.Count; i++)
                {
                    errorMessages.Append("Index #" + i + "\n" +
                        "Message: " + ex.Errors[i].Message + "\n" +
                        "LineNumber: " + ex.Errors[i].LineNumber + "\n" +
                        "Source: " + ex.Errors[i].Source + "\n" +
                        "Procedure: " + ex.Errors[i].Procedure + "\n");
                }
                Console.WriteLine(errorMessages.ToString());
            }
            catch (Exception)
            {

            }
            finally
            {
                _connection.Close();
            }
            return result;
        }

        protected IEnumerable<TEntity> ExecuteStoredProc(SqlCommand command)
        {
            var list = new List<TEntity>();
            command.Connection = _connection;
            command.CommandType = CommandType.StoredProcedure;
            _connection.Open();
            try
            {
                var reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        var record = PopulateRecord(reader);
                        if (record != null) list.Add(record);
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            finally
            {
                _connection.Close();
            }
            return list;
        }
        protected TEntity ExcStoreProcs(SqlCommand command)
        {
            TEntity records = null;
            command.Connection = _connection;
            command.CommandType = CommandType.StoredProcedure;
            _connection.Open();
            try
            {
                var reader = command.ExecuteReader();

                try
                {
                    while (reader.Read())
                    {
                        records = PopulateRecord(reader);
                        break;
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            finally
            {
                _connection.Close();
            }
            return records;
        }
        /*takes a single record from the database*/
        protected TEntity GetRecord(SqlCommand command)
        {
            TEntity record = null;
            command.Connection = _connection;
            _connection.Open();
            try
            {
                var reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        record = PopulateRecord(reader);
                        break;
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            finally
            {
                _connection.Close();
            }
            return record;
        }

        public void ExcStoreProcss(SqlCommand command)
        {
            command.Connection = _connection;
            command.CommandType = CommandType.StoredProcedure;
            _connection.Open();
            command.ExecuteNonQuery();
            _connection.Close();
        }
        public static TEntity ConvertFromDBValue<T>(object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return default(TEntity);
            else
                return (TEntity)obj;
        }

        /*takes multiple records from the database*/
        protected IEnumerable<TEntity> GetRecords(SqlCommand command)
        {
            var list = new List<TEntity>();
            command.Connection = _connection;
            _connection.Open();
            try
            {
                var reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                        list.Add(PopulateRecord(reader));
                }
                finally
                {
                    reader.Close();
                }
            }
            finally
            {
                _connection.Close();
            }
            return list;
        }
        protected TEntity ExecuteSingleQuery(SqlCommand command)
        {
            TEntity record = null;
            command.Connection = _connection;
            command.CommandType = CommandType.StoredProcedure;
            _connection.Open();
            try
            {
                var reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        record = PopulateRecord(reader);
                        break;
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            finally
            {
                _connection.Close();
            }
            return record;
        }
        protected IEnumerable<TEntity> ExecuteQuery(SqlCommand command)
        {
            var list = new List<TEntity>();
            command.Connection = _connection;
            command.CommandType = CommandType.StoredProcedure;
            _connection.Open();
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        var record = PopulateRecord(reader);
                        if (record != null)
                        {
                            list.Add(record);
                        }
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            finally
            {
                _connection.Close();
            }
            return list;
        }
    }
}