using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OthelloMinimaxAI
{
    enum PIECE { BLACK, WHITE, EMPTY };
    class Board
    {
        private int SIZE = 8;

        private PIECE[,] theBoard;

        private PIECE currentPlayer;

        private PIECE humanPlayer;

        private int totalTurns;

        public int getNumBlackPieces() => getNumPieces(PIECE.BLACK);
        public int getNumWhitePieces() => getNumPieces(PIECE.WHITE);

        private int getNumPieces(PIECE kind)
        {
            int count = 0;
            foreach(PIECE p in theBoard)
            {
                if(p == kind)
                {
                    count++;
                }
            }
            return count;
        }

        public Board(PIECE playerInput)
        {
            theBoard = new PIECE[SIZE, SIZE];
            humanPlayer = playerInput;
            currentPlayer = PIECE.BLACK;
            totalTurns = 0;
            makeStartingBoard();
        }


        private void makeStartingBoard()
        {
            for(int i = 0; i < theBoard.GetLength(0); i++)
            {
                for(int j = 0; j < theBoard.GetLength(1); j++)
                {
                    theBoard[i, j] = PIECE.EMPTY;
                }
            }

            theBoard[3, 3] = PIECE.WHITE;
            theBoard[3, 4] = PIECE.BLACK;
            theBoard[4, 3] = PIECE.BLACK;
            theBoard[4, 4] = PIECE.WHITE;
        }


        public bool makeMove(int row, int col)
        {

            if (isValidMove(row, col, currentPlayer))
            {
                flipFlanked(row, col, currentPlayer);
                theBoard[row, col] = currentPlayer;
                currentPlayer = Board.GetOppositePiece(currentPlayer);
                totalTurns++;
                return true;
            }
            else
            {
                return false;
            }
            
        }

        private void flipFlanked(int row, int col, PIECE who)
        {
           (int,int) [] toFlip = getPiecesToFlip(row, col, who);
            foreach((int row,int col) point in toFlip)
            {
                theBoard[point.row, point.col] = currentPlayer;
            }
        }

        private (int, int) [] getPiecesToFlip(int row, int col, PIECE who)
        {
            //check all the directions for a same piece
            Move tempMove = new Move();
            if (theBoard[row, col] == PIECE.EMPTY
           &&
           (checkDirection(row, col, -1, 0, who, tempMove) //up
           || checkDirection(row, col, 1, 0, who, tempMove) //down
           || checkDirection(row, col, 0, -1, who, tempMove) //left
           || checkDirection(row, col, 0, 1, who, tempMove) //right
           || checkDirection(row, col, -1, 1, who, tempMove) //up-right
           || checkDirection(row, col, 1, 1, who, tempMove) //down-right
           || checkDirection(row, col, -1, -1, who, tempMove) //up-left
           || checkDirection(row, col, 1, -1, who, tempMove))) //down-left
            {
                return tempMove.flanked.ToArray<(int,int)>();
            }
            return null;
        }

        private bool isValidMove(int row, int col, PIECE who)
        {
            Move givenMove = new Move(row, col, who);
            HashSet<Move> validMoves = getValidMoves(who);
            return validMoves.Contains(givenMove);
        }

        private HashSet<Move> getValidMoves(PIECE who)
        {
            HashSet<Move> possibleMoves = new HashSet<Move>();
            for(int i = 0; i < theBoard.GetLength(0); i++)
            {
                for(int j = 0; j < theBoard.GetLength(1); j++)
                {
                    //check all the directions for a same piece
                    Move tempMove = new Move();
                    if (theBoard[i,j] == PIECE.EMPTY 
                   && 
                   (checkDirection(i, j, -1, 0, who, tempMove) //up
                   || checkDirection(i, j, 1, 0, who, tempMove) //down
                   || checkDirection(i, j, 0, -1, who, tempMove) //left
                   || checkDirection(i, j, 0, 1, who, tempMove) //right
                   || checkDirection(i, j, -1, 1, who, tempMove) //up-right
                   || checkDirection(i, j, 1, 1, who, tempMove) //down-right
                   || checkDirection(i, j, -1, -1, who, tempMove) //up-left
                   || checkDirection(i, j, 1, -1, who, tempMove))) //down-left
                    {
                        tempMove.who = who;
                        tempMove.row = i;
                        tempMove.col = j;
                        possibleMoves.Add(tempMove);
                        //Console.WriteLine(tempMove.ToString());
                    }
                }
            }
            return possibleMoves;
        }

        private bool checkDirection(int row, int col, int rowChange, int colChange, PIECE who, Move currentMove)
        {
            row += rowChange;
            col += colChange;
            int distance = 1;
            HashSet<(int row, int col)> tempToFlip = new HashSet<(int row, int col)>();
            while (inBounds(row,col))
            {
                if (distance > 1 && theBoard[row, col] == who)
                {
                    foreach((int,int) i  in tempToFlip)
                    currentMove.flanked.Add(i);
                    return true;
                }
                else if (theBoard[row, col] == GetOppositePiece(who))
                {
                    tempToFlip.Add((row, col));
                    row += rowChange;
                    col += colChange;
                    distance++;
                }
                else 
                {
                    return false;
                }
            }
            return false;
        }

        private bool inBounds(int row, int col)
        {
            return (row >= 0 && row < theBoard.GetLength(0) && col >= 0 && col < theBoard.GetLength(1));
        }

        public static PIECE GetOppositePiece(PIECE p)
        {
            if (p == PIECE.BLACK) return PIECE.WHITE;
            else if (p == PIECE.WHITE) return PIECE.BLACK;
            else return PIECE.EMPTY;
        }

        public override string ToString()
        {
            string output = string.Empty;
            output += "Total Turns: " + totalTurns;
            output += "\n Current Player: " + currentPlayer ;
            output += "\n #Black: " + getNumBlackPieces();
            output += ", #White: " + getNumWhitePieces();
            output += "\n  0 1 2 3 4 5 6 7 \n";
            for (int i = 0; i < theBoard.GetLength(0); i++)
            {
                output += i + " ";
                for (int j = 0; j < theBoard.GetLength(1); j++)
                {
                    output += pieceToString(theBoard[i, j]) + " ";
                }

                output += "\n";
            }
            output += "--------------------- \n";
            return output;
        }

        private char pieceToString(PIECE p)
        {
            if(p == PIECE.BLACK)
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
