using UnityEngine;

namespace Runtime
{
    public class ItemMove : MonoBehaviour
    {
        private RectTransform rectItemMove;
        private int countTouch;
        
        public int CountTouch => countTouch;
        public RectTransform GetRectTransform() => rectItemMove;


        private void Awake()
        {
            rectItemMove = GetComponent<RectTransform>();
        }

        public void SetAnchorPositionItem(Vector2 vector2)
        {
            rectItemMove.anchoredPosition = vector2;
        }

        public void ResetCounter()
        {
            countTouch = 0;
        }

        public void HitFire()
        {
            countTouch++;
        }

    }
}