using UnityEngine;

namespace UnitySnake
{
    public class TileBehaviour : MonoBehaviour
    {
        public Material EmptyMaterial;
        public Material SnakeMaterial;
        public Material FoodMaterial;

        [SerializeField] private MeshRenderer _renderer;

        private void Start()
        {
            _renderer = GetComponent<MeshRenderer>();
            _renderer.sharedMaterial = EmptyMaterial;
        }

        public void UpdateMaterial(Tile tile)
        {
            if(tile.HasSnake) { _renderer.sharedMaterial = SnakeMaterial; }
            else if(tile.HasFood) { _renderer.sharedMaterial = FoodMaterial; }
            else { _renderer.sharedMaterial = EmptyMaterial; }
        }
    }
}
