namespace UnitySnake
{
    public class PlayerInput
    {
        private readonly IInputSource _input;

        public PlayerInput(IInputSource input)
        {
            _input = input ?? throw new System.ArgumentNullException("input");
        }

        public Direction GetInputDirection()
        {
            switch(_input.LastPressedKey)
            {
                case UnityEngine.KeyCode.W:
                case UnityEngine.KeyCode.UpArrow:
                    return Direction.Up;
                case UnityEngine.KeyCode.S:
                case UnityEngine.KeyCode.DownArrow:
                    return Direction.Down;
                case UnityEngine.KeyCode.A:
                case UnityEngine.KeyCode.LeftArrow:
                    return Direction.Left;
                case UnityEngine.KeyCode.D:
                case UnityEngine.KeyCode.RightArrow:
                    return Direction.Right;
                default:
                    return Direction.Right;
            }
        }
    }
}
