using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;

namespace TestPerf
{
    [MemoryDiagnoser]
    [MedianColumn]
    public class TestRefOnInt
    {
        private long _number = 2018;


        [Params(1_000, 1_000_000)]
        public int Loop { get; set; }

        [MethodImpl(MethodImplOptions.NoInlining)] private Int64 PassInt(Int64 value) => value * 2;
        [MethodImpl(MethodImplOptions.NoInlining)] private Int64 PassIntRefReadonly(in Int64 value) => value * 2;

        [MethodImpl(MethodImplOptions.NoInlining)] private Int64 Returnint() => _number;
        [MethodImpl(MethodImplOptions.NoInlining)] private ref Int64 ReturnIntRefReadonly() => ref _number;


        [Benchmark]
        public void CallPassParamInt()
        {
            for (int i = 0; i < Loop; i++)
            {
                PassInt(1);
            }
        }

        [Benchmark]
        public void CallPassIntRefReadonly()
        {
            long value = 1;
            for (int i = 0; i < Loop; i++)
            {
                PassIntRefReadonly(in value);
            }
        }

        [Benchmark]
        public void CallReturnInt()
        {
            for (int i = 0; i < Loop; i++)
            {
                var num = Returnint();
            }
        }

        [Benchmark]
        public void CallReturnRef()
        {
            for (int i = 0; i < Loop; i++)
            {
                ref var num = ref ReturnIntRefReadonly();
            }
        }

        [Benchmark]
        public void CallReturnRefAsValue()
        {
            for (int i = 0; i < Loop; i++)
            {
                var num = ReturnIntRefReadonly();
            }
        }

    }
}
