using System;
using System.Collections.Generic;
using System.Linq;

namespace OthelloMinimaxAI
{
    internal class Board
    {
        public PIECE CurrentPlayer;
        public PIECE HumanPlayer;
        public bool IsTerminal;
        private int SIZE = 8;

        private PIECE[,] TheBoard;

        private int totalTurns;

        public Board(PIECE playerInput)
        {
            TheBoard = new PIECE[SIZE, SIZE];
            HumanPlayer = playerInput;
            CurrentPlayer = PIECE.BLACK;
            totalTurns = 0;
            makeStartingBoard();
        }

        public Board(Board toCopy)
        {
            TheBoard = (PIECE[,])toCopy.TheBoard.Clone();
            CurrentPlayer = toCopy.CurrentPlayer;
            HumanPlayer = toCopy.HumanPlayer;
            totalTurns = toCopy.totalTurns;
        }

        public enum PIECE { BLACK, WHITE, EMPTY };
        public static PIECE GetOppositePiece(PIECE p)
        {
            if (p == PIECE.BLACK) return PIECE.WHITE;
            else if (p == PIECE.WHITE) return PIECE.BLACK;
            else return PIECE.EMPTY;
        }

        public int GetNumBlackPieces() => GetNumPieces(PIECE.BLACK);

        public int GetNumWhitePieces() => GetNumPieces(PIECE.WHITE);

        public bool makeMove(int row, int col)
        {
            if (isValidMove(row, col, CurrentPlayer))
            {
                flipFlanked(row, col, CurrentPlayer);
                TheBoard[row, col] = CurrentPlayer;
                CurrentPlayer = Board.GetOppositePiece(CurrentPlayer);
                totalTurns++;
                return true;
            }
            else
            {
                return false;
            }
        }

