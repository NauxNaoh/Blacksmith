using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    public class BlueprintBoardWorking : WorkingAction
    {
      
        public override void Initialized()
        {
            SetWorkingType(WorkingType.BlueprintBoardWorking);
        }
        public override void DoingWork()
        {
            InjectionLocalHelper.Instance.LocalPopupHandle.ShowLocalPopup(PopupType.BlueprintBoard);
        }


    }
}