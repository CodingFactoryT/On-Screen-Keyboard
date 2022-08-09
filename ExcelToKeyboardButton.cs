using Ganss.Excel;
using System.Collections.Generic;
using System.Linq;

namespace On_Screen_Keyboard
{
    internal class ExcelToKeyboardButton
    {
        public bool _isFirstRowHeader { get; set; }
        public List<KeyboardButton> Convert(string filePath)
        {
            List<KeyboardButton> keyboardButtons = new ExcelMapper(filePath).Fetch<KeyboardButton>().ToList();
            if (_isFirstRowHeader) keyboardButtons.RemoveAt(0);
            return keyboardButtons;
        }
    }
}