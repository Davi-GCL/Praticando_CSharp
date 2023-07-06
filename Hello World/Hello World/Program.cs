using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Xml;
using System.Security.Cryptography;
using Hello_World;



//IDEIAS:
//  X-Fazer com o que o valor da propriedade Informacoes.saldo só possa ser alterado através de um metodo do objeto, que receberá a senha digitada e valor a ser retirado. Se a senha for autentica, acessa e modifica o valor do saldo;
//  X-Salvar as informações de usuarios cadastrados e sua movimentações em tabelas num banco de dados;
//  X-Gerar um arquivo como relatório do extrato bancario;
//  X-Função que ao gerar a nota de extrato, abre e mostra a janela do arquivo (no navegador)
//  X-Ajustar a coluna de data
//  X-Perguntar o periodo de tempo que o extrato irá abrangir

namespace SistemaBanco
{
    public class Movs
    {
        public DateTime dataHora;
        public decimal valor;
        public string tipo;
    }

    class ContaBancaria
    {
        public ContaBancaria(string agencia, string conta)
        {
            this.agencia = agencia;
            this.codConta = conta;

            do
            {
                try
                {
                    (this.agencia, this.senha, this.saldo, this.tipo) = conexao.LerTabela(this.codConta);
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Digite a conta novamente: ");
                    this.codConta = Console.ReadLine();
                    continue;
                }
            }while(true);
            
            Console.ReadLine();
            //conexao.LerTabelaMov(this.codConta, ref this.historicoMovs);
        }

        public ConsultaSql conexao = new ConsultaSql();

        public string codConta;
        
        public string agencia;

        public int tipo;

        public decimal saldo { get; private set; } //Esta propriedade pode ser acessada fora da classe, mas só pode ser modificada por metodos de dentro da classe;

        private byte[] senhaByte;
        private string senha;

        public Queue<Movs> historicoMovs = new Queue<Movs>();
        public Queue<string> historico = new Queue<string>();

        public void sincMovs()
        {
            conexao.LerTabelaMov(this.codConta, ref this.historicoMovs);
        }

        public bool acessSenha(string param)
        {
            var sha = SHA256.Create();

            if (senha == null) 
            {

                senha = param.GerarHash();
                return true;
            }
            else
            {
                //Console.WriteLine($"Senha Salva:{senha} \nSenha digitada: {param.GerarHash()}");
                if (senha == param.GerarHash() ) { return true; }
                else { return false; }
            }
        }

        //Metodo de movimentar saldo que atualiza o registro no banco de dados
        public void movimentarSaldo(string tipo, decimal valor)
        {
            if (string.Equals(tipo, "deposito", StringComparison.OrdinalIgnoreCase))
            {
                this.saldo += valor;
                //this.historicoMovs.Enqueue(new Movs() { valor = valor, dataHora = DateTime.Now } );
                conexao.AtualizarSaldo(this.codConta,valor);
            }
            else if (string.Equals(tipo, "saque", StringComparison.OrdinalIgnoreCase))
            {
                valor = 0m - valor;
                this.saldo += valor;
                //this.historicoMovs.Enqueue(new Movs() { valor = valor, dataHora = DateTime.Now });
                conexao.AtualizarSaldo(this.codConta, valor);
            }
            else if (string.Equals(tipo, "transferencia", StringComparison.OrdinalIgnoreCase))
            {
                valor = 0m - valor;
                this.saldo += valor;
                //this.historicoMovs.Enqueue(new Movs() { valor = valor, dataHora = DateTime.Now });
            }


            //Console.WriteLine(this.conexao.RegistrarMov(this.codConta, valor, tipo, DateTime.Now.ToString("HH:mm,dd/MM/yy")));
            Console.WriteLine(this.conexao.RegistrarMov(this.codConta, valor, tipo, DateTime.Now));

        }

    //Sobrecarga do metodo de movimentar saldo que atualiza o registro no banco de dados e realiza transferencia de saldo
        //public void movimentarSaldo(string tipo, decimal valor , string destinatario )
        //{

