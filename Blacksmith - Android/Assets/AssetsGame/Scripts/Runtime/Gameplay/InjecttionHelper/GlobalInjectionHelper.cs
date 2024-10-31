using Naux.Patterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    public class GlobalInjectionHelper : Singleton<GlobalInjectionHelper>
    {
        [Space]
        [SerializeField] private BlueprintDataSO blueprintDataSO;
        public BlueprintDataSO BlueprintDataSO => blueprintDataSO;

    }
}