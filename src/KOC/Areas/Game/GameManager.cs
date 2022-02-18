namespace Appalachia.Prototype.KOC.Areas.Game
{
    public abstract class GameManager<TManager, TMetadata> : AreaManager<TManager, TMetadata>, IGameManager
        where TManager : GameManager<TManager, TMetadata>
        where TMetadata : GameMetadata<TManager, TMetadata>
    {
        /// <inheritdoc />
        protected override void OnActivation()
        {
            using (_PRF_Activate.Auto())
            {
                Context.Log.Info(nameof(OnActivation), this);
            }
        }

        /// <inheritdoc />
        protected override void OnDeactivation()
        {
            using (_PRF_Deactivate.Auto())
            {
                Context.Log.Info(nameof(OnDeactivation), this);
            }
        }

        /// <inheritdoc />
        protected override void ResetArea()
        {
            using (_PRF_ResetArea.Auto())
            {
                Context.Log.Info(nameof(ResetArea), this);
            }
        }

        #region IGameManager Members

        /// <inheritdoc />
        public override ApplicationArea Area => ApplicationArea.Game;

        /// <inheritdoc />
        public override ApplicationArea ParentArea => ApplicationArea.None;

        #endregion
    }
}
