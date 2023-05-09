using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hello_World
{
    class Program
    {
        enum Opcao {Jogar=1, Rank, Créditos, Sair }
        static void Main(string[] args)
        {
            Console.Title = "Show do Milhão"; //Define o texto na barra de cima da janela.

            gerarMenu();
            Console.Write("\n>> ");
            string resp = Console.ReadLine();

            /*try {
                Opcao opcaoEscolhida = (Opcao)int.Parse(resp);
            }
            catch (Exception ex)
            {
                switch (resp)
                {
                    case Opcao.ToString(Opcao.Jogar):
                        Console.WriteLine("git".ToUpper());

                }
            }*/

            //string resp = Opcao.Jogar.ToString();
            //Console.WriteLine(resp);



            Console.ReadLine();
        }

        static void gerarMenu()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Seja Bem-Vinde ao \nSHOW DO MILHAO 2023\n");
            Console.WriteLine("Menu de opções:");
                
            for(int i=1;i <= (int)Opcao.Sair; i++)
            {
                Console.WriteLine(" " + i + "-" + (Opcao)i );
            }
        }
    }
}
