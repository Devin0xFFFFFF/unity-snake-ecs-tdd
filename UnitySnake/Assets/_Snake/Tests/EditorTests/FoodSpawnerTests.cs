using NUnit.Framework;
using UnitySnake;
using System;
using UnityEngine;

namespace Tests.NoECS
{
    public class FoodSpawnerTests
    {
        public class ConstructorTests
        {
            [Test]
            public void Null_Random_Throws_ArgumentNullException()
            {
                Assert.That(() => new FoodSpawner(null), Throws.ArgumentNullException);
            }
        }

        public class SpawnFoodTests
        {
            [Test]
            public void Food_Spawned_Correctly()
            {
                var board = new Board(2);
                var foodSpawner = new FoodSpawner(new AlwaysMinRandomMock());

                foodSpawner.SpawnFood(board);

                Assert.That(board.Tiles[0].HasFood, Is.True);
            }

            [Test]
            public void Only_One_Food_Per_Board()
            {
                var board = new Board(2);
                var foodSpawner = new FoodSpawner(new LinearRandomMock());

                foodSpawner.SpawnFood(board);

                Assert.That(foodSpawner.SpawnFood(board), Is.False);

                int tilesWithFood = 0;
                foreach(Tile tile in board.Tiles) { if(tile.HasFood) { tilesWithFood++; } }

                Assert.That(tilesWithFood, Is.EqualTo(1));
            }

            [Test]
            public void Gracefully_Fail_If_No_Remaining_Spawn_Locations()
            {
                var board = new Board(1);
                var foodSpawner = new FoodSpawner(new AlwaysMinRandomMock());

                board.Tiles[0].HasSnake = true;

                Assert.That(foodSpawner.SpawnFood(board), Is.False);
            }
        }

        private class AlwaysMinRandomMock : IRandom
        {
            public int Range(int min, int max) { return min; }
        }

        private class LinearRandomMock : IRandom
        {
            private int _counter;

            public int Range(int min, int max)
            {
                return Mathf.Clamp(_counter++ % (max - min), min, max);
            }
        }
    }
}
