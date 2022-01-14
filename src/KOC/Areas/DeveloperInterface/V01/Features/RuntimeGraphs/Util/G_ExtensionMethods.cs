using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RuntimeGraphs.Util
{
    public static class G_ExtensionMethods
    {
        /// <summary>
        ///     Functions as the SetActive function in the GameObject class, but for a list of them.
        /// </summary>
        /// <param name="gameObjects">
        ///     List of GameObjects.
        /// </param>
        /// <param name="active">
        ///     Wether to turn them on or off.
        /// </param>
        public static List<GameObject> SetAllActive(this List<GameObject> gameObjects, bool active)
        {
            foreach (var gameObj in gameObjects)
            {
                gameObj.SetActive(active);
            }

            return gameObjects;
        }

        public static List<Image> SetAllActive(this List<Image> images, bool active)
        {
            foreach (var image in images)
            {
                image.gameObject.SetActive(active);
            }

            return images;
        }

        public static List<Image> SetOneActive(this List<Image> images, int active)
        {
            for (var i = 0; i < images.Count; i++)
            {
                images[i].gameObject.SetActive(i == active);
            }

            return images;
        }
    }
}
