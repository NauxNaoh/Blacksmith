using UnityEngine;
using UnityEngine.UI;

public class LoadingWorkUI : MonoBehaviour
{
    [SerializeField] private GameObject gobjLoading;
    [SerializeField] private Image imgCircleFill;

    public void SetActiveLoading(bool status)
    {
        gobjLoading.SetActive(status);
    }

    public void UpdateLoadingProcessUI(float value)
    {
        imgCircleFill.fillAmount = value;
    }
}