        //    if (string.Equals(tipo, "deposito", StringComparison.OrdinalIgnoreCase))
        //    {
        //        this.saldo += valor;
        //        this.historico.Enqueue("+R$" + valor.ToString());
        //        conexao.AtualizarSaldo(this.codConta, valor);
        //    }
        //    else if (string.Equals(tipo, "saque", StringComparison.OrdinalIgnoreCase))
        //    {
        //        this.saldo -= valor;
        //        this.historico.Enqueue("-R$" + valor.ToString());
        //        conexao.AtualizarSaldo(this.codConta, (valor * -1));
        //    }
        //    else if (string.Equals(tipo, "transferencia", StringComparison.OrdinalIgnoreCase))
        //    {
        //        this.saldo -= valor;
        //        this.historico.Enqueue("-R$" + valor.ToString());
        //    }


        //    Console.WriteLine(this.conexao.RegistrarMov(this.codConta, valor, tipo, DateTime.Now.ToString("HH:mm,dd/MM/yy")));

        //}

        public void transferirSaldo(string remetente, decimal valor, string destinatario)
        {
            //decimal sqlSaldo = new decimal();
            SqlCommand cmd = new SqlCommand();
            //cmd.CommandText = $"select saldo from dbo.contas where codConta={remetente}";
            cmd.Connection = conexao.conexao.Conectar();
            
            //SqlDataReader reader = cmd.ExecuteReader();

            //while (reader.Read())
            //{
            //    sqlSaldo = (decimal)reader.GetSqlMoney(0);
            //    Console.WriteLine(sqlSaldo);
            //}
            //reader.Close();

            //sqlSaldo < valor
            if (this.saldo < valor)
            {
                Console.WriteLine("Saldo insuficiente!");
            }
            else
            {
                //Retirando o valor da conta do remetente
                conexao.AtualizarSaldo(remetente, (valor * -1));

                //Adicionando o valor a conta do destinatario
                conexao.AtualizarSaldo(destinatario, valor);

                this.movimentarSaldo("transferencia", valor);
            }
            
            conexao.conexao.Desconectar();

        }
    }

    class Program
    {
        
        //enum Tipos {Deposito=0, Saque, Transferencia }
        static bool encerrar = false;

        static void Main(string[] args)
        {
            
            //ConsultaSql test = new ConsultaSql();
            //test.LerTabela();

            string[] opcoes = { "", "Depósito", "Saque", "Saldo", "Transferência", "Extrato", "Sair" };

        //Gera uma instancia da estrutura Dict<chave:valor>, onde chave receberá string e valor função que nao recebe e nem retorna valor (Action).
            //Dictionary<string, Func<ContaBancaria,bool>> operacoes = new Dictionary<string, Func<ContaBancaria, bool>>()
            //{
            //    { "depósito", (p) => depositar(p) },
            //    { "deposito", (p) => depositar(p) },
            //    { "depositar", (p) => depositar(p) },
            //    { "saque", (p) => sacar(p) },
            //    { "sacar", (p) => sacar(p) },
            //    { "extrato", (p) => gerarExtrato(p) },
            //    { "transferencia", (p) => transferir(p) },
            //    { "transferir", (p) => transferir(p) },
            //    { "sair", (p) => sair(p) }
            //};

            //operacoes.Add("deposito", (p) => depositar(p));
            //operacoes.Add("saque", (p) => sacar(p));
            //operacoes.Add("extrato", (p) => gerarExtrato(p));
            //operacoes.Add("sair", (p) => sair(p));

            Console.Title = "Interface do Banco"; //Define o texto na barra de cima da janela.

            //Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Você está entrando em nosso sistema de caixa eletrônico\n\nFavor informar o código e a agência de sua conta bancaria:\n");
            ( string agencia , string conta ) = entrarConta();
            ContaBancaria Usuario = new ContaBancaria(agencia , conta);

            do
            {
                Console.Clear();
                gerarMenu(opcoes);
                Console.Write("\n>> ");
                string opcaoEscolhida = Console.ReadLine();

                //Se o usuario digitar o numero da opcao, tentara passar o numero digitado como indice do vetor opcoes, onde a opcao corresponde esta posicionada
                if(string.Equals(opcaoEscolhida, "depósito", StringComparison.OrdinalIgnoreCase) == true 
                    ||  string.Equals(opcaoEscolhida, "depósito", StringComparison.OrdinalIgnoreCase) == true
                    || tryTransformOption(opcaoEscolhida, opcoes) == "Depósito")
                {
                    encerrar = depositar(Usuario);
                }
                else if(string.Equals(opcaoEscolhida, "saque", StringComparison.OrdinalIgnoreCase) == true
                    ||  string.Equals(opcaoEscolhida, "sacar", StringComparison.OrdinalIgnoreCase) == true
                    ||  tryTransformOption(opcaoEscolhida, opcoes) == "Saque")
                {
                    encerrar = sacar(Usuario);
                }
                else if(string.Equals(opcaoEscolhida, "saldo", StringComparison.OrdinalIgnoreCase) == true
                    ||  tryTransformOption(opcaoEscolhida, opcoes) == "Saldo")
                {
                    encerrar = exibirSaldo(Usuario);
                }
                else if (string.Equals(opcaoEscolhida, "transferência", StringComparison.OrdinalIgnoreCase) == true
                    ||  string.Equals(opcaoEscolhida, "transferencia", StringComparison.OrdinalIgnoreCase) == true
                    || tryTransformOption(opcaoEscolhida, opcoes) == "Transferência")
                {
                    encerrar = transferir(Usuario);
                }
                else if (string.Equals(opcaoEscolhida, "extrato", StringComparison.OrdinalIgnoreCase) == true
                    || tryTransformOption(opcaoEscolhida, opcoes) == "Extrato")
                {
                    encerrar = gerarExtrato(Usuario);
                }
                else if (string.Equals(opcaoEscolhida, "sair", StringComparison.OrdinalIgnoreCase) == true
                    || tryTransformOption(opcaoEscolhida, opcoes) == "Sair")
                {
                    encerrar = sair(Usuario);
                }
                else
                {
                    Console.WriteLine("Digite uma opção valida");
                    encerrar = false;
                }
                

                Console.WriteLine($"Relatorio final: Agência{Usuario.agencia}, Conta{Usuario.codConta}, Saldo{Usuario.saldo}");

                Console.ReadLine();
            }while (encerrar == false);

        }

