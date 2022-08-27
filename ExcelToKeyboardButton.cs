using Ganss.Excel;
using System.Collections.Generic;
using System.Linq;

namespace On_Screen_Keyboard
{
    internal class ExcelToKeyboardButton
    {
        public bool _isFirstRowHeader { get; set; }
        public List<KeyboardButtonData> Convert(string filePath)
        {
            List<KeyboardButtonData> keyboardButtons = new ExcelMapper(filePath).Fetch<KeyboardButtonData>().ToList();
            if (_isFirstRowHeader) keyboardButtons.RemoveAt(0);
            return keyboardButtons;
        }
    }
}