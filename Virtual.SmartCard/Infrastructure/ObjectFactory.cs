using System;

namespace Virtual.SmartCard.Infrastructure
{
    public static class ObjectFactory
    {
        public static T Create<T>()
        {
            return Create<T>(typeof(T));
        }

        public static T Create<T>(params object[] @params)
        {
            return Create<T>(typeof(T), @params);
        }

        public static T Create<T>(string typeName)
        {
            return Create<T>(Type.GetType(typeName));
        }

        public static T Create<T>(string typeName, params object[] @params)
        {
            return Create<T>(Type.GetType(typeName), @params);
        }

        public static T Create<T>(Type type)
        {
            return (T)Activator.CreateInstance(type);
        }

        public static T Create<T>(Type type, params object[] @params)
        {
            return (T)Activator.CreateInstance(type, @params);
        }
    }
}