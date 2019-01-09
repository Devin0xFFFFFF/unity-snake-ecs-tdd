using UnityEngine;

namespace UnitySnake
{
    public class UnityTime : ITime
    {
        public float DeltaTime => Time.deltaTime;
    }
}
