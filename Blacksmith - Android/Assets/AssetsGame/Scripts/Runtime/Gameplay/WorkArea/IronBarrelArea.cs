using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    public class IronBarrelArea : WorkArea
    {
        [Space]
        [SerializeField] private ResourceHandleUI resourceHandleUI;

        public int itemAmount = 150;

        protected override void Initialized()
        {
            base.Initialized();
            areaType = AreaType.IronBarrelArea;

            if (resourceHandleUI == null)
                resourceHandleUI = GetComponentInChildren<ResourceHandleUI>();
        }

        protected override void LoadUI()
        {
            resourceHandleUI.UpdateResourceAmountUI(itemAmount);
        }

        protected override bool CheckInvalidWorking(Character character)
        {
            return false;
        }

        public override void WorkerMoveIn(Character character)
        {
            if (CheckInvalidWorking(character)) return;

            SetWorker(true);
            itemAmount -= 1;
            resourceHandleUI.UpdateResourceAmountUI(itemAmount);

            character.WorkingForNowHAHA(areaType);
        }
        public override void WorkerMoveOut()
        {

        }

#if UNITY_EDITOR
        [ContextMenu(nameof(AutoSetRef))]
        public override void AutoSetRef()
        {
            base.AutoSetRef();
            areaType = AreaType.IronBarrelArea;
            resourceHandleUI = GetComponentInChildren<ResourceHandleUI>();
        }       
#endif
    }
}