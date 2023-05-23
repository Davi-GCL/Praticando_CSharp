using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace SistemaBanco
{
    public class ConsultaSql
    {
        ConexaoSql conexao = new ConexaoSql();

        //Criacao do comando para o sql executar
        SqlCommand cmd = new SqlCommand();
        public void LerTabela()
        {
            //Criacao do comando para o sql executar
            cmd.CommandText= "select * from dbo.clientes";

            //Atribui o endereco do banco de dados onde serão executados os comandos
            cmd.Connection = conexao.Conectar();

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
            conexao.Desconectar();

        }

        public int RegistrarMov(string idConta, decimal valor, string tipo, string dataHora)
        {
            int idUsuario = 2;
            //Criacao do comando para o sql executar
            cmd.CommandText = $"insert into dbo.mov (idConta,dataHora,valor,tipo,idUsuario) values ({idConta},{dataHora},{valor},{tipo},{idUsuario})";

            //Atribui o endereco do banco de dados onde serão executados os comandos
            cmd.Connection = conexao.Conectar();

            int rows = cmd.ExecuteNonQuery();

            conexao.Desconectar();

            return rows;
        }
    }
}
