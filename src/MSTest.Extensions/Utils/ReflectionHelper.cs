using System.Reflection;

namespace MSTest.Extensions.Utils
{
    /// <summary>
    /// 反射辅助类
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// 获取字段值
        /// </summary>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static object GetField([NotNull] object source, string propertyName)
        {
            var type = source.GetType();
            var field = type.GetField(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            return field.GetValue(source);
        }
        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static object GetProperty([NotNull] object source, string propertyName)
        {
            var type = source.GetType();
            var property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            return property.GetValue(source);
        }
        /// <summary>
        /// 设置字段值
        /// </summary>
        /// <param name="target"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public static void SetField([NotNull] object target, string propertyName, object value)
        {
            var type = target.GetType();
            var field = type.GetField(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            field.SetValue(target, value);

        }
        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="target"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public static void SetProperty([NotNull] object target, string propertyName, object value)
        {
            var type = target.GetType();
            var property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            property.SetValue(target, value);

        }

    }
}