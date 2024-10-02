using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    public abstract class WorkArea : MonoBehaviour
    {
        [SerializeField] private WorkingRange workingRange;
        [SerializeField] private bool hasWorker;

        protected void SetWorker(bool status) => hasWorker = status;
        protected bool CheckHasWorker() => hasWorker;


        void Start()
        {
            Initialized();
        }

        protected virtual void Initialized()
        {
            if (workingRange == null)
                workingRange = GetComponentInChildren<WorkingRange>();

            workingRange.SetWorkArea(this);
        }
        public abstract void ArrangeWorkers(Character character);


        public virtual void AutoSetRef()
        {
            workingRange = GetComponentInChildren<WorkingRange>();

            Debug.LogError($"GameObject {gameObject.name} - auto set ref completed");
        }
    }

    public enum AreaType
    {
        None = 0,
        IronBarrelArea = 50,

        WoodBarrelArea = 100,
    }
}