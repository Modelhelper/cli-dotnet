using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using ModelHelper.Core.Rules;

namespace ModelHelper.Commands
{
    [Export(typeof(ICommand))]   
    public class RunScriptCommand : BaseCommand
    {
        public RunScriptCommand()
        {
            Key = "run";
            IsPublic = false;
        }

        public override bool EvaluateArguments(IRuleEvaluator<Dictionary<string, string>> evaluator)
        {
            return true;
        }

        public override void Execute(Core.ApplicationContext context)
        {
            //var escapedArgs = cmd.Replace("\"", "\\\"");
            var scriptLocations = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));
            var file = Path.Combine(scriptLocations, "ModelHelper", "scripts", "create-api.sh"); //.Replace("\\", "/").Replace("C:", "/C");

            Console.WriteLine(File.Exists(file)); ;
            Console.WriteLine("Location = " + file);

            var psi = new ProcessStartInfo
            {
                FileName = @"C:\Program Files\Git\bin\bash.exe",
                
                Arguments = $"-c \"{file}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            var process = Process.Start(psi);

            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();            
        }
    }
}