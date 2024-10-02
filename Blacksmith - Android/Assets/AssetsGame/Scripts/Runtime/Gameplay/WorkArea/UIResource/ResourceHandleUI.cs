using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Runtime
{
    public class ResourceHandleUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI tmpResourceAmount;

        public void UpdateResourceAmountUI(int amount)
        {
            tmpResourceAmount.text = $"{amount}";
        }
    }
}