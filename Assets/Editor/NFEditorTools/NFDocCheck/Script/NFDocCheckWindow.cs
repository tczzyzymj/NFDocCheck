using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;


public class NFDocCheckWindow : EditorWindow
{
    private static NFDocCheckWindow mIns;


    private string mConfigFilePath = string.Empty;


    public string ConfigFilePath
    {
        get
        {
            return mConfigFilePath;
        }
    }


    private NFDocCheckConfig mConfig = null;


    public NFDocCheckConfig DocCheckConfig
    {
        get
        {
            return mConfig;
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
        mConfigFilePath = Path.Combine(
            Application.dataPath,
            string.Format(
                "Editor/{0}/NFDocCheck/Config/Config.asset",
                NFEditorPath.EditorFolderName
            )
        );

        mConfig = AssetDatabase.LoadAssetAtPath<NFDocCheckConfig>(NFEditorHelper.GetAssetDatabasePath(mConfigFilePath));

        if (mConfig == null)
        {
            mConfig = ScriptableObject.CreateInstance<NFDocCheckConfig>();
        }

        if (mMainCatalogDrawer == null)
        {
            mMainCatalogDrawer = new NFDocCheckDrawForMainCatalog();
        }

        mCurrentDrawer = mMainCatalogDrawer;

        var _tempPath = Path.Combine(
            Application.dataPath,
            this.DocCheckConfig.DocFolderRelativePath
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
