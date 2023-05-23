using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;


//IDEIAS:
//  X-Fazer com o que o valor da propriedade Informacoes.saldo só possa ser alterado através de um metodo do objeto, que receberá a senha digitada e valor a ser retirado. Se a senha for autentica, acessa e modifica o valor do saldo;
//  -Salvar as informações de usuarios cadastrados e sua movimentações em tabelas num banco de dados;
//  X-Gerar um arquivo como relatório do extrato bancario;

namespace SistemaBanco
{
    class ContaBancaria
    {
        private ConsultaSql conexao = new ConsultaSql();

        public string codConta;
        
        public string agencia;

        public decimal saldo { get; private set; } //Esta proprieda pode ser acessada fora da classe, mas só pode ser modificada por metodos de dentro da classe;

        private string senha;

        public Queue<string> historico = new Queue<string>();

        public bool acessSenha(string param)
        {
            if (senha == null) 
            { 
                senha = param;
                return true;
            }
            else
            {
                if (senha == param) { return true; }
                else { return false; }
            }
        }

        /*public void movimentarSaldo(string tipo, decimal valor)
        {

            if (string.Equals(tipo, "deposito", StringComparison.OrdinalIgnoreCase))
            {
                this.saldo += valor;
                this.historico.Enqueue("+R$" + valor.ToString());
            }
            else if (string.Equals(tipo, "saque", StringComparison.OrdinalIgnoreCase)) 
            {
                this.saldo -= valor;
                this.historico.Enqueue("-R$" + valor.ToString());
            }

        }*/

        //Sobrecarga do metodo de movimentar saldo que atualiza o registro no banco de dados
        public void movimentarSaldo(string tipo, decimal valor)
        {

            if (string.Equals(tipo, "deposito", StringComparison.OrdinalIgnoreCase))
            {
                this.saldo += valor;
                this.historico.Enqueue("+R$" + valor.ToString());
            }
            else if (string.Equals(tipo, "saque", StringComparison.OrdinalIgnoreCase))
            {
                this.saldo -= valor;
                this.historico.Enqueue("-R$" + valor.ToString());
            }

            Console.WriteLine(this.conexao.RegistrarMov(this.codConta, valor, tipo, DateTime.Now.ToString("HH:mm,dd/MM/yy")));

        }
    }

    class Program
    {

        //enum Opcao {Jogar=1, Rank, Créditos, Sair }
        static bool encerrar = false;

