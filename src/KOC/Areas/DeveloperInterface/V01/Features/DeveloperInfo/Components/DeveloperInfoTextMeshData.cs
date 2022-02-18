using System;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DeveloperInfo.Models;
using Appalachia.UI.Controls.Components.Text;
using Appalachia.UI.Core.Components.Data;
using Object = UnityEngine.Object;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DeveloperInfo.Components
{
    [Serializable]
    public sealed class DeveloperInfoTextMeshData : CalculatedTextMeshData<DeveloperInfoTextMesh,
        DeveloperInfoTextMeshData, DeveloperInfoType>
    {
        public DeveloperInfoTextMeshData()
        {
        }

        public DeveloperInfoTextMeshData(Object owner) : base(owner)
        {
        }

        #region Fields and Autoproperties

        public LayoutElementData layoutElementData;

        #endregion

        /// <inheritdoc />
        protected override void ApplyToComponentSet(DeveloperInfoTextMesh target)
        {
            using (_PRF_ApplyToComponentSet.Auto())
            {
                base.ApplyToComponentSet(target);

                target.layoutElement.enabled = true;

                LayoutElementData.RefreshAndUpdateComponent(
                    ref layoutElementData,
                    Owner,
                    target.layoutElement,
                    (data, comp) =>
                    {
                        data.minHeight.Disabled = target.LineCount > 1;

                        if (data.minHeight.Disabled)
                        {
                            var extraLineHeight = data.minHeight * .7f;
                            var extraLines = target.LineCount - 1;

                            var lineAdditive = extraLineHeight * extraLines;
                            comp.minHeight = data.minHeight + lineAdditive;
                        }
                    },
                    (data, comp) => { data.minHeight.Disabled = false; }
                );
            }
        }

        /// <inheritdoc />
        protected override DeveloperInfoType GetInitialEnum()
        {
            return DeveloperInfoType.MachineName;
        }

        /// <inheritdoc />
        protected override void SubscribeResponsiveChildren()
        {
            using (_PRF_SubscribeResponsiveChildren.Auto())
            {
                base.SubscribeResponsiveChildren();

                layoutElementData.Changed.Event += OnChanged;
            }
        }
    }
}
