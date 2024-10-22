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

        public void SetName(string name) => tmpName.text = name;
        public void SetSellingPrice(int? price) => tmpSellingPrice.text = $"{price}";
        public void SetLearnCost(int? cost) => tmpLearnCost.text = $"{cost}";
        public void SetImage(Sprite sprite) => imgBlueprint.sprite = sprite;
    }
}