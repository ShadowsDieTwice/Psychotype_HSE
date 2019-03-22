using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Psychotype_HSE.Models;

namespace Psychotype_HSE.Util
{
    public class PythonRunner
    {
        public static void RunScript(string scriptPath, string pythonPath)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
	            FileName = pythonPath,
	            Arguments = scriptPath,
	            WorkingDirectory = AppSettings.WorkingDir,
	            ErrorDialog = true
            };
            process.StartInfo = startInfo;
            process.Start();
        }
    }
}