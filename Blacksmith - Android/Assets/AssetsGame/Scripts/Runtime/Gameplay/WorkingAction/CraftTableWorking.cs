using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    public class CraftTableWorking : WorkingAction
    {
        public override void Initialized()
        {
            SetWorkingType(WorkingType.CraftTableWorking);
        }

        public override void DoingWork()
        {
            InjectionLocalHelper.Instance.LocalPopupHandle.ShowLocalPopup(PopupType.CraftMiniGame);
        }
    }
}