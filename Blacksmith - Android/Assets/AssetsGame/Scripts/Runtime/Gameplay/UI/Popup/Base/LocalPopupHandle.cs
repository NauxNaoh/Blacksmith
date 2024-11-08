using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    public class LocalPopupHandle : MonoBehaviour
    {
        [SerializeField] private GameObject fadePanel;
        [SerializeField] private List<Popup> lstLocalPopup = new List<Popup>();

        private LocalPopupState localPopupState;
        private PopupType showingPopup;

        void SetLocalPopupState(LocalPopupState state) => localPopupState = state;
        void ShowPanel(bool status) => fadePanel.SetActive(status);


        private void Awake()
        {
            Initialized();
        }

        void Initialized()
        {
            ShowPanel(false);
            for (int i = 0, _count = lstLocalPopup.Count; i < _count; i++)
            {
                lstLocalPopup[i].Initialized();
            }
            showingPopup = PopupType.None;
            SetLocalPopupState(LocalPopupState.Initialized);
        }

        Popup FindPopupWithType(PopupType type)
        {
            return lstLocalPopup.Find(x => x.PopupType == type);
        }

        
        public void ShowLocalPopup(PopupType popupType)
        {
            if (localPopupState == LocalPopupState.Showing) return;

            var _popup = FindPopupWithType(popupType);
            if (_popup == null)
            {
                Debug.LogError($"Can't find popupType: {popupType}");
                return;
            }

            SetLocalPopupState(LocalPopupState.Showing);
            showingPopup = popupType;
            ShowPanel(true);
            _popup.Show();
        }

        public void HideLocalPopup()
        {
            if (localPopupState == LocalPopupState.Hiding) return;

            var _popup = FindPopupWithType(showingPopup);
            if (_popup == null)
            {
                Debug.LogError($"Can't find showingPopup: {showingPopup}");
                return;
            }
            SetLocalPopupState(LocalPopupState.Hiding);
            showingPopup = PopupType.None;
            _popup.Hide();
            ShowPanel(false);
        }

        [ContextMenu(nameof(AutoAddPopup))]
        public void AutoAddPopup()
        {
            lstLocalPopup = new List<Popup>();
            var _popups = GetComponentsInChildren<Popup>();
            for (int i = 0; i < _popups.Length; i++)
            {
                lstLocalPopup.Add(_popups[i]);
            }
        }
    }
    public enum LocalPopupState
    {
        None = 0,
        Initialized = 1,
        Showing = 2,
        Hiding = 3,
    }
}