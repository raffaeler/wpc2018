using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipelines;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Slave.Helpers;

namespace Slave
{
    class Program
    {
        public static readonly string MasterExe = @"..\..\..\..\Master\bin\debug\master.exe";
        static async Task Main(string[] args)
        {
            var processFilename = Path.GetFullPath(MasterExe);
            if (!File.Exists(processFilename))
            {
                Console.WriteLine($"Can't find master: {processFilename}");
                return;
            }

            var p = new Program();
            await p.RunAsync(processFilename, "result.jpg");
            Console.ReadKey();
        }

        private async Task RunAsync(string processFilename, string targetFile)
        {
            var pipe = new Pipe();
            var cts = new CancellationTokenSource();

            using (var process = new Process())
            {
                // setup the process to run
                process.StartInfo = BuildOptions(processFilename);
                process.EnableRaisingEvents = true;
                if (!process.Start()) return;

                // binary reader will read «stdout»
                var binaryReader = new BinaryReader(process.StandardOutput.BaseStream);

                // the data coming out will be saved on targetFile
                using (var targetStream = File.Create(targetFile))
                {
                    // readerTask is the data coming from the Pipe and going into the target file
                    var readerTask = pipe.Reader.CopyToAsync(targetStream);

                    // writerTask is the data coming from stdout to the Pipe
                    var writerTask = binaryReader.BaseStream.CopyToAsync(pipe.Writer).AsTask();
                    await Task.WhenAll(readerTask, writerTask);

                    Console.WriteLine("Copy finished, disposing the process");
                }
            }
        }

        private ProcessStartInfo BuildOptions(string processFilename, string arguments = "")
        {
            var psi = new ProcessStartInfo();
            psi.FileName = processFilename;
            psi.Arguments = arguments;
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.CreateNoWindow = true;
            psi.WorkingDirectory = Directory.GetCurrentDirectory();

            return psi;
        }
    }
}
