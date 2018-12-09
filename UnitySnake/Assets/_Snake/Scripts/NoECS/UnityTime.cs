using UnityEngine;

namespace Game.NoECS
{
    public class UnityTime : ITime
    {
        public float DeltaTime => Time.deltaTime;
    }
}
