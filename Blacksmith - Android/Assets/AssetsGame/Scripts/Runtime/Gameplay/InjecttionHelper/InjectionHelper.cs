using Naux.Patterns;
using Runtime;
using UnityEngine;

public class InjectionHelper : Singleton<InjectionHelper>
{
    [SerializeField] private LocalPopupHandle localPopupHandle;
    [SerializeField] private KilnMiniGameHandle kilnMiniGameHandle;
    [SerializeField] private CraftMiniGameHandle craftMiniGameHandle;


    [Space]
    [SerializeField] private BlueprintDataSO blueprintDataSO;




    public LocalPopupHandle LocalPopupHandle => localPopupHandle;
    public KilnMiniGameHandle KilnMiniGameHandle => kilnMiniGameHandle;
    public CraftMiniGameHandle CraftMiniGameHandle => craftMiniGameHandle;


    public BlueprintDataSO BlueprintDataSO => blueprintDataSO;

}
