using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaBanco
{
    class Program
    {
        static void depositar()
        {
            Console.WriteLine("Depositando: R$" + 100);
            
        }

        //enum Opcao {Jogar=1, Rank, Créditos, Sair }
        static void Main(string[] args)
        {
            Console.Title = "Interface do Banco"; //Define o texto na barra de cima da janela.

            string[] opcoes = {"", "Deposito", "Saque", "Extrato", "Sair" };

            gerarMenu(opcoes);
            Console.Write("\n>> ");
            string opcaoEscolhida = Console.ReadLine();

            //Gera uma instancia da estrutura Dict<chave:valor>, onde chave receberá string e valor função que nao recebe e nem retorna valor (Action).
            Dictionary<string, Action> operacoes = new Dictionary<string, Action>();

            operacoes.Add("Deposito", () => depositar());

            operacoes["Deposito"]();

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
    }
}
