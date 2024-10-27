using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    public class IronBarrelWorking : WorkingAction
    {
        public override void Initialized()
        {
            SetWorkingType(WorkingType.IronBarrelWorking);
        }

        public override void DoingWork()
        {

        }
    }
}