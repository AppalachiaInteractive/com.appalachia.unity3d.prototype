namespace Appalachia.Prototype.KOC.Areas
{
    public partial interface IAreaManager
    {
        ApplicationArea Area { get; }
        ApplicationArea ParentArea { get; }
        bool HasParent { get; }
        string AreaName { get; }
        void Activate();
        void Deactivate();
    }
}
