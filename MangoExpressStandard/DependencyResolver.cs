using System;
using System.Collections.Generic;
using System.Reflection;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace MangoExpressStandard
{
    public class DependencyResolver
    {
        private static IWindsorContainer _container;

        private static void Initialize()
        {
            if (_container == null)
            {
                _container = new WindsorContainer();
                _container.Install(FromAssembly.InThisApplication(Assembly.GetExecutingAssembly()));
                //_container.Install(FromAssembly.This());
            }
        }

        public static T For<T>(Type @type)
        {
            var name = type.Name;
            var interfaces = new List<Type>(type.GetInterfaces());

            if (!interfaces.Contains(typeof(T)))
            {
                throw new Exception();
            }

            if (_container == null)
            {
                Initialize();
            }

            return _container.Resolve<T>(name);
        }
    }
}
