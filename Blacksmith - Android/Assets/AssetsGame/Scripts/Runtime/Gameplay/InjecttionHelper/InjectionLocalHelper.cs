using Naux.Patterns;
using UnityEngine;

namespace Runtime
{
    public class InjectionLocalHelper : Singleton<InjectionLocalHelper>
    {
        [SerializeField] private LocalPopupHandle localPopupHandle;
        //[SerializeField] private KilnMiniGameHandle kilnMiniGameHandle;
        //[SerializeField] private CraftMiniGameHandle craftMiniGameHandle;




        public LocalPopupHandle LocalPopupHandle => localPopupHandle;
       // public KilnMiniGameHandle KilnMiniGameHandle => kilnMiniGameHandle;
       // public CraftMiniGameHandle CraftMiniGameHandle => craftMiniGameHandle;



    }
}