using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DebugExtensions
{
    internal class DebugSettingsEditorWindow : EditorWindow
    {
        private DebugSettingsLocalCache LocalCache => DebugSettingsLocalCache.instance;

        private SerializedObject _serializedSettings;
        private SerializedProperty _defaultColorProp;
        private SerializedProperty _specifiedColorProp;
        private SerializedProperty _excludeDirProp;
        private SerializedProperty _includeDirProp;


        [MenuItem("Window/Debug Settings")]
        public static void ShowWindow()
        {
            var window = GetWindow<DebugSettingsEditorWindow>("Debug Settings");
            window.titleContent = new GUIContent("Debug Settings");
        }

        private void OnEnable()
        {
            // 获取单例的 SerializedObject
            // 此处在启动项目时会因为 ScriptableSingleton 转化为 SerializedObject 会报错,但是不影响使用
            _serializedSettings = new SerializedObject(LocalCache);
            _defaultColorProp = _serializedSettings.FindProperty("DefaultColor");
            _specifiedColorProp = _serializedSettings.FindProperty("SpecifiedColor");
            _excludeDirProp = _serializedSettings.FindProperty("ExcludeDir");
            _includeDirProp = _serializedSettings.FindProperty("IncludeDir");

        }

        private void CreateGUI()
        {
            var root = rootVisualElement;

            string uxmlGuid = "defec952eccde0f45969366fa6c822ce";
            string uxmlPath = AssetDatabase.GUIDToAssetPath(uxmlGuid);
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(uxmlPath);
            visualTree.CloneTree(root);

            rootVisualElement.Q<ColorField>("ColorField01").BindProperty(_defaultColorProp);
            rootVisualElement.Q<PropertyField>("PropertyField01").BindProperty(_specifiedColorProp);
            rootVisualElement.Q<PropertyField>("PropertyField02").BindProperty(_excludeDirProp);
            rootVisualElement.Q<PropertyField>("PropertyField03").BindProperty(_includeDirProp);
        }
    }

}
