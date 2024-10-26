using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime
{
    public class Blueprint : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI tmpName;
        [SerializeField] private TextMeshProUGUI tmpSellingPrice;
        [SerializeField] private TextMeshProUGUI tmpLearnCost;
        [SerializeField] private Image imgBlueprint;
        [SerializeField] private GameObject gobjSellingPrice;
        [SerializeField] private GameObject gobjLearnCost;
        [SerializeField] private GameObject gobjIconLock;
        private int idBlueprint;
        private bool isLock;


        public void SetLockState(bool state) => isLock = state;
        public void SetID(int id) => idBlueprint = id;
        public void SetName(string name) => tmpName.text = name;
        public void SetSellingPrice(int? price) => tmpSellingPrice.text = $"{price}";
        public void SetLearnCost(int? cost) => tmpLearnCost.text = $"{cost}";
        public void SetImage(Sprite sprite) => imgBlueprint.sprite = sprite;

        public void UpdateSelfUI()
        {
            var _color = isLock ? Color.grey : Color.white;
            imgBlueprint.color = _color;
            gobjSellingPrice.SetActive(!isLock);
            gobjLearnCost.SetActive(isLock);
            gobjIconLock.SetActive(isLock);
        }

    }
}