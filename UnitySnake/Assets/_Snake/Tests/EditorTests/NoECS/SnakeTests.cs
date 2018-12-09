using NUnit.Framework;
using Game.NoECS;

namespace Tests.NoECS
{
    public class SnakeTests
    {
        public class ConstructorTests
        {
            [Test]
            public void Has_Initialized_Segments()
            {
                Assert.That(new Snake().Segments, Is.Not.Null);
            }

            [Test]
            public void Has_Initialized_Head_Segment()
            {
                var snake = new Snake();

                Assert.That(snake.Segments.Count, Is.EqualTo(1));
                Assert.That(snake.Segments[0], Is.Not.Null);
            }
        }
    }
}
