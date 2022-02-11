using System;
using Appalachia.UI.Controls.Sets.RawImage;
using Appalachia.UI.Core.Components.Data;
using Appalachia.Utility.Async;
using Drawing;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Application.Features.Aspects
{
    public static class GizmoDrawer
    {
        #region Nested type: IService

        public interface IService
        {
            Camera DrawCamera { get; }
            CommandBuilder Draw { get; }
            RenderTexture GetRenderTexture();
            void OnPreCull();
            AppaTask WhenEnabled();
        }

        #endregion

        #region Nested type: IServiceMetadata

        public interface IServiceMetadata
        {
            CameraData CameraData { get; }
        }

        #endregion

        #region Nested type: IWidget

        public interface IWidget
        {
            RawImage RawImage { get; }
        }

        #endregion

        #region Nested type: IWidgetMetadata

        public interface IWidgetMetadata
        {
            RawImageComponentSetData RawImageSet { get; }
        }

        #endregion

        #region Nested type: Service

        public sealed class Service
        {
            public Service(Camera c)
            {
                _drawCamera = c;
            }

            #region Fields and Autoproperties

            [NonSerialized] private readonly Camera _drawCamera;

            [NonSerialized] private CommandBuilder? _drawCommandBuilder;

            private RenderTexture _renderTexture;

            #endregion

            public CommandBuilder Draw
            {
                get
                {
                    if (_drawCommandBuilder.HasValue)
                    {
                        return _drawCommandBuilder.Value;
                    }

                    var builder = DrawingManager.GetBuilder(true);

                    var cam = DrawCamera;

                    builder.cameraTargets = new[] { cam };

                    var matrix = cam.cameraToWorldMatrix *
                                 cam.nonJitteredProjectionMatrix.inverse *
                                 Matrix4x4.TRS(
                                     new Vector3(-1.0f, -1.0f, 0),
                                     Quaternion.identity,
                                     new Vector3(2.0f / cam.pixelWidth, 2.0f / cam.pixelHeight, 1)
                                 );

                    builder.PushMatrix(matrix);

                    _drawCommandBuilder = builder;

                    return builder;
                }
            }

            internal Camera DrawCamera => _drawCamera;

            public RenderTexture GetRenderTexture()
            {
                using (_PRF_GetRenderTexture.Auto())
                {
                    return _renderTexture;
                }
            }

            public async AppaTask Initialize()
            {
                using (_PRF_WhenEnabled.Auto())
                {
                    var cam = DrawCamera;
                    _renderTexture = new RenderTexture(cam.pixelWidth, cam.pixelHeight, 24);
                    cam.targetTexture = _renderTexture;

                    await AppaTask.CompletedTask;
                }
            }

            public void OnPreCull()
            {
                using (_PRF_OnPreCull.Auto())
                {
                    if (_drawCommandBuilder.HasValue)
                    {
                        var builder = _drawCommandBuilder.Value;
                        builder.PopMatrix();
                        builder.Dispose();
                    }

                    _drawCommandBuilder = null;
                }
            }

            #region Profiling

            private const string _PRF_PFX = nameof(GizmoDrawer) + "." + nameof(Service) + ".";

            private static readonly ProfilerMarker _PRF_GetRenderTexture =
                new ProfilerMarker(_PRF_PFX + nameof(GetRenderTexture));

            private static readonly ProfilerMarker _PRF_OnPreCull =
                new ProfilerMarker(_PRF_PFX + nameof(OnPreCull));

            private static readonly ProfilerMarker _PRF_WhenEnabled =
                new ProfilerMarker(_PRF_PFX + nameof(Initialize));

            #endregion
        }

        #endregion
    }
}
