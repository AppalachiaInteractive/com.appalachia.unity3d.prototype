namespace Appalachia.Prototype.KOC.Application.Services.Broadcast
{
    public abstract class
        BroadcastServiceMetadata<TService, TServiceMetadata, TServiceArgs> : ApplicationServiceMetadata<
            TService, TServiceMetadata>
        where TService : BroadcastService<TService, TServiceMetadata, TServiceArgs>
        where TServiceMetadata : BroadcastServiceMetadata<TService, TServiceMetadata, TServiceArgs>
        where TServiceArgs : BroadcastArgs
    {
    }
}
