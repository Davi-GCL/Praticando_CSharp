using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;


namespace SistemaBanco
{
    public class ConexaoSql
    {
        SqlConnection conn = new SqlConnection();
        public ConexaoSql()
        {
            conn.ConnectionString = @"Data Source=OPERACIONAL39\SQLEXPRESS;Initial Catalog=teste_db;Persist Security Info=True;User ID=sa;Password=root";
        }

        public SqlConnection Conectar()
        {

            //Verifica se a conexao está fechada ou já está aberta antes de conectar
            if (conn.State == System.Data.ConnectionState.Closed)
            {
                conn.Open();
            }

            return conn;
        }

        public void Desconectar()
        {
            if(conn.State == System.Data.ConnectionState.Open) 
            {
                conn.Close();
            }
        }
    }
}

