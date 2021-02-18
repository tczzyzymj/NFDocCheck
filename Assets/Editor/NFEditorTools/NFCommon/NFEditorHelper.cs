using System;
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
        if (string.IsNullOrEmpty(originPath))
        {
            return string.Empty;
        }

        var _firstIndex = originPath.IndexOf("Assets", StringComparison.Ordinal);

        return originPath.Substring(_firstIndex);
    }


    /// <summary>
    /// 获取相对于 Application.dataPath 的一个路径
    /// </summary>
    /// <returns></returns>
    public static string GetRelatePathToApplicationDataPath(string targetPath)
    {
        // 这里去获取一下相对路径
        var _dataPath = Application.dataPath;

        var _selectSplitArray = targetPath.Split('/');
        var _dataSplitArray = _dataPath.Split('/');

        int _compareIndex = 0;

        // 应该不会嵌套文件夹超过 1000 层这样的吧，默认
        for (int _i = 0; _i < 1000; ++_i)
        {
            if (_compareIndex >= _selectSplitArray.Length ||
                _compareIndex >= _dataSplitArray.Length)
            {
                break;
            }

            // 如果不一样，就裂开
            if (!_selectSplitArray[_compareIndex].Equals(
                _dataSplitArray[_compareIndex],
                StringComparison.Ordinal
            ))
            {
                break;
            }

            ++_compareIndex;
        }

        if (_compareIndex == 0)
        {
            // 这里表示根目录都不一样，就直接记录好了
            return targetPath;
        }

        // 这里表示是同一个目录
        var _leftIndex = _dataSplitArray.Length - _compareIndex;

        string _tempStr = string.Empty;

        for (int i = 0; i < _leftIndex; ++i)
        {
            _tempStr += @"../";
        }

        for (int i = _compareIndex; i < _selectSplitArray.Length; ++i)
        {
            if (i == _compareIndex)
            {
                // 第一个不加 / 符号
                _tempStr += $"{_selectSplitArray[i]}";
            }
            else
            {
                // 后面的加上符号
                _tempStr += $"/{_selectSplitArray[i]}";
            }
        }

        return _tempStr;
    }
}
