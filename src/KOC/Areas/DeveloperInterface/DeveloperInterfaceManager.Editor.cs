#if UNITY_EDITOR
using Appalachia.CI.Constants;
using Appalachia.Core.ControlModel.Extensions;
using Appalachia.Core.Objects.Initialization;
using Appalachia.UI.Functionality.Design.Controls.Template;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface
{
    public abstract partial class DeveloperInterfaceManager<TManager, TMetadata>
    {
        #region Fields and Autoproperties

        [SerializeField]
        [FoldoutGroup(APPASTR.Components + "/" + APPASTR.Unscaled_Templates)]
        protected TemplateControl unscaledTemplates;

        #endregion

        private void InitializeEditor(Initializer initializer)
        {
            using (_PRF_InitializeEditor.Auto())
            {
                TemplateControl.Refresh(
                    ref unscaledTemplates,
                    unscaledCanvas.GameObject,
                    nameof(unscaledTemplates)
                );

                areaMetadata.unscaledTemplates.Apply(unscaledTemplates);
            }
        }
    }
}

#endif
