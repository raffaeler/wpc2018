using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using BenchmarkDotNet.Running;

namespace TestPerf
{
    class Program
    {
        static void Main(string[] args)
        {
            //var t = new TestRef();
            //ref var x1 = ref t.ReturnRefReadonly();
            //var x2 = t.ReturnRefReadonly();

            //Debug.Assert(x1.Data1 == 1);
            //x1.Data1 = 999;
            //Debug.Assert(x1.Data1 == 999);
            //x2.Data2 = 888;
            //Debug.Assert(x1.Data1 == 999);

            var proc = Environment.Is64BitProcess ? "x64" : "x86";
            Console.WriteLine($"Process running at: {proc}");

            //BenchmarkRunner.Run<TestRefOnInt>();
            //BenchmarkRunner.Run<TestRef1>();  // compiler optimizations do not allow the comparison
            //BenchmarkRunner.Run<TestRef2>();
            BenchmarkRunner.Run<TestNative>();
        }
    }
}
