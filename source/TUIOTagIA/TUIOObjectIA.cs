using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUIOTagIA
{
    public class TUIOObjectIA : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        #region Private Properties

        private long m_iSessionId;
        private int m_iSymbolId;
        private float m_fAngle;
        private float m_fX, m_fY;

        #endregion

        #region Public Properties

        public long SessionId
        {
            get { return m_iSessionId; }
            set
            {
                if (m_iSessionId != value)
                {
                    m_iSessionId = value;
                    NotifyPropertyChanged("SessionId");
                }
            }
        }

        public int SymbolId
        {
            get { return m_iSymbolId; }
            set
            {
                if (m_iSymbolId != value)
                {
                    m_iSymbolId = value;
                    NotifyPropertyChanged("SymbolId");
                }
            }
        }

        public float XPos
        {
            get { return m_fX; }
            set
            {
                if (m_fX != value)
                {
                    m_fX = value;
                    NotifyPropertyChanged("XPos");
                }
            }
        }

        public float YPos
        {
            get { return m_fY; }
            set
            {
                if (m_fY != value)
                {
                    m_fY = value;
                    NotifyPropertyChanged("YPos");
                }
            }
        }

        public float Angle
        {
            get { return m_fAngle; }
            set
            {
                if (m_fAngle != value)
                {
                    m_fAngle = value;
                    NotifyPropertyChanged("Angle");
                }
            }
        }

        #endregion
    }
}
