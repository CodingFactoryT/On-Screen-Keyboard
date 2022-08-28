using System.Windows;
using System.Windows.Controls;

namespace On_Screen_Keyboard
{

    public partial class OnScreenKeyboard : UserControl
    {
        public static OnScreenKeyboard userControl { get; private set; }

        public OnScreenKeyboard()
        {
            InitializeComponent();
            userControl = this;

            mainGrid.Background = DataFetcher.GetBrushByName("PopupBackgroundColor");

            new ButtonGenerator().Generate();

            SetPopupSizeAndPosition(0.5, 0.3);  //the size of the popup is half of the screen width and a nearly a third od the screen height
            ActivateDragBehaviour();

            ConfigureBtn_Enter();
            ConfigureBtn_Exit();
        }

        private void SetPopupSizeAndPosition(double screenWidthMultiplier, double screenHeightMuliplier)
        {
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;

            popup.Width = screenWidth * screenWidthMultiplier;
            popup.Height = screenHeight * screenHeightMuliplier;

            popup.HorizontalOffset = screenWidth / 2 - popup.Width / 2; //positions the popup in the middle of the horizontal screen axis
            popup.VerticalOffset = screenHeight - popup.Height * (1.6 - screenHeightMuliplier); //always positions the popup above the taskbar
        }

        private void ActivateDragBehaviour()
        {
            popup.MouseDown += (s, e) =>
            {
                thumb.RaiseEvent(e);
            };

            thumb.DragDelta += (s, e) =>
            {
                popup.HorizontalOffset += e.HorizontalChange;
                popup.VerticalOffset += e.VerticalChange;
            };
        }

        private void ConfigureBtn_Enter()
        {
            Btn_Enter.Background = DataFetcher.GetBrushByName("KeyboardButtonColor");

            ButtonClickHandler buttonClickHandler = new ButtonClickHandler();
            Btn_Enter.Click += buttonClickHandler.Btn_Enter_Click;
        }

        private void ConfigureBtn_Exit()
        {
            Btn_Exit.Background = DataFetcher.GetBrushByName("KeyboardButtonColor");

            ButtonClickHandler buttonClickHandler = new ButtonClickHandler();
            Btn_Exit.Click += buttonClickHandler.Btn_Exit_Click;
        }
    }
}
