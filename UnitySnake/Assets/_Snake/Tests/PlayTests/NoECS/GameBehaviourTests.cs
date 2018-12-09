using NUnit.Framework;
using Game.NoECS;
using System.Collections;
using UnityEngine.TestTools;
using UnityEngine;

namespace Tests.NoECS
{
    public class GameBehaviourTests
    {
        [UnityTearDown]
        public IEnumerator AfterEveryTest()
        {
            var games = Object.FindObjectsOfType<GameBehaviour>();
            foreach (GameBehaviour game in games) { Object.Destroy(game.gameObject); }
            yield return null;
        }

        public class InstanceTests : GameBehaviourTests
        {
            protected GameBehaviour instance;

            [UnitySetUp]
            public IEnumerator BeforeEveryInstanceTest()
            {
                var prefab = Resources.Load<GameBehaviour>("NoECSGame");

                yield return null;

                instance = Object.Instantiate(prefab);
            }
        }

        public class InstantiationTests : GameBehaviourTests
        {
            [UnityTest]
            public IEnumerator Valid_Instance_Created_From_Prefab()
            {
                var prefab = Resources.Load<GameBehaviour>("NoECSGame");
                var instance = Object.Instantiate(prefab);

                yield return null;

                Assert.That(instance, Is.Not.Null);
                Assert.That(instance.ScoreText, Is.Not.Null);
                Assert.That(instance.HighScoreText, Is.Not.Null);
                Assert.That(instance.GameResultText, Is.Not.Null);
                Assert.That(instance.RestartButton, Is.Not.Null);
                Assert.That(instance.TilesParent, Is.Not.Null);
            }
        }

        public class InitializationTests : InstanceTests
        {
            [UnityTest]
            public IEnumerator Valid_Instance_Created_From_Prefab()
            {
                while (!instance.Loaded) { yield return null; }

                Assert.That(instance.Board, Is.Not.Null);
                Assert.That(instance.Board.Rows, Is.EqualTo(instance.BoardSize));
                Assert.That(instance.Board.Cols, Is.EqualTo(instance.BoardSize));
                Assert.That(instance.Board.Tiles.Length, Is.EqualTo(instance.BoardSize * instance.BoardSize));

                Assert.That(instance.Manager, Is.Not.Null);
                Assert.That(instance.Manager.GameOver, Is.False);
                Assert.That(instance.Manager.Victory, Is.False);
                Assert.That(instance.Manager.Score, Is.EqualTo(1));

                Assert.That(instance.Time, Is.Not.Null);
                Assert.That(instance.Time, Is.InstanceOf(typeof(UnityTime)));

                Assert.That(instance.InputSource, Is.Not.Null);
                Assert.That(instance.InputSource, Is.InstanceOf(typeof(UnityInputSource)));

                Assert.That(instance.Random, Is.Not.Null);
                Assert.That(instance.Random, Is.InstanceOf(typeof(UnityRandom)));

                Assert.That(instance.Score, Is.EqualTo(1));
                Assert.That(instance.Score, Is.EqualTo(instance.Manager.Score));

                Assert.That(instance.HighScore, Is.EqualTo(1));
                Assert.That(instance.HighScore, Is.EqualTo(instance.Score));

                Assert.That(instance.TilesParent, Is.Not.Null);
            }

            [UnityTest]
            public IEnumerator Camera_Initialized_Correctly()
            {
                instance.BoardSize = 10;

                while (!instance.Loaded) { yield return null; }

                Assert.That(instance.Camera, Is.Not.Null);
                Assert.That(instance.Camera.orthographicSize, Is.EqualTo(6));
            }

            [UnityTest]
            public IEnumerator UI_Initialized_Correctly()
            {
                while(!instance.Loaded) { yield return null; }

                Assert.That(instance.ScoreText, Is.Not.Null);
                Assert.That(instance.ScoreText.gameObject.activeInHierarchy, Is.True);
                Assert.That(instance.ScoreText.text, Is.EqualTo("Score: 1"));

                Assert.That(instance.HighScoreText, Is.Not.Null);
                Assert.That(instance.HighScoreText.gameObject.activeInHierarchy, Is.True);
                Assert.That(instance.HighScoreText.text, Is.EqualTo("High Score: 1"));

                Assert.That(instance.GameResultText, Is.Not.Null);
                Assert.That(instance.GameResultText.gameObject.activeInHierarchy, Is.False);

                Assert.That(instance.RestartButton, Is.Not.Null);
                Assert.That(instance.RestartButton.gameObject.activeInHierarchy, Is.False);
            }

