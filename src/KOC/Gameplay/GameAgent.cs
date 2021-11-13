using System;
using System.Collections.Generic;
using System.Diagnostics;
using Appalachia.Utility.Logging;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Gameplay
{
    public abstract class GameAgent : MonoBehaviour
    {
        private static readonly Dictionary<string, GameAgent> lookup = new();
        private static readonly HashSet<GameAgent> agents = new();

        public string agentIdentifier;

        public virtual void OnAfterSpawnPlayer(SpawnPoint point, bool reset)
        {
        }

        public virtual void OnBeforeSpawnPlayer(bool reset)
        {
        }

        public virtual void OnSpawn(SpawnPoint spawnPoint, bool reset)
        {
        }

        [DebuggerStepThrough] public override string ToString()
        {
            if (!string.IsNullOrEmpty(agentIdentifier))
            {
                return $"{base.ToString()} '{agentIdentifier}'";
            }

            return base.ToString();
        }

        protected void Awake()
        {
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
                        AppaLog.Exception(e);
                    }
                }
            }
        }

        protected void OnDestroy()
        {
            agents.Remove(this);

            if (!string.IsNullOrEmpty(agentIdentifier))
            {
                lookup.Remove(agentIdentifier);
            }
        }

        public static GameAgent Find(string id)
        {
            GameAgent agent = null;
            if (!string.IsNullOrEmpty(id))
            {
#if UNITY_EDITOR
                if (!UnityEngine.Application.isPlaying)
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
    }
} // Gameplay
