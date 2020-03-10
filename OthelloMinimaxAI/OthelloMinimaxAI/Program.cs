using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OthelloMinimaxAI
{
    class Program
    {
        static void Main(string[] args)
        {
            bool shouldExit = false;
            
            Console.WriteLine("Let's play Othello!");

            Board currentBoard = new Board();
            Console.Write(currentBoard.ToString());

            Console.WriteLine(currentBoard.makeMove(3, 2, PIECE.BLACK));

            Console.WriteLine(currentBoard.makeMove(3, 2, PIECE.WHITE));

            while (!shouldExit)
            {
                
                if (Console.ReadKey().Key.Equals(ConsoleKey.Escape))
                {
                    shouldExit = true;
                }
            }

        }

    }
}
