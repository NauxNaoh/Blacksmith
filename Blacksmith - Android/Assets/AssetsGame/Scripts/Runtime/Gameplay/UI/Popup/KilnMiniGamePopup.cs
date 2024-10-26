using UnityEngine;

namespace Runtime
{
    public class KilnMiniGamePopup : Popup
    {
        private KilnMiniGameHandle kilnMiniGameHandle;

        public override void Initialized()
        {
            base.Initialized();
            SetPopupType(PopupType.KilnMiniGame);

            kilnMiniGameHandle = GetComponentInChildren<KilnMiniGameHandle>();
            if (kilnMiniGameHandle == null)
                Debug.LogError($"Can't find component '{nameof(KilnMiniGameHandle)}' in children");
        }

        public override void SetupBeforeShow()
        {
            base.SetupBeforeShow();
            kilnMiniGameHandle.Initialized();
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