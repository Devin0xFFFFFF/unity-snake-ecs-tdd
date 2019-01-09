using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UnitySnake
{
    public class GameBehaviour : MonoBehaviour
    {
        public static readonly KeyCode[] Keys = new KeyCode[]
        {
            KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D,
            KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.RightArrow
        };

        public TextMeshProUGUI ScoreText;
        public TextMeshProUGUI HighScoreText;
        public TextMeshProUGUI GameResultText;
        public Button RestartButton;

        public Transform TilesParent;

        public Camera Camera;
        public int BoardSize = 15;
        [Range(0, 1)] public float TickRate = 0.1f;

        private TileBehaviour _tilePrefab;
        private List<TileBehaviour> _tiles;

        private float _accumulatedTime;

        public ITime Time { get; set; }
        public IInputSource InputSource { get; set; }
        public IRandom Random { get; set; }

        public Board Board { get; private set; }
        public GameManager Manager { get; private set; }

        public int Score { get { return Manager.Score; } }
        public int HighScore { get; private set; }

        public bool Loaded { get; private set; }

        private IEnumerator Start()
        {
            _tiles = new List<TileBehaviour>();

            yield return null;

            if(Time == null) { Time = new UnityTime(); }
            if(InputSource == null) { InputSource = new UnityInputSource(Keys); }
            if(Random == null) { Random = new UnityRandom(); }

            Board = new Board(BoardSize);
            Manager = new GameManager(Board, new Snake(), new PlayerInput(InputSource), new FoodSpawner(Random));

            HighScore = Score;

            Camera.orthographicSize = (((float)BoardSize) / 2) + 1;

            _tilePrefab = Resources.Load<TileBehaviour>("Tile");

            CreateBoard();

            GameResultText.gameObject.SetActive(false);
            RestartButton.onClick.AddListener(OnRestartClick);
            RestartButton.gameObject.SetActive(false);

            yield return null;

            Loaded = true;
        }

        private void Update()
        {
            if(!Loaded || Manager.GameOver) { return; }

            InputSource.Update();

            _accumulatedTime += Time.DeltaTime;

            if(_accumulatedTime >= TickRate)
            {
                Tick();

                _accumulatedTime = 0;
            }
        }

        private void CreateBoard()
        {
            Tile middleTile = Board.GetMiddleTile();
            int middleRow = middleTile.Row;
            int middleCol = middleTile.Col;

            foreach(Tile tile in Board.Tiles)
            {
                TileBehaviour tileInstance = Instantiate(_tilePrefab, TilesParent);

                tileInstance.transform.localPosition = new Vector3(0, tile.Row - middleRow, tile.Col - middleCol);
                tileInstance.UpdateMaterial(tile);

                _tiles.Add(tileInstance);
            }
        }

        private void Tick()
        {
            Manager.Tick();

            int score = Manager.Score;
            ScoreText.text = $"Score: {score}";

            if (score > HighScore)
            {
                HighScore = score;
                HighScoreText.text = $"High Score: {HighScore}";
            }

            for(int i = 0; i < _tiles.Count; i++) { _tiles[i].UpdateMaterial(Board.Tiles[i]); }

            if (Manager.GameOver)
            {
                GameResultText.gameObject.SetActive(true);
                RestartButton.gameObject.SetActive(true);

                if (Manager.Victory) { GameResultText.text = "You Won!"; }
                else { GameResultText.text = "You Lost."; }
            }
        }

        private void OnRestartClick()
        {
            Manager.Reset();
            GameResultText.gameObject.SetActive(false);
            RestartButton.gameObject.SetActive(false);
        }
    }
}
