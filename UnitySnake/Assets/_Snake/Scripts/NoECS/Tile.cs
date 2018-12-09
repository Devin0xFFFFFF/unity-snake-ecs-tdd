using System;

namespace Game.NoECS
{
    public class Tile
    {
        public readonly int Row;
        public readonly int Col;

        public bool HasSnake { get; set; }
        public bool HasFood { get; set; }

        public Tile(int row, int col)
        {
            if(row < 0) { throw new ArgumentOutOfRangeException("row", row, "must be positive"); }
            if(col < 0) { throw new ArgumentOutOfRangeException("col", col, "must be positive"); }

            Row = row;
            Col = col;
        }

        public override string ToString() { return $"Tile({Row}, {Col})"; }
    }
}
