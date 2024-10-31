using UnityEngine;

namespace Runtime
{
    public class WoodBarrelArea : WorkArea
    {
        [Space]
        [SerializeField] private LoadingResourceUI loadingResourceUI;
        public int itemAmount = 150;

       
        void UpdateLoadingUI()
        {
            var value = timer / waitingTime;
            loadingResourceUI.UpdateLoadingProcessUI(value);
        }
        void UpdateResourceUI()
        {
            loadingResourceUI.UpdateResourceAmountUI(itemAmount);
        }

        protected override void Initialized()
        {
            base.Initialized();
            areaType = AreaType.WoodBarrelArea;
            waitingTime = 2;

            if (loadingResourceUI == null)
                loadingResourceUI = GetComponentInChildren<LoadingResourceUI>();
            ResetInitialized();
        }
        protected override void InitializedUI()
        {
            UpdateLoadingUI();
            UpdateResourceUI();
            loadingResourceUI.SetActiveLoading(false);
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
                itemAmount -= 1;
                UpdateResourceUI();
                Debug.Log($"Get item {gameObject.name} success");
                character.WorkingOnWorkArea(areaType);
            }
            else
            {
                timer += Time.deltaTime;
                UpdateLoadingUI();
                loadingResourceUI.SetActiveLoading(true);
            }
        }
        public override void WorkerMoveOut()
        {
            ResetInitialized();
            loadingResourceUI.SetActiveLoading(false);
            UpdateLoadingUI();
            SetWorker(null);
        }


#if UNITY_EDITOR
        [ContextMenu(nameof(AutoSetRef))]
        public override void AutoSetRef()
        {
            base.AutoSetRef();
            loadingResourceUI = GetComponentInChildren<LoadingResourceUI>();
        }
#endif
    }
}