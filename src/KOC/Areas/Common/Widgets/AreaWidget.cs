using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Application.Widgets;

namespace Appalachia.Prototype.KOC.Areas.Common.Widgets
{
    [CallStaticConstructorInEditor]
    public abstract class
        AreaWidget<TWidget, TWidgetMetadata, TAreaManager, TAreaMetadata> :
            ApplicationWidget<TWidget, TWidgetMetadata>,
            IAreaWidget
        where TWidget : AreaWidget<TWidget, TWidgetMetadata, TAreaManager, TAreaMetadata>
        where TWidgetMetadata : AreaWidgetMetadata<TWidget, TWidgetMetadata, TAreaManager, TAreaMetadata>
        where TAreaManager : AreaManager<TAreaManager, TAreaMetadata>
        where TAreaMetadata : AreaMetadata<TAreaManager, TAreaMetadata>
    {
        static AreaWidget()
        {
            AreaManager<TAreaManager, TAreaMetadata>.InstanceAvailable += i => areaManager = i;
        }

        #region Static Fields and Autoproperties

        protected static TAreaManager areaManager;

        #endregion

        protected override bool NestUnderApplicationManager => false;
    }
}
