﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.IO;

namespace Psychotype_HSE.Util
{
    public class PythonRunner
    {
        public static void RunScript(string scriptPath, string pythonPath,
            string workingDir /*resultFilePath, string postsFilePath, string shutdownFilePath*/)
        {
            System.Diagnostics.ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = pythonPath;
            start.Arguments = $"\"{scriptPath}\" \"{workingDir}\"";//{resultFilePath}\" \"{postsFilePath}\"";

            start.UseShellExecute = false;
            start.CreateNoWindow = false;
            start.RedirectStandardOutput = true;
            start.RedirectStandardError = true;
            Process process = Process.Start(start);
            //using (Process process = Process.Start(start))
            {
                //using (StreamReader reader = process.StandardOutput)
                {
                    //String err = process.StandardError.ReadToEnd();
                    //String  res = reader.ReadToEnd();
                    //Console.WriteLine(res);
                    //process.WaitForExit();
                }
            }
        }
    }
}