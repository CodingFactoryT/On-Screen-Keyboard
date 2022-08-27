using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace On_Screen_Keyboard
{
    public class ButtonContentManager
    {
        public List<KeyboardButtonData> buttonData = new List<KeyboardButtonData>();
        public ButtonContentManager()
        {
            ExcelToKeyboardButton excelConverter = new ExcelToKeyboardButton();
            buttonData = excelConverter.Convert(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Resources\ButtonData.xlsx"));
        }

        public void SetButtonContent(ButtonMode buttonMode)
        {
            foreach(var button in buttonData)
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
        
        public ButtonMode GetCurrentButtonMode()
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

        private ButtonMode InvertButtonMode(ButtonMode buttonMode)
        {
            if (buttonMode == ButtonMode.NormalMode) return ButtonMode.ShiftMode;
            if (buttonMode == ButtonMode.ShiftMode) return ButtonMode.NormalMode;
            return ButtonMode.AltGrMode;
        }
    }
}
