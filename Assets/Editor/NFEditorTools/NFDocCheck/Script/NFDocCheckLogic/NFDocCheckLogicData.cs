using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 这里存储列需要处理的逻辑数据，
/// </summary>
[Serializable]
public class NFDocCheckLogicData
{
    /// <summary>
    /// 比较的类型，到时候不会实例化多个，单一的的就可以了
    /// </summary>
    [SerializeField]
    public NFDocCheckProcessorType ProcessorType
    {
        get;
        set;
    }


    /// <summary>
    /// 做比较的源列名，所以修改列名的时候得特别注意
    /// </summary>
    [SerializeField]
    public string SourceColName
    {
        get;
        set;
    }


    /// <summary>
    /// 比较目标的 Excel 名字
    /// </summary>
    [SerializeField]
    public string TargetExcelName
    {
        get;
        set;
    }


    /// <summary>
    /// 比较目标的列名
    /// </summary>
    [SerializeField]
    public string TargetColName
    {
        get;
        set;
    }
}
