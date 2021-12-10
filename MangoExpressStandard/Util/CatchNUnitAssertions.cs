using System;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace MangoExpressStandard.Util
{
    public class CatchNUnitAssertions
    {
        public static bool DoesCodeAssert(TestDelegate code)
        {
            using (new TestExecutionContext.IsolatedContext())
            {
                try
                {
                    code();
                    return false;
                }
                catch
                {
                    return true;
                }
            }
        }
    }
}
