using System;
using System.Collections.Generic;
using System.Diagnostics;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Async;
using Appalachia.Utility.Execution;
using Appalachia.Utility.Strings;

// ReSharper disable UnusedParameter.Global

namespace Appalachia.Prototype.KOC.Features.Gameplay
{
    public abstract class GameAgent<T> : AppalachiaBehaviour<T>
        where T : GameAgent<T>
    {
        #region Constants and Static Readonly

        private static readonly Dictionary<string, T> lookup = new();
        private static readonly HashSet<T> agents = new();

        #endregion

        #region Fields and Autoproperties

        public string agentIdentifier;

        #endregion

        public virtual void OnAfterSpawnPlayer(SpawnPoint point, bool reset)
        {
        }

        public virtual void OnBeforeSpawnPlayer(bool reset)
        {
        }

        public virtual void OnSpawn(SpawnPoint spawnPoint, bool reset)
        {
        }

        public static T Find(string id)
        {
            T agent = null;
            if (!string.IsNullOrEmpty(id))
            {
#if UNITY_EDITOR
                if (!AppalachiaApplication.IsPlayingOrWillPlay)
                {
                    foreach (var i in FindObjectsOfType<T>())
                    {
                        if (i.agentIdentifier == id)
                        {
                            return i;
                        }
                    }
                }
#endif
                lookup.TryGetValue(id, out agent);
            }

            return agent;
        }

        public static IEnumerable<T> GetAgents()
        {
            return agents;
        }

        public static HashSet<T>.Enumerator GetEnumerator()
        {
            return agents.GetEnumerator();
        }

        /// <inheritdoc />
        [DebuggerStepThrough]
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(agentIdentifier))
            {
                return ZString.Format("{0} '{1}'", base.ToString(), agentIdentifier);
            }

            return base.ToString();
        }

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            agents.Add(this as T);

            if (!string.IsNullOrEmpty(agentIdentifier))
            {
                try
                {
                    lookup.Add(agentIdentifier, this as T);
                }
                catch (ArgumentException e)
                {
                    if (!lookup[agentIdentifier])
                    {
                        lookup[agentIdentifier] = this as T;
                    }
                    else
                    {
                        Context.Log.Error(e);
                    }
                }
            }
        }

        /// <inheritdoc />
        protected override async AppaTask WhenDestroyed()
        {
            await base.WhenDestroyed();

            agents.Remove(this as T);

            if (!string.IsNullOrEmpty(agentIdentifier))
            {
                lookup.Remove(agentIdentifier);
            }
        }
    }
} // Gameplay
