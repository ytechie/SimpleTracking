using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.ApplicationModel;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using SimpleTracking.WindowsStore.DesignerData;

namespace SimpleTracking.WindowsStore.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var a = PackagesDesignerData.GetSamplePackageJson();
        }
    }
}
