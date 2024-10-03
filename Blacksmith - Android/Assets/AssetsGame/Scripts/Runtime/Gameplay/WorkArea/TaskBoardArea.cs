using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    public class TaskBoardArea : WorkArea
    {
        [Space]
        [SerializeField] private LoadingProcessUI loadingProcessUI;
        [SerializeField] private float waitingTime;
        private float timer;
        private bool showedTask;

        void ResetLoadingProcess()
        {
            timer = 0;
            showedTask = false;
        }

        protected override void Initialized()
        {
            base.Initialized();
            areaType = AreaType.TaskBoardArea;
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
            return character.CharacterType != CharacterType.Player || showedTask;
        }

        public override void WorkerMoveIn(Character character)
        {
            if (CheckInvalidWorking(character)) return;
            
            if (timer >= waitingTime)
            {
                showedTask = true;
                Debug.Log("show task board");
            }
            else
            {
                timer += Time.deltaTime;
                LoadUI();
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
            areaType = AreaType.TaskBoardArea;
            loadingProcessUI = GetComponentInChildren<LoadingProcessUI>();
        }
#endif
    }
}