using UnityEngine;

namespace Runtime
{
    public class KilnMiniGamePopup : Popup
    {
        public override void Initialized()
        {
            base.Initialized();
            SetPopupType(PopupType.KilnMiniGame);
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