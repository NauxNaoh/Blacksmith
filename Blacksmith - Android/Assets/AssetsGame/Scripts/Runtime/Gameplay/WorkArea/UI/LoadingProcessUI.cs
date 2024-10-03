using UnityEngine;
using UnityEngine.UI;

public class LoadingProcessUI : MonoBehaviour
{
    [SerializeField] private Image imgCircleFill;

    public void UpdateLoadingProcessUI(float value)
    {
        imgCircleFill.fillAmount = value;
    }
}
