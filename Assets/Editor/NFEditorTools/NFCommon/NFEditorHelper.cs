using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 这是一个帮助类
/// </summary>
public static class NFEditorHelper
{
    /// <summary>
    /// 获取AssetDatabase能用的路径
    /// </summary>
    /// <param name="originPath"></param>
    public static string GetAssetDatabasePath(string originPath)
    {
        var _firstIndex = originPath.IndexOf("Assets");

        return originPath.Substring(_firstIndex);
    }
}
