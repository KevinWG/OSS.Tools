using System.Reflection;

namespace OSS.Tools.Excel;

internal class ExcelProperty
{
    /// <summary>
    ///  所在列索引
    /// </summary>
    public int ExcelColIndex { get; set; }
        
    /// <summary>
    ///  Excel 标题名
    /// </summary>
    public string ExcelColTitle { get; set; } = string.Empty;

    ///// <summary>
    /////  <br/>IsDictionary = true 时，TitleKey =ExcelColTitle
    /////    <br/>IsDictionary = false 时，TitleKey 是 ExcelColTitle 前缀
    ///// </summary>
    //public string TitleKey { get; set; } = string.Empty;

    /// <summary>
    ///  字段属性信息
    /// </summary>
    public PropertyInfo Property { get; set; } = default!;




    #region 动态列属性

    /// <summary>
    /// 是否是动态字典列
    /// </summary>
    public bool IsDictionary { get; set; }

    /// <summary>
    ///  字典项属性（IsDictionary = true 时有值
    /// </summary>
    public PropertyInfo ItemProperty { get; set; } = default!;

    #endregion



    #region 自动根据最近使用数据填充属性

    /// <summary>
    /// 是否需要通过最近值填充
    /// </summary>
    public bool NeedFillByLatestValue { get; set; }

    /// <summary>
    /// 最近值
    /// </summary>
    public object? LatestValue { get; set; }

    #endregion


    public void SetValue(object obj, object? value)
    {
        if (NeedFillByLatestValue)
        {
            if (value != null && !string.IsNullOrEmpty(value.ToString()))
            {
                LatestValue = value;
            }
            else
            {
                if (LatestValue !=null)
                {
                    value = LatestValue;
                }
            }
        }
        
        if (value==null)
            return;
        
        if (!IsDictionary)
        {
            Property.SetValue(obj, value);
            return;
        }

        var dirVal = Property.GetValue(obj);
        if (dirVal == null)
        {
            dirVal = Activator.CreateInstance(Property.PropertyType);
            Property.SetValue(obj,dirVal);
        }
        
        ItemProperty.SetValue(dirVal, value, new object[] { ExcelColTitle });  
    }
}