using System.Collections.Generic;

namespace UnitySnake
{
    public class FoodSpawner
    {
        private readonly IRandom _random;

        private readonly List<Tile> _candidateFoodTiles;

        public FoodSpawner(IRandom random)
        {
            _random = random ?? throw new System.ArgumentNullException("input");

            _candidateFoodTiles = new List<Tile>();
        }

        public bool SpawnFood(Board board)
        {
            _candidateFoodTiles.Clear();

            foreach (Tile tile in board.Tiles)
            {
                if(tile.HasFood) { return false; } // Only one food per board
                if (!tile.HasSnake) { _candidateFoodTiles.Add(tile); }
            }

            if(_candidateFoodTiles.Count == 0) { return false; }

            _candidateFoodTiles[_random.Range(0, _candidateFoodTiles.Count)].HasFood = true;

            return true;
        }
    }
}
