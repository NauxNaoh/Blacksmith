using UnityEngine;

namespace Runtime
{
    public class WorkingRange : MonoBehaviour
    {
        private WorkArea workArea;

        public void SetWorkArea(WorkArea wArea)
        {
            workArea = wArea;
        }
        public void StandOnWorkingRange(Character character)
        {
            workArea.WorkerMoveIn(character);
        }
        public void ExitOnWorkingRange()
        {
            workArea.WorkerMoveOut();
        }
    }
}