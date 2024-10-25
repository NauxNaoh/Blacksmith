using Naux.Patterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    public class InjectionGlobalHelper : Singleton<InjectionGlobalHelper>
    {

        [Space]
        [SerializeField] private BlueprintDataSO blueprintDataSO;
        public BlueprintDataSO BlueprintDataSO => blueprintDataSO;

    }
}