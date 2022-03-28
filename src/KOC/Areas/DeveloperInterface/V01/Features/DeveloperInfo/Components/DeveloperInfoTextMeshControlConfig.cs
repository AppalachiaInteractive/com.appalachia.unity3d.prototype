using System;
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
                layoutElement.SuspendFieldApplication = true;
                base.OnApply(target);
                layoutElement.SuspendFieldApplication = false;

                target.layoutElement.enabled = true;

                layoutElement.Apply(
                    target.layoutElement,
                    (config, _) =>
                    {
                        config.minHeight.Disabled = target.LineCount > 1;

                        if (config.minHeight.Disabled)
                        {
                            var extraLineHeight = config.minHeight * .7f;
                            var extraLines = target.LineCount - 1;

                            var lineAdditive = extraLineHeight * extraLines;
                            config.minHeight.OverrideValue(config.minHeight + lineAdditive);
                        }
                    },
                    (config, _) => { config.minHeight.Disabled = false; }
                );
            }
        }
    }
}
