namespace Appalachia.KOC.Character
{
    [BOTDPlayerInputMapping(BOTDPlayerInputMapping.PlayStationForWindows)]
    internal class PlayStationForWindows : IBOTDPlayerInputMapping
    {
        public string jump => "PSWinCross";

        public string lookX => "PSWinRStickX";

        public string lookY => "PSWinRStickY";
        public string moveX => "PSWinLStickX";

        public string moveY => "PSWinLStickY";

        public string run => "PSWinLTrigger";
    }
}
