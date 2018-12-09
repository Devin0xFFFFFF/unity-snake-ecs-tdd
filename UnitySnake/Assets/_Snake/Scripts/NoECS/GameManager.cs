using System.Collections.Generic;

namespace Game.NoECS
{
    public class GameManager
    {
        private readonly Board _board;
        private readonly Snake _snake;
        private readonly PlayerInput _input;
        private readonly FoodSpawner _foodSpawner;

        public bool GameOver { get; private set; }
        public bool Victory { get; private set; }

        public int Score { get { return _snake.Segments.Count; } }

        public GameManager(Board board, Snake snake, PlayerInput input, FoodSpawner foodSpawner)
        {
            _board = board ?? throw new System.ArgumentNullException("board");
            _snake = snake ?? throw new System.ArgumentNullException("snake");
            _input = input ?? throw new System.ArgumentNullException("input");
            _foodSpawner = foodSpawner ?? throw new System.ArgumentNullException("foodSpawner");

            Initialize();
        }

        public void Tick()
        {
            if(GameOver) { return; }

            Tile tile = _board.GetTileInDirection(_snake.Head.Tile, _input.GetInputDirection());

            if (tile == null || tile.HasSnake) // End if head hits bounds or body segment
            {
                GameOver = true;
                return;
            }

            List<SnakeSegment> segments = _snake.Segments;
            int segmentsCount = segments.Count;
            SnakeSegment newSegment = null;
            Tile prevTile = _snake.Head.Tile;

            if (tile.HasFood)
            {
                newSegment = new SnakeSegment();
                var lastSegment = segments[segmentsCount - 1];

                newSegment.Tile = lastSegment.Tile;

                tile.HasFood = false;
            }

            MoveSegmentToTile(_snake.Head, tile);

            for (int i = 1; i < segmentsCount; i++) // Move Snake Segments
            {
                SnakeSegment segment = segments[i];

                tile = prevTile;
                prevTile = segment.Tile;
                MoveSegmentToTile(segment, tile);
            }

            if(newSegment != null)
            {
                newSegment.Tile.HasSnake = true;
                _snake.Segments.Add(newSegment);

                if(CheckGameOver()) { return; }

                _foodSpawner.SpawnFood(_board);
            }
        }

        public void Reset()
        {
            foreach(Tile tile in _board.Tiles)
            {
                tile.HasSnake = false;
                tile.HasFood = false;
            }

            _snake.Segments.Clear();
            _snake.Segments.Add(new SnakeSegment());

            GameOver = false;
            Victory = false;

            Initialize();
        }

        private void Initialize()
        {
            Tile startingTile = _board.GetTile(_board.Rows / 2, _board.Cols / 2);
            _snake.Head.Tile = startingTile;
            startingTile.HasSnake = true;

            _foodSpawner.SpawnFood(_board);

            CheckGameOver();
        }

        private void MoveSegmentToTile(SnakeSegment segment, Tile tile)
        {
            segment.Tile.HasSnake = false;
            segment.Tile = tile;
            tile.HasSnake = true;
        }

        private bool CheckGameOver()
        {
            if (_snake.Segments.Count == _board.Tiles.Length)
            {
                GameOver = true;
                Victory = true;
            }

            return GameOver;
        }
    }
}
