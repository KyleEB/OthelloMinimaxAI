using System;

namespace OthelloMinimaxAI
{
    class Program
    {
        static void Main(string[] args)
        {
            bool shouldExit = false;
            
            Console.WriteLine("Let's play Othello! \n Please Enter 'B' or 'W' for preferred piece");

            string player = Console.ReadLine();

            PIECE PlayerPiece;

            if(player.ToUpper().Equals("B"))
            {
                PlayerPiece = PIECE.BLACK;
            }
            else
            {
                PlayerPiece = PIECE.WHITE;
            }

            Board currentBoard = new Board(PlayerPiece);
            


            while (!shouldExit)
            {
                Console.Write(currentBoard.ToString());
                Console.WriteLine("Enter row: \n");
                int row = int.Parse(Console.ReadLine().Trim());
                Console.WriteLine("\n");

                Console.WriteLine("Enter col: \n");
                int col =int.Parse(Console.ReadLine().Trim());
                Console.WriteLine("\n");

                if (!currentBoard.makeMove(row, col)) 
                {
                    Console.WriteLine("Invalid Move \n");
                }

            }

        }

    }
}
