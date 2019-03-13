using Microsoft.VisualStudio.TestTools.UnitTesting;
using Psychotype_HSE.Util;
using Psychotype_HSE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psychotype_HSE_Tests.Util
{
    [TestClass()]
    public class PythonRunnerTest
    {
        [TestMethod()]
        public void RunTest()
        {
            PythonRunner.RunScript(@"../../Scripts/script.py", AppSettings.PythonPath, "arg1");//, "arg2");
        }
    }
}
