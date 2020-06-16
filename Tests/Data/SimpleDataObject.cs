using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObjectLayoutInspector;

namespace ObjectStackAllocation.Tests.Data
{
    public class SimpleDataObject
    {
        public static readonly TypeLayout TypeLayout;

        static SimpleDataObject()
        {
            SimpleDataObject.TypeLayout = TypeLayout.GetLayout<SimpleDataObject>();
        }

        public long Value;

        public SimpleDataObject(long value)
        {
            this.Value = value;
        }
    }
}
