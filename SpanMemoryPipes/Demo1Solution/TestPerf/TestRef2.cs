using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using TestPerf.Model;

//                       Method |    Loop |          Mean |       Error |      StdDev |
//----------------------------- |-------- |--------------:|------------:|------------:|
//            CallReturnClassic |    1000 |     14.756 us |   0.0513 us |   0.0455 us |
//        CallReturnRefReadonly |    1000 |      1.555 us |   0.0218 us |   0.0193 us |
// CallReturnRefReadonlyAsValue |    1000 |     14.648 us |   0.2477 us |   0.2317 us |
//            CallReturnClassic | 1000000 | 14,813.793 us | 167.3010 us | 148.3080 us |
//        CallReturnRefReadonly | 1000000 |  1,544.345 us |  16.3561 us |  14.4993 us |
// CallReturnRefReadonlyAsValue | 1000000 | 14,603.073 us | 346.5819 us | 340.3898 us |

namespace TestPerf
{
    //[MemoryDiagnoser]
    //[MedianColumn]
    public class TestRef2
    {
        private SampleStruct _data;
        private volatile int _x1;

        public TestRef2()
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

        [MethodImpl(MethodImplOptions.NoInlining)] private SampleStruct ReturnClassic() => _data;
        [MethodImpl(MethodImplOptions.NoInlining)] private ref SampleStruct ReturnRefReadonly() => ref _data;



        [Benchmark]
        public void CallReturnClassic()
        {

            for (int i = 0; i < Loop; i++)
            {
                SampleStruct sampleStruct = ReturnClassic();
                _x1 = sampleStruct.Data2;
            }
        }

        [Benchmark]
        public void CallReturnRefReadonly()
        {
            for (int i = 0; i < Loop; i++)
            {
                ref SampleStruct sampleStruct = ref ReturnRefReadonly();
                _x1 = sampleStruct.Data2;
            }
        }

        [Benchmark]
        public void CallReturnRefReadonlyAsValue()
        {
            for (int i = 0; i < Loop; i++)
            {
                SampleStruct sampleStruct = ReturnRefReadonly();
                _x1 = sampleStruct.Data2;
            }
        }





    }
}
