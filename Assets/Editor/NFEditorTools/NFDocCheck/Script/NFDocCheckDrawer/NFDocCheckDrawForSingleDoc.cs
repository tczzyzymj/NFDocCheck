using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using OfficeOpenXml;
using UnityEngine.PlayerLoop;


public class NFDocCheckDrawForSingleDoc : NFDocCheckDrawBase
{
    // 这里去获取一下
    private string mFilePath = string.Empty;


    private Vector2 mPosVector = Vector2.zero;


    private List<string> mKeyList = null;


    public bool InitTarget(string fileFullPath)
    {
        if (string.IsNullOrEmpty(fileFullPath))
        {
            Debug.LogError("传入的文件路径为空！请检查！");

            return false;
        }

        if (!File.Exists(fileFullPath))
        {
            Debug.LogError($"文件:{fileFullPath} 不存在，请检查！");

            return false;
        }

        if (!fileFullPath.EndsWith(".xlsx") && !fileFullPath.EndsWith(".xls"))
        {
            Debug.LogError($"文件:{fileFullPath} 的拓展名不正确，请检查！");

            return false;
        }

        if (!InternalAnalys(fileFullPath))
        {
            return false;
        }

        mFilePath = fileFullPath;

        return true;
    }


    private bool InternalAnalys(string fileFullPath)
    {
        var _excelHelper = new NFExcelHelper();

        var _startColIndex = NFDocCheckWindow.Ins.DocCheckScriptableData.ConfigData.StartColIndex;

        var _startRowIndex = NFDocCheckWindow.Ins.DocCheckScriptableData.ConfigData.StartRowIndex;

        // 这里去记录一下KEY就行了
        _excelHelper.LoopThroughExcel(
            fileFullPath,
            (
                workSheet,
                range,
                maxRowCount,
                maxColCount
            ) =>
            {
                mKeyList = new List<string>(maxColCount);

                for (int _colIndex = _startColIndex; _colIndex < maxColCount; ++_colIndex)
                {
                    var _tempValue = range[_startRowIndex, _colIndex].Value;

                    if (_tempValue == null)
                    {
                        break;
                    }

                    var _finalStr = _tempValue.ToString();

                    if (string.IsNullOrEmpty(_finalStr))
                    {
                        break;
                    }

                    mKeyList.Add(_finalStr);
                }
            },
            false
        );

        return true;
    }


    public override void Draw()
    {
        if (GUILayout.Button("返回"))
        {
            NFDocCheckWindow.Ins.ShowTotalDoc();
        }

        if (mKeyList == null || mKeyList.Count < 1)
        {
            EditorGUILayout.LabelField($"没有内容，请检查表格：{mFilePath}");

            return;
        }

        {
            mPosVector = EditorGUILayout.BeginScrollView(mPosVector);

            {
                for (int i = 0; i < mKeyList.Count; ++i)
                {
                    EditorGUILayout.BeginHorizontal("HelpBox");

                    {
                        EditorGUILayout.LabelField(mKeyList[i], GUILayout.Height(30));

                        if (GUILayout.Button("设置", GUILayout.Height(30)))
                        {
                        }
                    }

                    EditorGUILayout.EndHorizontal();

                    if (i < mKeyList.Count - 1)
                    {
                        EditorGUILayout.Space();
                    }
                }
            }

            EditorGUILayout.EndScrollView();
        }
    }
}
