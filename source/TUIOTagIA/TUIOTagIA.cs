using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUIO;

namespace TUIOTagIA
{
    internal class TUIOController
    {
          private static TUIOController instance;

          private TuioClient m_refClt;
          private int m_iListeners = 0;

          private TUIOController() {
              m_refClt = new TuioClient();                            
          }

          

        public static TUIOController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TUIOController();
                }
                return instance;
            }
        }

        public void addListener(TuioListener tl)
          {              
              m_refClt.addTuioListener(tl);
              m_iListeners++;

              //1st connection
              if (m_iListeners == 1)
                  m_refClt.connect();
          }

        public void removeListener(TuioListener tl)
          {
              m_refClt.removeTuioListener(tl);
              m_iListeners--;

              //last connection
              if (m_iListeners == 0)
                  m_refClt.disconnect();
          }

        public void UpdatePort(int iPort_)
        {
            m_refClt = new TuioClient(iPort_);
        }
    }

    public class TUIOTagIA : INotifyPropertyChanged, IDisposable, TuioListener
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }


        #region private Attributes

        private ObservableCollection<TUIOObjectIA> m_lstObjects;
        
        private TUIOController m_refCtrl;
        private int m_iport = 3333;

        private int m_iXPWidth = 1920, m_iXPHeight = 1080;


        //handle predefined IDs mode
        private bool m__bUsePredefinedIDs = false;
        private string m_strPredefinedIDs = "";
        private List<int> m_lstPredefinedIDs;
        private int m_iXOffset = 200, m_iYOffset = -200;


        #endregion

        #region public Properties

        public ObservableCollection<TUIOObjectIA> TUIOObjects
        {
            get
            { return m_lstObjects; }
            set
            {
                if (value != m_lstObjects)
                {
                    m_lstObjects = value;
                    NotifyPropertyChanged("TUIOObjects");
                }
            }
        }

        public int Port
        {
            get
            { return m_iport; }
            set
            {
                if (value != m_iport)
                {
                    m_iport = value;
                    NotifyPropertyChanged("Port");
                    UpdatePort(m_iport);
                }
            }
        }

        public int XPWidth
        {
            get
            { return m_iXPWidth; }
            set
            {
                if (value != m_iXPWidth)
                {
                    m_iXPWidth = value;
                    NotifyPropertyChanged("XPWidth");
                    UpdateIDs();
                }
            }
        }

        public int XPHeight
        {
            get
            { return m_iXPHeight; }
            set
            {
                if (value != m_iXPHeight)
                {
                    m_iXPHeight = value;
                    NotifyPropertyChanged("XPHeight");
                    UpdateIDs();
                }
            }
        }

        public bool UsePredefinedIDs
        {
            get
            { return m__bUsePredefinedIDs; }
            set
            {
                if (value != m__bUsePredefinedIDs)
                {
                    m__bUsePredefinedIDs = value;
                    NotifyPropertyChanged("UsePredefinedIDs");
                    UpdateIDs();
                }
            }
        }

        public string PredefinedIDs
        {
            get
            { return m_strPredefinedIDs; }
            set
            {
                if (value != m_strPredefinedIDs)
                {
                    m_strPredefinedIDs = value;
                    NotifyPropertyChanged("PredefinedIDs");
                    UpdateIDs();
                }
            }
        }

        public int XOffset
        {
            get
            { return m_iXOffset; }
            set
            {
                if (value != m_iXOffset)
                {
                    m_iXOffset = value;
                    NotifyPropertyChanged("XOffset");
                    UpdateIDs();
                }
            }
        }

        public int YOffset
        {
            get
            { return m_iYOffset; }
            set
            {
                if (value != m_iYOffset)
                {
                    m_iYOffset = value;
                    NotifyPropertyChanged("YOffset");
                    UpdateIDs();
                }
            }
        }


        #endregion

        #region Events

        public delegate void TUIOObjEventHandler(object sender, TUIOObjEventArgs e);
        public event TUIOObjEventHandler TUIOObjAdded, TUIOObjRemoved;
        

        #endregion

        #region Constructor

        public TUIOTagIA()
        {
            TUIOObjects = new ObservableCollection<TUIOObjectIA>();
            m_lstPredefinedIDs = new List<int>();

            m_refCtrl = TUIOController.Instance;
            m_refCtrl.addListener(this);
       
        }
       

        #endregion

        #region TUIO Implementation to handle 2Dobj
       
        //called when a new 2Dobj event is received
        public void addTuioObject(TuioObject tobj)
        {
            int index = -1;
            for (int i = 0; i < TUIOObjects.Count; i++)
            {
                if (TUIOObjects[i].SymbolId == tobj.SymbolID)
                {
                    index = i;
                    break;
                }
            }

            //case tag already exists
            if (index != -1)
            {
                TUIOObjects[index].SessionId = tobj.SessionID;
                TUIOObjects[index].SymbolId = tobj.SymbolID;
                TUIOObjects[index].Angle = tobj.AngleDegrees;
                TUIOObjects[index].XPos = tobj.X * XPWidth;
                TUIOObjects[index].YPos = tobj.Y * XPHeight;
            }
            else
            {
                //if IDs are predefined, no new tags should be added
                if (UsePredefinedIDs)
                    return;

                TUIOObjects.Add(new TUIOObjectIA()
                {
                    SessionId = tobj.SessionID,
                    SymbolId = tobj.SymbolID,
                    Angle = tobj.AngleDegrees,
                    XPos = tobj.X * XPWidth,
                    YPos = tobj.Y * XPHeight
                });

                if (TUIOObjAdded != null)
                    TUIOObjAdded(this, new TUIOObjEventArgs(tobj.SessionID, tobj.SymbolID, tobj.X, tobj.Y, tobj.AngleDegrees));
            }
        }

        //called when a 2Dobj disappears
        public void removeTuioObject(TuioObject tobj)
        {
            int index = -1;
            for (int i = 0; i < TUIOObjects.Count; i++)
            {
                if (TUIOObjects[i].SymbolId == tobj.SymbolID)
                {
                    index = i;
                    break;
                }
            }
            if (index != -1)
            {
                //if IDs are predefined, don't remove objets from the list but move them out of screen. 
                //This will keep the predefined order of tags. 
                if (UsePredefinedIDs)
                {
                    TUIOObjects[index].YPos = YOffset;
                }
                else
                {
                    TUIOObjects.RemoveAt(index);
                }
                if (TUIOObjRemoved != null)
                    TUIOObjRemoved(this, new TUIOObjEventArgs(tobj.SessionID, tobj.SymbolID, tobj.X, tobj.Y, tobj.AngleDegrees));

            }
        }

        //called when a 2Dobj is updated
        public void updateTuioObject(TuioObject tobj)
        {
            foreach (var item in TUIOObjects)
            {
                if (item.SessionId == tobj.SessionID)
                {
                    item.Angle = tobj.AngleDegrees;
                    item.XPos = tobj.X * XPWidth;
                    item.YPos = tobj.Y * XPHeight;

                    break;
                }
            }
        }
        #endregion

        #region Unused callbacks

        public void addTuioBlob(TuioBlob tblb)
        {
            //Debugger.Break();
        }

        public void addTuioCursor(TuioCursor tcur)
        {
            //Debugger.Break();
        }
       
        public void removeTuioBlob(TuioBlob tblb)
        {
            //Do nothing
        }

        public void removeTuioCursor(TuioCursor tcur)
        {
            //Do nothing
        }

        public void updateTuioBlob(TuioBlob tblb)
        {
            //Do nothing
        }

        public void updateTuioCursor(TuioCursor tcur)
        {
            //Do nothing
        }

        public void refresh(TuioTime ftime)
        {
            //Do nothing
        }

        #endregion

        private void UpdatePort(int iPort_)
        {
            m_refCtrl.removeListener(this);
            m_refCtrl.UpdatePort(iPort_);
            m_refCtrl.addListener(this);
        }

        private void UpdateIDs()
        {
            //reset list of existing tags
            TUIOObjects.Clear();
            m_lstPredefinedIDs.Clear();

            //in case the boolean was set to false
            if (!UsePredefinedIDs)
                return;


            string[] separators = { ";", "," ,"-"};
            PredefinedIDs = PredefinedIDs.Replace(" ", ""); // remove blanks in list. 
            string[] IDs = PredefinedIDs.Split(separators, StringSplitOptions.None); // split ID list 

            int x = 0, y = YOffset, sessionID = 1;
            //for each predefined ID, pre-genereate an object and set it out of scene. 
            foreach (var id in IDs)
            {
                try
                {
                    int iID = Int32.Parse(id);
                    TUIOObjects.Add(new TUIOObjectIA()
                    {
                        SessionId = sessionID,
                        SymbolId = iID,
                        Angle = 0,
                        XPos = x,
                        YPos = y
                    });

                    //save ID in list to be compared in TUIO callbacks
                    m_lstPredefinedIDs.Add(iID);

                    //increment position for next predefined tag
                    x += XOffset;

                }
                catch (FormatException e)
                {
                    //do nothing for this bad ID
                }

            }
        }

        public void Dispose()
        {
            m_refCtrl.removeListener(this);            
        }
    }
}
