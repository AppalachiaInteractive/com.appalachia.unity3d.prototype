namespace Appalachia.Prototype.KOC.Application.Features.Widgets
{
    public interface IApplicationWidgetMetadata
    {
    }

    public interface IApplicationWidgetMetadata<T> : IApplicationWidgetMetadata
        where T : IApplicationWidget
    {
    }

    public interface IApplicationWidgetMetadata<T, TMetadata> : IApplicationWidgetMetadata<T>
        where T : IApplicationWidget<T, TMetadata>
        where TMetadata : IApplicationWidgetMetadata<T, TMetadata>
    {
    }
}
