using NUnit.Framework;
using UnitySnake;
using UnityEngine;

namespace Tests.NoECS
{
    public class GameManagerTests
    {
        public class ConstructorTests
        {
            [Test]
            public void Null_Board_Throws_ArgumentNullException()
            {
                Assert.That(() => new GameManager(null, null, null, null), Throws.ArgumentNullException);
            }

            [Test]
            public void Null_Snake_Throws_ArgumentNullException()
            {
                Assert.That(() => new GameManager(new Board(1), null, null, null), Throws.ArgumentNullException);
            }

            [Test]
            public void Null_Input_Throws_ArgumentNullException()
            {
                Assert.That(() => new GameManager(new Board(1), new Snake(), null, null), Throws.ArgumentNullException);
            }

            [Test]
            public void Null_FoodSpawner_Throws_ArgumentNullException()
            {
                Assert.That(() => new GameManager(new Board(1), new Snake(), new PlayerInput(new ConstInputSourceMock(KeyCode.LeftArrow)), null), Throws.ArgumentNullException);
            }

            [Test]
            public void Even_Board_Size_Snake_Head_Tile_Set_Correctly()
            {
                var board = new Board(2);
                var snake = new Snake();
                var input = new PlayerInput(new ConstInputSourceMock(KeyCode.LeftArrow));
                var foodSpawner = new FoodSpawner(new AlwaysMinRandomMock());

                var manager = new GameManager(board, snake, input, foodSpawner);

                Assert.That(snake.Head.Tile, Is.Not.Null);
                Assert.That(snake.Head.Tile, Is.EqualTo(board.Tiles[3]));
            }

            [Test]
            public void Odd_Board_Size_Snake_Head_Tile_Set_Correctly()
            {
                var board = new Board(3);
                var snake = new Snake();
                var input = new PlayerInput(new ConstInputSourceMock(KeyCode.LeftArrow));
                var foodSpawner = new FoodSpawner(new AlwaysMinRandomMock());

                var manager = new GameManager(board, snake, input, foodSpawner);

                Assert.That(snake.Head.Tile, Is.Not.Null);
                Assert.That(snake.Head.Tile, Is.EqualTo(board.Tiles[4]));
            }

            [Test]
            public void Board_Has_One_Food_Tile()
            {
                var board = new Board(3);
                var snake = new Snake();
                var input = new PlayerInput(new ConstInputSourceMock(KeyCode.LeftArrow));
                var foodSpawner = new FoodSpawner(new AlwaysMinRandomMock());

                var manager = new GameManager(board, snake, input, foodSpawner);

                Assert.That(board.Tiles[0].HasFood, Is.True);
            }
        }

        public class TickTests
        {
            private Board _board;
            private Snake _snake;
            private PlayerInput _input;
            private FoodSpawner _foodSpawner;
            private GameManager _manager;

            public class AlwaysMoveUpTests : TickTests
            {
                [SetUp]
                public void BeforeEveryTest()
                {
                    _board = new Board(4);
                    _snake = new Snake();
                    _input = new PlayerInput(new ConstInputSourceMock(KeyCode.UpArrow));
                    _foodSpawner = new FoodSpawner(new AlwaysMinRandomMock());
                    _manager = new GameManager(_board, _snake, _input, _foodSpawner);
                }

                [Test]
                public void Snake_Head_Moved_Correctly()
                {
                    Tile prevTile = _snake.Head.Tile;

                    _manager.Tick();

                    Tile expectedTile = _board.GetTileInDirection(prevTile, Direction.Up);

                    Assert.That(_snake.Head.Tile, Is.Not.EqualTo(prevTile));
                    Assert.That(_snake.Head.Tile, Is.EqualTo(expectedTile));
                }

                [Test]
                public void Snake_Body_Moved_Correctly()
                {
                    _board.GetTileInDirection(_snake.Head.Tile, Direction.Up).HasFood = true;

                    _manager.Tick();
                    _manager.Tick();

                    Assert.That(_snake.Segments.Count, Is.EqualTo(2));
                    Assert.That(_snake.Segments[0].Tile, Is.EqualTo(_board.GetTile(0, 2)));
                    Assert.That(_snake.Segments[1].Tile, Is.EqualTo(_board.GetTile(1, 2)));
                }

                [Test]
                public void Lose_If_Snake_Head_Leaves_Bounds()
                {
                    Tile prevTile = _snake.Head.Tile;

                    _manager.Tick();
                    _manager.Tick();
                    _manager.Tick();

                    Assert.That(_manager.GameOver, Is.True);
                    Assert.That(_manager.Victory, Is.False);
                }
            }

