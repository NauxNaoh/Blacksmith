using Naux.Patterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Runtime
{
    public class KilnMiniGameHandle : Singleton<KilnMiniGameHandle>
    {
        [SerializeField] private Button btnLeft;
        [SerializeField] private Button btnRight;
        [SerializeField] private RectTransform rectBoard;
        [SerializeField] private RectTransform rectRowHolder;
        [SerializeField] private List<RectTransform> lstRectRow = new List<RectTransform>();
        [SerializeField] private ItemMove itemMove;
        [SerializeField] private List<FireMove> lstFireMove = new List<FireMove>();

        private KilnMiniGameState gameState;
        private RowPosition rowPosition;
        private float limitPlayTime = 5.5f;
        private float limitMoveTime = 0.15f;

        private Coroutine moveCoroutine;


        void SetGameState(KilnMiniGameState state) => gameState = state;
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


        public void BoardGameInitialized()
        {
            SetGameState(KilnMiniGameState.Initialized);
            SetupBoard();

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
                _fire.Initialized();
                _fire.SetParrentFire(lstRectRow[_rand]);
                _fire.SetAnchorPositionItem(_posSpawn);
                _fire.SetStatusFire(true);
            }
            rowPosition = RowPosition.Middle;

            itemMove.Initialized();
            itemMove.SetAnchorPositionItem(new Vector2(0, 100));
            itemMove.ResetCounter();
        }

        public void SetupBoard()
        {
            SetGameState(KilnMiniGameState.None);

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
        }

        public IEnumerator StartMiniGameRoutine(UnityAction<bool> callback)
        {
            SetGameState(KilnMiniGameState.Playing);
            RegisterButtonEvent();

            var _boardCoroutine = RunningBoardRoutine(limitPlayTime);
            var _checkHitFireCoroutine = RunningCheckHitFireRoutine(limitPlayTime);
            yield return StartCoroutine(RunMultipleCoroutines(_boardCoroutine, _checkHitFireCoroutine));
            EndGame();

            yield return new WaitUntil(() => gameState == KilnMiniGameState.EndGame);
            var _result = CheckValidResult();
            callback?.Invoke(_result);
        }

        void EndGame()
        {
            SetGameState(KilnMiniGameState.EndGame);
            UnregisterButtonEvent();
        }

        IEnumerator RunMultipleCoroutines(params IEnumerator[] enumerators)
        {
            var _lstCoroutine = new List<Coroutine>();

            for (int i = 0, _count = enumerators.Length; i < _count; i++)
                _lstCoroutine.Add(StartCoroutine(enumerators[i]));

            for (int i = 0, _count = _lstCoroutine.Count; i < _count; i++)
                yield return _lstCoroutine[i];
        }

        IEnumerator RunningBoardRoutine(float duration)
        {
            var _destinationY = -(rectBoard.sizeDelta.y + rectRowHolder.sizeDelta.y);
            var _startPos = rectRowHolder.anchoredPosition;
            var _endPos = new Vector2(_startPos.x, _destinationY);

            yield return StartCoroutine(SmoothMoveRoutine(rectRowHolder, _startPos, _endPos, duration));
        }

        IEnumerator SmoothMoveRoutine(RectTransform rectTransform, Vector2 startPos, Vector2 endPos, float duration)
        {
            float _elapsedTime = 0;
            while (_elapsedTime < duration)
            {
                var _t = Mathf.Clamp01(_elapsedTime / duration);
                rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, _t);

                _elapsedTime += Time.deltaTime;
                yield return null;
            }
            rectTransform.anchoredPosition = endPos;
        }

        IEnumerator RunningCheckHitFireRoutine(float duration)
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
                yield return null;
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
            if (gameState != KilnMiniGameState.Playing) return;
            StartCoroutine(MoveRoutine(-1, limitMoveTime));
        }

        void OnClickMoveRight()
        {
            if (gameState != KilnMiniGameState.Playing) return;
            StartCoroutine(MoveRoutine(1, limitMoveTime));
        }

        IEnumerator MoveRoutine(int direction, float duration = 0)
        {
            if (moveCoroutine != null)
                StopCoroutine(moveCoroutine);

            rowPosition = (RowPosition)Mathf.Clamp((int)rowPosition + direction, (int)RowPosition.Left, (int)RowPosition.Right);
            var _posRow = rectRowHolder.sizeDelta.x / 3;
            var _endPos = new Vector2((int)rowPosition * _posRow - _posRow, 100);
            var _rectTransItem = itemMove.GetRectTransform();

            moveCoroutine = StartCoroutine(SmoothMoveRoutine(_rectTransItem, _rectTransItem.anchoredPosition, _endPos, duration));
            yield return moveCoroutine;
            moveCoroutine = null;
        }

        bool CheckValidResult()
        {
            var _totalTouch = itemMove.CountTouch;
            return _totalTouch > 0;
        }
    }
    public enum KilnMiniGameState
    {
        None = 0,
        Initialized = 1,
        Playing = 2,
        EndGame = 3,
    }
    public enum RowPosition
    {
        Left = 0,
        Middle = 1,
        Right = 2,
    }
}