using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUIOTagIA
{
    public class TUIOObjEventArgs : EventArgs    
    {
        public long SessionId { get; set; }
        public int SymbolId { get; set; }
        public float XPos { get; set; }
        public float YPos { get; set; }
        public float Angle { get; set; }

        public TUIOObjEventArgs(long sesionId_, int symbolId_, float x_, float y_, float angle_)
        {
            SessionId = sesionId_;
            SymbolId = symbolId_;
            XPos = x_;
            YPos = y_;
            Angle = angle_;
        }
    }
}
