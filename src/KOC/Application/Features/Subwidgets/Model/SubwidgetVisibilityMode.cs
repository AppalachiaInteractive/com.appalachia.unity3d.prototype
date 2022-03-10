namespace Appalachia.Prototype.KOC.Application.Features.Subwidgets.Model
{
    /// <summary>
    ///     Visibility modes for describing how a subwidget is shown.
    /// </summary>
    public enum SubwidgetVisibilityMode
    {
        /// <summary>
        ///     The subwidget's visibility has not been specified.
        /// </summary>
        DoNotModify = 0,

        /// <summary>
        ///     The subwidget should be visible.
        /// </summary>
        Visible = 1,

        /// <summary>
        ///     The subwidget should not be visible.
        /// </summary>
        NotVisible = 2,
    }
}
