using UnityEngine;

namespace Runtime
{
    public abstract class Popup : MonoBehaviour
    {
        [SerializeField] protected GameObject contentPopup;

        private PopupState popupState = PopupState.None;
        private PopupType popupType = PopupType.None;
        public PopupType PopupType => popupType;

        private void SetPopupState(PopupState state) => popupState = state;
        protected void SetPopupType(PopupType type) => popupType = type;
       
        public virtual void Initialized()
        {
            SetPopupState(PopupState.Initialized);
        }

        public virtual void Show()
        {
            SetPopupState(PopupState.Showing);
            contentPopup.SetActive(true);
        }

        public virtual void Hide()
        {
            contentPopup.SetActive(false);
            SetPopupState(PopupState.Hiding);
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
    }
}