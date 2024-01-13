using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections;
using System.Reflection;

namespace OSS.Tools.Excel;

public static class ExcelFileExtension
{
    /// <summary>
    /// 从Excel文件加载列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="stream">Excel文件流</param>
    /// <param name="titleDefineRowIndex">标题定义所在行索引（从0开始）</param>
    /// <param name="dataStartRowIndex">数据起始行索引（从0开始）</param>
    /// <returns></returns>
    public static List<T> LoadListFromExcelFile<T>(this Stream stream, int titleDefineRowIndex = 0,
        int dataStartRowIndex = 1)
        where T : class, new()
    {
        return LoadListFromExcelFile<T>(stream, string.Empty, null, titleDefineRowIndex, dataStartRowIndex);
    }

    /// <summary>
    /// 从Excel文件加载列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="stream">Excel文件流</param>
    /// <param name="sheetName">执行sheet名称</param>
    /// <param name="titleDefineRowIndex">标题定义所在行索引（从0开始）</param>
    /// <param name="dataStartRowIndex">数据起始行索引（从0开始）</param>
    /// <returns></returns>
    public static List<T> LoadListFromExcelFile<T>(this Stream stream, string sheetName,
        int titleDefineRowIndex = 0, int dataStartRowIndex = 1)
        where T : class, new()
    {
        return LoadListFromExcelFile<T>(stream, sheetName, null, titleDefineRowIndex, dataStartRowIndex);
    }


    /// <summary>
    /// 从Excel文件加载列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="stream">Excel文件流</param>
    /// <param name="rowFilter">
    ///     行数据过滤方法(参数0，参数1，参数2) 。
    ///     如果为空，值转化异常时会直接抛出； 如果不为空，值转化异常会被拦截并通过当前方法的 参数2 传递
    ///     返回 true - 数据有效， 返回false - 数据无效
    ///     <br/>   --参数0：数据所在Excel行索引，
    ///     <br/>   --参数1：转化后的行数据，
    ///     <br/>   --参数2：列属性转化错误列表 - &lt;Excel标题，错误信息&gt;
    /// </param>
    /// <param name="titleDefineRowIndex">标题定义所在行索引（从0开始）</param>
    /// <param name="dataStartRowIndex">数据起始行索引（从0开始）</param>
    /// <returns></returns>
    public static List<T> LoadListFromExcelFile<T>(this Stream stream,
        Func<int, T, List<ErrorPropertyMessage>?, bool>? rowFilter, int titleDefineRowIndex = 0,
        int dataStartRowIndex = 1)
        where T : class, new()
    {
        return LoadListFromExcelFile(stream, string.Empty, rowFilter, titleDefineRowIndex, dataStartRowIndex);
    }


    /// <summary>
    /// 从Excel文件加载列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="stream">Excel文件流</param>
    /// <param name="sheetName">执行sheet名称</param>
    /// <param name="rowFilter">
    ///     行数据过滤方法(参数0，参数1，参数2) 。
    ///     如果为空，值转化异常时会直接抛出； 如果不为空，值转化异常会被拦截并通过当前方法的 参数2 传递
    ///     返回 true - 数据有效， 返回false - 数据无效
    ///     <br/>   --参数0：数据所在Excel行索引，
    ///     <br/>   --参数1：转化后的行数据，
    ///     <br/>   --参数2：列属性转化错误列表 - &lt;Excel标题，错误信息&gt;
    /// </param>
    /// <param name="titleDefineRowIndex">标题定义所在行索引（从0开始）</param>
    /// <param name="dataStartRowIndex">数据起始行索引（从0开始）</param>
    /// <returns></returns>
    public static List<T> LoadListFromExcelFile<T>(this Stream stream, string sheetName,
        Func<int, T, List<ErrorPropertyMessage>?, bool>? rowFilter, int titleDefineRowIndex = 0,
        int dataStartRowIndex = 1)
        where T : class, new()
    {
        using var workbook = new XSSFWorkbook(stream);

        var sheet = string.IsNullOrEmpty(sheetName)
            ? workbook.GetSheetAt(0)
            : workbook.GetSheet(sheetName);

        var excelProperties = GetExcelPropertiesFromTitleRow(sheet, typeof(T), ref titleDefineRowIndex);

        var rowMaxIndex = sheet.LastRowNum;
        var resList     = new List<T>(rowMaxIndex);

        if (dataStartRowIndex < sheet.FirstRowNum)
            dataStartRowIndex = sheet.FirstRowNum;

        for (var rowIndex = dataStartRowIndex; rowIndex <= rowMaxIndex; rowIndex++)
        {
            var rowObj = sheet.GetRow(rowIndex);
            var item   = GetRowItem(rowObj, rowIndex, excelProperties, rowFilter);
            if (item != null)
            {
                resList.Add(item);
            }
        }

        return resList;
    }

