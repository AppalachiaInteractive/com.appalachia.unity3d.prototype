namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01
{
    public class DeveloperInterfaceMetadata_V01 : DeveloperInterfaceMetadata<DeveloperInterfaceManager_V01,
        DeveloperInterfaceMetadata_V01>
    {
        #region Fields and Autoproperties

        public bool startHidden;

        #endregion

        public override AreaVersion Version => AreaVersion.V01;
    }
}
