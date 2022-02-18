using System;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Async;
using UnityEngine;
using UnityEngine.Playables;

namespace Appalachia.Prototype.KOC.Playables.TMPTextSwitcher
{
    [Serializable]
    public class TMPTextSwitcherBehaviour : AppalachiaPlayable<TMPTextSwitcherBehaviour>
    {
        #region Fields and Autoproperties

        public Color color = Color.white;
        public float fontSize = 14;
        public string text;

        #endregion

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await AppaTask.CompletedTask;
        }

        /// <inheritdoc />
        protected override void OnPause(Playable playable, FrameData info)
        {
        }

        /// <inheritdoc />
        protected override void OnPlay(Playable playable, FrameData info)
        {
        }

        /// <inheritdoc />
        protected override void Update(Playable playable, FrameData info, object playerData)
        {
        }

        /// <inheritdoc />
        protected override void WhenDestroyed(Playable playable)
        {
        }

        /// <inheritdoc />
        protected override void WhenStarted(Playable playable)
        {
        }

        /// <inheritdoc />
        protected override void WhenStopped(Playable playable)
        {
        }
    }
}
