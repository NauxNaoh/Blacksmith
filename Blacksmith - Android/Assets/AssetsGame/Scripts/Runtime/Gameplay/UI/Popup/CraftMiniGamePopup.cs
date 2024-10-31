using UnityEngine;

namespace Runtime
{
    public class CraftMiniGamePopup : Popup
    {
        private CraftMiniGameHandle craftMiniGameHandle;

        public override void Initialized()
        {
            base.Initialized();
            SetPopupType(PopupType.CraftMiniGame);

            craftMiniGameHandle = GetComponentInChildren<CraftMiniGameHandle>();
            if (craftMiniGameHandle == null)
                Debug.LogError($"Can't find component '{nameof(CraftMiniGameHandle)}' in children");
        }

        public override void SetupBeforeShow()
        {
            base.SetupBeforeShow();
            craftMiniGameHandle?.Initialized();
        }

        public override void Show()
        {
            base.Show();
            craftMiniGameHandle?.StartCoroutine(craftMiniGameHandle.StartMiniGameRoutine());
        }

        public override void Hide()
        {
            base.Hide();
        }
    }
}