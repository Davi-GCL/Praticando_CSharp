using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace SistemaBanco
{
    public class ConsultaSql
    {
        public ConexaoSql conexao = new ConexaoSql();

        //Criacao do comando para o sql executar
        SqlCommand cmd = new SqlCommand();

        public void LerTabela(string tabela)
        {
            //Criacao do comando para o sql executar
            cmd.CommandText= $"select * from dbo.{tabela}";

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
            
            //Criacao do comando para o sql executar
            cmd.CommandText = "insert into dbo.mov (idConta,dataHora,valor,tipo) values (@idConta,@dataHora,@valor,@tipo)";

            try
            {
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@idConta", idConta);
                cmd.Parameters.AddWithValue("@dataHora", dataHora);
                cmd.Parameters.AddWithValue("@valor", valor);
                cmd.Parameters.AddWithValue("@tipo", tipo);

            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                //cmd.Parameters["@idConta"].Value = idConta;
                //cmd.Parameters["@dataHora"].Value = dataHora;
                //cmd.Parameters["@valor"].Value = valor;
                //cmd.Parameters["@tipo"].Value = tipo;
                
            }

            try
            {
                //Atribui o endereco do banco de dados onde serão executados os comandos
                cmd.Connection = conexao.Conectar();

                int rows = cmd.ExecuteNonQuery();

                conexao.Desconectar();

                return rows;
            }
            catch (Exception err)
            {

                Console.WriteLine(err.Message);

                return 0;
            }
        }

        public void Atualizar(string textUpdt)
        {
            cmd.CommandText = "update dbo.contas set saldo += @valor where codConta = @idConta";
        }
    }
}
