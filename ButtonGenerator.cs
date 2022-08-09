using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;
using System;
using WindowsInput;
using WindowsInput.Native;
using System.Windows.Media;
using System.IO;
using System.Configuration;

namespace On_Screen_Keyboard
{
    public class ButtonGenerator
    {
        InputSimulator inputSimulator = new InputSimulator();

        public void Generate()
        {
            ExcelToKeyboardButton excelConverter = new ExcelToKeyboardButton();
            List<KeyboardButton> buttons = excelConverter.Convert(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Resources\ButtonData.xlsx"));

            for (int i = 0; i < 14; i++)
                OnScreenKeyboard.userControl.row1.Children.Add(GenerateButton(buttons[i], Visibility.Visible));
            for (int i = 15; i < 28; i++)
                OnScreenKeyboard.userControl.row2.Children.Add(GenerateButton(buttons[i], Visibility.Visible));
            for (int i = 28; i < 41; i++)
                OnScreenKeyboard.userControl.row3.Children.Add(GenerateButton(buttons[i], Visibility.Visible));
            for (int i = 41; i < 56; i++)
                OnScreenKeyboard.userControl.row4.Children.Add(GenerateButton(buttons[i], Visibility.Visible));
            for (int i = 56; i < 66; i++)
                OnScreenKeyboard.userControl.row5.Children.Add(GenerateButton(buttons[i], Visibility.Visible));
            for (int i = 66; i < 78; i++)
                OnScreenKeyboard.userControl.row1.Children.Add(GenerateButton(buttons[i], Visibility.Collapsed));

            OnScreenKeyboard.userControl.row1.Children.Add(GenerateButton(buttons[14], Visibility.Visible)); //Back Button
        }

        private Button GenerateButton(KeyboardButton buttonData, Visibility visibility)
        {
            Button keyboardButton = new Button();
            keyboardButton.Name = buttonData.name;
            keyboardButton.Content = buttonData.normalMode;
            keyboardButton.Style = OnScreenKeyboard.userControl.FindResource("KeyboardButtonStyle") as Style;
            keyboardButton.Click += SimulateButtonPress;
            keyboardButton.Visibility = visibility;
            keyboardButton.Background = (Brush)new BrushConverter().ConvertFrom(ConfigurationManager.AppSettings["KeyboardButtonColor"]);

            if (buttonData.specialWidth != 0)
            {
                keyboardButton.Width = buttonData.specialWidth;
            }

            if (buttonData.isToggable)
            {
                keyboardButton.Click += HandleToggableButtons;
            }

            return keyboardButton;

            void SimulateButtonPress(object sender, RoutedEventArgs e)
            {
                inputSimulator.Keyboard.KeyPress(getKeyCode(buttonData.keyCode));
            }

            void HandleToggableButtons(object sender, RoutedEventArgs e)
            {
                Button btn = (Button)sender;

                buttonData.isToggled = !buttonData.isToggled;
                VirtualKeyCode keyCode = getKeyCode(buttonData.keyCode);

                if (buttonData.isToggled)
                {
                    btn.Background = Brushes.LightSteelBlue;
                    inputSimulator.Keyboard.KeyDown(keyCode);
                }
                else
                {
                    btn.Background = Brushes.LightGray;
                    inputSimulator.Keyboard.KeyUp(keyCode);
                }
            }
        }

        private VirtualKeyCode getKeyCode(string keyCode_str)
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