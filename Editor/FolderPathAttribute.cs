using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace DebugExtensions
{
    internal class FolderPathAttribute : PropertyAttribute { }

    [CustomPropertyDrawer(typeof(FolderPathAttribute))]
    internal class FolderPathUIToolkitDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();
            container.style.flexDirection = FlexDirection.Row;

            // 文本字段
            var textField = new TextField(property.displayName)
            {
                bindingPath = property.propertyPath
            };
            textField.style.flexGrow = 1;

            // 选择按钮
            var button = new Button(() =>
            {
                string path = EditorUtility.OpenFolderPanel("选择文件夹",
                    Application.dataPath, "");
                if (!string.IsNullOrEmpty(path))
                {
                    if (path.StartsWith(Application.dataPath))
                    {
                        path = "Assets" + path.Substring(Application.dataPath.Length);
                    }
                    property.stringValue = path;
                    property.serializedObject.ApplyModifiedProperties();
                }
            })
            {
                text = "..."
            };
            button.style.width = 25;

            container.Add(textField);
            container.Add(button);
            return container;
        }
    }


}
