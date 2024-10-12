using UnityEngine;

namespace Runtime
{
    public class CraftMiniGamePopup : Popup
    {
        public override void Initialized()
        {
            base.Initialized();
            SetPopupType(PopupType.CraftMiniGame);
        }

        public override void SetupBeforeShow()
        {
        }

        public override void Show()
        {
            base.Show();
        }

        public override void Hide()
        {
            base.Hide();
        }
    }
}