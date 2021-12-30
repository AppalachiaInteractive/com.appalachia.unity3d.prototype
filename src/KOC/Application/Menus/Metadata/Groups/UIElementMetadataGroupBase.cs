using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Application.Components.UI;
using Appalachia.Prototype.KOC.Application.Menus.Metadata.Elements;
using Appalachia.Utility.Async;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Menus.Metadata.Groups
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
            using (_PRF_Initialize.Auto())
            {
                await base.Initialize(initializer);

                if (elements == null)
                {
                    elements = Array.Empty<TE>();
                }

                for (var i = 0; i < elements.Length; i++)
                {
                    await initializer.Do(
                        this,
                        i.ToString(),
                        () =>
                        {
                            var element = elements[i];

                            InitializeElement(element);

                            MarkAsModified();
                        }
                    );
                }
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(UIElementMetadataGroupBase<TG, TE, TC>) + ".";

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        private static readonly ProfilerMarker _PRF_Awake = new ProfilerMarker(_PRF_PFX + nameof(Awake));
        private static readonly ProfilerMarker _PRF_Apply = new ProfilerMarker(_PRF_PFX + nameof(Apply));

        #endregion
    }
}
