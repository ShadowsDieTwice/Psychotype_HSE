using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.IO;

namespace Psychotype_HSE.Util
{
    public class PythonRunner
    {
        public static string RunScript(string scriptPath, string pythonPath)
        {
            System.Diagnostics.ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = pythonPath;
            start.Arguments = $"\"{scriptPath}\"";

            start.UseShellExecute = false;
            start.CreateNoWindow = false;
            start.RedirectStandardOutput = true;
            start.RedirectStandardError = true;

            string err, res;


            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    err = process.StandardError.ReadToEnd();
                    res = reader.ReadToEnd();
                    process.WaitForExit();
                }
            }

            return string.Concat(err, "\n", res);
        }
    }
}