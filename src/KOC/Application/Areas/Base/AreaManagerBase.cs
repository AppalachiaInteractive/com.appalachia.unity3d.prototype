using Appalachia.Core.Behaviours;
using Appalachia.Core.Scriptables;
using Appalachia.Prototype.KOC.Application.Components;
using Appalachia.Prototype.KOC.Application.Screens.Fading;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Logging;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Application.Areas.Base
{
    [ExecuteAlways]
    public abstract class AreaManagerBase<T, TM> : SingletonAppalachiaBehaviour<T>
        where T : AreaManagerBase<T, TM>
        where TM : AreaMetadataBase<TM>
    {
        #region Profiling

        private const string _PRF_PFX = nameof(AreaManagerBase<T, TM>) + ".";
        private static readonly ProfilerMarker _PRF_OnAwake = new ProfilerMarker(_PRF_PFX + nameof(OnAwake));
        private static readonly ProfilerMarker _PRF_OnReset = new ProfilerMarker(_PRF_PFX + nameof(OnReset));

        private static readonly ProfilerMarker
            _PRF_OnEnable = new ProfilerMarker(_PRF_PFX + nameof(OnEnable));

        private static readonly ProfilerMarker _PRF_OnDisable =
            new ProfilerMarker(_PRF_PFX + nameof(OnDisable));

        #endregion

        #region Fields

        public TM metadata;

        protected ApplicationLifetimeComponents lifetimeComponents;

        protected CanvasFadeManager menuCanvasFader;

        #endregion

        protected abstract string AreaName { get; }

        #region Event Functions

        private void OnEnable()
        {
            using (_PRF_OnEnable.Auto())
            {
                AppaLog.Context.Area.Info(nameof(OnEnable));
                Activate();
            }
        }

        private void OnDisable()
        {
            using (_PRF_OnDisable.Auto())
            {
                AppaLog.Context.Area.Info(nameof(OnDisable));
                Deactivate();
            }
        }

        #endregion

        public abstract void Activate();
        public abstract void Deactivate();

        protected abstract void OnReset(bool resetting);

        protected virtual void OnceAwake()
        {
        }

        protected override void OnAwake()
        {
            using (_PRF_OnAwake.Auto())
            {
                AppaLog.Context.Area.Info(nameof(OnAwake));
                
                metadata = SingletonAppalachiaObject<TM>.instance;

                var canvasGroupName = $"CanvasGroup_{AreaName}";
                var canvasObjName = $"Canvas_{AreaName}";

                var canvasGroupObj = gameObject.transform.Find(canvasGroupName)?.gameObject;

                if (menuCanvasFader == null)
                {
                    menuCanvasFader = gameObject.GetComponentInChildren<CanvasFadeManager>();
                }

                if ((canvasGroupObj == null) && (menuCanvasFader != null))
                {
                    canvasGroupObj = menuCanvasFader.gameObject;
                }

                if (menuCanvasFader == null)
                {
                    if (canvasGroupObj == null)
                    {
                        canvasGroupObj = new GameObject(canvasGroupName).SetParentTo(gameObject);

                        canvasGroupObj.AddComponent<CanvasGroup>();

                        menuCanvasFader = canvasGroupObj.AddComponent<CanvasFadeManager>();
                    }

                    canvasGroupObj.name = canvasGroupName;
                }

                var canvas = canvasGroupObj.GetComponentInChildren<Canvas>();

                GameObject canvasObj = null;

                if (canvas != null)
                {
                    canvasObj = canvas.gameObject;
                }
                else
                {
                    canvasObj = canvasGroupObj.FindChild(canvasObjName);
                }

                if (canvasObj == null)
                {
                    canvasObj = canvasGroupObj.AddChild(canvasObjName);
                    canvas = canvasObj.AddComponent<Canvas>();
                }

                CanvasScaler canvasScaler = null;
                GraphicRaycaster graphicRaycaster = null;

                canvasObj.GetOrCreateComponent(ref canvas);
                canvasObj.GetOrCreateComponent(ref graphicRaycaster);

                lifetimeComponents = ApplicationLifetimeComponents.instance;

                name = typeof(T).Name;

                OnceAwake();
            }
        }

        protected override void OnReset()
        {
            using (_PRF_OnReset.Auto())
            {
                AppaLog.Context.Area.Info(nameof(OnReset));
                
                base.OnReset();

                menuCanvasFader = null;
                lifetimeComponents = null;
                metadata = null;

                OnReset(true);
            }
        }
    }
}
