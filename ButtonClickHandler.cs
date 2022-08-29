using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WindowsInput;
using WindowsInput.Native;

namespace On_Screen_Keyboard
{
    public class ButtonClickHandler
    {
        private InputSimulator inputSimulator = new InputSimulator();
        private KeyboardButtonData buttonData;

        public ButtonClickHandler(KeyboardButtonData buttonData)
        {
            this.buttonData = buttonData;
        }

        public ButtonClickHandler() { }

        public void NonToggableButton_Click(object sender, RoutedEventArgs e)
        {
            inputSimulator.Keyboard.KeyPress(DataFetcher.GetKeyCodeByName(buttonData.keyCode));
            if (Keyboard.IsKeyDown(Key.LeftShift))
            {
                Button leftShiftButton = DataFetcher.GetControlByName("Btn_LSHIFT") as Button;
                leftShiftButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }

        public void ToggableButton_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            if (e != null)
            {
                buttonData.isToggled = !buttonData.isToggled;
            }
            VirtualKeyCode keyCode = DataFetcher.GetKeyCodeByName(buttonData.keyCode);

            if (buttonData.isToggled)
            {
                btn.Background = DataFetcher.GetBrushByName("ToggleKeyboardButtonColor");
                inputSimulator.Keyboard.KeyDown(keyCode);
            }
            else
            {
                btn.Background = DataFetcher.GetBrushByName("KeyboardButtonColor");
                inputSimulator.Keyboard.KeyUp(keyCode);
            }

            if (btn.Name.Equals("Btn_LSHIFT") && e != null)
                ToggableButton_Click(sender, null);
            if (btn.Name.Equals("Btn_RSHIFT") && e != null)
                ToggableButton_Click(sender, null);
            if (btn.Name.Equals("Btn_LCTRL") && e != null)
                ToggableButton_Click(sender, null);
            if (btn.Name.Equals("Btn_RCTRL") && e != null)
                ToggableButton_Click(sender, null);


            if (btn.Name.Equals("Btn_FNCT"))
                ToggleRow1Visibility(buttonData.isToggled);

            ButtonDataManager.SetButtonContent(ButtonDataManager.GetCurrentButtonMode());

        }

        public void SimulateToggableButtonPress(string buttonName)
        {
            Button button = DataFetcher.GetControlByName(buttonName) as Button;      
        }

        private void ToggleRow1Visibility(bool isFnctToggled)
        {
            Visibility visibilityNumbers = Visibility.Visible;
            Visibility visibilityFunctionKeys = Visibility.Collapsed;
            if (isFnctToggled)
            {
                visibilityNumbers = Visibility.Collapsed;
                visibilityFunctionKeys = Visibility.Visible;
            }
            UIElementCollection row1Buttons = OnScreenKeyboard.userControl.row1.Children;
            for (int i = 2; i < 14; i++)
            {
                row1Buttons[i].Visibility = visibilityNumbers;
                row1Buttons[i + 12].Visibility = visibilityFunctionKeys;
            }
        }

        public void Btn_Enter_Click(object sender, RoutedEventArgs e)
        {
            inputSimulator.Keyboard.KeyPress(VirtualKeyCode.RETURN);
        }

        public void Btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            InputSimulator inputSimulator = new InputSimulator();

            foreach (KeyboardButtonData b in ButtonGenerator.buttonData)
            {
                if (b.isToggable)
                {
                    VirtualKeyCode keyCode = DataFetcher.GetKeyCodeByName(b.keyCode);
                    inputSimulator.Keyboard.KeyUp(keyCode);
                }
            }

            Environment.Exit(0);
        }
    }
}
