using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    public class WorkingActionHandler : MonoBehaviour
    {
        [SerializeField] private List<WorkingAction> lstWorkingAction;


        public void Initialized()
        {
            for (int i = 0, _count = lstWorkingAction.Count; i < _count; i++)
            {
                lstWorkingAction[i].Initialized();
            }
        }

        WorkingAction FindWorkingAction(WorkingType type)
        {
            return lstWorkingAction.Find(x => x.WorkingType == type);
        }

        public void DoingWorkingType(WorkingType type)
        {
            var _work = FindWorkingAction(type);
            if(_work == null)
            {
                Debug.LogError($"Can't find WorkingType: {type}");
                return;
            }

            _work.DoingWork();
        }
    }
}