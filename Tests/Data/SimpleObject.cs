using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObjectLayoutInspector;

namespace ObjectStackAllocation.Tests.Data
{
    public class SimpleObject
    {
        public static readonly TypeLayout TypeLayout;

        static SimpleObject()
        {
            SimpleObject.TypeLayout = TypeLayout.GetLayout<SimpleObject>();
        }
    }
}
