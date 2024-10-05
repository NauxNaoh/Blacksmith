using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime
{
    public class KilnMiniGameHandle : MonoBehaviour
    {
        [SerializeField] private RectTransform rectBoard;
        [SerializeField] private RectTransform rectRowHolder;
        [SerializeField] private List<RectTransform> lstRectRow = new List<RectTransform>();
        [SerializeField] private List<RectTransform> lstFire = new List<RectTransform>();

        [SerializeField] private Button btnLeft;
        [SerializeField] private Button btnRight;
        [SerializeField] private RectTransform itemGame;
        private float limitPlayTime = 5.5f;
        private float limitMoveTime = 0.15f;

        private GameState gameState;
        private RowPosition rowPosition;
        private CancellationTokenSource moveCancellationTokenSource;
          
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                InitializedBoardGame();
                StartMiniGame();
            }
        }

        public void InitializedBoardGame()
        {
            SetGameState(GameState.Initialized);

            var _posSpawn = Vector3.zero;
            var _oldRand = 0;
            var _countLoop = 0;
            for (int i = 0; i < lstFire.Count; i++)
            {
                var _rand = Random.Range(0, 3);
                if (_rand == _oldRand)
                {
                    _countLoop++;
                    if (_countLoop >= 3)
                    {
                        var _lstRowChoose = new List<int>() { 0, 1, 2 };
                        _lstRowChoose.RemoveAt(_rand);
                        _rand = _lstRowChoose[Random.Range(0, 2)];
                        _countLoop = 0;
                        _oldRand = _rand;
                        _posSpawn.y += 70;
                    }
                }
                else
                {
                    _oldRand = _rand;
                    _posSpawn.y += 80;
                }
                _posSpawn.y += 300;
                lstFire[i].SetParent(lstRectRow[_rand]);
                lstFire[i].anchoredPosition = _posSpawn;
            }

            rowPosition = RowPosition.Middle;
            itemGame.anchoredPosition = Vector2.zero;
        }

        public async void StartMiniGame()
        {
            SetGameState(GameState.Playing);
            await RunningBoard();
            SetGameState(GameState.None);
        }

        async Task RunningBoard()
        {
            var _destinationY = -(rectBoard.sizeDelta.y + rectRowHolder.sizeDelta.y);
            var _startPos = rectRowHolder.anchoredPosition;
            var _endPos = new Vector2(_startPos.x, _destinationY);

            await SmoothMove(rectRowHolder, _startPos, _endPos, limitPlayTime);
        }

        async Task SmoothMove(RectTransform rectTransf, Vector2 startPos, Vector2 endPos, float duration)
        {
            float elapsedTime = 0;
            while (elapsedTime < duration)
            {
                var _t = Mathf.Clamp01(elapsedTime / duration);
                rectTransf.anchoredPosition = Vector2.Lerp(startPos, endPos, _t);

                elapsedTime += Time.deltaTime;
                await Task.Yield();
            }
            rectTransf.anchoredPosition = endPos;
        }

        void SetGameState(GameState state) => gameState = state;

        void RegisterButtonEvent()
        {
            btnLeft.onClick.AddListener(OnClickMoveLeft);
            btnRight.onClick.AddListener(OnClickMoveRight);
        }

        void UnregisterButtonEvent()
        {
            btnLeft.onClick.RemoveListener(OnClickMoveLeft);
            btnRight.onClick.RemoveListener(OnClickMoveRight);
        }

        void OnClickMoveLeft()
        {
            Move(-1, limitMoveTime);
        }

        void OnClickMoveRight()
        {
            Move(1, limitMoveTime);
        }

        async void Move(int direction, float duration = 0)
        {
            if (moveCancellationTokenSource != null && !moveCancellationTokenSource.IsCancellationRequested)
                moveCancellationTokenSource.Cancel();
            moveCancellationTokenSource = new CancellationTokenSource();

            rowPosition = (RowPosition)Mathf.Clamp((int)rowPosition + direction, (int)RowPosition.Left, (int)RowPosition.Right);
            var _posRow = rectRowHolder.sizeDelta.x / 3;
            var _endPos = new Vector2((int)rowPosition * _posRow - _posRow, 0);

            await SmoothMove(itemGame, itemGame.anchoredPosition, _endPos, duration);
        }


        [ContextMenu(nameof(SetupBoard))]
        void SetupBoard()
        {
            var _pivot = new Vector2(0.5f, 0);
            var _anchoHolder = new Vector2(0.5f, 1);
            var _anchoRows = new Vector2(0.5f, 0);

            rectRowHolder.anchorMin = _anchoHolder;
            rectRowHolder.anchorMax = _anchoHolder;
            rectRowHolder.pivot = _pivot;
            rectRowHolder.sizeDelta = new Vector3(rectBoard.sizeDelta.x, 3200);
            rectRowHolder.anchoredPosition = Vector3.zero;
            rectRowHolder.anchoredPosition = Vector3.zero;

            var _widthSizeRow = rectRowHolder.sizeDelta.x / 3;
            var _sizeRow = new Vector2(_widthSizeRow, 0);
            for (int i = 0; i < 3; i++)
            {
                lstRectRow[i].anchorMin = _anchoRows;
                lstRectRow[i].anchorMax = _anchoRows;
                lstRectRow[i].pivot = _pivot;
                lstRectRow[i].sizeDelta = _sizeRow;

                if (i == 0)
                    lstRectRow[i].anchoredPosition = new Vector3(-_widthSizeRow, 0, 0);
                else if (i == 1)
                    lstRectRow[i].anchoredPosition = Vector3.zero;
                else if (i == 2)
                    lstRectRow[i].anchoredPosition = new Vector3(_widthSizeRow, 0, 0);
            }

            InitializedBoardGame();
            SetGameState(GameState.None);
        }
    }

    public enum GameState
    {
        None = 0,
        Initialized = 1,
        Playing = 2,
    }

    public enum RowPosition
    {
        Left = 0,
        Middle = 1,
        Right = 2,
    }
}