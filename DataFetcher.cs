using Ganss.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using WindowsInput.Native;

namespace On_Screen_Keyboard
{
    public class DataFetcher
    {
        public static Brush GetBrushByName(string name)
        {
            return (Brush) new BrushConverter().ConvertFrom(ConfigurationManager.AppSettings[name]);
        }

        public static List<KeyboardButtonData> getButtonDataFromExcel(string filePath)
        {
            List<KeyboardButtonData> keyboardButtons = new ExcelMapper(filePath).Fetch<KeyboardButtonData>().ToList();
            return keyboardButtons;
        }

        public static VirtualKeyCode GetKeyCodeByName(string name)
        {
            Enum.TryParse(name, out VirtualKeyCode keyCode);
            return keyCode;
        }

        public static object GetControlByName(string name)
        {
            return LogicalTreeHelper.FindLogicalNode(OnScreenKeyboard.userControl, name);
        }
    }
}
