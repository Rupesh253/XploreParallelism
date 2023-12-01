using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Text;
using static AuxParallel.Program;
using static System.Net.Mime.MediaTypeNames;

namespace AuxParallel {
    public class Program {
        private readonly PollingService _pollingService = new PollingService();
        protected void OnStart(string[] args) {
            _pollingService.StartPolling();
        }
        protected void OnStop() {
            _pollingService.StopPolling();
        }
        public static void Main(string[] args) {
            Thread t1 = new Thread(Process1);
            Thread t2 = new Thread(Process2);
            Thread t3 = new Thread(Process3);
            Thread t4 = new Thread(GetCMDInfo);

            //t1.Start();
            //t2.Start();
            //t3.Start();
            //t4.Start();

            Task task1 = Task.Factory.StartNew(() => Process1());
            Thread.Sleep(10000);
            Task task2 = Task.Factory.StartNew(() => Process2());
            Thread.Sleep(10000);
            Task task3 = Task.Factory.StartNew(() => Process3());
            Task.WaitAll(task1, task2, task3);
            ProcessBusinessLogic pb = new ProcessBusinessLogic();
            pb.ProcessCompleted += Pb_ProcessCompleted;
            pb.StartProcess();
        }

        private static void Pb_ProcessCompleted() {
            Console.WriteLine("Process Completed!");
        }

        public delegate void Notify();

        public static void GetCMDInfo() {
            List<string> pidList = new List<string>();
            StringBuilder sb = new StringBuilder();
            Process[] ipByName = Process.GetProcessesByName("cmd");

            foreach (Process process in ipByName) {
                pidList.Add(process.Id.ToString());
                sb.Append(process.Id.ToString() + ",");
                Console.WriteLine("Process: {0} ID: {1} Window title: {2}", process.ProcessName, process.Id, process.MainWindowTitle);
            }
            Console.WriteLine($"pid:{sb.ToString()}");
            string pidSequence = sb.Remove(sb.Length - 1, 1).ToString();

            //var scriptArguments = $"(Get-Process -id {pidSequence} -ErrorAction SilentlyContinue).MainWindowTitle";
            // string scriptArguments = " (Get-Process -id 21848,34116 -ErrorAction SilentlyContinue).MainWindowTitle";
            string scriptArguments = "Get-Process -Name cmd| Where-Object {$_.mainWindowTitle}| Format-Table Id, mainWindowtitle -AutoSize";
            var processStartInfo = new ProcessStartInfo("powershell.exe", scriptArguments);
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardError = true;

            using var process1 = new Process();
            process1.StartInfo = processStartInfo;
            process1.Start();
            string output = process1.StandardOutput.ReadToEnd();


            string error = process1.StandardError.ReadToEnd();
            Console.WriteLine(output);
            output = output.Replace("Id MainWindowTitle", "").Replace("-- ---------------", "");
            string[] array = output.Split(" ");
            Console.WriteLine(output);
        }
        public static void Process1() {
            Console.Beep();
            //string command = "cd \"C:\\Users\\git\\Desktop\\\" & color b & batch 1";
            string command = "cd C:\\Users\\git\\source\\XploreParallellism1 & color 5  & dotnet clean & dotnet build & dotnet test";
            //string command = File.ReadAllText(@"C:\Users\git\source\XploreAutomationIntegrations\AuxParallel\dotnetTestCommand1.txt");
            Console.WriteLine(command);
            var startInfo = new ProcessStartInfo {
                FileName = "cmd.exe",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                Arguments = command
            };

            var process = new Process { StartInfo = startInfo };
            process.Start();
            Console.WriteLine($"process1:{process.Id}");
            process.EnableRaisingEvents = true;
            process.StandardInput.WriteLine(command);
            process.OutputDataReceived += (sender, e) => { Console.WriteLine(e.Data); };
            process.ErrorDataReceived += (sender, e) => { Console.WriteLine(e.Data); };
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
            process.Exited += myProcess_Exited;
            void myProcess_Exited(object sender, System.EventArgs e) {
                Console.WriteLine(
                    $"Exit time    : {process.ExitTime}\n" +
                    $"Exit code    : {process.ExitCode}\n" +
                    $"Elapsed time : {Math.Round((process.ExitTime - process.StartTime).TotalMilliseconds)}");
            }
            process.Close();
        }
        public static void Process2() {
            Console.Beep();
            //string command = "cd \"C:\\Users\\git\\Desktop\\\" & color b & batch 2";
            string command = "cd C:\\Users\\git\\source\\XploreParallellism2 & color b & dotnet clean & dotnet build & dotnet test";
            //string command = File.ReadAllText(@"C:\Users\git\source\repos\XploreAutomationIntegrations\AuxParallel\dotnetTestCommand2.txt");
            Console.WriteLine(command);
            var startInfo = new ProcessStartInfo {
                FileName = "cmd.exe",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                Arguments = command,
            };

            var process = new Process { StartInfo = startInfo };
            process.Start();
            Console.WriteLine($"process2:{process.Id}");
            process.EnableRaisingEvents = true;
            process.StandardInput.WriteLine(command);
            process.OutputDataReceived += (sender, e) => { Console.WriteLine(e.Data); };
            process.ErrorDataReceived += (sender, e) => { Console.WriteLine(e.Data); };
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
            process.Exited += (sender, e) => {
                Console.WriteLine("Process exited with exit code " + process.ExitCode.ToString());
            };
            process.Close();
        }
        public static void Process3() {
            Console.Beep();
            //string command = "cd \"C:\\Users\\git\\Desktop\\\" & color b & batch 3";
            string command = "cd C:\\Users\\git\\source\\XploreParallellism3 & color 6 & dotnet clean & dotnet build & dotnet test";
            //string command = File.ReadAllText(@"C:\Users\git\source\XploreAutomationIntegrations\AuxParallel\dotnetTestCommand3.txt");
            Console.WriteLine(command);
            var startInfo = new ProcessStartInfo {
                FileName = "cmd.exe",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                Arguments = command,
            };

            var process = new Process { StartInfo = startInfo };
            process.Start();
            Console.WriteLine($"process3:{process.Id}");
            process.EnableRaisingEvents = true;
            process.StandardInput.WriteLine(command);
            process.OutputDataReceived += (sender, e) => { Console.WriteLine(e.Data); };
            process.ErrorDataReceived += (sender, e) => { Console.WriteLine(e.Data); };
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
            process.Exited += (sender, e) => {
                Console.WriteLine("Process exited with exit code " + process.ExitCode.ToString());
            };
            process.Close();
        }
    }
    public class PollingService {
        private Thread _workerThread;
        private AutoResetEvent _finished;
        private const int _timeout = 60 * 1000;

        public void StartPolling() {
            _workerThread = new Thread(Poll);
            _finished = new AutoResetEvent(false);
            _workerThread.Start();
        }

        private void Poll() {
            while (!_finished.WaitOne(_timeout)) {
                //do the task
            }
        }

        public void StopPolling() {
            _finished.Set();
            _workerThread.Join();
        }
    }
    public class ProcessBusinessLogic {
        public event Notify ProcessCompleted; // event

        public void StartProcess() {
            Console.WriteLine("Process Started!");
            // some code here..
            OnProcessCompleted();
        }

        protected virtual void OnProcessCompleted() //protected virtual method
        {
            //if ProcessCompleted is not null then call delegate
            ProcessCompleted?.Invoke();
        }
    }
}