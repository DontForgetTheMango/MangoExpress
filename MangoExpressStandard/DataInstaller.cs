using System;
using System.Collections.Generic;
using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using MangoExpressStandard.Attribute;

namespace MangoExpressStandard
{
    public class DataInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // register attributes
            container.Register(Component.For<TestCaseRetryAttribute>().LifeStyle.Transient);

            // register namespaces to which attributes may be applied
            var namespacesToRegister = new List<string>
            {
                "MangoExpressStandard.POM"
            };

            foreach (var namespaceToRegister in namespacesToRegister)
            {
                container.Register(
                    //Classes.FromThisAssembly()
                    Classes.FromAssembly(Assembly.GetExecutingAssembly())
                    .InNamespace(namespaceToRegister)
                    .WithService
                    .DefaultInterfaces()
                    .Configure(delegate (ComponentRegistration c)
                    {
                        var x = c
                        .Named(c.Implementation.Name)
                        .Interceptors(
                            typeof(TestCaseRetryAttribute));
                    }));
            }
        }
    }
}
