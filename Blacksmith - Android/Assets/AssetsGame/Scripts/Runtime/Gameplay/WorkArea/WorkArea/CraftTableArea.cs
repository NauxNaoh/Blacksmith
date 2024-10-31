using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    public class CraftTableArea : WorkArea
    {
        [Space]
        [SerializeField] private LoadingWorkUI loadingWorkUI;

        void UpdateLoadingUI()
        {
            var value = timer / waitingTime;
            loadingWorkUI.UpdateLoadingProcessUI(value);
        }

        protected override void Initialized()
        {
            base.Initialized();
            areaType = AreaType.CraftTableArea;
            waitingTime = 2;

            if (loadingWorkUI == null)
                loadingWorkUI = GetComponentInChildren<LoadingWorkUI>();
            ResetInitialized();
        }
        protected override void InitializedUI()
        {
            UpdateLoadingUI();
            loadingWorkUI.SetActiveLoading(false);
        }
        protected override void ResetInitialized()
        {
            timer = 0;
            acceptedWorker = false;
        }
        protected override bool CheckValidWorking(Character character)
        {
            return characterWork == null || characterWork == character && !acceptedWorker;
        }
        public override void WorkerMoveIn(Character character)
        {
            if (!CheckValidWorking(character)) return;
            SetWorker(character);

            if (timer >= waitingTime)
            {
                acceptedWorker = true;
                character.WorkingOnWorkArea(areaType);
            }
            else
            {
                timer += Time.deltaTime;
                InitializedUI();
                loadingWorkUI.SetActiveLoading(true);
            }
        }
        public override void WorkerMoveOut()
        {
            ResetInitialized();
            loadingWorkUI.SetActiveLoading(false);
            UpdateLoadingUI();
            SetWorker(null);
        }

#if UNITY_EDITOR
        [ContextMenu(nameof(AutoSetRef))]
        public override void AutoSetRef()
        {
            base.AutoSetRef();
            loadingWorkUI = GetComponentInChildren<LoadingWorkUI>();
        }
#endif
    }
}
