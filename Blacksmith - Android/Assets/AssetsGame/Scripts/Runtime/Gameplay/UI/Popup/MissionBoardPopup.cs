using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    public class MissionBoardPopup : Popup
    {
        private MissionBoardHandle missionBoardHandle;

        public override void Initialized()
        {
            base.Initialized();
            SetPopupType(PopupType.MissionBoard);

            missionBoardHandle = GetComponentInChildren<MissionBoardHandle>();
            if (missionBoardHandle == null)
                Debug.LogError($"Can't find component '{nameof(MissionBoardHandle)}' in children");
        }

        public override void SetupBeforeShow()
        {
            base.SetupBeforeShow();
            missionBoardHandle?.Initialized();
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