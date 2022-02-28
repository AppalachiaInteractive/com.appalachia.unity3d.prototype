namespace Appalachia.Prototype.KOC.Application.Features.Services.Model
{
    /// <summary>
    /// Execution modes for describing how a service is executed.
    /// </summary>
    public enum ServiceExecutionMode
    {
        /// <summary>
        /// The service's execution should not changed.
        /// </summary>
        DoNotModify = 0,

        /// <summary>
        /// The service should be executing.
        /// </summary>
        Enabled = 1,
        
        /// <summary>
        /// The service should not be executing.
        /// </summary>
        Disabled = 2,
    }
}
