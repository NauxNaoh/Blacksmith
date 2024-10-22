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

        private IEnumerator Start()
        {
            yield return new WaitUntil(() => InjectionHelper.Instance != null);
            Initialized();
        }

        void Initialized()
        {
            if (isFirstLoad)
            {
                var _dataSO = InjectionHelper.Instance.BlueprintDataSO.lstBlueprintSO;
                for (int i = 0, _count = _dataSO.Count; i < _count; i++)
                {
                    var _blueprint = Instantiate(prefabBlueprint, rectContent);
                    _blueprint.SetName(_dataSO[i].blueprintName);
                    _blueprint.SetSellingPrice(_dataSO[i].sellingPrice);
                    _blueprint.SetLearnCost(_dataSO[i].learnCost);
                    _blueprint.SetImage(_dataSO[i].blueprintSprite);
                }

                isFirstLoad = false;
            }

        }

    }
}