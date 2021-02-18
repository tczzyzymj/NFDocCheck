using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


public class NFDocCheckDrawForConfig : NFDocCheckDrawBase
{
    public override void Draw()
    {
        EditorGUILayout.BeginHorizontal();

        {
            if (GUILayout.Button("保存设置"))
            {
                OnClickSaveButton();
            }

            if (GUILayout.Button("返回主目录"))
            {
                OnClickReturnButton();
            }
        }

        EditorGUILayout.EndHorizontal();

        // 这里绘制一下设置
        EditorGUILayout.BeginHorizontal();

        {
            if (GUILayout.Button("选择Doc文件夹", GUILayout.Width(100)))
            {
                var _fullPath = string.Empty;

                if (!string.IsNullOrEmpty(NFDocCheckWindow.Ins.DocCheckScriptableData.ConfigData.DocFolderRelativePath))
                {
                    _fullPath = Path.Combine(
                        Application.dataPath,
                        NFDocCheckWindow.Ins.DocCheckScriptableData.ConfigData.DocFolderRelativePath
                    );
                }

                var _selectFolderPath = EditorUtility.OpenFolderPanel(
                    "选择Doc文件夹",
                    _fullPath,
                    string.Empty
                );

                NFDocCheckWindow.Ins.DocCheckScriptableData.ConfigData.DocFolderRelativePath =
                    NFEditorHelper.GetRelatePathToApplicationDataPath(
                        _selectFolderPath
                    );

                NFDocCheckWindow.Ins.RefreshDocFolderFullPath();
            }

            EditorGUILayout.LabelField(NFDocCheckWindow.Ins.DocCheckScriptableData.ConfigData.DocFolderRelativePath);
        }

        EditorGUILayout.EndHorizontal();

        NFDocCheckWindow.Ins.DocCheckScriptableData.ConfigData.StartRowIndex = EditorGUILayout.IntField(
            "KEY的行下标(从1开始)",
            NFDocCheckWindow.Ins.DocCheckScriptableData.ConfigData.StartRowIndex
        );

        NFDocCheckWindow.Ins.DocCheckScriptableData.ConfigData.StartColIndex = EditorGUILayout.IntField(
            "表格开始列下标(从1开始)",
            NFDocCheckWindow.Ins.DocCheckScriptableData.ConfigData.StartColIndex
        );

        NFDocCheckWindow.Ins.DocCheckScriptableData.ConfigData.SplitSymbol = EditorGUILayout.TextField(
            "表格数组分割符",
            NFDocCheckWindow.Ins.DocCheckScriptableData.ConfigData.SplitSymbol
        );
    }


    private void OnClickSaveButton()
    {
        // 这里先检测一下文件夹是否存在
        FileInfo _info = new FileInfo(NFDocCheckWindow.Ins.ScriptLogicDataFilePath);

        if (_info == null || _info.Directory == null)
        {
            Debug.LogError("错误，无法获取路径：" + NFDocCheckWindow.Ins.ScriptLogicDataFilePath);

            return;
        }

        if (!_info.Directory.Exists)
        {
            _info.Directory.Create();
        }

        bool _success = false;

        try
        {
            if (File.Exists(NFDocCheckWindow.Ins.ScriptLogicDataFilePath))
            {
                EditorUtility.SetDirty(NFDocCheckWindow.Ins.DocCheckScriptableData);

                AssetDatabase.SaveAssets();
            }
            else
            {
                AssetDatabase.CreateAsset(
                    NFDocCheckWindow.Ins.DocCheckScriptableData,
                    NFEditorHelper.GetAssetDatabasePath(NFDocCheckWindow.Ins.ScriptLogicDataFilePath)
                );
            }

            _success = true;
        }
        catch (Exception _e)
        {
            Debug.LogError(_e.ToString());
        }

        if (_success)
        {
            EditorUtility.DisplayDialog(string.Empty, "保存成功", "OK");
        }
    }


    private void OnClickReturnButton()
    {
        NFDocCheckWindow.Ins.ShowMainCatalog();
    }
}
