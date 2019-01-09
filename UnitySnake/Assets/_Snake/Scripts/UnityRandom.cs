namespace UnitySnake
{
    public class UnityRandom : IRandom
    {
        public int Range(int min, int max)
        {
            return UnityEngine.Random.Range(min, max);
        }
    }
}
