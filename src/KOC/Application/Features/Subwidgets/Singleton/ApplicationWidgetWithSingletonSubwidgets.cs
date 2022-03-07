using System;
using System.Collections.Generic;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Availability;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Services.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Common.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Singleton.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Widgets;
using Appalachia.Prototype.KOC.Application.Features.Widgets.Contracts;
using Appalachia.Prototype.KOC.Application.Functionality;
using Appalachia.Prototype.KOC.Application.Functionality.Contracts;
using Appalachia.Prototype.KOC.Application.FunctionalitySets;
using Appalachia.UI.Controls.Extensions;
using Appalachia.Utility.Async;
using Appalachia.Utility.Extensions;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Features.Subwidgets.Singleton
{
    [CallStaticConstructorInEditor]
    public abstract class ApplicationWidgetWithSingletonSubwidgets<TISubwidget, TISubwidgetMetadata, TWidget,
                                                                   TWidgetMetadata, TFeature, TFeatureMetadata,
                                                                   TFunctionalitySet, TIService, TIWidget, TManager> :
        ApplicationWidget<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget,
            TManager>
        where TISubwidget : class, ISingletonSubwidget<TISubwidget, TISubwidgetMetadata>
        where TISubwidgetMetadata : class, ISingletonSubwidgetMetadata<TISubwidget, TISubwidgetMetadata>
        where TWidget : ApplicationWidgetWithSingletonSubwidgets<TISubwidget, TISubwidgetMetadata, TWidget,
            TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget, TManager>, TIWidget
        where TWidgetMetadata : ApplicationWidgetWithSingletonSubwidgetsMetadata<TISubwidget, TISubwidgetMetadata,
            TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget, TManager>
        where TFeature : ApplicationFeature<TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget,
            TManager>
        where TFeatureMetadata : ApplicationFeatureMetadata<TFeature, TFeatureMetadata, TFunctionalitySet, TIService,
            TIWidget, TManager>
        where TFunctionalitySet : FeatureFunctionalitySet<TIService, TIWidget>, new()
        where TIService : IApplicationService
        where TIWidget : IApplicationWidget
        where TManager : SingletonAppalachiaBehaviour<TManager>, ISingleton<TManager>, IApplicationFunctionalityManager
    {
        #region Constants and Static Readonly

        public static readonly string SubwidgetParentName = typeof(TWidget).Name + " Subwidgets";

        #endregion

        static ApplicationWidgetWithSingletonSubwidgets()
        {
            var callbacks = RegisterInstanceCallbacks
               .For<ApplicationWidgetWithSingletonSubwidgets<TISubwidget, TISubwidgetMetadata, TWidget, TWidgetMetadata,
                    TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget, TManager>>();

            void HandleSubwidgetAvailability(TISubwidget subwidget)
            {
                void RegisterSubwidgetAction(TWidget widget)
                {
                    widget.RegisterSubwidget(subwidget);
                }

                callbacks.When.Behaviour<TWidget>().IsAvailableThen(RegisterSubwidgetAction);
            }

            callbacks.When.Any<TISubwidget>().IsAvailableThen(HandleSubwidgetAvailability);
        }

        #region Fields and Autoproperties

        private GameObject _subwidgetParent;

        protected List<TISubwidget> _subwidgets;

        #endregion

        public GameObject SubwidgetParent => _subwidgetParent;

        public IReadOnlyList<TISubwidget> Subwidgets
        {
            get
            {
                if (_subwidgets == null)
                {
                    _subwidgets = new List<TISubwidget>();
                }

                return _subwidgets;
            }
        }

        public abstract void ValidateSubwidgets();

        /// <summary>
        ///     Adds the specified element to the widget's collection.
        /// </summary>
        /// <param name="element">The element to add.</param>
        public void RegisterSubwidget(TISubwidget element)
        {
            using (_PRF_RegisterSubwidget.Auto())
            {
                _subwidgets ??= new();

                for (var i = 0; i < _subwidgets.Count; i++)
                {
                    if (_subwidgets[i] == element)
                    {
                        return;
                    }
                }

                _subwidgets.Add(element);

                OnRegisterSubwidget(element);
            }
        }

        /// <summary>
        ///     Implements any feature specific handling of the element when it is registered.
        /// </summary>
        /// <param name="subwidget"></param>
        protected abstract void OnRegisterSubwidget(TISubwidget subwidget);

        protected static void SortSubwidgetsByPriority<TI, TIM>(List<TI> subwidgets)
            where TI : class, ISingletonSubwidget<TI, TIM>
            where TIM : class, ISingletonSubwidgetMetadata<TI, TIM>, IPrioritySubwidgetMetadata
        {
            using (_PRF_SortSubwidgetsByPriority.Auto())
            {
                subwidgets.Sort((e1, e2) => e1.Metadata.Priority.CompareTo(e2.Metadata.Priority));
            }
        }

        /// <inheritdoc />
        protected override async AppaTask WhenEnabled()
        {
            await base.WhenEnabled();

            using (_PRF_WhenEnabled.Auto())
            {
                canvas.GameObject.GetOrAddChild(ref _subwidgetParent, SubwidgetParentName, true);

                var canvasChildCount = canvas.RectTransform.childCount;

                var subwidgetRect = SubwidgetParent.transform as RectTransform;
                subwidgetRect.FullScreen(true);

                _subwidgetParent.transform.SetSiblingIndex(canvasChildCount - 1);
            }
        }

        protected void EnsureSubwidgetsHaveCorrectParent(List<TISubwidget> subwidgets, Transform parent)
        {
            using (_PRF_EnsureSubwidgetsHaveCorrectParent.Auto())
            {
                for (var index = 0; index < subwidgets.Count; index++)
                {
                    var subwidget = subwidgets[index];
                    subwidget.Transform.SetParent(parent);
                }
            }
        }

        protected void RemoveIncorrectSubwidgetsFromList(
            List<TISubwidget> reviewing,
            List<TISubwidget> other,
            Predicate<TISubwidget> isCorrect)
        {
            using (_PRF_SwapSubwidgetsBetweenLists.Auto())
            {
                for (var index = reviewing.Count - 1; index >= 0; index--)
                {
                    var subwidget = reviewing[index];

                    if (isCorrect(subwidget))
                    {
                        continue;
                    }

                    reviewing.RemoveAt(index);
                    other.Add(subwidget);
                }
            }
        }

        #region Profiling

        protected static readonly ProfilerMarker _PRF_SortSubwidgetsByPriority =
            new ProfilerMarker(_PRF_PFX + nameof(SortSubwidgetsByPriority));

        protected static readonly ProfilerMarker _PRF_EnsureSubwidgetsHaveCorrectParent =
            new ProfilerMarker(_PRF_PFX + nameof(EnsureSubwidgetsHaveCorrectParent));

        protected static readonly ProfilerMarker _PRF_OnRegisterSubwidget =
            new ProfilerMarker(_PRF_PFX + nameof(OnRegisterSubwidget));

        protected static readonly ProfilerMarker _PRF_RegisterSubwidget =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterSubwidget));

        protected static readonly ProfilerMarker _PRF_SwapSubwidgetsBetweenLists =
            new ProfilerMarker(_PRF_PFX + nameof(RemoveIncorrectSubwidgetsFromList));

        protected static readonly ProfilerMarker _PRF_ValidateSubwidgets =
            new ProfilerMarker(_PRF_PFX + nameof(ValidateSubwidgets));

        #endregion
    }
}
