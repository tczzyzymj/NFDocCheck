using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class NFDocCheckConfig : ScriptableObject
{
    /// <summary>
    /// 开始的列下标
    /// </summary>
    public int StartColIndex;


    /// <summary>
    /// 开始的行下标
    /// </summary>
    public int StartRowindex;


    /// <summary>
    /// doc文件的相对路径，因为可能有不同的人使用，所以这里最好是这样做
    /// </summary>
    public string DocFolderRelativePath;


    /// <summary>
    /// 分割符，表里面有数组类型的化，统一用一个符号表示数组的分割
    /// </summary>
    public string SplitSymbol;
}
