using System;

using Sugar.Language.Exceptions;

namespace Sugar.Language.Services
{
    internal abstract class SingletonService<Type> where Type : SingletonService<Type>, new()
    {
        protected static Type instance;
        public static string SingletonID { get => typeof(Type).Name; }

        public static Type Instance
        {
            get
            {
                return instance;
            }
        }

        public static void CreateInstance()
        {
            if (instance != null)
                throw new ConstructException(typeof(Type).ToString());

            instance = new Type();
        }

        protected SingletonService()
        {

        }    
    }
}
