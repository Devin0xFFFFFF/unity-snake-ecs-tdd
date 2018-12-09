using UnityEngine;

namespace Game.NoECS
{
    public interface IInputSource
    {
        KeyCode LastPressedKey { get; }

        void Update();
    }
}
