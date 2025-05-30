using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;


namespace DebugExtensions
{
    [FilePath("Library/csmtools/DebugSettings.asset",
        FilePathAttribute.Location.ProjectFolder)]
    internal class DebugSettingsLocalCache : ScriptableSingleton<DebugSettingsLocalCache>
    {
        [SerializeField]
        public Color DefaultColor = Color.white;

        [SerializeField]
        public LogSpecificColor[] SpecifiedColor;

        [SerializeField]
        public LogDirSetting[] ExcludeDir;

        [SerializeField]
        public LogDirSetting[] IncludeDir;

        // private DebugSettingsLocalCache LocalCache => DebugSettingsLocalCache.instance;
        public void ForceSave()
        {
            Save(true);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            // Unity 自动调用，当值在Inspector中改变时
            Save(true);
        }
#endif
    }
}
