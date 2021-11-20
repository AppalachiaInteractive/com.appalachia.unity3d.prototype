namespace Appalachia.Prototype.KOC.Application.Areas
{
    public interface IAreaManager
    {
        ApplicationArea Area { get; }
        ApplicationArea ParentArea { get; }
        bool HasParent { get; }
        string AreaName { get; }
        void Activate();
        void Deactivate();
    }
}
