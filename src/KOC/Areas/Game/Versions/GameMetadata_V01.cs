using System;

namespace Appalachia.Prototype.KOC.Areas.Game.Versions
{
    
    [Serializable]
    public class GameMetadata_V01 : GameMetadata<GameManager_V01, GameMetadata_V01>
    {
        /// <inheritdoc />
        public override AreaVersion Version => AreaVersion.V01;
    }
}
