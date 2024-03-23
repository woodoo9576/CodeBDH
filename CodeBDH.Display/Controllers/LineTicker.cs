using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using WebGrease.Css.Extensions;

namespace CodeBDH.Display.Controllers
{
    using DataAccess;
    using Models;
    public class LineTicker
    {
        // ReSharper disable once InconsistentNaming
        private static readonly Lazy<LineTicker> _instance = new Lazy<LineTicker>(() => new LineTicker(GlobalHost.ConnectionManager.GetHubContext<LineTickerHub>().Clients));

        private static readonly XmlContext Context = new XmlContext();

        private readonly ConcurrentDictionary<int, Line> _lines = new ConcurrentDictionary<int, Line>();
        private readonly object _displayStateLock = new object();

        private readonly object _updatingLineValuesLock = new object();
        private volatile bool _updatingLineValues;

        private readonly TimeSpan _updateInterval = TimeSpan.FromMilliseconds(500);
        private Timer _timer;
        private volatile DisplayState _displayState;

        private LineTicker(IHubConnectionContext<dynamic> clients)
        {
            Clients = clients;
            LoadDefaultLines();
        }

        public static LineTicker Instance
        {
            get { return _instance.Value; }
        }

        public IHubConnectionContext<dynamic> Clients { get; set; }

        public DisplayState DisplayState
        {
            get { return _displayState; }
            set { _displayState = value; }
        }


        internal IEnumerable<Line> GetAllLines()
        {
            return _lines.Values;
        }
        #region DisplayStatusChange

        internal void OpenDisplay()
        {
            lock (_displayStateLock)
            {
                if (DisplayState != DisplayState.Open)
                {
                    _timer = new Timer(UpdateLineValues, null, _updateInterval, _updateInterval);
                    DisplayState = DisplayState.Open;
                    BroadcastDisplayStateChange(DisplayState.Open);
                }
            }
        }

        internal void CloseDisplay()
        {
            lock (_displayStateLock)
            {
                if (DisplayState == DisplayState.Open)
                {
                    if (_timer != null)
                    {
                        _timer.Dispose();
                    }
                }
                DisplayState = DisplayState.Closed;
                BroadcastDisplayStateChange(DisplayState.Closed);
            }
        }

        internal void ResetDisplay()
        {
            lock (_displayStateLock)
            {
                if (DisplayState != DisplayState.Closed)
                {
                    CloseDisplay();
                }
                LoadDefaultLines();
                BroadcastDisplayReset();
            }
        }

        #endregion

        #region Broadcast
        private void BroadcastDisplayStateChange(DisplayState displayState)
        {
            switch (displayState)
            {
                    case DisplayState.Open:
                    Clients.All.displayOpened();
                    break;
                    case DisplayState.Closed:
                    Clients.All.displayClosed();
                    break;
            }
        }

        private void BroadcastDisplayReset()
        {
            Clients.All.displayReset();
        }
        private void BroadcastLineValues(Line line)
        {
            Clients.All.updateLineValues(line);
        }

        #endregion
        private void LoadDefaultLines()
        {
            var olines = Context.GetLine();
            _lines.Clear();
            olines.ForEach(l => _lines.TryAdd(l.LineId, l));
        }
        private void UpdateLineValues(object state)
        {
            lock (_updatingLineValuesLock)
            {
                if (!_updatingLineValues)
                {
                    _updatingLineValues = true;
                    foreach (var line in _lines.Values)
                    {
                        if (TryUpdateLineValues(line))
                        {
                            BroadcastLineValues(line);
                        }
                    }
                    _updatingLineValues = false;
                }
            }
        }


        private bool TryUpdateLineValues(Line line)
        {
            var nLine = Context.GetLine(line.LineId);
            if (line.Patient == nLine.Patient&&line.LineNumber==nLine.LineNumber)
            {
                return false;
            }
            line.Patient = nLine.Patient;
            line.Doctor = nLine.Doctor;
            line.LineNumber = nLine.LineNumber;
            line.Patient = nLine.Patient;
            line.Total = nLine.Total;
            line.Examined = nLine.Examined;
            return true;
        }

    }

    public enum DisplayState
    {
        Closed,
        Open
    }
}