            [UnityTest]
            public IEnumerator Time_Can_Be_Overridden()
            {
                instance.Time = new TimeMock(1);

                while (!instance.Loaded) { yield return null; }

                Assert.That(instance.Time, Is.Not.Null);
                Assert.That(instance.Time, Is.InstanceOf(typeof(TimeMock)));
            }

            [UnityTest]
            public IEnumerator InputSource_Can_Be_Overridden()
            {
                instance.InputSource = new ConstInputSourceMock(KeyCode.UpArrow);

                while (!instance.Loaded) { yield return null; }

                Assert.That(instance.InputSource, Is.Not.Null);
                Assert.That(instance.InputSource, Is.InstanceOf(typeof(ConstInputSourceMock)));
            }

            [UnityTest]
            public IEnumerator Random_Can_Be_Overridden()
            {
                instance.Random = new AlwaysMinRandomMock();

                while (!instance.Loaded) { yield return null; }

                Assert.That(instance.Random, Is.Not.Null);
                Assert.That(instance.Random, Is.InstanceOf(typeof(AlwaysMinRandomMock)));
            }
        }

        public class UpdateTests : InstanceTests
        {
            public class UpdateConstInputTests : UpdateTests
            {
                [UnitySetUp]
                public IEnumerator BeforeEveryUpdateTest()
                {
                    instance.BoardSize = 10;
                    instance.Time = new TimeMock(0.0001f);
                    instance.InputSource = new ConstInputSourceMock(KeyCode.UpArrow);
                    instance.Random = new AlwaysMinRandomMock();

                    while (!instance.Loaded) { yield return null; }
                }

                [UnityTest]
                public IEnumerator No_Tick_If_TickRate_Not_Reached()
                {
                    Tile middleTile = instance.Board.GetMiddleTile();

                    yield return null;

                    Assert.That(middleTile.HasSnake, Is.True);
                }

                [UnityTest]
                public IEnumerator Snake_Grows_After_One_Update_Tick()
                {
                    instance.Time = new TimeMock(1);

                    Tile middleTile = instance.Board.GetMiddleTile();
                    Tile middleUpOneTile = instance.Board.GetTileInDirection(middleTile, Direction.Up);
                    middleUpOneTile.HasFood = true;

                    yield return null;

                    Assert.That(middleTile.HasSnake, Is.True);
                    Assert.That(middleUpOneTile.HasSnake, Is.True);
                }

                [UnityTest]
                public IEnumerator Lose_If_Hit_Edge()
                {
                    instance.Time = new TimeMock(1);

                    for (int i = 0; i < 10; i++) { yield return null; }

                    Assert.That(instance.Manager.GameOver, Is.True);
                    Assert.That(instance.Manager.Victory, Is.False);
                }
            }

            public class UpdateAlternatingInputTests : UpdateTests
            {
                [UnitySetUp]
                public IEnumerator BeforeEveryUpdateTest()
                {
                    instance.BoardSize = 10;
                    instance.Time = new TimeMock(0.0001f);
                    instance.InputSource = new AlternatingInputSourceMock(KeyCode.UpArrow, KeyCode.DownArrow);
                    instance.Random = new AlwaysMinRandomMock();

                    while (!instance.Loaded) { yield return null; }
                }

                [UnityTest]
                public IEnumerator Lose_If_Hit_Self()
                {
                    instance.Time = new TimeMock(1);

                    Tile middleTile = instance.Board.GetMiddleTile();
                    Tile middleUpOneTile = instance.Board.GetTileInDirection(middleTile, Direction.Up);
                    middleUpOneTile.HasFood = true;

                    yield return null;
                    yield return null;

                    Assert.That(instance.Manager.GameOver, Is.True);
                    Assert.That(instance.Manager.Victory, Is.False);
                }
            }
        }

