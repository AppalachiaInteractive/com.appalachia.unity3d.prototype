namespace Appalachia.KOC.Character
{
    [BOTDPlayerInputMapping(BOTDPlayerInputMapping.Vive)]
    internal class Vive : IBOTDPlayerInputMapping
    {
        public string jump => "ViveLThumb";

        public string lookX => "ViveRThumbX";

        public string lookY => "ViveRThumbY";
        public string moveX => "ViveLThumbX";

        public string moveY => "ViveLThumbY";

        public string run => "ViveLTrigger";
    }
}
