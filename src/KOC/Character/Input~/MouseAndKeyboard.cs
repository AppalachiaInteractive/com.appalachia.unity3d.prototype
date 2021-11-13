namespace Appalachia.KOC.Character
{
    [BOTDPlayerInputMapping(BOTDPlayerInputMapping.MouseAndKeyboard)]
    internal class MouseAndKeyboard : IBOTDPlayerInputMapping
    {
        public string jump => "Jump";

        public string lookX => "Mouse X";

        public string lookY => "Mouse Y";
        public string moveX => "Horizontal";

        public string moveY => "Vertical";

        public string run => "Fire3";
    }
}