            public class AlternateMoveUpDownTests : TickTests
            {
                [SetUp]
                public void BeforeEveryTest()
                {
                    _board = new Board(3);
                    _snake = new Snake();
                    _input = new PlayerInput(new AlternatingInputSourceMock(KeyCode.UpArrow, KeyCode.DownArrow));
                    _foodSpawner = new FoodSpawner(new AlwaysMinRandomMock());
                    _manager = new GameManager(_board, _snake, _input, _foodSpawner);
                }

                [Test]
                public void Lose_If_Snake_Head_Hits_Body()
                {
                    _board.GetTileInDirection(_snake.Head.Tile, Direction.Up).HasFood = true;

                    _manager.Tick();
                    _manager.Tick();

                    Assert.That(_manager.GameOver, Is.True);
                    Assert.That(_manager.Victory, Is.False);
                }

                [Test]
                public void Check_Head_Cannot_Hit_Head()
                {
                    _manager.Tick();
                    _manager.Tick();

                    Assert.That(_manager.GameOver, Is.False);
                }
            }
        }

        public class ScoreTests
        {
            [Test]
            public void Score_Matches_Snake_Length()
            {
                var board = new Board(3);
                var snake = new Snake();
                var manager = new GameManager(board, snake, new PlayerInput(new ConstInputSourceMock(KeyCode.UpArrow)), new FoodSpawner(new AlwaysMinRandomMock()));

                board.GetTileInDirection(snake.Head.Tile, Direction.Up).HasFood = true;

                manager.Tick();
                manager.Tick();

                Assert.That(manager.Score, Is.EqualTo(snake.Segments.Count));
            }
        }

        public class VictoryTests
        {
            [Test]
            public void Check_Victory_On_Board_Size_1()
            {
                var manager = new GameManager(new Board(1), new Snake(), new PlayerInput(new ConstInputSourceMock(KeyCode.UpArrow)), new FoodSpawner(new AlwaysMinRandomMock()));

                manager.Tick();

                Assert.That(manager.GameOver, Is.True);
                Assert.That(manager.Victory, Is.True);
            }
        }

        public class ResetTests
        {
            private Board _board;
            private Snake _snake;
            private PlayerInput _input;
            private FoodSpawner _foodSpawner;
            private GameManager _manager;

            [SetUp]
            public void BeforeEveryTest()
            {
                _board = new Board(3);
                _snake = new Snake();
                _input = new PlayerInput(new ConstInputSourceMock(KeyCode.RightArrow));
                _foodSpawner = new FoodSpawner(new AlwaysMinRandomMock());
                _manager = new GameManager(_board, _snake, _input, _foodSpawner);
            }

            [Test]
            public void Check_Reset_Cleans_Up_Board()
            {
                _board.GetTile(1, 2).HasFood = true;

                _manager.Tick();
                _manager.Tick();

                _manager.Reset();

                Assert.That(_board.GetTile(1, 1).HasSnake, Is.True);
            }

            [Test]
            public void Check_Reset_Cleans_Up_Snake()
            {
                _board.GetTile(1, 2).HasFood = true;

                _manager.Tick();
                _manager.Tick();

                _manager.Reset();

                Assert.That(_snake.Segments.Count, Is.EqualTo(1));
                Assert.That(_snake.Head.Tile, Is.EqualTo(_board.GetTile(1, 1)));
            }

            [Test]
            public void Check_Reset_Cleans_Up_Game_State()
            {
                _board.GetTile(1, 2).HasFood = true;

                _manager.Tick();
                _manager.Tick();

                _manager.Reset();

                Assert.That(_manager.GameOver, Is.False);
                Assert.That(_manager.Victory, Is.False);
            }
        }

        private class ConstInputSourceMock : IInputSource
        {
            public KeyCode LastPressedKey { get; private set; }

            public ConstInputSourceMock(KeyCode lastPressedKey)
            {
                LastPressedKey = lastPressedKey;
            }

            public void Update() { }
        }

        private class AlternatingInputSourceMock : IInputSource
        {
            private readonly KeyCode _key1;
            private readonly KeyCode _key2;

            private KeyCode _currentKey;

            public KeyCode LastPressedKey
            {
                get
                {
                    KeyCode key = _currentKey;

                    if(_currentKey == _key1) { _currentKey = _key2; }
                    else { _currentKey = _key1; }

                    return key;
                }
            }

            public AlternatingInputSourceMock(KeyCode key1, KeyCode key2)
            {
                _key1 = key1;
                _key2 = key2;

                _currentKey = key1;
            }

            public void Update() { }
        }

        private class AlwaysMinRandomMock : IRandom
        {
            public int Range(int min, int max) { return min; }
        }
    }
}
