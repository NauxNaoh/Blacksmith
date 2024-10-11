namespace Runtime
{
    public class CraftMiniGamePopup : Popup
    {
        public override void Initialized()
        {
            base.Initialized();
            SetPopupType(PopupType.CraftMiniGame);
        }

        public override void Show()
        {
            base.Show();
            contentPopup.SetActive(true);

        }

        public override void Hide()
        {
            base.Hide();
            contentPopup.SetActive(false);
        }
    }
}