        static void gerarMenu(string[]opcoes)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Seja Bem-Vindo ao \nBanco AEIOU\n");
            Console.WriteLine("Menu de opções:");
                
            for(int i=1;i < opcoes.Length; i++)
            {
                Console.WriteLine((i) + " - " + opcoes[i]);
            }
        }

        static bool exibirSaldo(ContaBancaria Usuario)
        {
            string resp;
            bool isValid ;


            Console.Clear();

        //Solicitação de senha para ver o dinheiro
            Console.Write("Digite a senha: ");
            do
            {
                resp = Console.ReadLine();
                //Verifica se essa conta já possui senha, se nao,a senha q o usuario digitar será a nova senha
                if (Usuario.acessSenha(resp))
                {
                    Console.WriteLine($"A conta de nº {Usuario.codConta} possui um saldo de: R${Usuario.saldo}");
                    isValid = true;
                }
                else
                {
                    Console.Write("Senha errada!Digite novamente");
                    isValid = false;
                }
            } while (isValid == false);

            Console.ReadLine();

            Console.Write("Deseja realizar outra operação?[S/N]: ");
            //Retorna true para encerrar caso o usuario escolha nao realizar outra operação
            return string.Equals(Console.ReadLine(), "N", StringComparison.OrdinalIgnoreCase);
        }
        static (string agencia , string conta) entrarConta()
        {
            int intAgencia;
            bool? isValid;
            string resp;
            //string[] retorno = new string[2];

            Console.WriteLine("Digite as informaçoes da conta:");

            //Recebe a resposta do usuario e verifica se é valida
            Console.Write("\n Agência: ");
            do
            {
                resp = Console.ReadLine();
                isValid = int.TryParse(resp, out intAgencia);
                if (isValid == false) { Console.Write("\nAgência invalida!\nDigite novamente: "); }
            } while (isValid == false);

            //retorno[0] = intAgencia.ToString();

        //Recebe o numero da conta
            Console.Write("\n Número da conta: ");
            do
            {
                isValid = null;
                resp = Console.ReadLine();

                foreach (char chr in resp)
                {
                    if ((int.TryParse(chr.ToString(), out int numero) != true) && (chr.ToString() != "-"))
                    {
                        isValid = false;
                        Console.Write("Número de conta invalido! Digite novamente: ");
                        break;
                    }
                }

            } while (isValid == false);

            //retorno[1] = resp;

            return (agencia: intAgencia.ToString() , conta:resp);
        }