        public class VictoryTests : InstanceTests
        {
            [UnitySetUp]
            public IEnumerator BeforeEveryVictoryTest()
            {
                instance.BoardSize = 1;
                instance.Time = new TimeMock(1);
                instance.InputSource = new ConstInputSourceMock(KeyCode.UpArrow);
                instance.Random = new AlwaysMinRandomMock();

                while (!instance.Loaded) { yield return null; }
            }

            [UnityTest]
            public IEnumerator Win_On_Board_Size_1()
            {
                yield return null;

                Assert.That(instance.Manager.GameOver, Is.True);
                Assert.That(instance.Manager.Victory, Is.True);
            }
        }

        public class ResetTests : InstanceTests
        {
            [UnitySetUp]
            public IEnumerator BeforeEveryResetTest()
            {
                instance.BoardSize = 2;
                instance.Time = new TimeMock(1);
                instance.InputSource = new ConstInputSourceMock(KeyCode.UpArrow);
                instance.Random = new AlwaysMinRandomMock();

                while (!instance.Loaded) { yield return null; }
            }

            [UnityTest]
            public IEnumerator Game_State_Correct_After_GameOver()
            {
                yield return null;
                yield return null;

                Assert.That(instance.Manager.GameOver, Is.True);

                Assert.That(instance.Manager.GameOver, Is.True);
                Assert.That(instance.Manager.Victory, Is.False);
            }

            [UnityTest]
            public IEnumerator Reset_UI_Shows_After_GameOver()
            {
                yield return null;
                yield return null;

                Assert.That(instance.Manager.GameOver, Is.True);

                Assert.That(instance.GameResultText.gameObject.activeInHierarchy, Is.True);
                Assert.That(instance.GameResultText.text, Is.EqualTo("You Lost."));

                Assert.That(instance.RestartButton.gameObject.activeInHierarchy, Is.True);
            }

            [UnityTest]
            public IEnumerator Clicking_Restart_Hides_Reset_UI()
            {
                yield return null;
                yield return null;

                Assert.That(instance.Manager.GameOver, Is.True);

                instance.RestartButton.onClick.Invoke();

                Assert.That(instance.GameResultText.gameObject.activeInHierarchy, Is.False);
                Assert.That(instance.RestartButton.gameObject.activeInHierarchy, Is.False);
            }

            [UnityTest]
            public IEnumerator Clicking_Restart_Resets_Manager()
            {
                yield return null;
                yield return null;

                Assert.That(instance.Manager.GameOver, Is.True);

                instance.RestartButton.onClick.Invoke();

                Assert.That(instance.Manager.GameOver, Is.False);
                Assert.That(instance.Manager.Score, Is.EqualTo(1));
                Assert.That(instance.Board.GetMiddleTile().HasSnake, Is.True);
            }
        }

        private class TimeMock : ITime
        {
            public float DeltaTime { get; private set; }

            public TimeMock(float deltaTime)
            {
                DeltaTime = deltaTime;
            }
        }

        private class ConstInputSourceMock : IInputSource
        {
            public KeyCode LastPressedKey { get; private set; }

            public ConstInputSourceMock(KeyCode lastPressedKey)
            {
                LastPressedKey = lastPressedKey;
            }

            public void Update() { }
        }

        private class AlternatingInputSourceMock : IInputSource
        {
            private readonly KeyCode _key1;
            private readonly KeyCode _key2;

            private KeyCode _currentKey;

            public KeyCode LastPressedKey
            {
                get
                {
                    KeyCode key = _currentKey;

                    if (_currentKey == _key1) { _currentKey = _key2; }
                    else { _currentKey = _key1; }

                    return key;
                }
            }

            public AlternatingInputSourceMock(KeyCode key1, KeyCode key2)
            {
                _key1 = key1;
                _key2 = key2;

                _currentKey = key1;
            }

            public void Update() { }
        }

        private class AlwaysMinRandomMock : IRandom
        {
            public int Range(int min, int max) { return min; }
        }
    }
}
