using System;
using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DebugExtensions
{
    [CustomPropertyDrawer(typeof(LogSpecificColor))]
    public class LogSpecificColorDrawer : PropertyDrawer
    {
        public bool showPosition = true;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // Create a new VisualElement to be the root the property UI.
            var container = new VisualElement();

            var cSharpFoldout = new Foldout { text = "Element" };

            // 添加bool字段
            var boolField = new Toggle("Enable")
            {
                value = property.FindPropertyRelative("Enable").boolValue
            };
            boolField.RegisterValueChangedCallback(evt =>
            {
                property.FindPropertyRelative("Enable").boolValue = evt.newValue;
                property.serializedObject.ApplyModifiedProperties();
            });
            cSharpFoldout.Add(boolField);


            // 添加Color字段
            var colorField = new ColorField("Log Color")
            {
                value = property.FindPropertyRelative("LogColor").colorValue
            };
            colorField.RegisterValueChangedCallback(evt =>
            {
                property.FindPropertyRelative("LogColor").colorValue = evt.newValue;
                property.serializedObject.ApplyModifiedProperties();
            });

            cSharpFoldout.Add(colorField);

            // C# Script
            var filePathProp = property.FindPropertyRelative("FilePath");
            var scriptField = new ObjectField("Script File")
            {
                objectType = typeof(MonoScript),
                allowSceneObjects = false,
                value = AssetDatabase.LoadAssetAtPath<MonoScript>(filePathProp.stringValue)
            };

            scriptField.RegisterValueChangedCallback(evt =>
            {
                if (evt.newValue != null)
                {
                    string path = AssetDatabase.GetAssetPath(evt.newValue);
                    filePathProp.stringValue = path;
                    filePathProp.serializedObject.ApplyModifiedProperties();

                }
            });


            cSharpFoldout.Add(scriptField);

            container.Add(cSharpFoldout);

            // Return the finished UI.
            return container;
        }


    }
}
