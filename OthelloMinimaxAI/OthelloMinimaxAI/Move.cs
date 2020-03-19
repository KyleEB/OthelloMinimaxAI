using System.Collections.Generic;

namespace OthelloMinimaxAI
{
    internal class Move
    {
        public int col;
        public HashSet<(int row, int col)> flanked;
        public int row;
        public Board.PIECE who;
        public Move()
        {
            row = -1;
            col = -1;
            who = Board.PIECE.EMPTY;
            flanked = new HashSet<(int, int)>();
        }

        public Move(int row, int col, Board.PIECE who)
        {
            this.row = row;
            this.col = col;
            this.who = who;
        }

        public Move(Move toCopy)
        {
            this.row = toCopy.row;
            this.col = toCopy.col;
            this.who = toCopy.who;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Move);
        }

        public bool Equals(Move other)
        {
            return (this.row == other.row && this.col == other.col && this.who == other.who);
        }

        public override int GetHashCode()
        {
            return row * 8 + col; //one-to-one
        }
        public override string ToString()
        {
            return "who: " + who + " row: " + row + " col: " + col + " hash: " + GetHashCode();
        }
    }
}