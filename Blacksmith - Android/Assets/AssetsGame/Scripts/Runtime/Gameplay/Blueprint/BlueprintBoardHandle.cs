using Naux.DB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    public class BlueprintBoardHandle : MonoBehaviour
    {
        [SerializeField] private Blueprint prefabBlueprint;
        [SerializeField] private RectTransform rectContent;
        bool isFirstLoad = true;

        private void Start()
        {
            Initialized();
        }

        void Initialized()
        {
            if (!isFirstLoad) return;

            LoadBlueprintFromDB();
            isFirstLoad = false;
        }

        void LoadBlueprintFromDB()
        {
            var _lstDB = DBController.Instance.BLUEPRINT_DB.lstBlueprintInfo;
            var _dataSO = InjectionGlobalHelper.Instance.BlueprintDataSO;

            for (int i = 0, _countDB = _lstDB.Count; i < _countDB; i++)
            {
                var _foundBlueprintSO = _dataSO.FindBlueprintWithID(_lstDB[i].id);
                if (_foundBlueprintSO == null) continue;

                var _blueprint = Instantiate(prefabBlueprint, rectContent);
                _blueprint.SetID(_lstDB[i].id);
                _blueprint.SetLockState(_lstDB[i].isLock);
                _blueprint.SetName(_foundBlueprintSO.name);
                _blueprint.SetSellingPrice(_foundBlueprintSO.sellingPrice);
                _blueprint.SetLearnCost(_foundBlueprintSO.learnCost);
                _blueprint.SetImage(_foundBlueprintSO.sprite);
            }
        }

    }
}