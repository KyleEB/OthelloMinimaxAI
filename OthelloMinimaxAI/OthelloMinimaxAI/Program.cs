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

            Board.PIECE PlayerPiece;

            if(player.ToUpper().Equals("B"))
            {
                PlayerPiece = Board.PIECE.BLACK;
            }
            else
            {
                PlayerPiece = Board.PIECE.WHITE;
            }

            Board currentBoard = new Board(PlayerPiece);
            


            while (!shouldExit)
            {
                Console.Write(currentBoard.ToString());
                Console.WriteLine(currentBoard.currentPlayer + " | Make your move!");

                Console.WriteLine("Enter row: ");
                int row = int.Parse(Console.ReadLine().Trim());
                

                Console.WriteLine("Enter col: ");
                int col =int.Parse(Console.ReadLine().Trim());
                

                if (!currentBoard.makeMove(row, col)) 
                {
                    Console.WriteLine("Invalid Move \n");
                }

            }

        }

    }
}
