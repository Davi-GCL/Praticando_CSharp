using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;


namespace SistemaBanco
{
    public class ConsultaSql
    {
        public ConexaoSql conexao = new ConexaoSql();

        //Criacao do comando para o sql executar
        SqlCommand cmd = new SqlCommand();

        public (string agencia, string senha, decimal saldo, int tipo) LerTabela(string paramConta)
        {
            string agenciaT = "";
            string senhaT = "";
            decimal saldoT = 0;
            var tipoT = 0;

            //Criacao do comando para o sql executar
            cmd.CommandText= $"select * from dbo.contas where codConta={paramConta}";

            //Atribui o endereco do banco de dados onde serão executados os comandos
            cmd.Connection = conexao.Conectar();

            //Executar comando retornando valor
            SqlDataReader reader = cmd.ExecuteReader();

            if(reader.HasRows == false)
            {
                reader.Close();
                conexao.Desconectar();
                throw new ArgumentException("Conta inexistente");
            }

            //Cria uma nova instancia do objeto que ira receber os valores presentes em uma das linhas da tabela
            //var dadosConta = new LinhaContas();

            //Atribui os valores de cada linha
            while (reader.Read())
            {
                

                try { agenciaT = reader.GetString(1); } catch(Exception) { }
                try { senhaT = reader.GetString(2); } catch (Exception) { }
                saldoT = (decimal)reader.GetSqlMoney(3);
                tipoT = (int)reader.GetSqlByte(4);
                //idUsuario = reader.GetInt32(4);
            }
            //Console.WriteLine(saldo);
            reader.Close();
            conexao.Desconectar();

            return (agencia: agenciaT, senha: senhaT, saldo: saldoT , tipo: tipoT);
        }

        public void LerTabelaMov(string paramConta, ref Queue<Movs> fila)
        {
            DateTime datahora = new DateTime();
            decimal saldo = new decimal();

            //Criacao do comando para o sql executar
            cmd.CommandText= $"select * from dbo.mov where idConta={paramConta}";

            //Atribui o endereco do banco de dados onde serão executados os comandos
            cmd.Connection = conexao.Conectar();

            //Executar comando retornando valor
            SqlDataReader reader = cmd.ExecuteReader();

            //Atribui os valores de cada linha
            while (reader.Read())
            {
                
                datahora = reader.GetDateTime(2);
                saldo = (decimal)reader.GetSqlMoney(3);

                fila.Enqueue(new Movs() { dataHora = datahora, valor = saldo});
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

        public int RegistrarMov(string idConta, decimal valor, string tipo, DateTime dataHora)
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

        public void AtualizarSaldo(string codConta, decimal valor)
        {
            cmd.Connection = conexao.Conectar();
            cmd.CommandText = "update dbo.contas set saldo += @valor where codConta = @idConta";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@idConta", codConta);
            cmd.Parameters.AddWithValue("@valor", valor);
            
            cmd.ExecuteNonQuery();

            conexao.Desconectar();
        }
    }
}
