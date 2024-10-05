using UnityEngine;
using UnityEngine.UI;

public class LoadingProcessUI : MonoBehaviour
{
    [SerializeField] private GameObject gobjLoadingProcess;
    [SerializeField] private Image imgCircleFill;

    public void SetActiveLoading(bool status)
    {
        gobjLoadingProcess.SetActive(status);
    }

    public void UpdateLoadingProcessUI(float value)
    {
        imgCircleFill.fillAmount = value;
    }
}
