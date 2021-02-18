using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework.Interfaces;
using UnityEditor;
using UnityEngine;


public class NFDocCheckDrawForTotalDoc : NFDocCheckDrawBase
{
    public NFDocCheckDrawForTotalDoc()
    {
        CacheAllFiles();
    }


    private Dictionary<string, string> mAllFileDic = null;


    private Vector2 mScrollPos = Vector2.zero;


    private string mSearchStr = string.Empty;


    private Dictionary<string, string> mSearchDic = new Dictionary<string, string>();


    private Dictionary<string, string> mCurrentDic = null;


    private void CacheAllFiles()
    {
        var _folderPath = NFDocCheckWindow.Ins.DocFolderFullPath;

        var _tempArray = Directory.GetFiles(
            _folderPath,
            "*.*",
            SearchOption.AllDirectories
        );

        if (_tempArray.Length < 1)
        {
            return;
        }

        var _tempIE = _tempArray.Where(
            file =>
            {
                return file.EndsWith(".xlsx") || file.EndsWith(".xls");
            }
        );

        if (_tempIE == null)
        {
            return;
        }

        mAllFileDic = new Dictionary<string, string>(_tempArray.Length);

        using (var _finalIE = _tempIE.GetEnumerator())
        {
            while (_finalIE.MoveNext())
            {
                var _fileName = Path.GetFileName(_finalIE.Current);

                if (!string.IsNullOrEmpty(_fileName))
                {
                    mAllFileDic.Add(
                        _fileName,
                        _finalIE.Current
                    );
                }
            }
        }

        mCurrentDic = mAllFileDic;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="targetDic">key 是文件名，包含了扩展名；VALUE是全路径</param>
    private void DrawList(Dictionary<string, string> targetDic)
    {
        if (targetDic == null || targetDic.Count < 1)
        {
            return;
        }

        int _index = 0;

        foreach (var _pair in targetDic)
        {
            EditorGUILayout.BeginHorizontal("HelpBox");

            {
                // 这里绘制每一个的
                EditorGUILayout.LabelField(
                    _pair.Key,
                    GUILayout.Height(30)
                );

                if (GUILayout.Button("编辑检查功能", GUILayout.Height(30)))
                {
                    // 这里就去绘制单个的
                    NFDocCheckWindow.Ins.ShowSingleDocConfig(
                        _pair.Value
                    );
                }
            }

            EditorGUILayout.EndHorizontal();

            if (_index < targetDic.Count - 1)
            {
                EditorGUILayout.Space();
            }

            ++_index;
        }
    }


    public override void Draw()
    {
        EditorGUILayout.BeginHorizontal();

        {
            if (GUILayout.Button("返回主目录"))
            {
                NFDocCheckWindow.Ins.ShowMainCatalog();
            }

            if (GUILayout.Button("刷新文件"))
            {
                CacheAllFiles();
            }
        }

        EditorGUILayout.EndHorizontal();

        // 这里绘制所有的
        if (mCurrentDic == null || mCurrentDic.Count < 1)
        {
            EditorGUILayout.LabelField(
                string.Format(
                    "路径：{0} 下无法找到文件",
                    NFDocCheckWindow.Ins.DocCheckScriptableData.ConfigData.DocFolderRelativePath
                )
            );

            return;
        }

        // 查找功能
        {
            EditorGUILayout.BeginHorizontal();

            {
                mSearchStr = EditorGUILayout.TextField("查找内容", mSearchStr);

                if (GUILayout.Button("开始查找"))
                {
                    foreach (var _pair in mAllFileDic)
                    {
                        if (_pair.Key.Contains(mSearchStr))
                        {
                            mSearchDic.Add(_pair.Key, _pair.Value);
                        }
                    }

                    mCurrentDic = mSearchDic;

                    return;
                }

                if (GUILayout.Button("重置"))
                {
                    mCurrentDic = mAllFileDic;
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        // 滚动框
        {
            mScrollPos = EditorGUILayout.BeginScrollView(mScrollPos);

            {
                DrawList(mCurrentDic);
            }

            EditorGUILayout.EndScrollView();
        }
    }
}
