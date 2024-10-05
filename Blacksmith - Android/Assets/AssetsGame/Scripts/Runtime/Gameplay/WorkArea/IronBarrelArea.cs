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
            return characterWork != null;
        }

        public override void WorkerMoveIn(Character character)
        {
            if (CheckInvalidWorking(character)) return;

            itemAmount -= 1;
            LoadUI();

            SetWorker(character);
            character.WorkingForNowHAHA(areaType);
        }
        public override void WorkerMoveOut()
        {
            SetWorker(null);
        }

#if UNITY_EDITOR
        [ContextMenu(nameof(AutoSetRef))]
        public override void AutoSetRef()
        {
            base.AutoSetRef();
            resourceHandleUI = GetComponentInChildren<ResourceHandleUI>();
        }       
#endif
    }
}