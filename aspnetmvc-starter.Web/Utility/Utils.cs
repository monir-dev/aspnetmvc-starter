using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace aspnetmvc_starter.Web.Utility
{
    public class NoCache : ActionFilterAttribute, IActionFilter
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            filterContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
            filterContext.HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            filterContext.HttpContext.Response.Cache.SetNoStore();

            base.OnResultExecuting(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
        }
    }

    public class PropertyReflector
    {
        public static T ClearProperties<T>(T obj)
        {
            foreach (PropertyInfo field in typeof(T).GetProperties())
            {
                if (field.Name != "Id" && field.Name != "EmployeeId" && field.Name != "EmpoyeeId" && field.Name != "EmpCode" && field.Name != "EmployeeInitial" && field.Name != "FullName" && field.Name != "Message" && field.Name != "errClass" && field.Name != "ButtonText" && field.Name != "strMode" && field.Name != "DateofInactive" && field.Name != "IsContractual")
                {
                    string type = field.PropertyType.Name;
                    if (type == "Nullable`1")
                    {
                        field.SetValue(obj, null, null);
                    }
                    else if (type == "Int32")
                    {
                        field.SetValue(obj, 0, null);
                    }
                    else if (type == "String")
                    {
                        field.SetValue(obj, "", null);
                    }
                    else if (type == "Decimal")
                    {
                        field.SetValue(obj, Convert.ToDecimal(0), null);
                    }
                    else if (type == "DateTime")
                    {
                        field.SetValue(obj, DateTime.Now, null);
                    }
                    else if (type == "Boolean")
                    {
                        field.SetValue(obj, false, null);
                    }
                }
                else if (field.Name == "strMode")
                {
                    field.SetValue(obj, "add", null);
                }
                else
                {
                    //Need to Implement if any other case occurs
                }
            }
            return obj;
        }

        // Clone/Copy an object without changing source
        public static T Clone<T>(T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
    }

    public static class Compare
    {
        public static IEnumerable<T> DistinctBy<T, TIdentity>(this IEnumerable<T> source, Func<T, TIdentity> identitySelector)
        {
            return source.Distinct(Compare.By(identitySelector));
        }

        public static IEqualityComparer<TSource> By<TSource, TIdentity>(Func<TSource, TIdentity> identitySelector)
        {
            return new DelegateComparer<TSource, TIdentity>(identitySelector);
        }

        private class DelegateComparer<T, TIdentity> : IEqualityComparer<T>
        {
            private readonly Func<T, TIdentity> identitySelector;

            public DelegateComparer(Func<T, TIdentity> identitySelector)
            {
                this.identitySelector = identitySelector;
            }

            public bool Equals(T x, T y)
            {
                return Equals(identitySelector(x), identitySelector(y));
            }

            public int GetHashCode(T obj)
            {
                return identitySelector(obj).GetHashCode();
            }
        }
    }

    public static class Converter
    {
        //using System.ComponentModel;
        public static DataTable ConvertObjectToDataTable<T>(T data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            DataRow row = table.NewRow();
            foreach (PropertyDescriptor prop in properties)
                row[prop.Name] = prop.GetValue(data) ?? DBNull.Value;
            table.Rows.Add(row);

            return table;

        }

        public static DataTable ConvertListObjectToDataTable<T>(IList<T> dataList)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in dataList)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;

        }

        public static DataSet ConvertObjectToDataSet<T>(T data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            DataRow row = table.NewRow();
            foreach (PropertyDescriptor prop in properties)
                row[prop.Name] = prop.GetValue(data) ?? DBNull.Value;
            table.Rows.Add(row);

            //return table;
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return dataSet;

        }

        public static string DataTableToJSON(DataTable table, bool IsAppendHeader)
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            Dictionary<string, object> dict = new Dictionary<string, object>();

            if (IsAppendHeader) //Append Header
            {
                foreach (DataColumn col in table.Columns)
                {
                    dict[col.ColumnName] = col.ColumnName;
                }
                list.Add(dict);
            }

            //Append Rows
            foreach (DataRow row in table.Rows)
            {
                dict = new Dictionary<string, object>();

                foreach (DataColumn col in table.Columns)
                {
                    dict[col.ColumnName] = row[col];
                }
                list.Add(dict);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(list);
        }
    }

    public static class Utils
    {
        public static int ParseInt(object value)
        {
            int intValue = 0;
            if (value == null) return 0;

            string strValue = value.ToString().Trim();
            if (!string.IsNullOrEmpty(strValue))
            {
                try
                {
                    intValue = int.Parse(strValue);
                }
                catch
                {
                    intValue = 0;
                }
            }

            return intValue;
        }
        public static int ParseIntNonNegative(object value)
        {
            int intValue = 0;
            if (value == null) return 0;

            string strValue = value.ToString().Trim();
            if (!string.IsNullOrEmpty(strValue))
            {
                try
                {
                    intValue = int.Parse(strValue);
                }
                catch
                {
                    intValue = 0;
                }
            }

            if (intValue < 0)
                intValue = 0;

            return intValue;
        }
        public static decimal ParseDecimal(object value)
        {
            decimal intValue = 0;
            if (value == null) return 0;

            string strValue = value.ToString().Trim();
            if (!string.IsNullOrEmpty(strValue))
            {
                try
                {
                    intValue = decimal.Parse(strValue);
                }
                catch
                {
                    intValue = 0;
                }
            }

            return intValue;
        }

        public static decimal GetRandomNumber(int type)
        {
            decimal numRandom = 0;
            if (type == 1)
            {
                numRandom = Convert.ToDecimal(DateTime.Now.Ticks);
            }
            else if (type == 2)
            {
                numRandom = Convert.ToDecimal(DateTime.Now.Year.ToString("0000") + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00")
                    + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00")
                    + DateTime.Now.Second.ToString("00") + DateTime.Now.Millisecond.ToString("000"));
            }
            return numRandom;
        }
        public static string GetRandomString(int type)
        {
            string strRandom = "";
            strRandom = GetRandomNumber(type).ToString();
            return strRandom;
        }
    }
}