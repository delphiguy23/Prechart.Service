using System;
using System.Data;
using System.Globalization;

namespace Prechart.Service.Core.Extensions
{
    public static class DataTableExtensions
    {
        public static T ToObject<T>(this DataRow dataRow)
           where T : new()
        {
            var item = new T();
            foreach (DataColumn column in dataRow.Table.Columns)
            {
                if (dataRow[column] != DBNull.Value)
                {
                    try
                    {
                        var prop = item.GetType().GetProperty(column.ColumnName);

                        if (prop != null)
                        {
                            var nulltype = Nullable.GetUnderlyingType(prop.PropertyType);
                            var datatype = nulltype ?? prop.PropertyType;

                            if (!dataRow.IsNull(column))
                            {
                                try
                                {
                                    var result = Convert.ChangeType(dataRow[column], datatype, CultureInfo.InvariantCulture);
                                    prop.SetValue(item, result, null);
                                }
                                catch (FormatException)
                                {
                                    prop.SetValue(item, null, null);
                                }
                            }
                        }
                        else
                        {
                            var fld = item.GetType().GetField(column.ColumnName);
                            if (fld != null)
                            {
                                var result = Convert.ChangeType(dataRow[column], fld.FieldType, CultureInfo.InvariantCulture);
                                fld.SetValue(item, result);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new System.ArgumentException($"Error encountered migrating data for [{column.ColumnName}]", ex);
                    }
                }
            }

            return item;
        }
    }
}
