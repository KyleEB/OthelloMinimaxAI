using System;

namespace OthelloMinimaxAI
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Let's play Othello! \n Please Enter 'B' or 'W' for preferred piece or 'A' to watch an AI BATTLE ");

            string player = Console.ReadLine();


            Board.PIECE PlayerPiece;
            bool AIHUMAN = false;

            if (player.ToUpper().Equals("B"))
            {
                PlayerPiece = Board.PIECE.BLACK;
            }
            else if(player.ToUpper().Equals("W"))
            {
                PlayerPiece = Board.PIECE.WHITE;
            }
            else
            {
                PlayerPiece = Board.PIECE.BLACK;
                AIHUMAN = true;
            }

            Board currentBoard = new Board(PlayerPiece);

            Console.WriteLine("Choose your difficulty 1-10");

            int minimaxDepth = int.Parse(Console.ReadLine());

            Console.WriteLine("Row and Column numbers are 0-7 instead of 1-8");

            while (true)
            {
                if (currentBoard.IsTerminal)
                {
                    if (currentBoard.GetNumBlackPieces() > currentBoard.GetNumWhitePieces())
                    {
                        Console.WriteLine("Black Wins!");
                    }
                    else
                    {
                        Console.WriteLine("White Wins!");
                    }

                    while (true)
                    {
                    }
                }

                Console.Write(currentBoard.ToString());
                Console.WriteLine(currentBoard.CurrentPlayer + " | Make your move!");

                if (currentBoard.CurrentPlayer == PlayerPiece)
                {
                    if (!AIHUMAN)
                    {
                        Console.WriteLine("Enter row: ");
                        int row = int.Parse(Console.ReadLine().Trim());

                        Console.WriteLine("Enter col: ");
                        int col = int.Parse(Console.ReadLine().Trim());

                        if (!currentBoard.makeMove(row, col))
                        {
                            Console.WriteLine("Invalid Move \n");
                            continue;
                        }
                    }
                    else
                    {
                        (int score, Move move) = currentBoard.minimax(currentBoard, Board.PIECE.BLACK, minimaxDepth, 0, int.MinValue, int.MaxValue);

                        if (move == null)
                        {
                            currentBoard.IsTerminal = true;
                            continue;
                        }
                        currentBoard.makeMove(move.row, move.col);
                        Console.WriteLine("AI placed a piece at " + "row: " + move.row + "col: " + move.col + "\n");
                    }
                }
                else if (currentBoard.CurrentPlayer != PlayerPiece)
                {
                    (int score, Move move) = currentBoard.minimax(currentBoard, Board.PIECE.WHITE, minimaxDepth, 0, int.MinValue, int.MaxValue);

                    if (move == null)
                    {
                        currentBoard.IsTerminal = true;
                        continue;
                    }
                    currentBoard.makeMove(move.row, move.col);
                    Console.WriteLine("AI placed a piece at " + "row: " + move.row + "col: " + move.col + "\n");
                }
            }
        }
    }
}