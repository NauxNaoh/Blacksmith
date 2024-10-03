using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    public abstract class WorkArea : MonoBehaviour
    {
        [SerializeField] private WorkingRange workingRange;

        protected AreaType areaType;
        private bool hasWorker;
        protected void SetWorker(bool status) => hasWorker = status; //change or remove?


        void Start()
        {
            Initialized();
            LoadUI();
        }

        protected virtual void Initialized()
        {
            if (workingRange == null)
                workingRange = GetComponentInChildren<WorkingRange>();
            workingRange.SetWorkArea(this);
        }

        protected abstract void LoadUI();
        protected abstract bool CheckInvalidWorking(Character character);
        public abstract void WorkerMoveIn(Character character);
        public abstract void WorkerMoveOut();


#if UNITY_EDITOR
        public virtual void AutoSetRef()
        {
            workingRange = GetComponentInChildren<WorkingRange>();
            Debug.LogWarning($"GameObject {gameObject.name} - auto set ref completed");
        }
#endif
    }

    public enum AreaType
    {
        None = 0,
        MissionBoardArea = 40,
        IronBarrelArea = 50,
        WoodBarrelArea = 100,
    }
}