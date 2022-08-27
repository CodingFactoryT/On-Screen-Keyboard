using System;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WindowsInput;
using WindowsInput.Native;

namespace On_Screen_Keyboard
{

    public partial class OnScreenKeyboard : UserControl
    {
        public static OnScreenKeyboard userControl { get; private set; }

        public OnScreenKeyboard()
        {
            InitializeComponent();
            mainGrid.Background = (Brush)new BrushConverter().ConvertFrom(ConfigurationManager.AppSettings["PopupBackgroundColor"]);
            userControl = this;
            new ButtonGenerator().Generate();
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            popup.Width = screenWidth * 0.5;
            popup.Height = screenHeight * 0.3;

            popup.HorizontalOffset = screenWidth/2 - popup.Width/2;
            popup.VerticalOffset = screenHeight - popup.Height * 1.3;

            popup.MouseDown += (s, e) =>
            {
                thumb.RaiseEvent(e);
            };

            thumb.DragDelta += (s, e) =>
            {
                popup.HorizontalOffset += e.HorizontalChange;
                popup.VerticalOffset += e.VerticalChange;
            };
            Brush backgroundColor = (Brush)new BrushConverter().ConvertFrom(ConfigurationManager.AppSettings["KeyboardButtonColor"]);
            Btn_Enter.Background = backgroundColor;
            Btn_Exit.Background = backgroundColor;
        }
        private void Btn_Enter_Click(object sender, RoutedEventArgs e)
        {
            InputSimulator sim = new InputSimulator();
            sim.Keyboard.KeyPress(VirtualKeyCode.RETURN);
        }

        private void Btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            InputSimulator inputSimulator = new InputSimulator();

            foreach (KeyboardButtonData b in ButtonGenerator.buttons)
            {
                if (b.isToggable)
                {
                    VirtualKeyCode keyCode = ButtonGenerator.getKeyCode(b.keyCode);
                    inputSimulator.Keyboard.KeyUp(keyCode);
                }
            }

            Environment.Exit(0);
        }
    }
}
