using UnityEngine;

namespace Runtime
{
    public class BlueprintBoardPopup : Popup
    {
        private BlueprintBoardHandle blueprintBoadHandle;

        public override void Initialized()
        {
            base.Initialized();
            SetPopupType(PopupType.BlueprintBoard);

            blueprintBoadHandle = GetComponentInChildren<BlueprintBoardHandle>();
            if (blueprintBoadHandle == null)
                Debug.LogError($"Can't find component '{nameof(BlueprintBoardHandle)}' in children");
        }

        public override void SetupBeforeShow()
        {
            base.SetupBeforeShow();
            blueprintBoadHandle?.Initialized();
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