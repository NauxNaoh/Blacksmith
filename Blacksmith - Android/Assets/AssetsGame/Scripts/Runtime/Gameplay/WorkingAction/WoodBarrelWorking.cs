using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    public class WoodBarrelWorking : WorkingAction
    {
        public override void Initialized()
        {
            SetWorkingType(WorkingType.WoodBarrelWorking);
        }

        public override void DoingWork()
        {

        }
    }
}