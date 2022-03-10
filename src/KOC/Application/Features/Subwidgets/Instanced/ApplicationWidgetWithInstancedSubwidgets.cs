using System;
using System.Collections.Generic;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Availability;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Services.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Common.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Instanced.Contracts;
using Appalachia.Prototype.KOC.Application.Features.Widgets;
using Appalachia.Prototype.KOC.Application.Features.Widgets.Contracts;
using Appalachia.Prototype.KOC.Application.Functionality.Contracts;
using Appalachia.Prototype.KOC.Application.FunctionalitySets;
using Appalachia.UI.Controls.Extensions;
using Appalachia.Utility.Async;
using Appalachia.Utility.Events;
using Appalachia.Utility.Extensions;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Features.Subwidgets.Instanced
{
    [CallStaticConstructorInEditor]
    public abstract class ApplicationWidgetWithInstancedSubwidgets<TSubwidget, TSubwidgetMetadata, TISubwidget,
                                                                   TISubwidgetMetadata, TWidget, TWidgetMetadata,
                                                                   TFeature, TFeatureMetadata, TFunctionalitySet,
                                                                   TIService, TIWidget, TManager> : ApplicationWidget<
        TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget, TManager>
        where TSubwidget :
        ApplicationInstancedSubwidget<TSubwidget, TSubwidgetMetadata, TISubwidget, TISubwidgetMetadata, TWidget,
            TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget, TManager>, TISubwidget,
        IApplicationInstancedSubwidget<TISubwidget, TISubwidgetMetadata>
        where TSubwidgetMetadata :
        ApplicationInstancedSubwidgetMetadata<TSubwidget, TSubwidgetMetadata, TISubwidget, TISubwidgetMetadata, TWidget,
            TWidgetMetadata, TFeature, TFeatureMetadata, TFunctionalitySet, TIService, TIWidget, TManager>,
        TISubwidgetMetadata, IApplicationInstancedSubwidgetMetadata<TISubwidget, TISubwidgetMetadata>
        where TISubwidget : class, IApplicationInstancedSubwidget<TISubwidget, TISubwidgetMetadata>
        where TISubwidgetMetadata : class, IApplicationInstancedSubwidgetMetadata<TISubwidget, TISubwidgetMetadata>
        where TWidget : ApplicationWidgetWithInstancedSubwidgets<TSubwidget, TSubwidgetMetadata, TISubwidget,
            TISubwidgetMetadata, TWidget, TWidgetMetadata, TFeature,
            TFeatureMetadata, TFunctionalitySet, TIService,
            TIWidget, TManager>, TIWidget
        where TWidgetMetadata : ApplicationWidgetWithInstancedSubwidgetsMetadata<TSubwidget, TSubwidgetMetadata, TISubwidget,
            TISubwidgetMetadata, TWidget, TWidgetMetadata, TFeature,
            TFeatureMetadata, TFunctionalitySet, TIService,
            TIWidget, TManager>
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

        static ApplicationWidgetWithInstancedSubwidgets()
        {
            var callbacks = RegisterInstanceCallbacks
               .For<ApplicationWidgetWithInstancedSubwidgets<TSubwidget, TSubwidgetMetadata, TISubwidget,
                TISubwidgetMetadata, TWidget, TWidgetMetadata,
                TFeature, TFeatureMetadata, TFunctionalitySet,
                TIService, TIWidget, TManager>>();

            void HandleSubwidgetAvailability(AppaEvent<TSubwidget>.Args args)
            {
                void RegisterSubwidgetAction(TWidget widget)
                {
                    widget.AddSubwidget(args.value);
                }

                callbacks.When.Behaviour<TWidget>().IsAvailableThen(RegisterSubwidgetAction);
            }

            callbacks.When.AnyInstance<TSubwidget>().IsEnabledThen(HandleSubwidgetAvailability);
        }

        #region Fields and Autoproperties

        private GameObject _subwidgetParent;

        protected List<TSubwidget> _subwidgets;

        #endregion

        public GameObject SubwidgetParent
        {
            get
            {
                canvas.GameObject.GetOrAddChild(ref _subwidgetParent, SubwidgetParentName, true);
                return _subwidgetParent;
            }
        }

        public IReadOnlyList<TSubwidget> Subwidgets
        {
            get
            {
                if (_subwidgets == null)
                {
                    _subwidgets = new List<TSubwidget>();
                }

                return _subwidgets;
            }
        }

        public abstract void ValidateSubwidgets();

        /// <summary>
        ///     Adds the specified element to the widget's collection.
        /// </summary>
        /// <param name="element">The element to add.</param>
        public void AddSubwidget(TSubwidget element)
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
        protected abstract void OnRegisterSubwidget(TSubwidget subwidget);

        protected static void SortSubwidgetsByPriority<TI, TIM>(List<TI> subwidgets)
            where TI : class, IApplicationSubwidget<TI, TIM>
            where TIM : class, IApplicationSubwidgetMetadata<TI, TIM>, IPrioritySubwidgetMetadata
        {
            using (_PRF_SortSubwidgetsByPriority.Auto())
            {
                subwidgets.Sort((e1, e2) => { return e1.Priority.CompareTo(e2.Priority); });
            }
        }

        /// <inheritdoc />
        protected override async AppaTask WhenEnabled()
        {
            await base.WhenEnabled();

            using (_PRF_WhenEnabled.Auto())
            {
                var canvasChildCount = canvas.RectTransform.childCount;

                var subwidgetRect = SubwidgetParent.transform as RectTransform;
                subwidgetRect.FullScreen(true);

                SubwidgetParent.transform.SetSiblingIndex(canvasChildCount - 1);
            }
        }

        protected void EnsureSubwidgetsHaveCorrectParent(List<TSubwidget> subwidgets, Transform parent)
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
            List<TSubwidget> reviewing,
            List<TSubwidget> other,
            Predicate<TSubwidget> isCorrect)
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
            new ProfilerMarker(_PRF_PFX + nameof(AddSubwidget));

        protected static readonly ProfilerMarker _PRF_SwapSubwidgetsBetweenLists =
            new ProfilerMarker(_PRF_PFX + nameof(RemoveIncorrectSubwidgetsFromList));

        protected static readonly ProfilerMarker _PRF_ValidateSubwidgets =
            new ProfilerMarker(_PRF_PFX + nameof(ValidateSubwidgets));

        #endregion
    }
}
