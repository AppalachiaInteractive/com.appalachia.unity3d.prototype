using System;
using Appalachia.Core.Objects.Initialization;
using Appalachia.UI.Controls.Animation.Controllers;
using Appalachia.UI.Controls.Animation.Controllers.Core;
using Appalachia.Utility.Async;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Animation
{
    [Serializable]
    public partial class CursorAnimationController : AnimationControllerAPI<CursorAnimationController>
    {
        public CursorAnimationController()
        {
        }

        public CursorAnimationController(Animator animator, Object owner) : base(animator, owner)
        {
        }

        #region Fields and Autoproperties

        [SerializeField] public AnimatorBoolProperty Disabled;
        [SerializeField] public AnimatorBoolProperty Hovering;
        [SerializeField] public AnimatorBoolProperty Pressed;
        [SerializeField] public AnimatorTriggerProperty Show;
        [SerializeField] public AnimatorTriggerProperty Hide;
        [SerializeField] public AnimatorLayerWeight BaseLayer;
        [SerializeField] public AnimatorLayerWeight SpeedLayer;

        #endregion

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                Disabled = Bool(Parameters.Disabled, Animator);
                Hovering = Bool(Parameters.Hovering, Animator);
                Pressed = Bool(Parameters.Pressed,   Animator);
                Show = Trigger(Parameters.Show, Animator);
                Hide = Trigger(Parameters.Hide, Animator);
                BaseLayer = LayerWeight(Layers.Base_Layer,   Animator);
                SpeedLayer = LayerWeight(Layers.Speed_Layer, Animator);
            }
        }

        #region Nested type: Layers

        public static class Layers
        {
            #region Constants and Static Readonly

            public const string Base_Layer = "Base Layer";
            public const string Speed_Layer = "Speed Layer";

            #endregion
        }

        #endregion

        #region Nested type: Motions

        public static class Motions
        {
            #region Constants and Static Readonly

            public const string Disabled = "Disabled";
            public const string Hidden = "Hidden";
            public const string Hide = "Hide";
            public const string Hovering = "Hovering";
            public const string Normal = "Normal";
            public const string Pressed = "Pressed";
            public const string Show = "Show";
            public const string Speed = "Speed";

            #endregion

            #region Static Fields and Autoproperties

            public static string[] Layer0 = { Hidden, Show, Hide, Normal, Pressed, Disabled, Hovering, };

            public static string[] Layer1 = { Speed, };

            public static string[][] Layers = { Layer0, Layer1 };

            #endregion
        }

        #endregion

        #region Nested type: Parameters

        public static class Parameters
        {
            #region Constants and Static Readonly

            public const string Disabled = "Disabled";
            public const string Hide = "Hide";
            public const string Hovering = "Hovering";
            public const string Pressed = "Pressed";
            public const string Show = "Show";

            #endregion
        }

        #endregion
    }
}
