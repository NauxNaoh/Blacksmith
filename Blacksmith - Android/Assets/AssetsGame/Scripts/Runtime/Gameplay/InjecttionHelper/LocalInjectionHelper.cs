using Naux.Patterns;
using UnityEngine;

namespace Runtime
{
    public class LocalInjectionHelper : Singleton<LocalInjectionHelper>
    {
        [SerializeField] private LocalPopupHandle localPopupHandle;

        public LocalPopupHandle LocalPopupHandle => localPopupHandle;
    }
}