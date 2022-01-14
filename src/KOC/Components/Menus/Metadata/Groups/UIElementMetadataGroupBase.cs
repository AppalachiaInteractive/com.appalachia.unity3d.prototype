using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Components.Menus.Metadata.Elements;
using Appalachia.Prototype.KOC.Components.UI;
using Appalachia.Utility.Async;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Components.Menus.Metadata.Groups
{
    [SmartLabelChildren]
    public abstract class UIElementMetadataGroupBase<TG, TE, TC> : AppalachiaObject<TG>
        where TG : UIElementMetadataGroupBase<TG, TE, TC>
        where TE : UIElementMetadataBase<TE, TC>
        where TC : IComponentSet
    {
        #region Fields and Autoproperties

        public TE[] elements;

        #endregion

        public void Apply(GameObject parent, string baseName, TC[] components)
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

                    element.Apply(parent, baseName, component);
                }
            }
        }

        protected abstract void InitializeElement(TE element);

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                if (elements == null)
                {
                    elements = Array.Empty<TE>();
                }
            }

            for (var i = 0; i < elements.Length; i++)
            {
                initializer.Do(
                    this,
                    i.ToString(),
                    () =>
                    {
                        using (_PRF_Initialize.Auto())
                        {
                            var element = elements[i];

                            InitializeElement(element);

                            MarkAsModified();
                        }
                    }
                );
            }
        }

        #region Profiling


        

        private static readonly ProfilerMarker _PRF_Awake = new ProfilerMarker(_PRF_PFX + nameof(Awake));
        private static readonly ProfilerMarker _PRF_Apply = new ProfilerMarker(_PRF_PFX + nameof(Apply));

        #endregion
    }
}