    //Algoritmo para o usuario realizar o "deposito", recebe como parametro o ponteiro para o local da memoria do objeto passado.
        static bool depositar(ContaBancaria Usuario)
        {
            decimal valDeposito;
            bool? isValid;
            string resp;

            Console.Clear();
            //if (Usuario.codConta == null) { entrarConta(Usuario); }

            //Entrada do valor do deposito
            Console.Write("\nInforme o valor que deseja depositar: R$");
            do
            {
                resp = Console.ReadLine();
                isValid = decimal.TryParse(resp, out valDeposito) && valDeposito >= 0? true : false;

                if (isValid == false) { Console.Write("\nValor invalido!\nDigite novamente: "); }
            } while (isValid == false);

            Usuario.movimentarSaldo("deposito",valDeposito); //"Lembrar de Salvar no banco de dados os depositos e saques feitos nessa conta" 
            Console.WriteLine($"\n\nDepósito no valor de R${valDeposito} realizado com sucesso na conta {Usuario.codConta}, na agência de nº{Usuario.agencia}");
            Console.Write("Deseja realizar outra operação?[S/N]: ");

            //Retorna true para encerrar caso o usuario escolha nao realizar outra operação
            return string.Equals(Console.ReadLine(), "N", StringComparison.OrdinalIgnoreCase);
        }
        //</deposito>

        static bool sacar(ContaBancaria Usuario)
        {
            decimal valSaque;
            bool isValid = false;
            string resp;

            Console.Clear();
            //if (Usuario.codConta == null) { entrarConta(Usuario); }
            
            Console.WriteLine($"Olá dono da conta nº{Usuario.codConta}! Você possui atualmente R${Usuario.saldo} na conta.");
            Console.Write("\nInforme o valor que deseja sacar: R$");
            do
            {
                resp = Console.ReadLine();
                isValid = decimal.TryParse(resp, out valSaque);
                if (isValid == false || valSaque == 0m) {
                    isValid = false;
                    Console.Write("\nValor invalido!\nDigite novamente: "); 
                }
                else
                {
                    //Verifica se o usuario tem o saldo suficiente para sacar
                    if (valSaque > Usuario.saldo) 
                    {
                        Console.Write("Você não possui esse dinheiro em sua conta! Digite um valor valido: R$");
                        //Console.Write("Deseja solicitar um emprestimo?");
                        //Console.ReadLine();
                        isValid = false;
                    }
                    
                }
            } while (isValid == false);

        //Solicitação de senha para retirar dinheiro
            Console.Write("Digite a senha: ");
            do
            {
                resp = Console.ReadLine();
            //Verifica se essa conta já possui senha, se nao,a senha q o usuario digitar será a nova senha
                if (Usuario.acessSenha(resp)) 
                {
                    Console.WriteLine("Sacando dindin");
                    Usuario.movimentarSaldo("saque",valSaque);
                    isValid = true;
                }
                else 
                { 
                    Console.Write("Senha errada!Digite novamente: ");
                    isValid = false;
                }
            } while (isValid==false);

            //Retorna true para encerrar caso o usuario escolha nao realizar outra operação
            return string.Equals(Console.ReadLine(), "N", StringComparison.OrdinalIgnoreCase);
        }
    // </sacar>

