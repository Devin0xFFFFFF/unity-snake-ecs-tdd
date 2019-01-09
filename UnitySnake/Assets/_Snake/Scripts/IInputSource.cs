using UnityEngine;

namespace UnitySnake
{
    public interface IInputSource
    {
        KeyCode LastPressedKey { get; }

        void Update();
    }
}
