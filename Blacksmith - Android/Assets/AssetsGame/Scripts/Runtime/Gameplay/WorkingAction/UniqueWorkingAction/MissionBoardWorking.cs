namespace Runtime
{
    public class MissionBoardWorking : WorkingAction
    {
        public override void Initialized()
        {
            SetWorkingType(WorkingType.MissionBoardWorking);
        }

        public override void DoingWork()
        {
            LocalInjectionHelper.Instance.LocalPopupHandle.ShowLocalPopup(PopupType.MissionBoard);
        }
    }
}