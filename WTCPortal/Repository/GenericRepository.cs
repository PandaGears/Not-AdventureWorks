using System;
using System.Text;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using WTCPortal.Models;
using System.Data.Entity;

namespace WTCPortal.Repository
{
    public class GenericRepository<TEntity> where TEntity : class
    {

        private static SqlConnection _connection;

        public GenericRepository(string connectionStringName)
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
                result = -99;
            }
            catch (Exception)
            {
                result = -1000;
            }
            finally
            {
                _connection.Close();
            }
            return result;
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

    public static class SqlExtensions
    {
        public static bool IsAvailable(this SqlConnection conn)
        {
            try
            {
                if (conn.State != ConnectionState.Open)                
                    return false;                

            }
            catch (SqlException)
            {
                return false;
            }

            return true;
        }
    }

}