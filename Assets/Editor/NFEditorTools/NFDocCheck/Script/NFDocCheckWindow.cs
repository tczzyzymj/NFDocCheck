using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;


public class NFDocCheckWindow : EditorWindow
{
    private static NFDocCheckWindow mIns;


    private string mScriptLogicDataFilePath = string.Empty;


    public string ScriptLogicDataFilePath
    {
        get
        {
            return mScriptLogicDataFilePath;
        }
    }


    private NFDocCheckScriptableData mScriptableData = null;


    public NFDocCheckScriptableData DocCheckScriptableData
    {
        get
        {
            return mScriptableData;
        }
    }


    /// <summary>
    /// DOC 文件夹的全路径，保存的是一个相对路径
    /// </summary>
    public string DocFolderFullPath
    {
        get;
        set;
    }


    private NFDocCheckDrawBase mCurrentDrawer = null;


    private NFDocCheckDrawForConfig mConfigDrawer = null;


    private NFDocCheckDrawForTotalDoc mTotalDocDraw = null;


    private NFDocCheckDrawForSingleDoc mSingleDocConfigDraw = null;


    private NFDocCheckDrawForMainCatalog mMainCatalogDrawer = null;


    public static NFDocCheckWindow Ins
    {
        get
        {
            return mIns;
        }
    }


    private void OnDestroy()
    {
        mIns = null;
    }


    private void Awake()
    {
        // 这里去读取一下
        mScriptLogicDataFilePath = Path.Combine(
            Application.dataPath,
            string.Format(
                "Editor/{0}/NFDocCheck/ScriptLogicData.asset",
                NFEditorPath.EditorFolderName
            )
        );

        mScriptableData =
            AssetDatabase.LoadAssetAtPath<NFDocCheckScriptableData>(
                NFEditorHelper.GetAssetDatabasePath(mScriptLogicDataFilePath)
            );

        if (mScriptableData == null)
        {
            mScriptableData = ScriptableObject.CreateInstance<NFDocCheckScriptableData>();
        }

        if (mMainCatalogDrawer == null)
        {
            mMainCatalogDrawer = new NFDocCheckDrawForMainCatalog();
        }

        mCurrentDrawer = mMainCatalogDrawer;

        RefreshDocFolderFullPath();
    }


    public void RefreshDocFolderFullPath()
    {
        if (string.IsNullOrEmpty(DocCheckScriptableData.ConfigData.DocFolderRelativePath))
        {
            return;
        }

        var _tempPath = Path.Combine(
            Application.dataPath,
            this.DocCheckScriptableData.ConfigData.DocFolderRelativePath
        );

        DirectoryInfo _info = new DirectoryInfo(_tempPath);

        if (!_info.Exists)
        {
            Debug.LogError($"错误，路径:[{_info.FullName}]不存在，请检查！");
        }

        this.DocFolderFullPath = _info.FullName + "\\";
    }


    public static void Showwindow()
    {
        if (mIns == null)
        {
            mIns = EditorWindow.CreateInstance<NFDocCheckWindow>();
        }

        if (mIns == null)
        {
            return;
        }

        mIns.Show();

        mIns.Focus();
    }


    public void ShowChangeConfig()
    {
        if (mConfigDrawer == null)
        {
            mConfigDrawer = new NFDocCheckDrawForConfig();
        }

        mCurrentDrawer = mConfigDrawer;
    }


    /// <summary>
    /// 绘制单个的
    /// </summary>
    /// <param name="fileFullPath">传入的是文件全路径，包含了拓展名</param>
    public void ShowSingleDocConfig(string fileFullPath)
    {
        if (mSingleDocConfigDraw == null)
        {
            mSingleDocConfigDraw = new NFDocCheckDrawForSingleDoc();
        }

        if (!mSingleDocConfigDraw.InitTarget(fileFullPath))
        {
            return;
        }

        mCurrentDrawer = mSingleDocConfigDraw;
    }


    /// <summary>
    /// 显示所有文件的总配置
    /// </summary>
    public void ShowTotalDoc()
    {
        if (mTotalDocDraw == null)
        {
            mTotalDocDraw = new NFDocCheckDrawForTotalDoc();
        }

        mCurrentDrawer = mTotalDocDraw;
    }


    public void ShowMainCatalog()
    {
        mCurrentDrawer = mMainCatalogDrawer;
    }


    private void OnGUI()
    {
        mCurrentDrawer?.Draw();
    }
}
