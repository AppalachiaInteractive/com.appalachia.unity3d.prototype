using System;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DeveloperInfo.Models;
using Appalachia.UI.ControlModel.ComponentGroups.Layout;
using Appalachia.UI.Functionality.Text.Controls.Calculated;
using Object = UnityEngine.Object;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DeveloperInfo.Components
{
    [Serializable]
    public sealed class DeveloperInfoTextMeshControlConfig : CalculatedTextMeshControlConfig<
        DeveloperInfoTextMeshControl, DeveloperInfoTextMeshControlConfig, DeveloperInfoType>
    {
        public DeveloperInfoTextMeshControlConfig()
        {
        }

        public DeveloperInfoTextMeshControlConfig(Object owner) : base(owner)
        {
        }

        #region Fields and Autoproperties

        public LayoutElementComponentGroupConfig layoutElement;

        #endregion

        /// <inheritdoc />
        protected override DeveloperInfoType GetInitialEnum()
        {
            return DeveloperInfoType.MachineName;
        }

        /// <inheritdoc />
        protected override void OnApply(DeveloperInfoTextMeshControl target)
        {
            using (_PRF_OnApply.Auto())
            {
                base.OnApply(target);

                target.layoutElement.enabled = true;

                LayoutElementComponentGroupConfig.Apply(
                    ref layoutElement,
                    Owner,
                    target.layoutElement,
                    (data, comp) =>
                    {
                        data.layoutElement.minHeight.Disabled = target.LineCount > 1;

                        if (data.layoutElement.minHeight.Disabled)
                        {
                            var extraLineHeight = data.layoutElement.minHeight * .7f;
                            var extraLines = target.LineCount - 1;

                            var lineAdditive = extraLineHeight * extraLines;
                            data.layoutElement.minHeight.OverrideValue(data.layoutElement.minHeight + lineAdditive);
                        }
                    },
                    (data, comp) => { data.layoutElement.minHeight.Disabled = false; }
                );
            }
        }

        protected override void OnInitializeFields(Initializer initializer, Object owner)
        {
            using (_PRF_OnInitializeFields.Auto())
            {
                base.OnInitializeFields(initializer, owner);

                layoutElement.Changed.Event += OnChanged;
            }
        }
    }
}
