namespace Appalachia.KOC.Character
{
    [BOTDPlayerInputMapping(BOTDPlayerInputMapping.PlayStationForMac)]
    internal class PlayStationForMac : IBOTDPlayerInputMapping
    {
        public string jump => "PSMacCross";

        public string lookX => "PSMacRStickX";

        public string lookY => "PSMacRStickY";
        public string moveX => "PSMacLStickX";

        public string moveY => "PSMacLStickY";

        public string run => "PSMacLTrigger";
    }
}
