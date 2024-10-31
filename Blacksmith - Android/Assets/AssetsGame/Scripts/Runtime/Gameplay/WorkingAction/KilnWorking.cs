using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    public class KilnWorking : WorkingAction
    {
        public override void Initialized()
        {
            SetWorkingType(WorkingType.KilnWorking);
        }

        public override void DoingWork()
        {
            LocalInjectionHelper.Instance.LocalPopupHandle.ShowLocalPopup(PopupType.KilnMiniGame);
        }


    }
}