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
        private int idBluprint;
        private bool isLock;


        public void SetLockState(bool state) => isLock = state;
        public void SetID(int id) => idBluprint = id;
        public void SetName(string name) => tmpName.text = name;
        public void SetSellingPrice(int? price) => tmpSellingPrice.text = $"{price}";
        public void SetLearnCost(int? cost) => tmpLearnCost.text = $"{cost}";
        public void SetImage(Sprite sprite) => imgBlueprint.sprite = sprite;

        // init self
    }
}