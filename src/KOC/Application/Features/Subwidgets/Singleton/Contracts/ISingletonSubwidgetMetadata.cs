namespace Appalachia.Prototype.KOC.Application.Features.Subwidgets.Singleton.Contracts
{
    public interface ISingletonSubwidgetMetadata<in TSubwidget, TSubwidgetMetadata>        
        where TSubwidget : class, ISingletonSubwidget<TSubwidget, TSubwidgetMetadata>
        where TSubwidgetMetadata : class, ISingletonSubwidgetMetadata<TSubwidget, TSubwidgetMetadata>
    {
        void SubscribeResponsiveComponents(TSubwidget functionality);
    }
}
