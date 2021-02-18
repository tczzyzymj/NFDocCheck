using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using OfficeOpenXml;


public class NFExcelHelper
{
    /// <summary>
    /// 注意，在比较的时候 MaxRowCount MaxColCount 的时候，一定要用 <= 或者 >=，是包含了最后一个数的
    /// </summary>
    /// <param name="targetPackage">目标Excel</param>
    /// <param name="loopCallback">参数表示 _workSheet, _cells, _maxRowCount, _maxColCount </param>
    /// <param name="needSave"></param>
    /// <param name="create"></param>
    public void LoopThroughExcel(
        ExcelPackage targetPackage,
        Action<ExcelWorksheet, ExcelRange, int, int> loopCallback,
        bool needSave,
        bool create = false
    )
    {
        if (targetPackage == null)
        {
            Debug.LogError("传入的 ExcelPackage 为空，请检查！");

            return;
        }

        var _package = targetPackage;

        var _workBood = _package.Workbook;

        if (_workBood.Worksheets.Count < 1)
        {
            if (create)
            {
                _workBood.Worksheets.Add("Sheet1");
            }
            else
            {
                Debug.LogError("表格无法读取，请检查！");

                return;
            }
        }

        ExcelWorksheet _valueSheet = _workBood.Worksheets[1];

        if (_valueSheet == null)
        {
            Debug.LogErrorFormat(
                "Excel 表格:{0} 格式不对，请检查代码！",
                targetPackage.ToString()
            );

            return;
        }

        var _workSheet = _valueSheet.Workbook.Worksheets[1];

        var _cells = _workSheet.Cells;

        var _maxRowCount = 1;

        if (_workSheet.Dimension?.End?.Row != null)
        {
            _maxRowCount = _workSheet.Dimension.End.Row;
        }

        if (_maxRowCount > 10000)
        {
            Debug.LogError(
                string.Format(
                    "警告，表格 ： [{0}] 的 行 超过4000, 共 ：【{1}】行，请检查！",
                    targetPackage.File.Name,
                    _maxRowCount
                )
            );
        }

        var _maxColCount = 1;

        if (_workSheet?.Dimension?.End?.Column != null)
        {
            _maxColCount = _workSheet.Dimension.End.Column;
        }

        if (_maxColCount > 100)
        {
            Debug.LogError(
                string.Format(
                    "警告，表格 ： [{0}] 的 列 超过100, 共 ：【{1}】行，请检查！",
                    targetPackage.File.Name,
                    _maxColCount
                )
            );
        }

        loopCallback(_workSheet, _cells, _maxRowCount, _maxColCount);

        if (needSave)
        {
            _package.Save();
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="filePath">目标Excel</param>
    /// <param name="loopCallback">参数表示 _workSheet, _cells, _maxRowCount, _maxColCount </param>
    /// <param name="needSave"></param>
    public void LoopThroughExcel(
        string filePath,
        Action<ExcelWorksheet, ExcelRange, int, int> loopCallback,
        bool needSave
    )
    {
        if (string.IsNullOrEmpty(filePath))
        {
            Debug.LogError("传入的路径为空，请检查！");

            return;
        }

        if (loopCallback == null)
        {
            Debug.LogError("没有回调函数，请检查！");

            return;
        }

        FileInfo _existingFile = new FileInfo(filePath);

        if (!_existingFile.Exists)
        {
            Debug.LogError($"传入的文件路径：{_existingFile} 不存在，请检查！");

            return;
        }

        using (ExcelPackage _package = new ExcelPackage(_existingFile))
        {
            LoopThroughExcel(_package, loopCallback, needSave);
        }
    }


    public bool DeleteExcelBySingleID(ExcelPackage targetPackage, int targetID, bool isCSVDataExcel)
    {
        if (targetPackage == null)
        {
            Debug.LogError("传入的参数 ExcelPackage 为空，请检查！");

            return false;
        }

        LoopThroughExcel(
            targetPackage,
            (
                targetSheet,
                targetRange,
                maxRowCount,
                maxColCount
            ) =>
            {
                for (int i = 5; i <= maxRowCount; ++i)
                {
                    var _stringValue = targetRange[i, 2].Value.ToString();

                    if (!int.TryParse(_stringValue, out var _tempID))
                    {
                        continue;
                    }

                    if (_tempID == targetID)
                    {
                        targetSheet.DeleteRow(i);

                        if (isCSVDataExcel && i == 5)
                        {
                            targetRange[5, 1].Value = "Value";
                        }

                        return;
                    }
                }
            },
            true
        );

        return true;
    }


    /// <summary>
    /// 默认修改 WorkSheet1
    /// </summary>
    /// <param name="targetPackage"></param>
    /// <param name="index"></param>
    /// <param name="needSave"></param>
    /// <param name="isCSVDataExcel"></param>
    /// <returns></returns>
    public bool DeleteExcelRow(
        ExcelPackage targetPackage,
        int index,
        bool needSave,
        bool isCSVDataExcel
    )
    {
        if (targetPackage == null)
        {
            Debug.LogErrorFormat("传入的 Package 为空，请检查！");

            return false;
        }

        if (index <= 0)
        {
            Debug.LogErrorFormat("传入的 Index 小于等于0 ，请重新输入!");

            return false;
        }

        var _package = targetPackage;

        var _workBood = _package.Workbook;

        if (_workBood.Worksheets.Count < 1)
        {
            Debug.LogError("表格无法读取，请检查！");

            return false;
        }

        var _valueSheet = _workBood.Worksheets[1];

        if (_valueSheet == null)
        {
            Debug.LogErrorFormat(
                "表格：{0} 格式不对，请检查代码！",
                targetPackage.ToString()
            );

            return false;
        }

        var _workSheet = _valueSheet.Workbook.Worksheets[1];

        var _maxRowCount = _workSheet.Dimension.End.Row + 1;

        if (index >= _maxRowCount)
        {
            Debug.LogErrorFormat(
                "传入的index : {0} 超过了最大值 : {1}，请检查！",
                index,
                _maxRowCount
            );

            return false;
        }

        _workSheet.DeleteRow(index);

        if (needSave)
        {
            targetPackage.Save();
        }

        return true;
    }


    public bool InsertExcelDataAtEnd(
        string filePath,
        int targetKey,
        params string[] args
    )
    {
        bool _result = false;

        try
        {
            if (string.IsNullOrEmpty(filePath))
            {
                Debug.LogError("传入的路径为空！请检查！");

                return false;
            }

            if (args == null || args.Length < 1)
            {
                Debug.LogError("传入的参数为空！请检查！");

                return false;
            }

            FileInfo _existingFile = new FileInfo(filePath);

            using (ExcelPackage _package = new ExcelPackage(_existingFile))
            {
                _result = InsertExcelDataAtEnd(_package, targetKey, args);
            }
        }
        catch (Exception _e)
        {
            Debug.LogError(_e.ToString());
        }

        return _result;
    }


    /// <summary>
    /// 默认修改的是WorkSheet1
    /// </summary>
    /// <param name="fileFullPath"></param>
    /// <param name="index"></param>
    /// <param name="needSave"></param>
    /// <param name="isCSVDataExcel">如果是，那么在5,1的地方应该是Value</param>
    /// <returns></returns>
    public bool DeleteExcelRow(
        string fileFullPath,
        int index,
        bool needSave,
        bool isCSVDataExcel
    )
    {
        if (String.IsNullOrEmpty(fileFullPath))
        {
            Debug.LogErrorFormat("传入的路径为空，请检查！");

            return false;
        }

        bool _result = false;

        FileInfo _existingFile = new FileInfo(fileFullPath);

        using (ExcelPackage _package = new ExcelPackage(_existingFile))
        {
            _result = DeleteExcelRow(_package, index, needSave, isCSVDataExcel);
        }

        return _result;
    }


    public bool InsertExcelDataAtEnd(
        ExcelPackage targetExcelPackage,
        int targetKey,
        params string[] args
    )
    {
        if (targetExcelPackage == null)
        {
            Debug.LogError("错误，传入的Package为空！");

            return false;
        }

        bool _result = false;

        try
        {
            var _package = targetExcelPackage;

            {
                var _workBood = _package.Workbook;

                if (_workBood.Worksheets.Count < 1)
                {
                    Debug.LogError("表格无法读取，请检查！");

                    return false;
                }

                var _valueSheet = _workBood.Worksheets[1];

                if (_valueSheet == null)
                {
                    Debug.LogErrorFormat(
                        "表格：{0} 格式不对，请检查代码！",
                        targetExcelPackage.ToString()
                    );

                    return false;
                }

                var _workSheet = _valueSheet.Workbook.Worksheets[1];

                var _cells = _workSheet.Cells;

                var _maxRowCount = _workSheet.Dimension.End.Row + 1;

                if (_maxRowCount > 4000)
                {
                    Debug.LogError(
                        String.Format(
                            "警告，表格 ： Timelnie 的 行 超过4000 为 ：【{0}】行，请检查！",
                            _maxRowCount
                        )
                    );
                }

                var _maxColCount = _workSheet.Dimension.End.Column;

                if (_maxColCount > 100)
                {
                    Debug.LogError(
                        String.Format(
                            "警告，表格 ： Timeline 的 列 超过100 为 ：【{0}】列，请检查！",
                            _maxColCount
                        )
                    );
                }

                int _startColIndex = 2;

                for (int _colIndex = _startColIndex; _colIndex <= _maxColCount; ++_colIndex)
                {
                    if (_colIndex == _startColIndex)
                    {
                        _cells[_maxRowCount, _colIndex].Value = targetKey.ToString();
                    }
                    else
                    {
                        var _targetIndex = _colIndex - _startColIndex - 1;

                        if (_targetIndex >= args.Length)
                        {
                            break;
                        }

                        _cells[_maxRowCount, _colIndex].Value = args[_targetIndex];
                    }
                }

                _package.Save();

                _result = true;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return _result;
    }
}
