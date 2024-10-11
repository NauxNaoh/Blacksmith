using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    public abstract class WorkArea : MonoBehaviour
    {
        [SerializeField] private WorkingRange workingRange;
        protected AreaType areaType;
        protected Character characterWork;
        
        protected float waitingTime;
        protected float timer;
        protected bool acceptedWorker;
        protected void SetWorker(Character character) => characterWork = character;

        private void Start()
        {
            Initialized();
            InitializedUI();
        }

        protected virtual void Initialized()
        {
            if (workingRange == null)
                workingRange = GetComponentInChildren<WorkingRange>();
            workingRange.SetWorkArea(this);
        }
        protected abstract void InitializedUI();
        protected abstract void ResetInitialized();
        protected abstract bool CheckValidWorking(Character character);
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
        KilnArea = 51,
        WoodBarrelArea = 100,
    }
}