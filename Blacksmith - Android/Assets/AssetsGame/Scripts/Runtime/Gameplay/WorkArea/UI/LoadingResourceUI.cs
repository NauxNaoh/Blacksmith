using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime
{
    public class LoadingResourceUI : MonoBehaviour
    {
        [SerializeField] private GameObject gobjLoading;
        [SerializeField] private Image imgCircleFill;

        [SerializeField] private TextMeshProUGUI tmpResourceAmount;

        public void SetActiveLoading(bool status)
        {
            imgCircleFill.enabled = status;
        }

        public void UpdateLoadingProcessUI(float value)
        {
            imgCircleFill.fillAmount = value;
        }

        public void UpdateResourceAmountUI(int amount)
        {
            tmpResourceAmount.text = $"{amount}";
        }
    }
}