        static bool gerarExtrato(ContaBancaria Usuario)
        {
            Usuario.sincMovs();
            DateTime dataAtual = DateTime.Now;
            string nomeArquivo = $"{dataAtual.ToString("dd-MM-yyyy")}_{Usuario.codConta.ToString()}_extrato.xml";
            string diretorioAtual = Environment.CurrentDirectory; //Pega o caminho da pasta em que o codigo está sendo executado.
            string caminhoCompleto = Path.Combine(diretorioAtual, nomeArquivo);
            
            bool isValid = false;
            string resp;

            //Exige a senha para acessar o extrato
            Console.Write("Digite a senha: ");
            do
            {
                resp = Console.ReadLine();
                //Verifica se essa conta já possui senha, se nao,a senha q o usuario digitar será a nova senha
                if (Usuario.acessSenha(resp))
                {
                    isValid = true;
                }
                else
                {
                    Console.Write("Senha errada! Digite novamente");
                    isValid = false;
                }
            } while (isValid==false);

            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null));

            XmlElement rootElement = xmlDoc.CreateElement("xml");
            xmlDoc.AppendChild(rootElement);

            XmlElement header = xmlDoc.CreateElement("Cabecalho");
            //header.InnerXml = "<numConta>Olha Galera</numConta>";
            rootElement.AppendChild(header);

            XmlElement numConta = xmlDoc.CreateElement("Conta");
            numConta.InnerText = Usuario.codConta;
            header.AppendChild(numConta);

            XmlElement numAgencia = xmlDoc.CreateElement("Agencia");
            numAgencia.InnerText = Usuario.agencia;
            header.AppendChild(numAgencia);

            XmlElement body = xmlDoc.CreateElement("Transacoes");
            body.SetAttribute("DataExped", dataAtual.ToString("dd/MM/yyyy"));
            rootElement.AppendChild(body);

            //Pergunta o periodo e verifica se o periodo de tempo informado é valido
            int inicio, fim;
            do
            {
                Console.Write("Digite o mês de início: ");
                inicio = int.Parse(Console.ReadLine());
                Console.Write("Digite o mês de fim: ");
                fim = int.Parse(Console.ReadLine());

                if (inicio > fim) { isValid=false; Console.WriteLine("\n Periodo invalido! Especifique um periodo valido"); }
                else { isValid=true; }

            } while (isValid == false);

             foreach (var aux in Usuario.historicoMovs)
             {
                DateTime data = aux.dataHora;

                if (data.Month >= inicio && data.Month <= fim)
                {
                     body.InnerXml += $"<p data=\"{aux.dataHora} \">{aux.valor}</p>";
                }

             }


            body.InnerXml += $"<SaldoAtual>{Usuario.saldo}</SaldoAtual>";
            xmlDoc.Save(caminhoCompleto);

            Console.WriteLine("Gerando o extrato...");
            Thread.Sleep(2000);

            System.Diagnostics.Process.Start("Chrome", Uri.EscapeDataString(caminhoCompleto));

            Console.Write("Deseja realizar outra operação?[S/N]: ");

            //Retorna true para encerrar caso o usuario escolha nao realizar outra operação
            return string.Equals(Console.ReadLine(), "N", StringComparison.OrdinalIgnoreCase);
        }

        static bool transferir(ContaBancaria Usuario)
        {

            decimal valSaque;
            bool? isValid;
            string resp;
            string destino;

            Console.Clear();
            if (Usuario.codConta == null) { entrarConta(); }

            Console.WriteLine("Transferencia via TED");

            Console.WriteLine($"Olá dono da conta nº{Usuario.codConta}! Você possui atualmente R${Usuario.saldo} na conta.");
            Console.Write("\nDigite o valor que deseja transferir: R$");
            do
            {
                resp = Console.ReadLine();
                isValid = decimal.TryParse(resp, out valSaque);
                if (isValid == false) { Console.Write("\nValor invalido!\nDigite novamente: "); }
                else
                {
                    //Verifica se o usuario tem o saldo suficiente para sacar
                    if (valSaque > Usuario.saldo)
                    {
                        Console.Write("Você não possui esse dinheiro em sua conta! Digite um valor valido: R$");
                        //Console.Write("Deseja solicitar um emprestimo?");
                        //Console.ReadLine();
                        isValid = false;
                    }

                }
            } while (isValid == false);

            Console.Write("Digite o código da conta que irá receber a transferência: ");
            destino = Console.ReadLine();

            //Solicitação de senha para retirar dinheiro
            Console.Write("Digite a senha: ");
            
            do
            {
                resp = Console.ReadLine();
                //Verifica se essa conta já possui senha, se nao,a senha q o usuario digitar será a nova senha
                if (Usuario.acessSenha(resp))
                {
                    Console.WriteLine("transferindo dindin");
                    Usuario.transferirSaldo(Usuario.codConta, valSaque, destino);
                    isValid = true;
                }
                else
                {
                    Console.Write("senha errada!digite novamente");
                    isValid = false;
                }
            } while (isValid==false);

            Console.Write("Deseja realizar outra operação?[S/N]: ");

            //Retorna true para encerrar caso o usuario escolha nao realizar outra operação
            return string.Equals(Console.ReadLine(), "N", StringComparison.OrdinalIgnoreCase);
        }

        static bool sair(ContaBancaria p)
        {
            Console.WriteLine("Fechando a interface...");
            return true;
        }

        static string tryTransformOption(string op, string[] opArray)
        {
            try
            {
                return opArray[int.Parse(op)];

            }
            catch (Exception)
            {

                return "Error";
            }
            
            
             
        }

    }
}
