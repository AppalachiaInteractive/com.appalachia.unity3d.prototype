using System;
using Appalachia.CI.Integration.Attributes;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Input;
using Appalachia.Utility.Async;
using Appalachia.Utility.Constants;
using Appalachia.Utility.Execution;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Strings;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Behaviours
{
    [SmartLabelChildren]
    [Serializable, ExecuteAlways]
    [InspectorIcon(Brand.GlobalSingletonAppalachiaBehaviour.Icon)]
    public abstract class GlobalSingletonAppalachiaBehaviour<T> : SingletonAppalachiaBehaviour<T>
        where T : GlobalSingletonAppalachiaBehaviour<T>
    {
        #region Constants and Static Readonly

        public const HideFlags MAIN_SCENE_FLAGS = HideFlags.None;

        public const HideFlags SUBSCENE_FLAGS = HideFlags.DontSaveInBuild | HideFlags.DontSaveInEditor;
        public const string MAIN_SCENE = "KeepersOfCreation";

        #endregion

        #region Fields and Autoproperties

        [FoldoutGroup("Events & Input")]
        private KOCInputActions _inputActions;

        #endregion

        public bool RunningAsSubScene => gameObject.scene.name != MAIN_SCENE;

        protected KOCInputActions InputActions
        {
            get
            {
                if (_inputActions == null)
                {
                    _inputActions = new KOCInputActions();
                }

                return _inputActions;
            }
        }

        #region Event Functions

        protected virtual void Update()
        {
            using (_PRF_Update.Auto())
            {
                if (ShouldSkipUpdate)
                {
                    return;
                }

                ValidateSceneLocation();
            }
        }

        #endregion

        protected override string GetBackgroundColor()
        {
            return Brand.GlobalSingletonAppalachiaBehaviour.Banner;
        }

        protected override string GetFallbackTitle()
        {
            return Brand.GlobalSingletonAppalachiaBehaviour.Fallback;
        }

        protected override string GetTitle()
        {
            return Brand.GlobalSingletonAppalachiaBehaviour.Text;
        }

        protected override string GetTitleColor()
        {
            return Brand.GlobalSingletonAppalachiaBehaviour.Color;
        }

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                var obj = gameObject;

                if (obj == null)
                {
                    return;
                }

                ValidateSceneLocation();

                if (RunningAsSubScene)
                {
                    if (!AppalachiaApplication.IsPlaying)
                    {
                        Context.Log.Info(
                            ZString.Format(
                                "[{0}] is running in the scene but will not be saved.",
                                obj.name.FormatNameForLogging()
                            ),
                            this
                        );
                    }

                    if (obj.hideFlags != SUBSCENE_FLAGS)
                    {
                        obj.hideFlags = SUBSCENE_FLAGS;
                    }
                }
                else
                {
                    if (obj.hideFlags != SUBSCENE_FLAGS)
                    {
                        obj.hideFlags = MAIN_SCENE_FLAGS;
                    }
                }
            }
        }

        private void ValidateSceneLocation()
        {
            using (_PRF_ValidateSceneLocation.Auto())
            {
                if (AppalachiaApplication.IsPlaying)
                {
                    return;
                }

                var obj = gameObject;

                if (obj == null)
                {
                    return;
                }

                if (!obj.scene.isLoaded)
                {
                    Context.Log.Info(
                        ZString.Format(
                            "[{0}] is moving to a loaded scene but will not be saved.",
                            obj.name.FormatNameForLogging()
                        ),
                        this
                    );

                    obj.MoveToLoadedScene();
                }
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_ValidateSceneLocation =
            new ProfilerMarker(_PRF_PFX + nameof(ValidateSceneLocation));

        #endregion
    }
}
