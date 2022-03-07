using System;
using Appalachia.CI.Constants;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Objects.Sets2;
using Appalachia.UI.Controls.Sets2.Buttons.Button;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Sets2
{
    /// <inheritdoc />
    [Serializable]
    [SmartLabelChildren]
    public sealed class StatusBarSubwidgetComponentSet : BaseButtonComponentSet<StatusBarSubwidgetComponentSet
        , StatusBarSubwidgetComponentSetData>
    {
        /// <inheritdoc />
        public override ComponentSetSorting DesiredComponentOrder => ComponentSetSorting.CloseToLast;

        /// <summary>
        ///     Defines the name of the component set.
        /// </summary>
        public override string ComponentSetNamePrefix => APPASTR.Status_Bar_Subwidget;

        protected override bool IsUI => true;
    }
}
