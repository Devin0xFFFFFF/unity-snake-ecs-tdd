using NUnit.Framework;
using Game.NoECS;
using System;

namespace Tests.NoECS
{
    public class TileTests
    {
        public class ConstructorTests
        {
            [Test]
            public void Created_At_0_0_Has_Correct_Row()
            {
                Assert.That(new Tile(0, 0).Row, Is.EqualTo(0));
            }

            [Test]
            public void Created_At_0_0_Has_Correct_Col()
            {
                Assert.That(new Tile(0, 0).Col, Is.EqualTo(0));
            }

            [Test]
            public void Negative_Row_Throws_Exception()
            {
                Assert.That(() => new Tile(-1, 0), Throws.TypeOf<ArgumentOutOfRangeException>());
            }

            [Test]
            public void Negative_Col_Throws_Exception()
            {
                Assert.That(() => new Tile(0, -1), Throws.TypeOf<ArgumentOutOfRangeException>());
            }

            [Test]
            public void Created_At_1_0_Has_Correct_Row()
            {
                Assert.That(new Tile(1, 0).Row, Is.EqualTo(1));
            }

            [Test]
            public void Created_At_0_1_Has_Correct_Col()
            {
                Assert.That(new Tile(0, 1).Col, Is.EqualTo(1));
            }

            [Test]
            public void Created_With_No_Occupants()
            {
                Assert.That(new Tile(0, 0).HasSnake, Is.False);
                Assert.That(new Tile(0, 0).HasFood, Is.False);
            }
        }

        public class ToStringTests
        {
            [Test]
            public void ToString_Correctly_Formatted()
            {
                Assert.That(new Tile(0, 1).ToString(), Is.EqualTo("Tile(0, 1)"));
            }
        }
    }
}
