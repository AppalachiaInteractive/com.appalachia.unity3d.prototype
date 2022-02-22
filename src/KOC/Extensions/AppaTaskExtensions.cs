using Appalachia.Core.Attributes;
using Appalachia.Utility.Async;

namespace Appalachia.Prototype.KOC.Extensions
{
    [CallStaticConstructorInEditor]
    public static class AppaTaskExtensions
    {
        static AppaTaskExtensions()
        {
            //AppalachiaApplication
        }
        
        
        //private static CancellationTokenSource
        public static void ForgetUntilQuitting(this AppaTask task)
        {
        }
    }
}
