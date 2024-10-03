using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    public class MissionBoardArea : WorkArea
    {
        [Space]
        [SerializeField] private LoadingProcessUI loadingProcessUI;
        [SerializeField] private float waitingTime;
        private float timer;
        private bool showedMission;

        void ResetLoadingProcess()
        {
            timer = 0;
            showedMission = false;
        }

        protected override void Initialized()
        {
            base.Initialized();
            areaType = AreaType.MissionBoardArea;
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
            return character.CharacterType != CharacterType.Player || showedMission;
        }

        public override void WorkerMoveIn(Character character)
        {
            if (CheckInvalidWorking(character)) return;
            
            if (timer >= waitingTime)
            {
                showedMission = true;
                Debug.Log("show Mission board");
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
            areaType = AreaType.MissionBoardArea;
            loadingProcessUI = GetComponentInChildren<LoadingProcessUI>();
        }
#endif
    }
}