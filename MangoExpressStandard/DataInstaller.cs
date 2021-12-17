using System;
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
            container.Register(Component.For<TestCaseRetryAttribute>().LifeStyle.Transient);

            container.Register(
                Classes.FromAssembly(Assembly.GetExecutingAssembly())
                .InNamespace("MangoExpressStandard.POM")
                .WithService
                .DefaultInterfaces()
                .Configure(delegate (ComponentRegistration c)
                {
                    var x = c.Named(c.Implementation.Name)
                    .Interceptors(
                        typeof(TestCaseRetryAttribute));
                }));
        }
    }
}
