using System;
using System.Reflection;

namespace DSPAssistant
{
    /**
     * 反射获取、赋值对象的private属性值
     */
    public class ReflectionUtil
    {
        public static T GetPrivateField<T>(object instance, string fieldname)
        {
            BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = instance.GetType();
            FieldInfo field = type.GetField(fieldname, flag);
            if (field.GetValue(instance) == null)
            {
                return default;
            }
            return (T)field.GetValue(instance);
        }

        public static T GetPrivateProperty<T>(object instance, string propertyname)
        {
            BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = instance.GetType();
            PropertyInfo field = type.GetProperty(propertyname, flag);
            return (T)field.GetValue(instance, null);
        }

        public static MethodInfo GetPrivateMethod(object instance, string MethodName)
        {
            BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = instance.GetType();
            MethodInfo methodInfo = type.GetMethod(MethodName, flag);
            return methodInfo;
        }

        public static void SetPrivateField(object instance, string fieldname, object value)
        {
            BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = instance.GetType();
            FieldInfo field = type.GetField(fieldname, flag);
            field.SetValue(instance, value);
        }

        public static void SetPrivateProperty(object instance, string propertyname, object value)
        {
            BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = instance.GetType();
            PropertyInfo field = type.GetProperty(propertyname, flag);
            field.SetValue(instance, value, null);
        }

        public static void SetParentPublicProperty(object instance, string propertyname, object value)
        {
            BindingFlags flag = BindingFlags.Instance | BindingFlags.Public;
            Type type = instance.GetType();
            PropertyInfo field = type.BaseType.GetProperty(propertyname, flag);
            field.SetValue(instance, value, null);
        }

    }
}
