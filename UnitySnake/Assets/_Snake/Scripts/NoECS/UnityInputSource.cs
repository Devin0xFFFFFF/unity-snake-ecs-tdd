using UnityEngine;

namespace Game.NoECS
{
    public class UnityInputSource : IInputSource
    {
        private readonly KeyCode[] _keys;

        public KeyCode LastPressedKey { get; private set; }

        public UnityInputSource(KeyCode[] keys) { _keys = keys; }

        public void Update()
        {
            foreach(KeyCode key in _keys)
            {
                if(Input.GetKeyDown(key)) { LastPressedKey = key; }
            }
        }
    }
}
