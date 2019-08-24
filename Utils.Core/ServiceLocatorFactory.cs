using Utils.Core.Registration;

namespace Utils.Core
{
    public static class ServiceLocatorFactory
    {
        private static ServiceLocator _instance;
        public static IServiceLocator Get()
        {
            if (_instance != null)
            {
                return _instance;
            }

            var instance = new ServiceLocator();
            instance.Initialize();
            _instance = instance;
            return _instance;
        }
    }
}
