namespace Appalachia.Prototype.KOC.Application.Functionality
{
    public interface IApplicationFunctionality
    {
        void UpdateFunctionality();

        /*void SubscribeToOtherFunctionalities();*/
        void UnsubscribeFromAllFunctionalities();
    }
}
