namespace On_Screen_Keyboard
{
    internal class KeyboardButton
    {
        public string name { set; get; }
        public string keyCode { set; get; }
        public string normalMode { set; get; }
        public string shiftMode { set; get; }
        public string altGrMode { set; get; }
        public int specialWidth { set; get; }
        public bool isToggable { set; get; }

        public bool isToggled = false; //is only used if isToggable is set to true
    }
}