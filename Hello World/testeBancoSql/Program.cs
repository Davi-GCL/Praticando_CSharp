using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace testeBancoSql
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SqlConnection conn = new SqlConnection();

            conn.ConnectionString = @"Data Source=OPERACIONAL39\SQLEXPRESS;Initial Catalog=teste_db;Persist Security Info=True;User ID=sa;Password=root";
        
            //Verifica se a conexao está fechada ou já está aberta antes de conectar
            if(conn.State == System.Data.ConnectionState.Closed)
            {
                conn.Open();
            }

            //Criacao do comando para o sql executar
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText="select * from dbo.clientes";

            //Atribui o endereco do banco de dados onde serão executados os comandos
            cmd.Connection = conn;

            //Executar comando retornando valor
            SqlDataReader reader = cmd.ExecuteReader();

            //Imprime os valores de cada linha
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string nome = reader.GetString(1);

                Console.WriteLine(id + "" + nome);
            }

            reader.Close();
            conn.Close();
            Console.ReadLine();
        }
    }
}
