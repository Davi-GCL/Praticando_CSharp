using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaBanco
{
    class Program
    {

        //enum Opcao {Jogar=1, Rank, Créditos, Sair }
        static void Main(string[] args)
        {

            string[] opcoes = {"", "Deposito", "Saque", "Extrato", "Sair" };

            //Gera uma instancia da estrutura Dict<chave:valor>, onde chave receberá string e valor função que nao recebe e nem retorna valor (Action).
            Dictionary<string, Action> operacoes = new Dictionary<string, Action>();

            operacoes.Add("deposito", () => depositar());
            operacoes.Add("saque", () => sacar());
            operacoes.Add("extrato", () => gerarExtrato());

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

            try { operacoes[opcaoEscolhida](); }
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

        static void depositar()
        {
            Console.Write("Digite o valor que deseja depositar: R$");
            
            if (true){
                Console.WriteLine("Depositando:");
            }

        }

        static void sacar()
        {
            Console.WriteLine("Sacando dindin");
        }

        static void gerarExtrato()
        {
            Console.WriteLine("Gerando extrato...");
        }

        static void sair()
        {
            Console.WriteLine("Fechando a interface");
        }
    }
}
