using UnityEngine;

namespace Runtime
{
    public abstract class Popup : MonoBehaviour
    {
        [SerializeField] protected GameObject contentPopup;
        private CanvasGroup canvasGroup;
        private PopupState popupState = PopupState.None;
        private PopupType popupType = PopupType.None;

        public PopupType PopupType => popupType;


        void SetPopupState(PopupState state) => popupState = state;
        protected void SetPopupType(PopupType type) => popupType = type;

        public virtual void Initialized()
        {
            SetPopupState(PopupState.Initialized);

            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                Debug.LogError($"Popup {gameObject.name} need add CanvasGroup component!");
                return;
            }
            ActiveCanvasGroup(false);
        }

        void ActiveCanvasGroup(bool status)
        {
            canvasGroup.interactable = status;
            canvasGroup.blocksRaycasts = status;
            var _alpha = status ? 1.0f : 0.0f;
            canvasGroup.alpha = _alpha;
        }


        public virtual void SetupBeforeShow()
        {
            ActiveCanvasGroup(true);
        }

        public virtual void SetupAfterHide()
        {
            ActiveCanvasGroup(false);
        }

        public virtual void Show()
        {
            SetPopupState(PopupState.Showing);
            SetupBeforeShow();
            contentPopup.SetActive(true);
        }

        public virtual void Hide()
        {
            SetPopupState(PopupState.Hiding);
            contentPopup.SetActive(false);
            SetupAfterHide();
        }
    }
    public enum PopupState
    {
        None = 0,
        Initialized = 1,
        Showing = 2,
        Hiding = 3,
    }
    public enum PopupType
    {
        None = 0,
        KilnMiniGame = 1,
        CraftMiniGame = 2,
        BlueprintBoard = 3,
    }
}