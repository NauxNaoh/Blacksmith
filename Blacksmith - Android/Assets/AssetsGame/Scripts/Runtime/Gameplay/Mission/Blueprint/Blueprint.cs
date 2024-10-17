using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime
{
    public class Blueprint : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI tmpName;
        [SerializeField] private TextMeshProUGUI tmpSellingPrice;
        [SerializeField] private Image imgBlueprint;
        [SerializeField] private TextMeshProUGUI tmpLearnCost;

        DrivenRectTransformTracker drivenRect = new DrivenRectTransformTracker();
    }
}