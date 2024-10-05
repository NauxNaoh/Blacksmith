using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    public class KilnArea : WorkArea
    {
        [Space]
        [SerializeField] private LoadingProcessUI loadingProcessUI;
        [SerializeField] private float waitingTime;
        private float timer;
        private bool showedKiln;

        void ResetLoadingProcess()
        {
            timer = 0;
            showedKiln = false;
            loadingProcessUI.SetActiveLoading(false);
        }

        protected override void Initialized()
        {
            base.Initialized();
            areaType = AreaType.KilnArea;
            if (loadingProcessUI == null)
                loadingProcessUI = GetComponentInChildren<LoadingProcessUI>();

            ResetLoadingProcess();
        }
        protected override void LoadUI()
        {
            var value = timer / waitingTime;
            loadingProcessUI.UpdateLoadingProcessUI(value);
        }

        protected override bool CheckInvalidWorking(Character character)
        {
            return character.CharacterType != CharacterType.Player || showedKiln;
        }

        public override void WorkerMoveIn(Character character)
        {
            if (CheckInvalidWorking(character)) return;

            if (timer >= waitingTime)
            {
                showedKiln = true;
                Debug.Log($"show {gameObject.name} board");
            }
            else
            {
                timer += Time.deltaTime;
                LoadUI();
                loadingProcessUI.SetActiveLoading(true);
            }
        }

        public override void WorkerMoveOut()
        {
            ResetLoadingProcess();
            LoadUI();
        }

#if UNITY_EDITOR
        [ContextMenu(nameof(AutoSetRef))]
        public override void AutoSetRef()
        {
            base.AutoSetRef();
            loadingProcessUI = GetComponentInChildren<LoadingProcessUI>();
        }
#endif
    }
}