        static void Main(string[] args)
        {

            //ConsultaSql test = new ConsultaSql();
            //test.LerTabela();

            string[] opcoes = { "", "Deposito", "Saque", "Transferencia", "Extrato", "Sair" };

            ContaBancaria Usuario = new ContaBancaria();

            //Gera uma instancia da estrutura Dict<chave:valor>, onde chave receberá string e valor função que nao recebe e nem retorna valor (Action).
            Dictionary<string, Func<ContaBancaria,bool>> operacoes = new Dictionary<string, Func<ContaBancaria, bool>>();

            operacoes.Add("deposito", (p) => depositar(p));
            operacoes.Add("saque", (p) => sacar(p));
            operacoes.Add("extrato", (p) => gerarExtrato(p));
            operacoes.Add("sair", (p) => sair(p));

            Console.Title = "Interface do Banco"; //Define o texto na barra de cima da janela.

            do
            {
                Console.Clear();
                gerarMenu(opcoes);
                Console.Write("\n>> ");
                string opcaoEscolhida = Console.ReadLine();

                //Se o usuario digitar o numero da opcao, tentara passar o numero digitado como indice do vetor opcoes, onde a opcao corresponde esta posicionada
                try
                {
                    opcaoEscolhida = opcoes[int.Parse(opcaoEscolhida)].ToLower();
                    Console.WriteLine(opcaoEscolhida);
                }
                //Se o usuario digitar o nome da opção(caracteres), a entrada é tratada e posteriormente passada como chave no dictionary
                catch (Exception ex)
                {
                    opcaoEscolhida = opcaoEscolhida.ToLower();
                }

                try { encerrar = operacoes[opcaoEscolhida](Usuario); }
                catch(Exception ERR) { Console.WriteLine("Digite uma opção valida!");
                    Console.WriteLine(ERR.Message);
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



        static void entrarConta(ContaBancaria Usuario)
        {
            int intAgencia;
            bool? isValid;
            string resp;

            Console.WriteLine("Digite as informaçoes da conta:");

            //Recebe a resposta do usuario e verifica se é valida
            Console.Write("Agência: ");
            do
            {
                resp = Console.ReadLine();
                isValid = int.TryParse(resp, out intAgencia);
                if (isValid == false) { Console.Write("\nAgência invalida!\nDigite novamente: "); }
            } while (isValid == false);

            Usuario.agencia = intAgencia.ToString();

            //Recebe o numero da conta
            Console.Write("Número da conta: ");
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

            Usuario.codConta = resp;
        }

    //Algoritmo para o usuario realizar o "deposito", recebe como parametro o ponteiro para o local da memoria do objeto passado.
        static bool depositar(ContaBancaria Usuario)
        {
            decimal valDeposito;
            bool? isValid;
            string resp;

            Console.Clear();
            if (Usuario.codConta == null) { entrarConta(Usuario); }

            //Entrada do valor do deposito
            Console.Write("\nInforme o valor que deseja depositar: R$");
            do
            {
                resp = Console.ReadLine();
                isValid = decimal.TryParse(resp, out valDeposito);
                if (isValid == false) { Console.Write("\nValor invalido!\nDigite novamente: "); }
            } while (isValid == false);

            Usuario.movimentarSaldo("deposito",valDeposito); //"Lembrar de Salvar no banco de dados os depositos e saques feitos nessa conta" 
            Console.WriteLine($"\n\nDepósito no valor de R${valDeposito} realizado com sucesso na conta {Usuario.codConta}, na agência de nº{Usuario.agencia}");
            Console.Write("Deseja realizar outra operação?[S/N]: ");

            return string.Equals(Console.ReadLine(), "N", StringComparison.OrdinalIgnoreCase);
        }

        static bool sacar(ContaBancaria Usuario)
        {
            decimal valSaque;
            bool? isValid;
            string resp;

            Console.Clear();
            if (Usuario.codConta == null) { entrarConta(Usuario); }
            
            Console.WriteLine($"Olá dono da conta nº{Usuario.codConta}! Você possui atualmente R${Usuario.saldo} na conta.");
            Console.Write("\nInforme o valor que deseja sacar: R$");
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

        //Solicitação de senha para retirar dinheiro
            Console.Write("Digite a senha: ");
            do
            {
                resp = Console.ReadLine();
                if (Usuario.acessSenha(resp)) 
                {
                    Console.WriteLine("Sacando dindin");
                    Usuario.movimentarSaldo("saque", valSaque);
                    isValid = true;
                }
                else 
                { 
                    Console.Write("Senha errada!Digite novamente");
                    isValid = false;
                }
            } while (isValid==false);

            //Fazer algoritmo para gerar um arquivo de nota 
            return false;
        }
    // </sacar>
        static bool gerarExtrato(ContaBancaria Usuario)
        {
            DateTime dataAtual = DateTime.Now;
            string nomeArquivo = $"{dataAtual.ToString("dd-MM-yyyy")}_{Usuario.codConta.ToString()}_extrato.xml";
            string diretorioAtual = Environment.CurrentDirectory; //Pega o caminho da pasta em que o codigo está sendo executado.
            string caminhoCompleto = Path.Combine(diretorioAtual, nomeArquivo);


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

            foreach(string aux in Usuario.historico)
            {
                body.InnerXml += $"<p data=\"{dataAtual.ToString("HH:mm dd/MM")} \">{aux}</p>";
                
            }
            body.InnerXml += $"<saldoAtual>{Usuario.saldo}</saldoAtual>";
            xmlDoc.Save(caminhoCompleto);

            return false;
        }

        static bool sair(ContaBancaria p)
        {
            Console.WriteLine("Fechando a interface");
            return true;
        }
    }
}
