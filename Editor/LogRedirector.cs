using UnityEditor;
using System;
using System.Reflection;
using UnityEngine;

namespace DebugExtensions
{
    public static class LogRedirector
    {
        // 支持两种签名
        [UnityEditor.Callbacks.OnOpenAsset(0)]
        private static bool OnOpenAsset(int instanceID, int line)
        {
            return HandleOpenAsset(instanceID);
        }

        private static bool HandleOpenAsset(int instanceID)
        {
            string assetPath = AssetDatabase.GetAssetPath(instanceID);
            // 这里可以加多个封装类判断
            if (assetPath.EndsWith("DebugExtension.cs") || assetPath.EndsWith("LogRedirector.cs"))
            {

                var consoleWindow = Type.GetType("UnityEditor.ConsoleWindow,UnityEditor");
                var activeText = consoleWindow.GetField("m_ActiveText",
                    BindingFlags.Instance | BindingFlags.NonPublic);

                var window = EditorWindow.focusedWindow;
                if (window != null && window.GetType() == consoleWindow)
                {
                    string stackTrace = activeText.GetValue(window) as string;

                    return RedirectToActualCall(stackTrace);
                }
            }
            return false;
        }

        private static bool RedirectToActualCall(string stackTrace)
        {
            var matches = System.Text.RegularExpressions.Regex.Matches(stackTrace, @"\(at (.+?):(\d+)\)");
            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                string filePath = match.Groups[1].Value.Replace("\\", "/");
                int lineNumber = int.Parse(match.Groups[2].Value);

                // 跳过封装类
                if (filePath.Contains("DebugExtension.cs"))
                    continue;

                int assetsIndex = filePath.IndexOf("Assets/");
                if (assetsIndex >= 0)
                {
                    string relativePath = filePath.Substring(assetsIndex);

                    var script = AssetDatabase.LoadAssetAtPath<MonoScript>(relativePath);
                    if (script != null)
                    {
                        AssetDatabase.OpenAsset(script, lineNumber);

                        return true;
                    }
                }
            }
            return false;
        }
    }
}