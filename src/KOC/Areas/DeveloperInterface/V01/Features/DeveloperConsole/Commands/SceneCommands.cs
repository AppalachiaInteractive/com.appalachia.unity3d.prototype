using System;
using Appalachia.CI.Constants;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DeveloperConsole.Commands
{
    public class SceneCommands
    {
        #region Static Fields and Autoproperties

        [NonSerialized] private static AppaContext _context;

        #endregion

        private static AppaContext Context
        {
            get
            {
                if (_context == null)
                {
                    _context = new AppaContext(typeof(SceneCommands));
                }

                return _context;
            }
        }

        [DeveloperConsole("scene.load", "Loads a scene")]
        [Preserve]
        public static void LoadScene(string sceneName)
        {
            LoadSceneInternal(sceneName, false, LoadSceneMode.Single);
        }

        [DeveloperConsole("scene.load", "Loads a scene")]
        [Preserve]
        public static void LoadScene(string sceneName, LoadSceneMode mode)
        {
            LoadSceneInternal(sceneName, false, mode);
        }

        [DeveloperConsole("scene.loadasync", "Loads a scene asynchronously")]
        [Preserve]
        public static void LoadSceneAsync(string sceneName)
        {
            LoadSceneInternal(sceneName, true, LoadSceneMode.Single);
        }

        [DeveloperConsole("scene.loadasync", "Loads a scene asynchronously")]
        [Preserve]
        public static void LoadSceneAsync(string sceneName, LoadSceneMode mode)
        {
            LoadSceneInternal(sceneName, true, mode);
        }

        [DeveloperConsole("scene.restart", "Restarts the active scene")]
        [Preserve]
        public static void RestartScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }

        [DeveloperConsole("scene.unload", "Unloads a scene")]
        [Preserve]
        public static void UnloadScene(string sceneName)
        {
            SceneManager.UnloadSceneAsync(sceneName);
        }

        private static void LoadSceneInternal(string sceneName, bool isAsync, LoadSceneMode mode)
        {
            if (SceneManager.GetSceneByName(sceneName).IsValid())
            {
                Context.Log.Info("Scene " + sceneName + " is already loaded");
                return;
            }

            if (isAsync)
            {
                SceneManager.LoadSceneAsync(sceneName, mode);
            }
            else
            {
                SceneManager.LoadScene(sceneName, mode);
            }
        }
    }
}
