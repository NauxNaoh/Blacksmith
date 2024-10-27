using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    public class MissionBoardWorking : WorkingAction
    {
        public override void Initialized()
        {
            SetWorkingType(WorkingType.MissionBoardWorking);
        }

        public override void DoingWork()
        {
            //InjectionLocalHelper.Instance.LocalPopupHandle.ShowLocalPopup(PopupType.);
        }
    }
}