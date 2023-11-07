using System.Collections.Generic;

namespace Virtual.SmartCard.TLV.Simple
{
    public class BERTLV
    {
        public BERTLV()
        {
            Parent = null;
            Childs = null;
        }

        public BERTAG Tag { get; set; }
        public BERLength Length { get; set; }
        public byte[] Contents { get; set; }

        public BERTLV Parent { get; set; }
        public IList<BERTLV> Childs { get; set; }

        public void AddChilds(IList<BERTLV> childs)
        {
            Childs = childs;
        }
    }
}