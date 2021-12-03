using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Scriptables;
using Appalachia.Prototype.KOC.Application.Components.UI;
using Appalachia.Prototype.KOC.Application.Menus.Metadata.Elements;
using Appalachia.Prototype.KOC.Application.Scriptables;
using Appalachia.Utility.Extensions;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Application.Menus.Metadata.Groups
{
    [SmartLabelChildren]
    public abstract class UIElementMetadataGroupBase<TE, TC> : AppalachiaApplicationObject
        where TE : UIElementMetadataBase<TC>
        where TC : IComponentSet
    {
        #region Fields and Autoproperties

        public TE[] elements;

        #endregion

        #region Event Functions

        protected override void Awake()
        {
            using (_PRF_Awake.Auto())
            {
                if (elements == null)
                {
                    elements = Array.Empty<TE>();
                }

                for (var i = 0; i < elements.Length; i++)
                {
                    initializer.Initialize(
                        this,
                        i.ToString(),
                        () =>
                        {
                            var element = elements[i];

                            InitializeElement(element);
                            
                            this.MarkAsModified();
                        }
                    );
                }
            }
        }

        protected abstract void InitializeElement(TE element);

        #endregion

        public void Apply(TC[] components)
        {
            using (_PRF_Apply.Auto())
            {
                if (components.Length != elements.Length)
                {
                    throw new NotSupportedException("Component count must match element count!");
                }

                for (var index = 0; index < components.Length; index++)
                {
                    var component = components[index];
                    var element = elements[index];

                    element.Apply(component);
                }
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(UIElementMetadataGroupBase<TE, TC>) + ".";
        private static readonly ProfilerMarker _PRF_Awake = new ProfilerMarker(_PRF_PFX + nameof(Awake));
        private static readonly ProfilerMarker _PRF_Apply = new ProfilerMarker(_PRF_PFX + nameof(Apply));

        #endregion
    }
}
