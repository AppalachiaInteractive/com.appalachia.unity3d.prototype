using Appalachia.CI.Constants;
using Appalachia.UI.Controls.Sets.Canvases.RootCanvas;
using Appalachia.UI.Controls.Sets.Images.Background;
using Appalachia.Utility.Events;
using Appalachia.Utility.Events.Extensions;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Lifetime
{
    public partial class LifetimeComponentManager
    {
        #region Constants and Static Readonly

        public const int REFERENCE_RESOLUTION_HEIGHT = 1080;
        public const int REFERENCE_RESOLUTION_WIDTH = 1920;

        public static readonly Vector2 REFERENCE_RESOLUTION = new Vector2(
            REFERENCE_RESOLUTION_WIDTH,
            REFERENCE_RESOLUTION_HEIGHT
        );

        public static readonly Vector2 SCREEN_CENTER = REFERENCE_RESOLUTION * .5F;

        private const string GROUP_UI = GROUP_BASE + PARENT_NAME_UI;

        private const string PARENT_NAME_UI = "UI";

        #endregion

        #region Fields and Autoproperties

        [FoldoutGroup(GROUP_UI)]
        [SerializeField]
        private RootCanvasComponentSet _rootCanvas;

        [FoldoutGroup(GROUP_UI)]
        [SerializeField]
        private BackgroundComponentSet _rootBackground;

        [FoldoutGroup(GROUP_UI), SerializeField]
        private GameObject _uiObject;

        public AppaEvent<RootCanvasComponentSet>.Data RootCanvasComponentSetReady;

        #endregion

        public GameObject UIObject => _uiObject;

        public Rect ScaledCanvasBounds => ScaledCanvas.rect;

        public RectTransform ScaledCanvas => _rootCanvas.ScaledCanvas.transform as RectTransform;
        public RootCanvasComponentSet RootCanvasComponentSet => _rootCanvas;

        public Vector2 GetPositionInScaledCanvas(Vector2 position)
        {
            using (_PRF_GetPositionInScaledCanvas.Auto())
            {
                var isInBounds = ScreenPointToScaledCanvasPoint(position, out var localPosition);

                return localPosition;
            }
        }

        public bool ScreenPointToLocalPointInRectangle(
            RectTransform rect,
            Vector2 screenPoint,
            Camera cam,
            out Vector2 localPoint)
        {
            using (_PRF_ScreenPointToLocalPointInRectangle.Auto())
            {
                localPoint = Vector2.zero;
                Vector3 worldPoint;
                if (!ScreenPointToWorldPointInRectangle(rect, screenPoint, cam, out worldPoint))
                {
                    return false;
                }

                localPoint = rect.InverseTransformPoint(worldPoint);
                return true;
            }
        }

        public Ray ScreenPointToRay(Camera cam, Vector2 screenPos)
        {
            using (_PRF_ScreenPointToRay.Auto())
            {
                if (cam != null)
                {
                    return cam.ScreenPointToRay(screenPos);
                }

                Vector3 origin = screenPos;
                origin.z -= 100f;
                return new Ray(origin, Vector3.forward);
            }
        }

        public bool ScreenPointToScaledCanvasPoint(Vector2 screenPoint, out Vector2 localPoint)
        {
            using (_PRF_ScreenPointToScaledCanvasPoint.Auto())
            {
                return ScreenPointToLocalPointInRectangle(ScaledCanvas, screenPoint, null, out localPoint);
            }
        }

        public bool ScreenPointToWorldPointInRectangle(
            RectTransform rect,
            Vector2 screenPoint,
            Camera cam,
            out Vector3 worldPoint)
        {
            using (_PRF_ScreenPointToWorldPointInRectangle.Auto())
            {
                worldPoint = Vector2.zero;
                var ray = ScreenPointToRay(cam, screenPoint);
                var plane = new Plane(rect.rotation * Vector3.back, rect.position);
                var enter = 0.0f;
                if ((Vector3.Dot(Vector3.Normalize(rect.position - ray.origin), plane.normal) != 0.0) &&
                    !plane.Raycast(ray, out enter))
                {
                    return false;
                }

                worldPoint = ray.GetPoint(enter);
                return true;
            }
        }

        private void InitializeUI()
        {
            using (_PRF_InitializeUI.Auto())
            {
                gameObject.GetOrAddChild(ref _uiObject, PARENT_NAME_UI, false);

                RootCanvasComponentSetData.RefreshAndApply(
                    ref _lifetimeMetadata.rootCanvas,
                    ref _rootCanvas,
                    _uiObject,
                    APPASTR.ObjectNames.Master_Canvas
                );

                BackgroundComponentSetData.RefreshAndApply(
                    ref _lifetimeMetadata.rootBackground,
                    ref _rootBackground,
                    _rootCanvas.ScaledCanvas.gameObject,
                    APPASTR.ObjectNames.Master_Canvas
                );

                RootCanvasComponentSetReady.RaiseEvent(RootCanvasComponentSet);
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_GetPositionInScaledCanvas =
            new ProfilerMarker(_PRF_PFX + nameof(GetPositionInScaledCanvas));

        private static readonly ProfilerMarker _PRF_InitializeUI =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeUI));

        private static readonly ProfilerMarker _PRF_ScreenPointToLocalPointInRectangle =
            new ProfilerMarker(_PRF_PFX + nameof(ScreenPointToLocalPointInRectangle));

        private static readonly ProfilerMarker _PRF_ScreenPointToRay =
            new ProfilerMarker(_PRF_PFX + nameof(ScreenPointToRay));

        private static readonly ProfilerMarker _PRF_ScreenPointToScaledCanvasPoint =
            new ProfilerMarker(_PRF_PFX + nameof(ScreenPointToScaledCanvasPoint));

        private static readonly ProfilerMarker _PRF_ScreenPointToWorldPointInRectangle =
            new ProfilerMarker(_PRF_PFX + nameof(ScreenPointToWorldPointInRectangle));

        #endregion
    }
}
