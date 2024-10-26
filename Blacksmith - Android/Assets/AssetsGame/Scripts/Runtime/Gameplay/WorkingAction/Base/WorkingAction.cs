using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    public abstract class WorkingAction : MonoBehaviour
    {
        private WorkingType workingType;
        public WorkingType WorkingType => workingType;


        protected void SetWorkingType(WorkingType type) => workingType = type;

        public abstract void Initialized();

        public abstract void DoingWork();
    }

    public enum WorkingType
    {
        None = 0,

        BlueprintBoardWorking = 10,
        MissionBoardWorking = 11,

        IronBarrelWorking = 50,
        KilnWorking = 51,

        WoodBarrelWorking = 100,
        CraftTableWorking = 101,
    }
}