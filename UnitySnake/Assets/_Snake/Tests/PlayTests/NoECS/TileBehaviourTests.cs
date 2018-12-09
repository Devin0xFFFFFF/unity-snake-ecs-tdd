using NUnit.Framework;
using Game.NoECS;
using System.Collections;
using UnityEngine.TestTools;
using UnityEngine;

namespace Tests.NoECS
{
    public class TileBehaviourTests
    {
        [UnityTearDown]
        public IEnumerator AfterEveryTest()
        {
            var tiles = Object.FindObjectsOfType<TileBehaviour>();
            foreach (TileBehaviour tile in tiles) { Object.Destroy(tile.gameObject); }
            yield return null;
        }

        public class InstantiationTests : TileBehaviourTests
        {
            [UnityTest]
            public IEnumerator Instance_Has_Renderer()
            {
                var prefab = Resources.Load<TileBehaviour>("Tile");
                var instance = Object.Instantiate(prefab);

                yield return null;

                Assert.That(instance.GetComponent<MeshRenderer>(), Is.Not.Null);
            }

            [UnityTest]
            public IEnumerator Instance_Renderer_Material_Defaults_To_EmptyMaterial()
            {
                var prefab = Resources.Load<TileBehaviour>("Tile");
                var instance = Object.Instantiate(prefab);

                yield return null;

                Assert.That(instance.GetComponent<MeshRenderer>().sharedMaterial, Is.EqualTo(instance.EmptyMaterial));
            }
        }

        public class UpdateMaterialTests : TileBehaviourTests
        {
            [UnityTest]
            public IEnumerator Correct_Material_Selected_For_Empty()
            {
                var prefab = Resources.Load<TileBehaviour>("Tile");
                var instance = Object.Instantiate(prefab);

                yield return null;

                var tile = new Tile(0, 0);

                instance.UpdateMaterial(tile);

                Assert.That(instance.GetComponent<MeshRenderer>().sharedMaterial, Is.EqualTo(prefab.EmptyMaterial));
            }

            [UnityTest]
            public IEnumerator Correct_Material_Selected_For_Snake()
            {
                var prefab = Resources.Load<TileBehaviour>("Tile");
                var instance = Object.Instantiate(prefab);

                yield return null;

                var tile = new Tile(0, 0);
                tile.HasSnake = true;

                instance.UpdateMaterial(tile);

                Assert.That(instance.GetComponent<MeshRenderer>().sharedMaterial, Is.EqualTo(prefab.SnakeMaterial));
            }

            [UnityTest]
            public IEnumerator Correct_Material_Selected_For_Food()
            {
                var prefab = Resources.Load<TileBehaviour>("Tile");
                var instance = Object.Instantiate(prefab);

                yield return null;

                var tile = new Tile(0, 0) { HasFood = true };

                instance.UpdateMaterial(tile);

                Assert.That(instance.GetComponent<MeshRenderer>().sharedMaterial, Is.EqualTo(prefab.FoodMaterial));
            }
        }
    }
}
