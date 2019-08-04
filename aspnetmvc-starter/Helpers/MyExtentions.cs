using System;

namespace aspnetmvc_starter.Helpers
{
    public static class MyExtentions
    {
        public static object GetProperty<T>(this T obj, string name) where T : class
        {
            Type t = typeof(T);
            return t.GetProperty(name).GetValue(obj, null);
        }
    }
}