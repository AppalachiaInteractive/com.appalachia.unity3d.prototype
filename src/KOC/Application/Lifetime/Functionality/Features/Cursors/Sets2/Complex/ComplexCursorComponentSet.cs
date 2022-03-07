using System;
using Appalachia.CI.Constants;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Objects.Sets2;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Sets2.Complex
{
    /// <inheritdoc />
    [Serializable]
    [SmartLabelChildren]
    public sealed class ComplexCursorComponentSet : BaseComplexCursorComponentSet<ComplexCursorComponentSet,
        ComplexCursorComponentSetData, IComplexCursorComponentSetData>
    {
        /// <inheritdoc />
        public override ComponentSetSorting DesiredComponentOrder => ComponentSetSorting.Anywhere;

        /// <summary>
        ///     Defines the name of the component set.
        /// </summary>
        public override string ComponentSetNamePrefix => APPASTR.Complex_Cursor;

        protected override bool IsUI => true;
    }
}