        public (int bestScore, Move bestMove) minimax(Board currentBoard, PIECE player, int maxDepth, int currentDepth, int alpha, int beta)
        {
            int bestScore;
            Move bestMove = null;

            if (currentBoard.isGameOver() || currentDepth == maxDepth)
            {
                return (currentBoard.evaluate(player), null);
            }

            if (currentBoard.CurrentPlayer == player)
            {
                bestScore = int.MinValue;
            }
            else
            {
                bestScore = int.MaxValue;
            }

            var MoveSet = currentBoard.getValidMoves(CurrentPlayer);
            foreach (Move move in MoveSet)
            {
                Board newBoard = new Board(currentBoard);

                newBoard.makeMove(move.row, move.col);

                (int currentScore, Move currentMove) = minimax(newBoard, player, maxDepth, currentDepth + 1, alpha, beta);

                if (currentBoard.CurrentPlayer == player)
                {
                    if (currentScore > bestScore)
                    {
                        bestScore = currentScore;
                        bestMove = move;
                        alpha = Math.Max(alpha, bestScore);
                        if (beta <= alpha)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    if (currentScore < bestScore)
                    {
                        bestScore = currentScore;
                        bestMove = move;
                        beta = Math.Max(beta, bestScore);
                        if (beta <= alpha)
                        {
                            break;
                        }
                    }
                }
            }

            return (bestScore, bestMove);
        }

        public override string ToString()
        {
            string output = string.Empty;
            output += "Total Turns: " + totalTurns;
            output += "\n Current Player: " + CurrentPlayer;
            output += "\n #Black: " + GetNumBlackPieces();
            output += ", #White: " + GetNumWhitePieces();
            output += "\n  0 1 2 3 4 5 6 7 \n";
            for (int i = 0; i < TheBoard.GetLength(0); i++)
            {
                output += i + " ";
                for (int j = 0; j < TheBoard.GetLength(1); j++)
                {
                    output += pieceToString(TheBoard[i, j]) + " ";
                }

                output += "\n";
            }
            output += "--------------------- \n";
            return output;
        }

        private void checkDirection(int row, int col, int rowChange, int colChange, PIECE who, Move currentMove)
        {
            row += rowChange;
            col += colChange;
            int distance = 1;
            HashSet<(int row, int col)> tempToFlip = new HashSet<(int row, int col)>();
            while (inBounds(row, col))
            {
                if (distance > 1 && TheBoard[row, col] == who)
                {
                    foreach ((int, int) i in tempToFlip)
                        currentMove.flanked.Add(i);
                    return;
                }
                else if (TheBoard[row, col] == GetOppositePiece(who))
                {
                    tempToFlip.Add((row, col));
                    row += rowChange;
                    col += colChange;
                    distance++;
                }
                else
                {
                    return;
                }
            }
            return;
        }

        private int evaluate(PIECE player)
        {
            return GetNumPieces(player) - GetNumPieces(GetOppositePiece(player));
        }

        private void flipFlanked(int row, int col, PIECE who)
        {
            (int, int)[] toFlip = getPiecesToFlip(row, col, who);
            foreach ((int row, int col) point in toFlip)
            {
                TheBoard[point.row, point.col] = CurrentPlayer;
            }
        }

        private int GetNumPieces(PIECE kind)
        {
            int count = 0;
            foreach (PIECE p in TheBoard)
            {
                if (p == kind)
                {
                    count++;
                }
            }
            return count;
        }
        private (int, int)[] getPiecesToFlip(int row, int col, PIECE who)
        {
            //check all the directions for a same piece
            Move tempMove = new Move();

            checkDirection(row, col, -1, 0, who, tempMove); //up
            checkDirection(row, col, 1, 0, who, tempMove); //down
            checkDirection(row, col, 0, -1, who, tempMove); //left
            checkDirection(row, col, 0, 1, who, tempMove); //right
            checkDirection(row, col, -1, 1, who, tempMove); //up-right
            checkDirection(row, col, 1, 1, who, tempMove); //down-right
            checkDirection(row, col, -1, -1, who, tempMove); //up-left
            checkDirection(row, col, 1, -1, who, tempMove); //down-left

            return tempMove.flanked.ToArray<(int, int)>();
        }

        private HashSet<Move> getValidMoves(PIECE who)
        {
            HashSet<Move> possibleMoves = new HashSet<Move>();
            for (int i = 0; i < TheBoard.GetLength(0); i++)
            {
                for (int j = 0; j < TheBoard.GetLength(1); j++)
                {
                    //check all the directions for a same piece
                    Move tempMove = new Move();
                    if (TheBoard[i, j] == PIECE.EMPTY)
                    {
                        checkDirection(i, j, -1, 0, who, tempMove); //up
                        checkDirection(i, j, 1, 0, who, tempMove); //down
                        checkDirection(i, j, 0, -1, who, tempMove); //left
                        checkDirection(i, j, 0, 1, who, tempMove); //right
                        checkDirection(i, j, -1, 1, who, tempMove); //up-right
                        checkDirection(i, j, 1, 1, who, tempMove); //down-right
                        checkDirection(i, j, -1, -1, who, tempMove); //up-left
                        checkDirection(i, j, 1, -1, who, tempMove); //down-left

                        tempMove.who = who;
                        tempMove.row = i;
                        tempMove.col = j;
                        if (tempMove.flanked.Count() > 0)
                        {
                            possibleMoves.Add(tempMove);
                        }
                    }
                }
            }
            return possibleMoves;
        }

        private bool inBounds(int row, int col)
        {
            return (row >= 0 && row < TheBoard.GetLength(0) && col >= 0 && col < TheBoard.GetLength(1));
        }

        private bool isGameOver()
        {
            if (getValidMoves(PIECE.BLACK).Count == 0 || getValidMoves(PIECE.WHITE).Count == 0)
            {
                IsTerminal = true;
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool isValidMove(int row, int col, PIECE who)
        {
            Move givenMove = new Move(row, col, who);
            HashSet<Move> validMoves = getValidMoves(who);
            return validMoves.Contains(givenMove);
        }

        private void makeStartingBoard()
        {
            for (int i = 0; i < TheBoard.GetLength(0); i++)
            {
                for (int j = 0; j < TheBoard.GetLength(1); j++)
                {
                    TheBoard[i, j] = PIECE.EMPTY;
                }
            }

            TheBoard[3, 3] = PIECE.WHITE;
            TheBoard[3, 4] = PIECE.BLACK;
            TheBoard[4, 3] = PIECE.BLACK;
            TheBoard[4, 4] = PIECE.WHITE;
        }
        private char pieceToString(PIECE p)
        {
            if (p == PIECE.BLACK)
            {
                return 'O';
            }
            else if (p == PIECE.WHITE)
            {
                return 'X';
            }
            else
            {
                return '.';
            }
        }
    }
}