    private static T? GetRowItem<T>(IRow? rowObj, int rowIndex, List<ExcelProperty> colPropertyMap,
        Func<int, T, List<ErrorPropertyMessage>?, bool>? rowFilter) where T : class, new()
    {
        if (rowObj == null)
            return null;

        var                         resItem = new T();
        List<ErrorPropertyMessage>? errors  = null;

        foreach (var excelProperty in colPropertyMap)
        {
            var cell = rowObj.GetCell(excelProperty.ExcelColIndex);
            if (cell == null)
                continue;

            var valType = excelProperty.IsDictionary
                ? excelProperty.ItemProperty.PropertyType
                : excelProperty.Property.PropertyType;

            try
            {
                var cellVal = cell.GetCellValue(cell.CellType, valType);

                excelProperty.SetValue(resItem, cellVal);
            }
            catch
            {
                if (rowFilter == null)
                    throw;

                errors ??= new List<ErrorPropertyMessage>();
                errors.Add(
                    new ErrorPropertyMessage()
                    {
                        title         = excelProperty.ExcelColTitle,
                        err_msg       = $"值({cell})转化类型({valType.Name})错误",
                        property_name = excelProperty.Property.Name,
                        column_index  = excelProperty.ExcelColIndex
                    });
            }
        }

        if (rowFilter == null || rowFilter(rowIndex, resItem, errors))
            return resItem;

        return null;
    }



    /// <summary>
    /// 获取Excel列和实体属性的映射关系实体
    /// </summary>
    /// <param name="sheet"></param>
    /// <param name="entType"></param>
    /// <param name="titleDefineRow"></param>
    /// <returns></returns>
    private static List<ExcelProperty> GetExcelPropertiesFromTitleRow(ISheet sheet, IReflect entType,
        ref int titleDefineRow)
    {
        if (titleDefineRow < sheet.FirstRowNum)
            titleDefineRow = sheet.FirstRowNum;

        var properties =
            entType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty);
        var colProperties = new List<ExcelProperty>(properties.Length);

        var sheetTitleRow    = sheet.GetRow(titleDefineRow);
        var sheetColumnCount = sheetTitleRow.LastCellNum;

        foreach (var p in properties)
        {
            var attr     = p.GetCustomAttribute<ExcelTitleAttribute>(true);
            var isDic    = attr?.IsDictionary ?? false;
            var titleKey = attr?.TitleName ?? p.Name;

            var isDictionary   = attr is { IsDictionary: true };
            var needFillLatest = p.GetCustomAttribute<ExcelFillLatestValueAttribute>() != null;

            for (var i = 0; i < sheetColumnCount; i++)
            {
                if (colProperties.Any(c => c.ExcelColIndex == i))
                    continue;

                var excelTitle = sheetTitleRow.GetCell(i)?.ToString() ?? string.Empty;
                if ((!isDic && excelTitle != titleKey) || (isDic && !excelTitle.StartsWith(titleKey)))
                    continue;

                var excelProperty = GetExcelProperty(excelTitle, i, p, isDictionary, needFillLatest);
                colProperties.Add(excelProperty);
            }
        }

        return colProperties;
    }

    private static ExcelProperty GetExcelProperty( string excelTitle, int index,
        PropertyInfo property, bool isDictionary, bool needFillByLatest)
    {
        var excelProperty = new ExcelProperty()
        {
            //TitleKey              = titleKey,
            ExcelColIndex         = index,
            ExcelColTitle         = excelTitle,
            Property              = property,
            NeedFillByLatestValue = needFillByLatest
        };
        if (!isDictionary)
            return excelProperty;

        if (!property.PropertyType.IsAssignableTo(typeof(IDictionary))
            || property.PropertyType.GetGenericArguments()[0] != typeof(string))
        {
            throw new ArgumentException($"动态列属性({property.Name})必须是字典类型(且Key必须是string类型)");
        }

        excelProperty.IsDictionary = isDictionary;
        excelProperty.ItemProperty =
            property.PropertyType.GetProperty("Item", BindingFlags.Public | BindingFlags.Instance)!;

        return excelProperty;
    }


    /// <summary>
    ///  获取单元格值
    ///     如果是计算类型，需要获取计算后的类型值
    /// </summary>
    /// <returns></returns>
    private static object? GetCellValue(this ICell? cell, CellType cellType, Type properType)
    {
        if (cell == null)
            return null;

        switch (cellType)
        {
            case CellType.Formula:
                return GetCellValue(cell, cell.CachedFormulaResultType, properType);
            case CellType.Numeric:
                if (properType == typeof(DateTime)
                    || properType == typeof(DateTime?))
                {
                    return cell.DateCellValue;
                }

                return Convert.ChangeType(cell.NumericCellValue, properType);


            case CellType.Boolean:
                return Convert.ChangeType(cell.BooleanCellValue, properType);
            case CellType.String:
                return Convert.ChangeType(cell.StringCellValue, properType);

            case CellType.Unknown:
            case CellType.Blank:
            case CellType.Error:
            default:
                return Convert.ChangeType(cell.ToString(), properType);
        }
    }
}