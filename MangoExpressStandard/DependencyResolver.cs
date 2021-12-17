using System;
using System.Collections.Generic;
using Castle.Windsor;

namespace MangoExpressStandard
{
    public class DependencyResolver
    {
        private static IWindsorContainer windsorContainer;

        private static void Initialize()
        {
            if (windsorContainer == null)
            {
                windsorContainer = new WindsorContainer();
                windsorContainer.Install();
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

            if (windsorContainer == null)
            {
                Initialize();
            }

            return windsorContainer.Resolve<T>(name);
        }
    }
}
