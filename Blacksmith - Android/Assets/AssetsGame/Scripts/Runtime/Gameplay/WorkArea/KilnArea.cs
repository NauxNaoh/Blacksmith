using UnityEngine;

namespace Runtime
{
    public class KilnArea : WorkArea
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
            areaType = AreaType.KilnArea;
            waitingTime = 2;

            if (loadingWorkUI == null)
                loadingWorkUI = GetComponentInChildren<LoadingWorkUI>();
            ResetInitialized();
        }
        protected override void InitializedUI()
        {
            UpdateLoadingUI();
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
            if (CheckValidWorking(character)) return;

            if (timer >= waitingTime)
            {
                acceptedWorker = true;
                Debug.Log($"show {gameObject.name} mini game");
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