using UnityEngine.Playables;

namespace Appalachia.Prototype.KOC.Application.Extensions
{
    public static class PlayableDirectorExtensions
    {
        public static void PauseTimeline(this PlayableDirector director)
        {
            director.playableGraph.GetRootPlayable(0).SetSpeed(0d);
        }

        //Called by the InputManager
        public static void ResumeTimeline(this PlayableDirector director)
        {
            director.playableGraph.GetRootPlayable(0).SetSpeed(1d);
        }
    }
}
