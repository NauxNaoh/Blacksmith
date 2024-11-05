using Naux.DB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime
{
    public class BlueprintBoardHandle : MonoBehaviour
    {
        [SerializeField] private Blueprint prefabBlueprint;
        [SerializeField] private RectTransform rectContent;
        [SerializeField] private Button btnQuit;
        bool isFirstLoad = true;

        public void Initialized()
        {
            if (!isFirstLoad) return;

            LoadBlueprintFromDB();
            isFirstLoad = false;
            btnQuit.onClick.AddListener(OnClickButtonQuit);
        }

        void LoadBlueprintFromDB()
        {
            var _lstDB = DBController.Instance.BLUEPRINT_DB.lstBlueprint;
            var _dataSO = GlobalInjectionHelper.Instance.BlueprintDataSO;

            for (int i = 0, _countDB = _lstDB.Count; i < _countDB; i++)
            {
                var _foundBlueprintSO = _dataSO.FindBlueprintWithID(_lstDB[i].idBlueprint);
                if (_foundBlueprintSO == null) continue;

                var _blueprint = Instantiate<Blueprint>(prefabBlueprint, rectContent);
                _blueprint.SetID(_lstDB[i].idBlueprint);
                _blueprint.SetLockState(_lstDB[i].isLock);
                _blueprint.SetName(_foundBlueprintSO.name);
                _blueprint.SetSellingPrice(_foundBlueprintSO.sellingPrice);
                _blueprint.SetLearnCost(_foundBlueprintSO.learnCost);
                _blueprint.SetImage(_foundBlueprintSO.sprite);
                _blueprint.UpdateSelfUI();
            }
        }


        void OnClickButtonQuit()
        {
            LocalInjectionHelper.Instance.LocalPopupHandle.HideLocalPopup();
        }
    }
}