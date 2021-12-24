using System;
using Appalachia.CI.Constants;
using Appalachia.Prototype.KOC.Application.Behaviours;
using Appalachia.Prototype.KOC.Data.Configuration;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Data
{
    [ExecuteAlways]
    public class DatabaseManager : SingletonAppalachiaApplicationBehaviour<DatabaseManager>
    {
        #region Fields and Autoproperties

        [BoxGroup("Data Set")] public ActiveDataSet dataSet;

        [BoxGroup("Data Set")] public bool overrideActiveDataSet;

        [BoxGroup("Data Set")]
        [EnableIf(nameof(overrideActiveDataSet))]
        public string overrideActiveDataSetName;

        public DatabaseCollection databases;

        [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
        public DatabaseConfiguration configuration;

        #endregion

        

        #region Event Functions

        protected override async AppaTask WhenDisabled()

        {
            using (_PRF_OnDisable.Auto())
            {
                await base.WhenDisabled();

                databases?.Dispose();
            }
        }

        #endregion

        [ButtonGroup(APPASTR.General)]
        public void CreateDatabases()
        {
            using (_PRF_CreateDatabases.Auto())
            {
                Context.Log.Info(nameof(CreateDatabases), this);

                var dataSetName = GetDataSetName();
                databases = DatabaseCollection.CreateOrLoad(configuration, dataSetName);
            }
        }

        public void LoadGame(long gameId)
        {
            using (_PRF_LoadGame.Auto())
            {
                Context.Log.Info(nameof(LoadGame), this);
                databases.LoadGame(configuration, gameId);
            }
        }

        public void NewGame()
        {
            using (_PRF_NewGame.Auto())
            {
                Context.Log.Info(nameof(NewGame), this);
            }
        }

        public void SaveGame()
        {
            using (_PRF_SaveGame.Auto())
            {
                Context.Log.Info(nameof(SaveGame), this);
            }
        }

        protected override void Initialize()
        {
            using (_PRF_Initialize.Auto())
            {
                base.Initialize();

                await initializer.Do(
                    this,
                    nameof(dataSet),
                    () =>
                    {
                        dataSet = ActiveDataSet.Developer;
                        overrideActiveDataSet = false;
                        overrideActiveDataSetName = string.Empty;
                    }
                );

                configuration = DatabaseConfiguration.instance;
                configuration.InitializeExternal();
            }
        }

        private string GetDataSetName()
        {
            if (overrideActiveDataSet)
            {
                return overrideActiveDataSetName;
            }

            switch (dataSet)
            {
                case ActiveDataSet.Developer:
                    var username = Environment.UserName.ToLower();

                    if (username == "janic")
                    {
                        username = "janice";
                    }

                    return username;

                case ActiveDataSet.Team:
                    return nameof(ActiveDataSet.Team);
                case ActiveDataSet.Alpha:

                    return nameof(ActiveDataSet.Alpha);
                case ActiveDataSet.Beta:

                    return nameof(ActiveDataSet.Beta);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(DatabaseManager) + ".";

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        private static readonly ProfilerMarker _PRF_CreateDatabases =
            new ProfilerMarker(_PRF_PFX + nameof(CreateDatabases));

        private static readonly ProfilerMarker _PRF_Awake = new ProfilerMarker(_PRF_PFX + nameof(Awake));

        private static readonly ProfilerMarker
            _PRF_LoadGame = new ProfilerMarker(_PRF_PFX + nameof(LoadGame));

        private static readonly ProfilerMarker _PRF_NewGame = new ProfilerMarker(_PRF_PFX + nameof(NewGame));

        private static readonly ProfilerMarker _PRF_OnDisable =
            new ProfilerMarker(_PRF_PFX + nameof(OnDisable));

        private static readonly ProfilerMarker
            _PRF_OnEnable = new ProfilerMarker(_PRF_PFX + nameof(OnEnable));

        private static readonly ProfilerMarker
            _PRF_SaveGame = new ProfilerMarker(_PRF_PFX + nameof(SaveGame));

        #endregion
    }
}
