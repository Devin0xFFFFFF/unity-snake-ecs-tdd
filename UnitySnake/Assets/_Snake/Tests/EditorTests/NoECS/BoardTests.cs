using NUnit.Framework;
using Game.NoECS;
using System;

namespace Tests.NoECS
{
    public class BoardTests
    {
        public class ConstructorTests
        {
            [Test]
            public void Created_With_Size_1_Has_Correct_Rows()
            {
                Assert.That(new Board(1).Rows, Is.EqualTo(1));
            }

            [Test]
            public void Created_With_Size_1_Has_Correct_Cols()
            {
                Assert.That(new Board(1).Cols, Is.EqualTo(1));
            }

            [Test]
            public void Zero_Size_Throws_ArgumentOutOfRangeException()
            {
                Assert.That(() => new Board(0), Throws.TypeOf<ArgumentOutOfRangeException>());
            }

            [Test]
            public void Negative_Size_Throws_ArgumentOutOfRangeException()
            {
                Assert.That(() => new Board(-1), Throws.TypeOf<ArgumentOutOfRangeException>());
            }

            [Test]
            public void Size_1_Has_Correct_Rows()
            {
                Assert.That(new Board(1).Rows, Is.EqualTo(1));
            }

            [Test]
            public void Size_1_Has_Correct_Cols()
            {
                Assert.That(new Board(1).Cols, Is.EqualTo(1));
            }

            [Test]
            public void Has_Initialized_Tiles()
            {
                Assert.That(new Board(1).Tiles, Is.Not.Null);
            }

            [Test]
            public void Size_1_Has_Correct_Number_Of_Tiles()
            {
                Assert.That(new Board(1).Tiles.Length, Is.EqualTo(1));
            }

            [Test]
            public void Size_10_Has_Correct_Number_Of_Tiles()
            {
                Assert.That(new Board(10).Tiles.Length, Is.EqualTo(100));
            }

            [Test]
            public void Size_1_Has_Created_Valid_Tile()
            {
                Assert.That(new Board(1).Tiles[0], Is.Not.Null);
            }

            [Test]
            public void Size_2_Has_Created_Valid_Tile_Row()
            {
                var board = new Board(2);

                Assert.That(board.Tiles[1].Row, Is.EqualTo(0));
            }

            [Test]
            public void Size_2_Has_Created_Valid_Tile_Col()
            {
                var board = new Board(2);

                Assert.That(board.Tiles[2].Col, Is.EqualTo(0));
            }
        }

        public class GetTileTests
        {
            [Test]
            public void Size_2_Get_0_1_Is_Correct_Tile()
            {
                var board = new Board(2);

                Assert.That(board.GetTile(0, 1), Is.EqualTo(board.Tiles[1]));
            }

            [Test]
            public void Size_1_Get_1_0_Throws_ArgumentOutOfRangeException()
            {
                var board = new Board(1);

                Assert.That(() => board.GetTile(1, 0), Throws.TypeOf<ArgumentOutOfRangeException>());
            }

            [Test]
            public void Size_3_Get_3_0_Throws_ArgumentOutOfRangeException()
            {
                var board = new Board(3);

                Assert.That(() => board.GetTile(3, 0), Throws.TypeOf<ArgumentOutOfRangeException>());
            }
        }

        public class GetTileInDirectionTests
        {
            [Test]
            public void Size_2_Get_0_0_Right_Is_Correct_Tile()
            {
                var board = new Board(2);

                Assert.That(board.GetTileInDirection(board.Tiles[0], Direction.Right), Is.EqualTo(board.Tiles[1]));
            }

            [Test]
            public void Size_2_Get_0_0_Left_Is_Null()
            {
                var board = new Board(2);

                Assert.That(board.GetTileInDirection(board.Tiles[0], Direction.Left), Is.Null);
            }

            [Test]
            public void Size_2_Get_1_1_Up_Is_Correct_Tile()
            {
                var board = new Board(2);

                Assert.That(board.GetTileInDirection(board.Tiles[3], Direction.Up), Is.EqualTo(board.Tiles[1]));
            }

            [Test]
            public void Size_2_Get_0_0_Down_Is_Correct_Tile()
            {
                var board = new Board(2);

                Assert.That(board.GetTileInDirection(board.Tiles[0], Direction.Down), Is.EqualTo(board.Tiles[2]));
            }
        }

        public class GetMiddleTileTests
        {
            [Test]
            public void Even_Board_Middle_Tile_Is_Correct()
            {
                var board = new Board(2);

                Assert.That(board.GetMiddleTile(), Is.EqualTo(board.GetTile(1, 1)));
            }

            [Test]
            public void Odd_Board_Middle_Tile_Is_Correct()
            {
                var board = new Board(3);

                Assert.That(board.GetMiddleTile(), Is.EqualTo(board.GetTile(1, 1)));
            }
        }

        public class ToStringTests
        {
            [Test]
            public void Empty_Board_Correctly_Formatted()
            {
                var board = new Board(2);

                string expectedString = $"[ ] [ ]{Environment.NewLine}[ ] [ ]{Environment.NewLine}";

                Assert.That(board.ToString(), Is.EqualTo(expectedString));
            }

            [Test]
            public void Board_With_Snake_Correctly_Formatted()
            {
                var board = new Board(1);

                board.Tiles[0].HasSnake = true;

                string expectedString = $"[S]{Environment.NewLine}";

                Assert.That(board.ToString(), Is.EqualTo(expectedString));
            }

            [Test]
            public void Board_With_Food_Correctly_Formatted()
            {
                var board = new Board(1);

                board.Tiles[0].HasFood = true;

                string expectedString = $"[F]{Environment.NewLine}";

                Assert.That(board.ToString(), Is.EqualTo(expectedString));
            }
        }

    }
}
