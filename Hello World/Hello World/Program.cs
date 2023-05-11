using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaBanco
{
    class Informacoes
    {
        public int agencia { get; set; }
        public string codConta { get; set; }
        public int saldo { get; set; }
    }
    class Program
    {

        //enum Opcao {Jogar=1, Rank, Créditos, Sair }
        static void Main(string[] args)
        {
            string[] opcoes = {"", "Deposito", "Saque", "Extrato", "Sair" };
            
            Informacoes Usuario = new Informacoes();

            //Gera uma instancia da estrutura Dict<chave:valor>, onde chave receberá string e valor função que nao recebe e nem retorna valor (Action).
            Dictionary<string, Action<Informacoes>> operacoes = new Dictionary<string, Action<Informacoes>>();

            operacoes.Add("deposito", (p) => depositar(p));
            operacoes.Add("saque", (p) => sacar(p));
            operacoes.Add("extrato", (p) => gerarExtrato(p));

            Console.Title = "Interface do Banco"; //Define o texto na barra de cima da janela.
            
            gerarMenu(opcoes);
            Console.Write("\n>> ");
            string opcaoEscolhida = Console.ReadLine();

            try
            {
                opcaoEscolhida = opcoes[int.Parse(opcaoEscolhida)].ToLower();
                Console.WriteLine(opcaoEscolhida);
            }
            catch (Exception ex)
            {
                opcaoEscolhida = opcaoEscolhida.ToLower();
            }
            Usuario.agencia = 102;
            try { operacoes[opcaoEscolhida](Usuario); }
            catch { Console.WriteLine("Digite uma opção valida!"); }

            Console.ReadLine();
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

        //Algoritmo para o usuario realizar o "deposito", recebe como parametro o ponteiro para o local da memoria do objeto passado.
        static void depositar(Informacoes Usuario)
        {
            int intAgencia;
            decimal deposito;
            bool? isValid;
            string resp;

            Console.Clear();
            Console.WriteLine("Digite as informaçoes da conta:");

        //Recebe a resposta do usuario e verifica se é valida
            Console.Write("Agência: ");
            do {
                resp = Console.ReadLine();
                isValid = int.TryParse(resp, out intAgencia);
                if (isValid == false) {Console.Write("\nAgência invalida!\nDigite novamente: ");}
            } while (isValid == false);

            Usuario.agencia = intAgencia;

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
                
            }while (isValid == false);

            Usuario.codConta = resp;

            //Entrada do valor do deposito
            Console.Write("\nInforme a quantidade que deseja depositar: R$");
            do
            {
                resp = Console.ReadLine();
                deposito = decimal.Parse(resp);
                if (isValid == false) { Console.Write("\nAgência invalida!\nDigite novamente: "); }
            } while (isValid == false);
            Console.WriteLine(deposito);
        }

        static void sacar(Informacoes p)
        {
            Console.WriteLine("Sacando dindin");
        }

        static void gerarExtrato(Informacoes p)
        {
            Console.WriteLine("Gerando extrato...");
        }

        static void sair(Informacoes p)
        {
            Console.WriteLine("Fechando a interface");
        }
    }
}
