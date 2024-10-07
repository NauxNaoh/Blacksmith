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
        [SerializeField] private Button btnLeft;
        [SerializeField] private Button btnRight;
        [SerializeField] private RectTransform rectBoard;
        [SerializeField] private RectTransform rectRowHolder;
        [SerializeField] private List<RectTransform> lstRectRow = new List<RectTransform>();
        [SerializeField] private ItemMove itemMove;
        [SerializeField] private List<FireMove> lstFireMove = new List<FireMove>();

        private GameState gameState;
        private RowPosition rowPosition;
        private float limitPlayTime = 5.5f;
        private float limitMoveTime = 0.15f;
        private CancellationTokenSource moveCancellationTokenSource;

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
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                SetupBoard();
                StartMiniGame();
            }
        }
        public void InitializedBoardGame()
        {
            SetGameState(GameState.Initialized);

            var _posSpawn = Vector3.zero;
            var _oldRand = 0;
            var _countLoop = 0;
            var _countFire = lstFireMove.Count;
            for (int i = 0; i < _countFire; i++)
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
                var _fire = lstFireMove[i];
                _fire.SetParrentFire(lstRectRow[_rand]);
                _fire.SetAnchorPositionItem(_posSpawn);
                _fire.SetStatusFire(true);
            }
            rowPosition = RowPosition.Middle;

            itemMove.SetAnchorPositionItem(new Vector2(0, 100));
            itemMove.ResetCounter();
        }

        public async void StartMiniGame()
        {
            RegisterButtonEvent();
            SetGameState(GameState.Playing);
            await Task.WhenAll(RunningBoard(limitPlayTime), RunningCheckHitFire(limitPlayTime));

            var _totalTouch = itemMove.CountTouch;
            Debug.LogError($"total touch = {_totalTouch}");
            SetGameState(GameState.None);
            UnregisterButtonEvent();
        }

        async Task RunningBoard(float duration)
        {
            var _destinationY = -(rectBoard.sizeDelta.y + rectRowHolder.sizeDelta.y);
            var _startPos = rectRowHolder.anchoredPosition;
            var _endPos = new Vector2(_startPos.x, _destinationY);

            await SmoothMove(rectRowHolder, _startPos, _endPos, duration);
        }

        async Task SmoothMove(RectTransform rectTransform, Vector2 startPos, Vector2 endPos, float duration)
        {
            float _elapsedTime = 0;
            while (_elapsedTime < duration)
            {
                var _t = Mathf.Clamp01(_elapsedTime / duration);
                rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, _t);

                _elapsedTime += Time.deltaTime;
                await Task.Yield();
            }
            rectTransform.anchoredPosition = endPos;
        }

        async Task RunningCheckHitFire(float duration)
        {
            var _elapsedTime = 0f;
            var _countFire = lstFireMove.Count;
            while (_elapsedTime < duration)
            {
                var _rectTransItem = itemMove.GetRectTransform();
                var _rectItem = GetWorldRect(_rectTransItem);

                for (int i = 0; i < _countFire; i++)
                {
                    if (!lstFireMove[i].FireStatus) continue;
                    var _rectTransFire = lstFireMove[i].GetRectTransform();
                    var _rectFire = GetWorldRect(_rectTransFire);

                    if (!_rectItem.Overlaps(_rectFire)) continue;
                    lstFireMove[i].SetStatusFire(false);
                    itemMove.HitFire();
                }

                _elapsedTime += Time.deltaTime;
                await Task.Yield();
            }
        }

        Rect GetWorldRect(RectTransform rectTransform)
        {
            var _corners = new Vector3[4];
            rectTransform.GetWorldCorners(_corners);

            var _size = new Vector2(_corners[2].x - _corners[0].x, _corners[2].y - _corners[0].y);
            return new Rect(_corners[0], _size);
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
            var _endPos = new Vector2((int)rowPosition * _posRow - _posRow, 100);
            var _rectTransItem = itemMove.GetRectTransform();

            await SmoothMove(_rectTransItem, _rectTransItem.anchoredPosition, _endPos, duration);
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