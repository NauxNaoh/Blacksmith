using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
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


        private void Awake()
        {
            rectFire = GetComponent<RectTransform>();
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