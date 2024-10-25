using UnityEngine;
using UnityEngine.EventSystems;

namespace Naux.Joystick
{
    public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [SerializeField] private RectTransform rectBackground;
        [SerializeField] private RectTransform rectHandle;

        private Camera cam;
        private Canvas canvas;
        private RectTransform baseRect;
        private Vector2 directInput = Vector2.zero;
        public Vector2 Direction => directInput;

       
        void SetActiveRectBackground(bool status) => rectBackground.gameObject.SetActive(status);
        void SetRectHandleAnchorPos(Vector3 vector3) => rectHandle.anchoredPosition = vector3;


        void Start()
        {
            Initialized();
        }

        void Initialized()
        {
            baseRect = GetComponent<RectTransform>();
            canvas = GetComponentInParent<Canvas>();
            if (canvas == null)
                Debug.LogError($"The joystick is not placed inside a canvas!");
            if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
                cam = canvas.worldCamera;

            var _center = new Vector2(0.5f, 0.5f);
            rectBackground.pivot = _center;
            rectHandle.anchorMin = _center;
            rectHandle.anchorMax = _center;
            rectHandle.pivot = _center;

            SetRectHandleAnchorPos(Vector2.zero);
            SetActiveRectBackground(false);
        }


        public void OnPointerDown(PointerEventData eventData)
        {
            rectBackground.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
            SetActiveRectBackground(true);

            OnDrag(eventData);
        }

        Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(baseRect, screenPosition, cam, out var localPoint))
            {
                Vector2 _pivotOffset = baseRect.pivot * baseRect.sizeDelta;
                return localPoint - (rectBackground.anchorMax * baseRect.sizeDelta) + _pivotOffset;
            }

            return Vector2.zero;
        }


        public void OnDrag(PointerEventData eventData)
        {
            Vector2 _bgPos = RectTransformUtility.WorldToScreenPoint(cam, rectBackground.position);
            Vector2 _radius = rectBackground.sizeDelta / 2;

            directInput = (eventData.position - _bgPos) / (_radius * canvas.scaleFactor);
            HandleInput(directInput.magnitude, directInput.normalized, _radius, cam);
            SetRectHandleAnchorPos(directInput * _radius);
        }

        void HandleInput(float magnitude, Vector2 normalized, Vector2 radius, Camera cam)
        {
            if (magnitude > 0)
            {
                if (magnitude > 1)
                    directInput = normalized;
            }
            else
                directInput = Vector2.zero;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            directInput = Vector2.zero;
            SetActiveRectBackground(false);
            SetRectHandleAnchorPos(Vector2.zero);
        }
    }
}