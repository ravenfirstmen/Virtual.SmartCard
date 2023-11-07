namespace Virtual.SmartCard
{
    public class SmartCardStatus
    {
        public SmartCardStatus(
            string readerName,
            SmartCardReaderState smartCardReaderState,
            AsynchronousSmartCardProtocol asynchronousSmartCardProtocol,
            byte[] atr)
        {
            ReaderName = readerName;
            SmartCardReaderState = smartCardReaderState;
            AsynchronousSmartCardProtocol = asynchronousSmartCardProtocol;
            ATR = atr;
        }

        public string ReaderName { get; private set; }
        public SmartCardReaderState SmartCardReaderState { get; private set; }
        public AsynchronousSmartCardProtocol AsynchronousSmartCardProtocol { get; set; }
        public byte[] ATR { get; private set; }
    }
}