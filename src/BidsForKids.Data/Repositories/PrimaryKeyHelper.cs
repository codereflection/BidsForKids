using System;
using System.Data.Linq.Mapping;
using System.Reflection;

namespace BidsForKids.Data.Repositories
{
    internal static class PrimaryKeyHelper
    {
        public static PropertyInfo GetPrimaryKey(this Type entityType)
        {
            foreach (var property in entityType.GetProperties())
            {
                var attributes = (ColumnAttribute[])property.GetCustomAttributes(
                    typeof(ColumnAttribute), true);
                if (attributes.Length == 1)
                {
                    var columnAttribute = attributes[0];
                    if (columnAttribute.IsPrimaryKey)
                    {
                        if (property.PropertyType != typeof(int))
                        {
                            throw new ApplicationException(
                                string.Format("Primary key, '{0}', of type '{1}' is not int",
                                              property.Name, entityType));
                        }
                        return property;
                    }
                }
            }
            throw new ApplicationException(
                string.Format("No primary key defined for type {0}", entityType.Name));
        }
    }
}