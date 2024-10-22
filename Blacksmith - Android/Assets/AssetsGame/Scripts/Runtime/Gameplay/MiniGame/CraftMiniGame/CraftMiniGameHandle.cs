using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Runtime
{
    public class CraftMiniGameHandle : MonoBehaviour
    {
        [SerializeField] private RectTransform rectCraftingBar;
        [SerializeField] private RectTransform rectTargetZone;
        [SerializeField] private RectTransform rectCraftSlider;
        [SerializeField] private Button btnCraft;

        private CraftMiniGameState gameState;
        private float minPosTarget;
        private float maxPosTarget;
        private Vector2 rangePosSlider;
        private float timeSlider = 1.4f;
        private float elapsedTimeSlider;
        private int countHitTarget;

        private Coroutine startMoveSliderCoroutine;
        private Coroutine smoothSliderCoroutine;

        void SetGameState(CraftMiniGameState state) => gameState = state;
        void RegisterButtonEvent() => btnCraft.onClick.AddListener(OnClickCraft);
        void UnregisterButtonEvent() => btnCraft.onClick.RemoveListener(OnClickCraft);


        public void BoardGameInitialized()
        {
            SetGameState(CraftMiniGameState.Initialized);

            var _sizeCraftingBar = rectCraftingBar.sizeDelta;
            var _sizeTarget = rectTargetZone.sizeDelta;
            maxPosTarget = (_sizeCraftingBar.x - _sizeTarget.x) / 2;
            minPosTarget = 0;
            RandomPosTargetZone();

            var _sizeSlider = rectCraftSlider.sizeDelta;
            rangePosSlider = new Vector2((_sizeCraftingBar.x - _sizeSlider.x) / 2, 0);
            rectCraftSlider.anchoredPosition = rangePosSlider * -1;

            elapsedTimeSlider = 0;
            countHitTarget = 0;
        }

        public IEnumerator StartMiniGameRoutine(UnityAction<bool> callback)
        {
            RegisterButtonEvent();
            startMoveSliderCoroutine = StartCoroutine(MoveCraftSliderRoutine());

            yield return new WaitUntil(() => gameState == CraftMiniGameState.EndGame);
            var _result = CheckValidResult();
            callback?.Invoke(_result);
        }

        void RandomPosTargetZone()
        {
            var _posX = Random.Range(minPosTarget, maxPosTarget);
            rectTargetZone.anchoredPosition = new Vector2(_posX, 0);
        }

        IEnumerator MoveCraftSliderRoutine()
        {
            //Delay for now, change later
            yield return new WaitForSeconds(1);
            SetGameState(CraftMiniGameState.Playing);
            while (true)
            {
                var _startPos = rectCraftSlider.anchoredPosition;
                var _lastTimeSlide = timeSlider - elapsedTimeSlider;
                smoothSliderCoroutine = StartCoroutine(SmoothSliderRoutine(rectCraftSlider, _startPos, rangePosSlider, _lastTimeSlide));

                yield return smoothSliderCoroutine;
                rangePosSlider *= -1;
                elapsedTimeSlider = 0;
            }
        }

        IEnumerator SmoothSliderRoutine(RectTransform rectTransform, Vector2 startPos, Vector2 endPos, float duration)
        {
            float _elapsedTime = 0;
            while (_elapsedTime < duration)
            {
                var _t = Mathf.Clamp01(_elapsedTime / duration);
                rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, _t);

                _elapsedTime += Time.deltaTime;
                elapsedTimeSlider += Time.deltaTime;
                yield return null;
            }
            rectTransform.anchoredPosition = endPos;
        }

        void StopMoveCraftSlider()
        {
            if (startMoveSliderCoroutine != null)
            {
                StopCoroutine(startMoveSliderCoroutine);
                startMoveSliderCoroutine = null;
            }

            if (smoothSliderCoroutine != null)
            {
                StopCoroutine(smoothSliderCoroutine);
                smoothSliderCoroutine = null;
            }
        }

        void OnClickCraft()
        {
            if (gameState != CraftMiniGameState.Playing) return;

            SetGameState(CraftMiniGameState.Checking);
            StopMoveCraftSlider();

            if (CheckHitTargetZone())
                countHitTarget++;

            if (CheckValidResult())
            {
                EndGame();
                return;
            }

            RandomPosTargetZone();
            startMoveSliderCoroutine = StartCoroutine(MoveCraftSliderRoutine());
        }

        void EndGame()
        {
            SetGameState(CraftMiniGameState.EndGame);
            UnregisterButtonEvent();
        }

        bool CheckHitTargetZone()
        {
            var _rectSlider = GetWorldRect(rectCraftSlider);
            var _rectTargetZone = GetWorldRect(rectTargetZone);
            return _rectSlider.Overlaps(_rectTargetZone);
        }

        Rect GetWorldRect(RectTransform rectTransform)
        {
            var _corners = new Vector3[4];
            rectTransform.GetWorldCorners(_corners);

            var _size = new Vector2(_corners[2].x - _corners[0].x, _corners[2].y - _corners[0].y);
            return new Rect(_corners[0], _size);
        }
        bool CheckValidResult()
        {
            var _totalTouch = countHitTarget;
            return _totalTouch >= 4;
        }
    }
    public enum CraftMiniGameState
    {
        None = 0,
        Initialized = 1,
        Playing = 2,
        Checking = 3,
        EndGame = 4,
    }
}