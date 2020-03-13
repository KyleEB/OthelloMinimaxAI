﻿using System;

namespace OthelloMinimaxAI
{
    class Program
    {
        static void Main(string[] args)
        {
            
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

            Console.WriteLine("Choose your difficulty 1-10");

            int minimaxDepth = int.Parse(Console.ReadLine());

            Board currentBoard = new Board(PlayerPiece);

            (int test, Move testMove) = currentBoard.minimax(currentBoard, PlayerPiece, 5, 0);
            


            while (true)
            {
                if (currentBoard.terminal)
                {
                    if(currentBoard.getNumBlackPieces() > currentBoard.getNumWhitePieces())
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
                Console.WriteLine(currentBoard.currentPlayer + " | Make your move!");

                if (currentBoard.currentPlayer == PlayerPiece)
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
                else if (currentBoard.currentPlayer != PlayerPiece)
                {
                    (int score, Move move) = currentBoard.minimax(currentBoard, Board.PIECE.WHITE, minimaxDepth, 0);

                    if (move == null)
                    {
                        currentBoard.terminal = true;
                        continue;
                    }
                    currentBoard.makeMove(move.row, move.col);
                    Console.WriteLine("AI placed a piece at " + "row: " + move.row + "col: " + move.col + "\n");
                    
                }

                

            }

        }

    }
}
