using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;

namespace DebugExtensions
{
    [Serializable]
    internal struct LogSpecificColor
    {
        public bool Enable;
        public string FilePath;
        public Color LogColor;
    }

    [Serializable]
    internal struct LogDirSetting
    {
        public bool Enable;
        [FolderPath]
        public string DirPath;
    }
}
