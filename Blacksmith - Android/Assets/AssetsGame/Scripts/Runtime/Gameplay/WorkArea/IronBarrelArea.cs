using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    public class IronBarrelArea : WorkArea
    {

        [SerializeField] private ResourceHandleUI resourceHandleUI;
        [SerializeField] private Character Character;

        [SerializeField] private AreaType areaType;
        public int itemAmount = 150;

        protected override void Initialized()
        {
            base.Initialized();
            areaType = AreaType.IronBarrelArea;

            if (resourceHandleUI == null)
                resourceHandleUI = GetComponentInChildren<ResourceHandleUI>();

            // change to new method Load?
            resourceHandleUI.UpdateResourceAmountUI(itemAmount); 
        }

        public override void ArrangeWorkers(Character character)
        {
            if (CheckHasWorker()) return;
            Debug.Log($"Iron Barrel doing!");

            SetWorker(true);
            itemAmount -= 1;
            resourceHandleUI.UpdateResourceAmountUI(itemAmount);

            character.WorkingForNowHAHA(areaType);
        }

        [ContextMenu(nameof(AutoSetRef))]
        public override void AutoSetRef()
        {
            base.AutoSetRef();
            resourceHandleUI = GetComponentInChildren<ResourceHandleUI>();
        }
    }

}