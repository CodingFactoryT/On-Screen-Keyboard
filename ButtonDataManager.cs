using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace On_Screen_Keyboard
{
    public class ButtonDataManager
    {
        private static string buttonDataFilePath = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Resources\ButtonData.xlsx");
        public static List<KeyboardButtonData> buttonData = DataFetcher.getButtonDataFromExcel(buttonDataFilePath);

        public static void SetButtonContent(ButtonMode buttonMode)
        {
            foreach(KeyboardButtonData button in buttonData)
            {
                Button buttonControl = LogicalTreeHelper.FindLogicalNode(OnScreenKeyboard.userControl, button.name) as Button;
                switch (buttonMode)
                {
                    case ButtonMode.NormalMode:
                        buttonControl.Content = button.normalMode;
                        break;
                    case ButtonMode.ShiftMode:
                        buttonControl.Content = button.shiftMode;
                        break;
                    case ButtonMode.AltGrMode:
                        buttonControl.Content = button.altGrMode;
                        break;
                    case ButtonMode.AltGrShiftMode:
                        buttonControl.Content = button.altGrShiftMode;
                        break;
                }
            }
        }
        
        public static ButtonMode GetCurrentButtonMode()
        {
            ButtonMode buttonMode = ButtonMode.NormalMode;

            if (Keyboard.IsKeyDown(Key.LeftShift)) buttonMode = InvertButtonMode(buttonMode);

            if (Keyboard.IsKeyDown(Key.RightAlt) ||
               (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.LeftCtrl)))
            {
                if (Keyboard.IsKeyDown(Key.LeftShift))
                    buttonMode = ButtonMode.AltGrShiftMode;
                else
                    buttonMode = ButtonMode.AltGrMode;
            }
            if (Keyboard.IsKeyDown(Key.CapsLock))
            {
                if (Keyboard.IsKeyDown(Key.RightAlt))
                    buttonMode = ButtonMode.AltGrShiftMode;
                else
                    buttonMode = InvertButtonMode(buttonMode);
            }
            return buttonMode;
        }

        private static ButtonMode InvertButtonMode(ButtonMode buttonMode)
        {
            if (buttonMode == ButtonMode.NormalMode) return ButtonMode.ShiftMode;
            if (buttonMode == ButtonMode.ShiftMode) return ButtonMode.NormalMode;
            return ButtonMode.AltGrMode;
        }
    }
}
