namespace Appalachia.KOC.Character
{
    [BOTDPlayerInputMapping(BOTDPlayerInputMapping.XboxForMac)]
    internal class XboxForMac : IBOTDPlayerInputMapping
    {
        public string jump => "XboxMacButtonA";

        public string lookX => "XboxMacRStickX";

        public string lookY => "XboxMacRStickY";
        public string moveX => "XboxMacLStickX";

        public string moveY => "XboxMacLStickY";

        public string run => "XboxMacLTrigger";
    }
}
