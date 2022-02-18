using System;
using Appalachia.CI.Constants;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Objects.Sets;

namespace Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Sets.Simple
{
    /// <inheritdoc />
    [Serializable]
    [SmartLabelChildren]
    public sealed class SimpleCursorComponentSet : BaseSimpleCursorComponentSet<SimpleCursorComponentSet,
        SimpleCursorComponentSetData, ISimpleCursorComponentSetData>
    {
        /// <inheritdoc />
        public override ComponentSetSorting DesiredComponentOrder => ComponentSetSorting.Anywhere;

        /// <summary>
        ///     Defines the name of the component set.
        /// </summary>
        public override string ComponentSetName => APPASTR.Simple_Cursor;

        protected override bool IsUI => true;
    }
}
