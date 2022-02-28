#if UNITY_EDITOR
using Appalachia.CI.Integration.Assets;
using Appalachia.UI.Controls.Animation.Extensions;
using Unity.Profiling;
using UnityEditor.Animations;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Animation
{
    public partial class CursorAnimationController
    {
        public static RuntimeAnimatorController Default(string name)
        {
            using (_PRF_Default.Auto())
            {
                var savePath =
                    AssetDatabaseManager
                       .GetSaveDirectoryForOwnedAsset<ApplicationManager, AnimatorController>(
                            $"{name}_{nameof(CursorAnimationController)}.controller"
                        );

                var uniqueSavePath = AssetDatabaseManager.GenerateUniqueAssetPath(savePath);
                var controller = AnimatorController.CreateAnimatorControllerAtPath(uniqueSavePath);

                controller.AddLayer(Layers.Base_Layer);
                controller.AddLayer(Layers.Speed_Layer);

                controller.AddParameter(Parameters.Disabled, AnimatorControllerParameterType.Bool);
                controller.AddParameter(Parameters.Hovering, AnimatorControllerParameterType.Bool);
                controller.AddParameter(Parameters.Pressed,  AnimatorControllerParameterType.Bool);
                controller.AddParameter(Parameters.Show,     AnimatorControllerParameterType.Trigger);
                controller.AddParameter(Parameters.Hide,     AnimatorControllerParameterType.Trigger);

                var hidden = controller.CreateStateWithClip(Motions.Hidden,     0);
                var hide = controller.CreateStateWithClip(Motions.Hide,         0);
                var show = controller.CreateStateWithClip(Motions.Show,         0);
                var normal = controller.CreateStateWithClip(Motions.Normal,     0);
                var disabled = controller.CreateStateWithClip(Motions.Disabled, 0);
                var hovering = controller.CreateStateWithClip(Motions.Hovering, 0);
                var pressed = controller.CreateStateWithClip(Motions.Pressed,   0);

                var speed = controller.CreateStateWithClip(Motions.Speed, 1);

                controller.AddTransition(hidden, show, true)
                          .Configure(t => t.hasFixedDuration = true)
                          .Configure(t => t.duration = 0f)
                          .Configure(t => t.hasExitTime = false)
                          .AllowedIf(Parameters.Show);

                controller.AddTransition(show, normal, true)
                          .Configure(t => t.hasFixedDuration = false)
                          .Configure(t => t.duration = 1f)
                          .Configure(t => t.hasExitTime = true)
                          .Configure(t => t.exitTime = 1f);

                controller.AddTransition(normal, hide, true)
                          .Configure(t => t.hasFixedDuration = true)
                          .Configure(t => t.duration = 0f)
                          .Configure(t => t.hasExitTime = false)
                          .AllowedIf(Parameters.Hide);

                controller.AddTransition(hide, hidden, true)
                          .Configure(t => t.hasFixedDuration = false)
                          .Configure(t => t.duration = 1f)
                          .Configure(t => t.hasExitTime = true)
                          .Configure(t => t.exitTime = 1f);

                controller.AddTransition(normal, disabled, true)
                          .Configure(t => t.hasFixedDuration = true)
                          .Configure(t => t.duration = 0.5f)
                          .Configure(t => t.hasExitTime = false)
                          .AllowedIf(Parameters.Disabled);

                controller.AddTransition(hovering, disabled, true)
                          .Configure(t => t.hasFixedDuration = true)
                          .Configure(t => t.duration = 0.5f)
                          .Configure(t => t.hasExitTime = false)
                          .AllowedIf(Parameters.Disabled);

                controller.AddTransition(pressed, disabled, true)
                          .Configure(t => t.hasFixedDuration = true)
                          .Configure(t => t.duration = 0.5f)
                          .Configure(t => t.hasExitTime = false)
                          .AllowedIf(Parameters.Disabled);

                controller.AddTransition(normal, pressed, true)
                          .Configure(t => t.hasFixedDuration = true)
                          .Configure(t => t.duration = 0.25f)
                          .Configure(t => t.hasExitTime = false)
                          .AllowedIf(Parameters.Pressed)
                          .AllowedIfNot(Parameters.Disabled);

                controller.AddTransition(disabled, pressed, true)
                          .Configure(t => t.hasFixedDuration = true)
                          .Configure(t => t.duration = 0.25f)
                          .Configure(t => t.hasExitTime = false)
                          .AllowedIf(Parameters.Pressed)
                          .AllowedIfNot(Parameters.Disabled);

                controller.AddTransition(hovering, pressed, true)
                          .Configure(t => t.hasFixedDuration = true)
                          .Configure(t => t.duration = 0.25f)
                          .Configure(t => t.hasExitTime = false)
                          .AllowedIf(Parameters.Pressed)
                          .AllowedIfNot(Parameters.Disabled);

                controller.AddTransition(normal, hovering, true)
                          .Configure(t => t.hasFixedDuration = true)
                          .Configure(t => t.duration = 0.5f)
                          .Configure(t => t.hasExitTime = false)
                          .AllowedIf(Parameters.Hovering)
                          .AllowedIfNot(Parameters.Disabled, Parameters.Pressed);

                controller.AddTransition(disabled, hovering, true)
                          .Configure(t => t.hasFixedDuration = true)
                          .Configure(t => t.duration = 0.5f)
                          .Configure(t => t.hasExitTime = false)
                          .AllowedIf(Parameters.Hovering)
                          .AllowedIfNot(Parameters.Disabled, Parameters.Pressed);

                controller.AddTransition(pressed, hovering, true)
                          .Configure(t => t.hasFixedDuration = true)
                          .Configure(t => t.duration = 0.5f)
                          .Configure(t => t.hasExitTime = false)
                          .AllowedIf(Parameters.Hovering)
                          .AllowedIfNot(Parameters.Disabled, Parameters.Pressed);

                controller.AddTransition(disabled, normal, true)
                          .Configure(t => t.hasFixedDuration = true)
                          .Configure(t => t.duration = 0.5f)
                          .Configure(t => t.hasExitTime = false)
                          .AllowedIfNot(Parameters.Pressed, Parameters.Disabled, Parameters.Hovering);

                controller.AddTransition(hovering, normal, true)
                          .Configure(t => t.hasFixedDuration = true)
                          .Configure(t => t.duration = 0.5f)
                          .Configure(t => t.hasExitTime = false)
                          .AllowedIfNot(Parameters.Pressed, Parameters.Disabled, Parameters.Hovering);

                controller.AddTransition(pressed, normal, true)
                          .Configure(t => t.hasFixedDuration = true)
                          .Configure(t => t.duration = 0.5f)
                          .Configure(t => t.hasExitTime = false)
                          .AllowedIfNot(Parameters.Pressed, Parameters.Disabled, Parameters.Hovering);

                return controller;
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_Default = new ProfilerMarker(_PRF_PFX + nameof(Default));

        #endregion
    }
}

#endif
