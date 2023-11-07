using System;
using Virtual.SmartCard.Winscard;

namespace Virtual.SmartCard
{
    public class SmartCardContext : SmartCardResourceAware
    {
        private readonly IntPtr _smartCardContext = IntPtr.Zero;

        protected SmartCardContext(IntPtr smartCardContext)
        {
            _smartCardContext = smartCardContext;
        }

        public static SmartCardContext Create(SmartCardScope scope)
        {
            var ctx = InitContext(scope);

            return new SmartCardContext(ctx);
        }

        public IntPtr GetContext()
        {
            return _smartCardContext;
        }

        public bool IsValid()
        {
            return IsAValidContext();
        }

        #region Resources

        private static readonly object Section = new object();

        private static IntPtr InitContext(SmartCardScope scope)
        {
            lock (Section)
            {
                IntPtr ctx = IntPtr.Zero;
                var or = NativeAPI.SCardEstablishContext(
                    (UInt32)scope,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    ref ctx);
                if (or != NativeAPI.OperationResult.SUCCESS)
                {
                    throw new SmartCardException("SmartCardContext: InitContext", or);
                }

                return ctx;
            }
        }

        private bool IsAValidContext()
        {
            lock (Section)
            {
                var or = NativeAPI.SCardIsValidContext(GetContext());
                if (or == NativeAPI.OperationResult.SUCCESS)
                {
                    return true;
                }
                if (SmartCardErrors.GetCodeFromErrorCode(or) == SmartCardErrors.Codes.InvalidHandle)
                {
                    return false;
                }

                throw new SmartCardException("SmartCardContext: IsAValidContext", or);
            }
        }

        protected override void DisposeResources()
        {
            if (_smartCardContext != IntPtr.Zero)
            {
                var or = NativeAPI.SCardReleaseContext(_smartCardContext);
                if (or != NativeAPI.OperationResult.SUCCESS)
                {
                    throw new SmartCardException("SmartCardContext: DisposeResources", or);
                }
            }
        }

        #endregion
    }
}