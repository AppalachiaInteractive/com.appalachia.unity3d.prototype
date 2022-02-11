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

        protected override void ExecuteComponentDataApplication(DeveloperInfoTextMesh target)
        {
            using (_PRF_ExecuteComponentDataApplication.Auto())
            {
                base.ExecuteComponentDataApplication(target);

                var textMesh = target.textMeshValue;

                LayoutElementData.UpdateComponent(
                    ref layoutElementData,
                    target.layoutElement,
                    Owner,
                    (data, comp) =>
                    {
                        if ((textMesh.firstOverflowCharacterIndex > -1) && data.minHeight.Overriding)
                        {
                            data.minHeight.Disabled = true;
                        }
                    },
                    (data, comp) =>
                    {
                        if ((textMesh.firstOverflowCharacterIndex > -1) && data.minHeight.Disabled)
                        {
                            data.minHeight.Disabled = false;
                            comp.minHeight = data.minHeight * 1.5f;
                        }
                    }
                );
            }
        }

        protected override DeveloperInfoType GetInitialEnum()
        {
            return DeveloperInfoType.MachineName;
        }

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
