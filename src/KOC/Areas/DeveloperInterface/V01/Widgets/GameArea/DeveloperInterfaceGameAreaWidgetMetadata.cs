using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.Common.Widgets;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.GameArea
{
    public sealed class DeveloperInterfaceGameAreaWidgetMetadata : AreaWidgetMetadata<
        DeveloperInterfaceGameAreaWidget, DeveloperInterfaceGameAreaWidgetMetadata,
        DeveloperInterfaceManager_V01, DeveloperInterfaceMetadata_V01>
    {
        #region Fields and Autoproperties

        [BoxGroup(APPASTR.GroupNames.Logo)]
        [OnValueChanged(nameof(InvokeSettingsChanged))]
        public Color logoColor;

        [BoxGroup(APPASTR.GroupNames.Logo)]
        [OnValueChanged(nameof(InvokeSettingsChanged))]
        [PropertyRange(0.1f, 1f)]
        public float logoSize;

        [BoxGroup(APPASTR.GroupNames.Logo)]
        [OnValueChanged(nameof(InvokeSettingsChanged))]
        public Sprite logo;

        #endregion

        public override void Apply(DeveloperInterfaceGameAreaWidget functionality)
        {
            using (_PRF_Apply.Auto())
            {
                base.Apply(functionality);
            }
        }

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            initializer.Do(this, nameof(maintainAspectRatio), () => maintainAspectRatio = true);
            initializer.Do(this, nameof(logoSize),            () => logoSize = .5f);
        }
    }
}
