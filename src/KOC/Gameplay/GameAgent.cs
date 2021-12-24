using System;
using System.Collections.Generic;
using System.Diagnostics;
using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Async;
using Appalachia.Utility.Execution;
using Appalachia.Utility.Strings;

namespace Appalachia.Prototype.KOC.Gameplay
{
    public abstract class GameAgent : AppalachiaBehaviour<GameAgent>
    {
        #region Constants and Static Readonly

        private static readonly Dictionary<string, GameAgent> lookup = new();
        private static readonly HashSet<GameAgent> agents = new();

        #endregion

        

        public string agentIdentifier;


        #region Event Functions

        protected override void Awake()
        {
            base.Awake();
            
            agents.Add(this);

            if (!string.IsNullOrEmpty(agentIdentifier))
            {
                try
                {
                    lookup.Add(agentIdentifier, this);
                }
                catch (ArgumentException e)
                {
                    if (!lookup[agentIdentifier])
                    {
                        lookup[agentIdentifier] = this;
                    }
                    else
                    {
                        Context.Log.Error(e);
                    }
                }
            }
        }

        protected override async AppaTask WhenDestroyed()
        {
            await base.WhenDestroyed();
            
            agents.Remove(this);

            if (!string.IsNullOrEmpty(agentIdentifier))
            {
                lookup.Remove(agentIdentifier);
            }
        }

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

        public static GameAgent Find(string id)
        {
            GameAgent agent = null;
            if (!string.IsNullOrEmpty(id))
            {
#if UNITY_EDITOR
                if (!AppalachiaApplication.IsPlayingOrWillPlay)
                {
                    foreach (var i in FindObjectsOfType<GameAgent>())
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

        public static T Find<T>(string id)
            where T : GameAgent
        {
            return (T) Find(id);
        }

        public static IEnumerable<GameAgent> GetAgents()
        {
            return agents;
        }

        public static HashSet<GameAgent>.Enumerator GetEnumerator()
        {
            return agents.GetEnumerator();
        }

        [DebuggerStepThrough]
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(agentIdentifier))
            {
                return ZString.Format("{0} '{1}'", base.ToString(), agentIdentifier);
            }

            return base.ToString();
        }
    }
} // Gameplay
