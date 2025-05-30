using System;
using System.Runtime.CompilerServices;
using System.IO;
using System.Text;
using UnityEngine;
using DebugExtensions;


public class Debug : UnityEngine.Debug
{

    private static DebugSettingsLocalCache LocalCache => DebugSettingsLocalCache.instance;

    [HideInCallstack]
    public static void Log(object message, string specifiedHexColor = null, [CallerFilePath] string callerFilePath = null)
    {

        callerFilePath = callerFilePath.Replace("\\", "/");

        if (PathInExcludeDir(callerFilePath))
        {
            return;
        }

        if (!PathInIncludeDir(callerFilePath))
        {
            return;
        }

        if (specifiedHexColor != null)
        {
            string log = ProcessLog(message, specifiedHexColor);
            unityLogger.Log(LogType.Log, log);
        }
        else
        {
            string log = ProcessLog(message, RetrieveColor(callerFilePath));
            unityLogger.Log(LogType.Log, log);
        }
    }

    [HideInCallstack]
    private static bool PathInExcludeDir(string callerFilePath)
    {
        foreach (var item in LocalCache.ExcludeDir)
        {
            if (item.Enable)
            {
                if (!string.IsNullOrEmpty(item.DirPath))
                {
                    if (callerFilePath.Contains(item.DirPath))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    [HideInCallstack]
    private static bool PathInIncludeDir(string callerFilePath)
    {
        var enabledCondition = 0;

        foreach (var item in LocalCache.IncludeDir)
        {
            if (item.Enable)
            {
                enabledCondition++;
                if (!string.IsNullOrEmpty(item.DirPath))
                {
                    if (callerFilePath.Contains(item.DirPath))
                    {
                        return true;
                    }
                }
            }
        }

        if (enabledCondition > 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    [HideInCallstack]
    private static Color RetrieveColor(string callerFilePath)
    {
        foreach (var item in LocalCache.SpecifiedColor)
        {
            if (item.Enable)
            {
                if (!string.IsNullOrEmpty(item.FilePath))
                {
                    if (callerFilePath.Contains(item.FilePath))
                    {
                        return item.LogColor;
                    }
                }
            }
        }

        return LocalCache.DefaultColor;
    }

    [HideInCallstack]
    private static string ProcessLog(object message, Color unityColor)
    {
        string hexColor = ColorUtility.ToHtmlStringRGB(unityColor);

        return HandleStringBuilderEvent(message, hexColor);
    }

    [HideInCallstack]
    private static string ProcessLog(object message, string hexColor)
    {
        if (hexColor.Contains("#"))
        {
            hexColor = hexColor.Replace("#", "");
        }

        return HandleStringBuilderEvent(message, hexColor);
    }

    [HideInCallstack]
    private static string HandleStringBuilderEvent(object message, string hexColor)
    {
        string log;
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("<color=#");
        stringBuilder.Append(hexColor);
        stringBuilder.Append(">");
        stringBuilder.Append(message);
        stringBuilder.Append("</color>");
        log = stringBuilder.ToString();
        return log;
    }
}


