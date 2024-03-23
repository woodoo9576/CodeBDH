using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace CodeBDH.Display.Controllers
{
    using Models;
    [HubName("LineTicker")]
    public class LineTickerHub : Hub
    {
        private readonly LineTicker _lineTicker;
        private static List<int> _clinics = new List<int>();

       public LineTickerHub() : this(LineTicker.Instance) { }

        private LineTickerHub(LineTicker lineTicker)
        {
            _lineTicker = lineTicker;
        }

        public IEnumerable<Line> GetAllLines(int displayGroup)
        {
            var all = _lineTicker.GetAllLines().Select(l => l.LineId).ToList();
            _clinics = displayGroup == 1
                ? new List<int> { 4104, 4030, 4054 }
                : (displayGroup == 2
                    ? new List<int> { 4065, 4100, 4042, 4094, 4031, 4046, 4054, 14323, 4103, 4027, 4077 }
                    : (displayGroup == 3) ? new List<int> { 4087, 4066, 4067, 4093, 4023 } : all);

            return _lineTicker.GetAllLines().Where(l => _clinics.Contains(l.LineId)).OrderBy(l => l.Clinic);
        }

        public string GetDisplayState()
        {
            return _lineTicker.DisplayState.ToString();
        }

        public void OpenDisplay()
        {
            _lineTicker.OpenDisplay();
        }

        public void CloseDisplay()
        {
            _lineTicker.CloseDisplay();
        }

        public void ResetDisplay()
        {
            _lineTicker.ResetDisplay();
        }
    }
}