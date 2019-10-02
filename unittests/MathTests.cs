using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace unittests
{
    [TestClass]
    public class MathTests
    {
        [TestMethod]
        public void TestModOperation()
        {

            for (int value = 0; value < 100; value++)
            {
                int mode_result_expected = value % 32;
                int mode_result_value = value - (value >> 5 << 5);// (value >> divider << divider);//value - (value >> divider);


                Assert.AreEqual(mode_result_expected, mode_result_value, $"Value:{value}");
            }
            
        }

        [TestMethod]
        public void TestModOperationSMODESpeed()
        {
            for (int i = 0; i < 40000000; i++)
            {
                int mode_result_expected = i % 32;
            }
        }
        [TestMethod]
        public void TestModOperationCMODESpeed()
        {
            for (int i = 0; i < 40000000; i++)
            {
                int mode_result_value = i - (i >> 5 << 5);
            }
        }

        [TestMethod]
        public void MultiplyTest()
        {
            float value = .01F;

            int mult = (int)(value * 256F);

            for(int i =0; i < 40000000; i++)
            {
                int result_expected = (int)(i * value);
                int result = i & mult;

                Assert.AreEqual(result_expected, result, $"I:{i} Mult:{mult}");
            }
        }

        [TestMethod]
        public void RectContainTest()
        {
            Rect currentRect = new Rect(0, 0, 1, 1);
            //Rect[] rects_i = new Rect[]
            //{
            //    new Rect(0.5F, 0.5F, 0.5F, 0.5F),
            //    new Rect(0, 0, 0.5F, 0.5F),
            //};

            //foreach (Rect rect_i in rects_i)
                Assert.AreEqual(true, currentRect.Contain(new Rect(0.5F, 0.5F, 0.5F, 0.5F)));
        }
    }
}
