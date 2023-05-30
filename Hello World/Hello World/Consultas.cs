using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Security.Cryptography.X509Certificates;

namespace SistemaBanco
{
    public class ConsultaSql
    {
        public ConexaoSql conexao = new ConexaoSql();

        //Criacao do comando para o sql executar
        SqlCommand cmd = new SqlCommand();

        class LinhaContas
        {
            public string codConta;
            public string senha;
            public decimal saldo;
            public int tipo;
            public int idUsuario;
        }

        public void LerTabela(string paramConta, out decimal saldo)
        {
            saldo = 0;
            //Criacao do comando para o sql executar
            cmd.CommandText= $"select * from dbo.contas where codConta={paramConta}";

            //Atribui o endereco do banco de dados onde serão executados os comandos
            cmd.Connection = conexao.Conectar();

            //Executar comando retornando valor
            SqlDataReader reader = cmd.ExecuteReader();

            //Cria uma nova instancia do objeto que ira receber os valores presentes em uma das linhas da tabela
            var dadosConta = new LinhaContas();

            //Atribui os valores de cada linha
            while (reader.Read())
            { 
                //codConta = reader.GetString(0);
                //dadosConta.senha = reader.GetString(1);
                saldo = (decimal)reader.GetSqlMoney(2);
                //tipo = reader.GetInt32(3);
                //idUsuario = reader.GetInt32(4);
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
