using UnityEngine;
using UnityEngine.UI;

namespace Runtime
{
    public class FireMove : MonoBehaviour
    {
        private RectTransform rectFire;
        private Image imgFire;
        private bool fireStatus;
        public bool FireStatus => fireStatus;
        public RectTransform GetRectTransform() => rectFire;

        public void Initialized()
        {
            if (rectFire == null)
                rectFire = GetComponent<RectTransform>();
            if (imgFire == null)
                imgFire = GetComponent<Image>();
        }

        public void SetParrentFire(Transform transform)
        {
            rectFire.SetParent(transform);
        }
        public void SetAnchorPositionItem(Vector2 vector2)
        {
            rectFire.anchoredPosition = vector2;
        }

        public void SetStatusFire(bool status)
        {
            fireStatus = status;
            imgFire.enabled = fireStatus;
        }
    }
}