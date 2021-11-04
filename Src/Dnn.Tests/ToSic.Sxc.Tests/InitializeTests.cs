﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToSic.Eav;

namespace ToSic.Sxc.Tests
{
    [TestClass]
    public class InitializeTests
    {
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context) 
            => ConfigureEfcDi(sc => { });


        public static void ConfigureEfcDi(Factory.ServiceConfigurator configure)
        {
            Factory.ActivateNetCoreDi(sc =>
            {
                sc.AddSxcCore()
                    .AddEavCore()
                    .AddEavCorePlumbing()
                    .AddEavCoreFallbackServices();
                //Trace.WriteLine("di configuration core");
                //sc.TryAddTransient<IValueConverter, MockValueConverter>();
                //sc.TryAddTransient<IRuntime, RuntimeUnknown>();
                //sc.TryAddTransient<IFingerprint, FingerprintUnknown>();
                configure.Invoke(sc);   // call parent invoker if necessary (usually not relevant at core, as this is the top-level
            });

        }
    }
}
