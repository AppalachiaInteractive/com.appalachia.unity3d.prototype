using System;
using System.IO;
using System.Linq;
using Appalachia.CI.Integration.Assets;
using Appalachia.Prototype.KOC.Application.Areas;
using Appalachia.Utility.Extensions;
using Doozy.Engine.Nody;
using Doozy.Engine.Nody.Models;
using Doozy.Engine.Nody.Nodes;
using Doozy.Engine.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Appalachia.Prototype.KOC.Application.Graphs
{
    public static class NodyGraphGenerator
    {
        public static void GenerateTestGraph<T, TM>(GraphController target, AreaMetadata<T, TM> metadata)
            where T : AreaManager<T, TM>
            where TM : AreaMetadata<T, TM>
        {
#if UNITY_EDITOR

            var graph = metadata.testGraph;

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
#endif
        }

#if UNITY_EDITOR

        private static UILanguagePack UILabels => UILanguagePack.Instance;

        public static T CreateGraph<T>(string path, bool createSubGraph)
            where T : Graph
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            var graph = ScriptableObject.CreateInstance<T>();
            graph.name = Path.GetFileNameWithoutExtension(path);
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

            graph.SetDirty(true);
            return graph;
        }

        public static T CreateGraphWidthDialog<T>(bool createSubGraph = false)
            where T : Graph
        {
            string path = UnityEditor.EditorUtility.SaveFilePanelInProject(
                createSubGraph ? UILabels.CreateSubGraph : UILabels.CreateGraph,
                createSubGraph ? UILabels.SubGraph : UILabels.Graph,
                "asset",
                createSubGraph ? UILabels.CreateNewGraphAsSubGraph : UILabels.CreateNewGraph
            );
            return string.IsNullOrEmpty(path) ? null : CreateGraph<T>(path, createSubGraph);
        }

        public static T LoadGraph<T>(string path)
            where T : Graph
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            var graph = AssetDatabaseManager.LoadAssetAtPath<T>(UnityEditor.FileUtil.GetProjectRelativePath(path));
            return graph == null ? null : graph;
        }

        public static T LoadGraphWithDialog<T>()
            where T : Graph
        {
            string path = UnityEditor.EditorUtility.OpenFilePanelWithFilters(
                UILabels.OpenGraph,
                "",
                new[] {typeof(T).Name, "asset"}
            );
            return LoadGraph<T>(path);
        }

        private static void SetGraphVersionAndLastModifiedTime<T>(this T graph)
            where T : Graph
        {
            graph.SetVersion(Graph.FILE_VERSION);
            graph.SetLastModified(DateTime.UtcNow.ToFileTimeUtc().ToString());
        }

        public static void SetDirty<T>(this T graph, bool saveAssets = false)
            where T : Graph
        {
            graph.SetGraphVersionAndLastModifiedTime();
            graph.IsDirty = true;
            UnityEditor.EditorUtility.SetDirty(graph);
            if (saveAssets)
            {
                AssetDatabaseManager.SaveAssets();
            }
        }

        public static void CheckAndCreateAnyMissingSystemNodes(Graph graph)
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

        public static Node CreateNode(Graph graph, Type nodeType, Vector2 nodeWorldPosition)
        {
            var node = ScriptableObject.CreateInstance(nodeType) as Node;
            Debug.Assert(node != null, "node != null");
            nodeWorldPosition.x -= node.GetDefaultNodeWidth() / 2;
            nodeWorldPosition.y -= NodySettings.Instance.NodeHeaderHeight / 2;
            node.InitNode(graph, nodeWorldPosition);
            node.OnCreate();
            node.AddDefaultSockets();
            node.hideFlags = NodySettings.Instance.DefaultHideFlagsForNodes;
            UnityEditor.EditorUtility.SetDirty(node);
            return node;
        }

        public static void CreateStartNode(Graph graph)
        {
            var startNode = graph.GetStartNode();
            var enterNode = graph.GetEnterNode();
            var exitNode = graph.GetExitNode();

            var startNodeWasCreated = false;
            if (startNode == null)
            {
                startNode = CreateNode(graph, typeof(StartNode), Vector2.zero);
                graph.Nodes.Add(startNode);
                if (!UnityEditor.EditorUtility.IsPersistent(startNode))
                {
                    UnityEditor. AssetDatabase.AddObjectToAsset(startNode, graph);
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
                    var targetNode = graph.GetNodeById(enterNode.OutputSockets[0].Connections[0].InputNodeId);
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

                Object.DestroyImmediate(enterNode, true);
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
            graph.SetDirty(false);
        }

        public static void CreateEnterAndExitNodes(Graph graph)
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
                if (!UnityEditor.EditorUtility.IsPersistent(enterNode))
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
                    var targetNode = graph.GetNodeById(startNode.OutputSockets[0].Connections[0].InputNodeId);
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
                if (!UnityEditor.EditorUtility.IsPersistent(exitNode))
                {
                    AssetDatabaseManager.AddObjectToAsset(exitNode, graph);
                }
            }

            graph.IsSubGraph = true;
            graph.SetDirty(false);
        }

        public static void ConnectSockets(
            Graph graph,
            Socket outputSocket,
            Socket inputSocket,
            bool saveAssets = false)
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
            graph.SetDirty(saveAssets);
        }

#endif
    }
}
