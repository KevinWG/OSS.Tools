namespace OSS.Tools.Excel;

public class ErrorPropertyMessage
{
    /// <summary>
    /// 列索引
    /// </summary>
    public int column_index { get; set; }

    /// <summary>
    ///  列标题
    /// </summary>
    public string title { get; set; } = string.Empty;

    /// <summary>
    ///  字段属性名称
    /// </summary>
    public string property_name { get; set; } = string.Empty;

    /// <summary>
    ///  错误信息
    /// </summary>
    public string err_msg { get; set; } = string.Empty;
}