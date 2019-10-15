using System;
using System.Data;
using System.Reflection;

namespace aspnetmvc_starter.Web.Utility
{
    public class MapperBase
    {
        public static MapperBase objMaper;


        public static MapperBase GetInstance()
        {
            if (objMaper == null)
            {
                objMaper = new MapperBase();
                return objMaper;
            }
            else
            {
                return objMaper;
            }
        }


        public static T MapItem<T>(T obj, DataRow drContent)
        {

            PropertyInfo[] properties = obj.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                
                object[] attributes = property.GetCustomAttributes(typeof(aspnetmvc_starter.Web.Utility.TableDataColumn), true);

                if (attributes.GetLength(0) > 0)
                {

                    if (((aspnetmvc_starter.Web.Utility.TableDataColumn)attributes[0]).IsDataColumn())
                    {
                        if (drContent.Table.Columns.Contains(property.Name))
                        {

                            if (property.PropertyType == typeof(System.String))
                            {
                                //property.SetValue(obj, drContent[property.Name] as string, null);
                                property.SetValue(obj, drContent[property.Name] == DBNull.Value ? "" : Convert.ToString(drContent[property.Name]), null);
                            }
                            else if (property.PropertyType == typeof(System.Guid))
                            {
                                Guid g = Guid.Empty;
                                if (drContent[property.Name] != DBNull.Value)
                                    g = new Guid(Convert.ToString(drContent[property.Name]));

                                property.SetValue(obj, g, null);
                            }

                            else if (property.PropertyType == typeof(System.Int32))
                            {
                                property.SetValue(obj, drContent[property.Name] == DBNull.Value ? 0 : Convert.ToInt32(drContent[property.Name]), null);
                            }
                            else if (property.PropertyType == typeof(System.Int64))
                            {
                                property.SetValue(obj, drContent[property.Name] == DBNull.Value ? 0 : Convert.ToInt64(drContent[property.Name]), null);
                            }
                            else if (property.PropertyType == typeof(System.Double))
                            {
                                property.SetValue(obj, drContent[property.Name] == DBNull.Value ? 0 : Convert.ToDouble(drContent[property.Name]), null);
                            }
                            else if (property.PropertyType == typeof(System.Decimal))
                            {
                                property.SetValue(obj, drContent[property.Name] == DBNull.Value ? 0 : Convert.ToDecimal(drContent[property.Name]), null);
                            }
                            else if (property.PropertyType == typeof(System.Boolean))
                            {
                                property.SetValue(obj, drContent[property.Name] == DBNull.Value ? false : Convert.ToBoolean(drContent[property.Name]), null);
                            }
                            else if (property.PropertyType == typeof(System.DateTime))
                            {
                                property.SetValue(obj, drContent[property.Name] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(drContent[property.Name]), null);
                            }
                            else if (property.PropertyType == typeof(System.Byte[]))
                            {
                                property.SetValue(obj, drContent[property.Name] == DBNull.Value ? null : (Byte[])(drContent[property.Name]), null);
                            } 
                               
                            else if (property.PropertyType == typeof(System.DateTime?))
                            {
                                property.SetValue(obj, drContent[property.Name] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(drContent[property.Name]), null);
                            }


                        }
                        else if (property.Name == "bitEditable")
                        {
                            property.SetValue(obj, 1, null);
                        }
                    }

                }

            }

            return obj;

        }


    }

    public class TableDataColumn : Attribute
    {
        private bool bolIsDataColumn = false;

        public TableDataColumn(bool status)
        {
            this.bolIsDataColumn = status;
        }

        public bool IsDataColumn()
        {
            return bolIsDataColumn;
        }

    } 
}