using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OthelloMinimaxAI
{
    class Board
    {
        public char[,] spaces;
        int rows = 8;
        int columns = 8;

        public Board()
        {
            spaces = new char[rows, columns];
            makeStartingBoard();
        }


        private void makeStartingBoard()
        {
            for(int i = 0; i < rows; i++)
            {
                for(int j = 0; j < columns; j++)
                {
                    spaces[i,j] = '.';
                }
            }


        }

        public override string ToString()
        {
            string output = string.Empty;
            output += "  0 1 2 3 4 5 6 7 \n";
            for (int i = 0; i < rows; i++)
            {
                output += i + " ";
                for (int j = 0; j < columns; j++)
                {
                    output += spaces[i, j] + " ";
                }

                output += "\n";
            }

            return output;
        }
    }

}
