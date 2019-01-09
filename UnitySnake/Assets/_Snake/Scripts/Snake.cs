using System.Collections.Generic;

namespace UnitySnake
{
    public class Snake
    {
        public readonly List<SnakeSegment> Segments;

        public SnakeSegment Head { get { return Segments[0]; } }

        public Snake()
        {
            Segments = new List<SnakeSegment>() { new SnakeSegment() };
        }
    }
}
