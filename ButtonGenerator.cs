using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;

namespace On_Screen_Keyboard
{
    public class ButtonGenerator
    {
        public static List<KeyboardButtonData> buttonData = ButtonDataManager.buttonData;

        public void Generate()
        {
            for (int i = 0; i < 14; i++)
                OnScreenKeyboard.userControl.row1.Children.Add(GetSingleButton(buttonData[i], Visibility.Visible));
            for (int i = 15; i < 28; i++)
                OnScreenKeyboard.userControl.row2.Children.Add(GetSingleButton(buttonData[i], Visibility.Visible));
            for (int i = 28; i < 41; i++)
                OnScreenKeyboard.userControl.row3.Children.Add(GetSingleButton(buttonData[i], Visibility.Visible));
            for (int i = 41; i < 56; i++)
                OnScreenKeyboard.userControl.row4.Children.Add(GetSingleButton(buttonData[i], Visibility.Visible));
            for (int i = 56; i < 66; i++)
                OnScreenKeyboard.userControl.row5.Children.Add(GetSingleButton(buttonData[i], Visibility.Visible));
            for (int i = 66; i < 78; i++)
                OnScreenKeyboard.userControl.row1.Children.Add(GetSingleButton(buttonData[i], Visibility.Collapsed));

            OnScreenKeyboard.userControl.row1.Children.Add(GetSingleButton(buttonData[14], Visibility.Visible)); //Back Button    
        }

        private Button GetSingleButton(KeyboardButtonData buttonData, Visibility visibility)
        {
            Button keyboardButton = new Button();
            keyboardButton.Name = buttonData.name;
            keyboardButton.Content = buttonData.normalMode;
            keyboardButton.Style = OnScreenKeyboard.userControl.FindResource("KeyboardButtonStyle") as Style;
            keyboardButton.Visibility = visibility;
            keyboardButton.Background = DataFetcher.GetBrushByName("KeyboardButtonColor");

            if (buttonData.specialWidth != 0)
            {
                keyboardButton.Width = buttonData.specialWidth;
            }

            ButtonClickHandler buttonClickHandler = new ButtonClickHandler(buttonData);

            if (buttonData.isToggable)
                keyboardButton.Click += buttonClickHandler.ToggableButton_Click;
            else
                keyboardButton.Click += buttonClickHandler.NonToggableButton_Click;

            return keyboardButton;
        }
    }
}