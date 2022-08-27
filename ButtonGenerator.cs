using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;
using System;
using WindowsInput;
using WindowsInput.Native;
using System.Windows.Media;
using System.IO;
using System.Configuration;
using System.Diagnostics;
using System.Windows.Input;

namespace On_Screen_Keyboard
{
    public class ButtonGenerator
    {
        InputSimulator inputSimulator = new InputSimulator();
        private static ButtonContentManager buttonContentManager = new ButtonContentManager();
        public static List<KeyboardButtonData> buttons = buttonContentManager.buttonData;

        public void Generate()
        {
            for (int i = 0; i < 14; i++)
                OnScreenKeyboard.userControl.row1.Children.Add(GetSingleButton(buttons[i], Visibility.Visible));
            for (int i = 15; i < 28; i++)
                OnScreenKeyboard.userControl.row2.Children.Add(GetSingleButton(buttons[i], Visibility.Visible));
            for (int i = 28; i < 41; i++)
                OnScreenKeyboard.userControl.row3.Children.Add(GetSingleButton(buttons[i], Visibility.Visible));
            for (int i = 41; i < 56; i++)
                OnScreenKeyboard.userControl.row4.Children.Add(GetSingleButton(buttons[i], Visibility.Visible));
            for (int i = 56; i < 66; i++)
                OnScreenKeyboard.userControl.row5.Children.Add(GetSingleButton(buttons[i], Visibility.Visible));
            for (int i = 66; i < 78; i++)
                OnScreenKeyboard.userControl.row1.Children.Add(GetSingleButton(buttons[i], Visibility.Collapsed));

            OnScreenKeyboard.userControl.row1.Children.Add(GetSingleButton(buttons[14], Visibility.Visible)); //Back Button    
        }

        private Button GetSingleButton(KeyboardButtonData buttonData, Visibility visibility)
        {
            Button keyboardButton = new Button();
            keyboardButton.Name = buttonData.name;
            keyboardButton.Content = buttonData.normalMode;
            keyboardButton.Style = OnScreenKeyboard.userControl.FindResource("KeyboardButtonStyle") as Style;
            keyboardButton.Visibility = visibility;
            keyboardButton.Background = (Brush)new BrushConverter().ConvertFrom(ConfigurationManager.AppSettings["KeyboardButtonColor"]);
            
            if (buttonData.specialWidth != 0)
            {
                keyboardButton.Width = buttonData.specialWidth;
            }

            if (buttonData.isToggable)
                keyboardButton.Click += HandleToggableButtons;
            else
                keyboardButton.Click += SimulateButtonPress;

      
            return keyboardButton;

            void SimulateButtonPress(object sender, RoutedEventArgs e)
            {
                inputSimulator.Keyboard.KeyPress(getKeyCode(buttonData.keyCode));
                if (Keyboard.IsKeyDown(Key.LeftShift))
                {
                    Button leftShiftButton = LogicalTreeHelper.FindLogicalNode(OnScreenKeyboard.userControl, "Btn_LSHIFT") as Button;
                    leftShiftButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                }
            }

            void HandleToggableButtons(object sender, RoutedEventArgs e)
            {
                Button btn = (Button)sender;
                if(e != null)
                {
                    buttonData.isToggled = !buttonData.isToggled;
                }
                VirtualKeyCode keyCode = getKeyCode(buttonData.keyCode);

                if (buttonData.isToggled)
                {
                    btn.Background = (Brush)new BrushConverter().ConvertFrom(ConfigurationManager.AppSettings["ToggleKeyboardButtonColor"]);
                    Console.WriteLine(btn.Background);
                    inputSimulator.Keyboard.KeyDown(keyCode);
                }
                else
                {
                    btn.Background = (Brush)new BrushConverter().ConvertFrom(ConfigurationManager.AppSettings["KeyboardButtonColor"]);
                    inputSimulator.Keyboard.KeyUp(keyCode);
                }
                if (btn.Name.Equals("Btn_LSHIFT") && e != null) SimulateToggableButtonPress("Btn_RSHIFT");
                if (btn.Name.Equals("Btn_RSHIFT") && e != null) SimulateToggableButtonPress("Btn_LSHIFT");
                if (btn.Name.Equals("Btn_LCTRL") && e != null) SimulateToggableButtonPress("Btn_RCTRL");
                if (btn.Name.Equals("Btn_RCTRL") && e != null) SimulateToggableButtonPress("Btn_LCTRL");
                Debug.WriteLine(btn.Name);
                buttonContentManager.SetButtonContent(buttonContentManager.GetCurrentButtonMode());

                if (btn.Name.Equals("Btn_FNCT")) ToggleRow1Visibility(buttonData.isToggled);
            }

            void SimulateToggableButtonPress(string buttonName)
            {
                Button button = LogicalTreeHelper.FindLogicalNode(OnScreenKeyboard.userControl, buttonName) as Button;
                HandleToggableButtons(button, null);
            }
        }

        public static VirtualKeyCode getKeyCode(string keyCode_str)
        {
            Enum.TryParse(keyCode_str, out VirtualKeyCode keyCode);
            return keyCode;
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
    }
}