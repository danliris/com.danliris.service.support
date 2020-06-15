using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace com.danliris.support.test.library.Helpers
{
    public class LabTest
    {
        [Fact]
        public void succesB()
        {
            var b = new B();
            var c = b.cetakB();
            Assert.Equal("cetak A", c);
        }
    }

    public class A
    {
        public virtual string cetakA()
        {
            var a = "cetak A";
            return a;
        }
    }

    public class B
    {
        public string cetakB()
        {
            var b = new A().cetakA();
            return b;
        }
    }
}
