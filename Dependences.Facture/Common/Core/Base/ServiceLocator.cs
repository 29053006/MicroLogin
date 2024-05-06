using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dependences.Facture.Common.Core.Base
{
    public abstract class ServiceLocator<TContract, TSelf> where TSelf : ServiceLocator<TContract, TSelf>, new()
    {
        private static Lazy<TContract> _instance = new Lazy<TContract>(InitInstance, true);
        private static TContract _testableInstance;
        private static bool _useTestable;

        protected static Func<TContract> Factory { get; set; }

        private static TContract InitInstance()
        {
            if (Factory == null)
            {
                var controllerInstance = new TSelf();
                Factory = controllerInstance.GetFactory();
            }

            return Factory();
        }

        /// <summary>
        /// Returns a singleton of T
        /// </summary>
        public static TContract Instance
        {
            get
            {
                if (_useTestable)
                {
                    return _testableInstance;
                }

                return _instance.Value;
            }
        }

        /// <summary>
        /// Registers an instance to use for the Singleton
        /// </summary>
        /// <remarks>Intended for unit testing purposes, not thread safe</remarks>
        /// <param name="instance"></param>
        public static void SetTestableInstance(TContract instance)
        {
            _testableInstance = instance;
            _useTestable = true;
        }

        /// <summary>
        /// Clears the current instance, a new instance will be initialized when next requested
        /// </summary>
        /// <remarks>Intended for unit testing purposes, not thread safe</remarks>
        public static void ClearInstance()
        {
            _useTestable = false;
            _testableInstance = default;
            _instance = new Lazy<TContract>(InitInstance, true);
        }

        protected abstract Func<TContract> GetFactory();
    }
}
