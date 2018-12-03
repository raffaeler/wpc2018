using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using TestPerf.Model;

// This code cannot be measured due to compiler optimizations

namespace TestPerf
{
    //[MemoryDiagnoser]
    //[MedianColumn]
    public class TestRef1
    {
        private SampleStruct _data;
        protected int _x1;
        protected SampleStruct _x2;

        public TestRef1()
        {
            // some random data
            _data = new SampleStruct()
            {
                Id = Guid.NewGuid(),
                Name = "abcdefg",

                Data1 = 1,
                Data2 = 2,
                Data3 = 3,

                D1 = 1.456m,
                D2 = 2.456m,
                D3 = 3.456m,
                D4 = 4.456m,
                D5 = 5.456m,
                D6 = 6.456m,
                D7 = 7.456m,
                D8 = 8.456m,

                N1 = 1,
                N2 = 2,
                N3 = 3,
                N4 = 4,
                N5 = 5,
                N6 = 6,
                N7 = 7,
                N8 = 8,

            };
        }


        [Params(1_000, 1_000_000)]
        public int Loop { get; set; }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private SampleStruct PassParamClassic(SampleStruct sampleStruct)
        {
            sampleStruct.Data1 += 1;
            return sampleStruct;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private SampleStruct PassParamRefReadonly(in SampleStruct sampleStruct)
        {
            _x1 = sampleStruct.Data1 + 1;
            return sampleStruct;
        }

        [Benchmark]
        public void CallPassParamClassic()
        {
            for (int i = 0; i < Loop; i++)
            {
                _x2 = PassParamClassic(_data);
            }
        }

        [Benchmark]
        public void CallPassParamRefReadonly()
        {
            for (int i = 0; i < Loop; i++)
            {
                _x2 = PassParamRefReadonly(in _data);
            }
        }

    }
}
