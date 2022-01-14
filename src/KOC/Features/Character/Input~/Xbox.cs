namespace Appalachia.KOC.Character
{
    [BOTDPlayerInputMapping(BOTDPlayerInputMapping.Xbox)]
    internal class Xbox : IBOTDPlayerInputMapping
    {
        public string jump => "XboxButtonA";

        public string lookX => "XboxRStickX";

        public string lookY => "XboxRStickY";
        public string moveX => "XboxLStickX";

        public string moveY => "XboxLStickY";

        public string run => "XboxLTrigger";
    }
}
