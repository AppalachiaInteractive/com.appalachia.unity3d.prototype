using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.ControlModel.Controls.Model;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Controls.Complex.Base;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Controls.Complex
{
    /// <inheritdoc />
    [Serializable]
    [SmartLabelChildren]
    public sealed class ComplexCursorControl : BaseComplexCursorControl<ComplexCursorControl,
        ComplexCursorControlConfig>
    {
        /// <inheritdoc />
        public override ControlSorting DesiredComponentOrder => ControlSorting.Anywhere;

        
    }
}
