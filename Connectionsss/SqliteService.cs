using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connectionsss
{
    public class SqliteService
    {
       
        static SqliteService sqliteService;

        readonly string connectionString = ConfigurationManager.ConnectionStrings["SqliteConnection"].ConnectionString;
        SqliteConnection connection;
      
        public  SqliteService()
        {
            connection = new SqliteConnection();
            connection.ConnectionString = connectionString;
            CreateTables();


            
        }
        void CreateTables()
        {
            string table = "Create Table If Not Exists KULLANICILAR (ID INTEGER PRIMARY KEY AUTOINCREMENT, MAIL VARCHAR(50), SIFRE VARCHAR(50),REMEMBER INTEGER DEFAULT 0)";
            string table2 = "create table if not exists MAILLER(ID INTEGER PRIMARY KEY AUTOINCREMENT,Mail varchar(50),Kullanici_Id INTEGER)";
          





            Execute(table);
            Execute(table2);
           

            
        }
            SqliteConnection OpenConnection()
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            return connection;
        }

        void CloseConnection()
        {
            connection.Close();
        }


        public SqliteCommand Execute(string commandText, params SqliteParameter[] parameters)
        {
            using (SqliteCommand command = new SqliteCommand()) { 

                command.CommandText = commandText;
                command.Connection = OpenConnection();
                command.CommandType = CommandType.Text;

                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }
                command.ExecuteNonQuery();
                CloseConnection();
                return command;
            
            }
        }

        public SqliteDataReader Reader(string commandText, params SqliteParameter[] parameters)
        {
            SqliteCommand command = new SqliteCommand();
            
                command.CommandText = commandText;
                command.Connection = OpenConnection();
                command.CommandType = CommandType.Text;

                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }
                SqliteDataReader dataReader = command.ExecuteReader();
                return dataReader;
            }

        public static SqliteService GetInstance()
        {
            if (sqliteService == null)
            {
                sqliteService = new SqliteService();
            }
            return sqliteService;
        }


    }
}
