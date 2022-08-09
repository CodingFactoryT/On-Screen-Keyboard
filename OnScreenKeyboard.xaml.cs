using System;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WindowsInput;
using WindowsInput.Native;

namespace On_Screen_Keyboard
{
    /// <summary>
    /// Interaction logic for OnScreenKeyboard.xaml
    /// </summary>
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
        }

        private void Btn_Enter_Click(object sender, RoutedEventArgs e)
        {
            InputSimulator sim = new InputSimulator();
            sim.Keyboard.KeyPress(VirtualKeyCode.RETURN);
        }

        private void Btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
