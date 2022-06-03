using System;

namespace Sugar.Language.Semantics.Services
{
    internal sealed class ServiceProvider
    {
        public ServiceProvider()
        {

        }

        public K ProvideService<T, K>(params object[] paramaters) where T : ISemanticService<T> where K : T
        {
            var instance = Activator.CreateInstance(typeof(K), paramaters);

            return (K)instance;
        }
    }
}
