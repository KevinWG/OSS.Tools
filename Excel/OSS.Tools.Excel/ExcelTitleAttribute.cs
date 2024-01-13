namespace OSS.Tools.Excel;

/// <summary>
/// Excel自定义标题属性
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class ExcelTitleAttribute : Attribute
{
    /// <summary>
    ///  是否是动态列字典
    /// </summary>
    internal bool IsDictionary { get; set; } = false;

    /// <summary>
    /// excel 对应的标题
    /// </summary>
    internal string TitleName { get; set; }

    /// <summary>
    /// Excel自定义标题类构造函数
    /// </summary>
    /// <param name="titleName">自定义列名</param>
    public ExcelTitleAttribute(string titleName)
    {
        TitleName = titleName;
    }
}


/// <summary>
/// Excel 动态列字典 ，字典的 key 必须是 String 类型，Value 可以指定类型
///    <br/>  可以指定 Excel 标题以特定前缀开头的列放入当前字典，格式化每行数据时，Key对应的是当前行对应的列标题名，Value对应的是单元格值
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class ExcelTitleDictionaryAttribute : ExcelTitleAttribute
{
    /// <summary>
    /// Excel字典动态列
    /// </summary>
    /// <param name="preTitleName">自定义标题前缀</param>
    public ExcelTitleDictionaryAttribute(string preTitleName) : base(preTitleName)
    {
        IsDictionary = true;
    }
}



/// <summary>
///  Excel使用最近值单元格
///  <br/>   如果单元格为空，同列向上找最近非空单元格，取对应值
///  <br/>   如果不为空，使用当前值
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class ExcelFillLatestValueAttribute : Attribute
{

    /// <summary>
    /// Excel使用最近值单元格
    ///     如果单元格为空，同列向上找最近非空单元格，取对应值
    ///     如果不为空，使用当前值
    /// </summary>
    public ExcelFillLatestValueAttribute() 
    {
      
    }
}


