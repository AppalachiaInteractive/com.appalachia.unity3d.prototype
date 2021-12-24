using System;
using System.Linq;
using Appalachia.CI.Constants;
using Appalachia.CI.Integration.Assets;
using Appalachia.CI.Integration.Extensions;
using Appalachia.CI.Integration.FileSystem;
using Appalachia.Prototype.KOC.Application.Areas;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Strings;
using Doozy.Engine.Nody;
using Doozy.Engine.Nody.Models;
using Doozy.Engine.Nody.Nodes;
using Unity.Profiling;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Appalachia.Prototype.KOC.Application.Graphs
{
    public static class NodyGraphGenerator
    {
        [NonSerialized] private static AppaContext _context;

        private static AppaContext Context
        {
            get
            {
                if (_context == null)
                {
                    _context = new AppaContext(typeof(NodyGraphGenerator));
                }

                return _context;
            }
        }
#if UNITY_EDITOR

        public static void CheckAndCreateAnyMissingSystemNodes(this Graph graph)
        {
            using (_PRF_CheckAndCreateAnyMissingSystemNodes.Auto())
            {
                if (graph.IsSubGraph)
                {
                    CreateEnterAndExitNodes(graph);
                }
                else
                {
                    CreateStartNode(graph);
                }
            }
        }

        public static void ConnectSockets(
            this Graph graph,
            Socket outputSocket,
            Socket inputSocket,
            bool saveAssets = false)
        {
            using (_PRF_ConnectSockets.Auto())
            {
                if (outputSocket.OverrideConnection)
                {
                    outputSocket.Disconnect();
                }

                if (inputSocket.OverrideConnection)
                {
                    inputSocket.Disconnect();
                }

                var connection = new Connection(outputSocket, inputSocket);
                outputSocket.Connections.Add(connection);
                inputSocket.Connections.Add(connection);
                graph.MarkAsModifiedCustom(saveAssets);
            }
        }

        public static void CreateEnterAndExitNodes(this Graph graph)
        {
            using (_PRF_CreateEnterAndExitNodes.Auto())
            {
                const float spaceBetweenNodes = 24f;

                var startNode = graph.GetStartNode();
                var enterNode = graph.GetEnterNode();
                var exitNode = graph.GetExitNode();

                var enterNodeWasCreated = false;
                if (enterNode == null)
                {
                    enterNode = CreateNode(graph, typeof(EnterNode), Vector2.zero);
                    graph.Nodes.Add(enterNode);
                    if (!EditorUtility.IsPersistent(enterNode))
                    {
                        AssetDatabaseManager.AddObjectToAsset(enterNode, graph);
                    }

                    enterNodeWasCreated = true;
                }

                if (startNode != null)
                {
                    if (enterNodeWasCreated)
                    {
                        enterNode.SetPosition(startNode.GetPosition());
                    }

                    if (startNode.IsConnected())
                    {
                        var targetSocketId = startNode.OutputSockets[0].Connections[0].InputSocketId;
                        var targetNode =
                            graph.GetNodeById(startNode.OutputSockets[0].Connections[0].InputNodeId);
                        if (targetNode != null)
                        {
                            targetNode.GetSocketFromId(targetSocketId).DisconnectFromNode(startNode.Id);
                            ConnectSockets(
                                graph,
                                enterNode.OutputSockets[0],
                                targetNode.GetSocketFromId(targetSocketId)
                            );
                        }
                    }

                    if (graph.Nodes.Contains(startNode))
                    {
                        graph.Nodes.Remove(startNode);
                    }

                    Object.DestroyImmediate(startNode, true);
                }

                if (exitNode == null)
                {
                    exitNode = CreateNode(
                        graph,
                        typeof(ExitNode),
                        new Vector2(
                            enterNode.GetX() + (NodySettings.Instance.ExitNodeWidth * 2) + spaceBetweenNodes,
                            enterNode.GetY() + (NodySettings.Instance.NodeHeaderHeight / 2)
                        )
                    );
                    graph.Nodes.Add(exitNode);
                    if (!EditorUtility.IsPersistent(exitNode))
                    {
                        AssetDatabaseManager.AddObjectToAsset(exitNode, graph);
                    }
                }

                graph.IsSubGraph = true;
                graph.MarkAsModifiedCustom();
            }
        }

        public static T CreateGraph<T>(string path, bool createSubGraph)
            where T : Graph
        {
            using (_PRF_CreateGraph.Auto())
            {
                if (string.IsNullOrEmpty(path))
                {
                    return null;
                }

                var graph = ScriptableObject.CreateInstance<T>();
                graph.name = AppaPath.GetFileNameWithoutExtension(path);
                graph.Id = Guid.NewGuid().ToString();
                graph.SetGraphVersionAndLastModifiedTime();
                if (AssetDatabaseManager.LoadAssetAtPath<ScriptableObject>(path) != null)
                {
                    AssetDatabaseManager.MoveAssetToTrash(path);
                }

                AssetDatabaseManager.CreateAsset(graph, path);
                if (createSubGraph)
                {
                    CreateEnterAndExitNodes(graph);
                }
                else
                {
                    CreateStartNode(graph);
                }

                graph.MarkAsModifiedCustom(true);
                return graph;
            }
        }

        public static Node CreateNode(this Graph graph, Type nodeType, Vector2 nodeWorldPosition)
        {
            using (_PRF_CreateNode.Auto())
            {
                var node = ScriptableObject.CreateInstance(nodeType) as Node;
                nodeWorldPosition.x -= node.GetDefaultNodeWidth() / 2;
                nodeWorldPosition.y -= NodySettings.Instance.NodeHeaderHeight / 2;
                node.InitNode(graph, nodeWorldPosition);
                node.OnCreate();
                node.AddDefaultSockets();
                node.hideFlags = NodySettings.Instance.DefaultHideFlagsForNodes;
                node.MarkAsModified();
                return node;
            }
        }

        public static void CreateStartNode(this Graph graph)
        {
            using (_PRF_CreateStartNode.Auto())
            {
                var startNode = graph.GetStartNode();
                var enterNode = graph.GetEnterNode();
                var exitNode = graph.GetExitNode();

                var startNodeWasCreated = false;
                if (startNode == null)
                {
                    startNode = CreateNode(graph, typeof(StartNode), Vector2.zero);
                    graph.Nodes.Add(startNode);
                    if (!EditorUtility.IsPersistent(startNode))
                    {
                        AssetDatabaseManager.AddObjectToAsset(startNode, graph);
                    }

                    startNodeWasCreated = true;
                }

                if (enterNode != null)
                {
                    if (startNodeWasCreated)
                    {
                        startNode.SetPosition(enterNode.GetPosition());
                    }

                    if (enterNode.IsConnected())
                    {
                        var targetSocketId = enterNode.OutputSockets[0].Connections[0].InputSocketId;
                        var targetNode =
                            graph.GetNodeById(enterNode.OutputSockets[0].Connections[0].InputNodeId);
                        if (targetNode != null)
                        {
                            targetNode.GetSocketFromId(targetSocketId).DisconnectFromNode(enterNode.Id);
                            if (startNodeWasCreated)
                            {
                                ConnectSockets(
                                    graph,
                                    startNode.OutputSockets[0],
                                    targetNode.GetSocketFromId(targetSocketId)
                                );
                            }
                        }
                    }

                    if (graph.Nodes.Contains(enterNode))
                    {
                        graph.Nodes.Remove(enterNode);
                    }

                    enterNode.DestroySafely();
                }

                if (exitNode != null)
                {
                    if (exitNode.IsConnected())
                    {
                        for (var i = exitNode.InputSockets[0].Connections.Count - 1; i >= 0; i--)
                        {
                            var connection = exitNode.InputSockets[0].Connections[i];
                            var connectedNode = graph.GetNodeById(connection.OutputNodeId);
                            exitNode.InputSockets[0].DisconnectFromNode(connection.OutputNodeId);
                            if (connectedNode != null)
                            {
                                connectedNode.GetSocketFromId(connection.OutputSocketId)
                                             .DisconnectFromNode(exitNode.Id);
                            }
                        }
                    }

                    if (graph.Nodes.Contains(exitNode))
                    {
                        graph.Nodes.Remove(exitNode);
                    }

                    Object.DestroyImmediate(exitNode, true);
                }

                graph.IsSubGraph = false;
                graph.MarkAsModifiedCustom();
            }
        }

        public static Graph GenerateNodyGraph(string graphName, bool createSubGraph)
        {
            using (_PRF_GenerateNodyGraph.Auto())
            {
                var existing = AssetDatabaseManager.FindFirstAssetMatch<Graph>(graphName);

                if (existing != null)
                {
                    return existing;
                }

                var saveLocationFolder =
                    AssetDatabaseManager.GetSaveDirectoryForOwnedAsset<ApplicationManager, Graph>(graphName);

                var relativeSaveLocationFolder = saveLocationFolder.ToRelativePath();

                var saveLocation = AppaPath.Combine(relativeSaveLocationFolder, graphName) + ".asset";

                if (AppaFile.Exists(saveLocation))
                {
                    var existingGraph = AssetDatabaseManager.ImportAndLoadAssetAtPath<Graph>(saveLocation);

                    if (existingGraph != null)
                    {
                        existingGraph.CheckAndCreateAnyMissingSystemNodes();

                        return existingGraph;
                    }

                    Context.Log.Warn(ZString.Format("Deleting Nody graph at {0}.", saveLocation));
                    AppaFile.Delete(saveLocation);
                }

                var newGraph = ScriptableObject.CreateInstance<Graph>();
                newGraph.name = AppaPath.GetFileNameWithoutExtension(saveLocation);
                newGraph.Id = Guid.NewGuid().ToString();
                newGraph.SetGraphVersionAndLastModifiedTime();

                AssetDatabaseManager.CreateAsset(newGraph, saveLocation);

                if (createSubGraph)
                {
                    newGraph.CreateEnterAndExitNodes();
                }
                else
                {
                    newGraph.CreateStartNode();
                }

                newGraph.MarkAsModifiedCustom(true);

                return newGraph;
            }
        }

        public static void GenerateTestGraph<T, TM>(this GraphController target, AreaMetadata<T, TM> metadata)
            where T : AreaManager<T, TM>
            where TM : AreaMetadata<T, TM>
        {
            using (_PRF_GenerateTestGraph.Auto())
            {
                var graph = metadata.doozyGraph.testGraph;

                target.SetGraph(graph);

                if (graph.Nodes.Count == 0)
                {
                    CreateStartNode(graph);
                    var enterNode = graph.GetStartOrEnterNode();

                    var subgraphNode = CreateNode(graph, typeof(SubGraphNode), Vector2.right) as SubGraphNode;

                    subgraphNode.OnCreate();
                    subgraphNode.AddDefaultSockets();
                    subgraphNode.SubGraph = graph;

                    var enterOutputSocket = enterNode.OutputSockets.FirstOrDefault();
                    var subgraphInputSocket = subgraphNode.InputSockets.FirstOrDefault();

                    ConnectSockets(graph, enterOutputSocket, subgraphInputSocket, true);

                    AssetDatabaseManager.SaveAssets();
                }
            }
        }

        public static void MarkAsModifiedCustom<T>(this T graph, bool saveAssets = false)
            where T : Graph
        {
            using (_PRF_MarkAsModifiedCustom.Auto())
            {
                graph.SetGraphVersionAndLastModifiedTime();
                graph.IsDirty = true;
                graph.MarkAsModified();
                if (saveAssets)
                {
                    AssetDatabaseManager.SaveAssets();
                }
            }
        }

        private static void SetGraphVersionAndLastModifiedTime<T>(this T graph)
            where T : Graph
        {
            using (_PRF_SetGraphVersionAndLastModifiedTime.Auto())
            {
                graph.SetVersion(Graph.FILE_VERSION);
                graph.SetLastModified(DateTime.UtcNow.ToFileTimeUtc().ToString());
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(NodyGraphGenerator) + ".";

        private static readonly ProfilerMarker _PRF_GenerateNodyGraph =
            new ProfilerMarker(_PRF_PFX + nameof(GenerateNodyGraph));

        private static readonly ProfilerMarker _PRF_GenerateTestGraph =
            new ProfilerMarker(_PRF_PFX + nameof(GenerateTestGraph));

        private static readonly ProfilerMarker _PRF_CreateEnterAndExitNodes =
            new ProfilerMarker(_PRF_PFX + nameof(CreateEnterAndExitNodes));

        private static readonly ProfilerMarker _PRF_ConnectSockets =
            new ProfilerMarker(_PRF_PFX + nameof(ConnectSockets));

        private static readonly ProfilerMarker _PRF_CreateStartNode =
            new ProfilerMarker(_PRF_PFX + nameof(CreateStartNode));

        private static readonly ProfilerMarker _PRF_CreateGraph =
            new ProfilerMarker(_PRF_PFX + nameof(CreateGraph));

        private static readonly ProfilerMarker _PRF_SetGraphVersionAndLastModifiedTime =
            new ProfilerMarker(_PRF_PFX + nameof(SetGraphVersionAndLastModifiedTime));

        private static readonly ProfilerMarker _PRF_MarkAsModifiedCustom =
            new ProfilerMarker(_PRF_PFX + nameof(MarkAsModifiedCustom));

        private static readonly ProfilerMarker _PRF_CheckAndCreateAnyMissingSystemNodes =
            new ProfilerMarker(_PRF_PFX + nameof(CheckAndCreateAnyMissingSystemNodes));

        private static readonly ProfilerMarker _PRF_CreateNode =
            new ProfilerMarker(_PRF_PFX + nameof(CreateNode));

        #endregion
#endif
    }
}
