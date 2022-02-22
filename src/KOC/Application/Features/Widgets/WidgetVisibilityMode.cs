namespace Appalachia.Prototype.KOC.Application.Features.Widgets
{
    /// <summary>
    /// Visibility modes for describing how a widget is shown.
    /// </summary>
    public enum WidgetVisibilityMode
    {
        /// <summary>
        /// The widget's visibility has not been specified.
        /// </summary>
        DoNotModify = 0,

        /// <summary>
        /// The widget should be visible.
        /// </summary>
        Visible = 1,
        
        /// <summary>
        /// The widget should not be visible.
        /// </summary>
        NotVisible = 2,
    }
}
