using System;
using System.Text;

namespace Game.NoECS
{
    public class Board
    {
        public readonly int Rows;
        public readonly int Cols;
        public readonly Tile[] Tiles;

        public Board(int size)
        {
            if (size <= 0) { throw new ArgumentOutOfRangeException("size", size, "must be greater than 0"); }

            Rows = size;
            Cols = size;

            Tiles = new Tile[size * size];

            for(int r = 0; r < Rows; r++)
            {
                for(int c = 0; c < Cols; c++)
                {
                    int index = r * Rows + c;
                    Tiles[index] = new Tile(r, c);
                }
            }
        }

        public Tile GetTile(int row, int col)
        {
            if (row < 0 || row >= Rows) { throw new ArgumentOutOfRangeException("row", row, $"must be within 0 and {Rows - 1}"); }
            if (col < 0 || col >= Cols) { throw new ArgumentOutOfRangeException("col", col, $"must be within 0 and {Cols - 1}"); }

            int index = row * Rows + col;

            return Tiles[index];
        }

        public Tile GetTileInDirection(Tile tile, Direction direction)
        {
            int row = tile.Row;
            int col = tile.Col;

            switch(direction)
            {
                case Direction.Up:
                    row -= 1;
                    break;
                case Direction.Down:
                    row += 1;
                    break;
                case Direction.Right:
                    col += 1;
                    break;
                case Direction.Left:
                    col -= 1;
                    break;
                default:
                    return null;
            }

            if(row >= 0 && row < Rows && col >= 0 && col < Cols) { return GetTile(row, col); }

            return null;
        }

        public Tile GetMiddleTile() { return GetTile(Rows / 2, Cols / 2); }

        public override string ToString()
        {
            var builder = new StringBuilder();

            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Cols; c++)
                {
                    int index = r * Rows + c;
                    Tile tile = Tiles[index];

                    if(tile.HasSnake) { builder.Append("[S]"); }
                    else if(tile.HasFood) { builder.Append("[F]");  }
                    else { builder.Append("[ ]"); }

                    if(c != Cols - 1) { builder.Append(" "); }
                }

                builder.AppendLine();
            }

            return builder.ToString();
        }
    }
}
