using System;

namespace Mxp.UITests.CrossPlatform.Helpers.Requests
{
    public class WaitForElementRequest
    {
        private TimeSpan? _timeOut;

        public string Id { get; set; }
        public string Marked { get; set; }
        public string Text { get; set; }
        public string Class { get; set; }
        public string ErrorMessage { get; set; }
        public int Index { get; set; }
        // Controltype supercede class. Controltype = any = check class property if set
        public ControlType ControlType { get; set; }

        public TimeSpan? TimeOut
        {
            get { return _timeOut.HasValue ? _timeOut : DefaultTimeOut; }
            set { _timeOut = value; }
        }

        public static TimeSpan DefaultTimeOut { get; set; } = TimeSpan.FromSeconds(30);

        public WaitForElementRequest(ControlType controltype = ControlType.Any)
        {
            ControlType = controltype;
        }
    }

    public enum ControlType
    {
        Any,
        Switch,
        TextField,
        Label,
        Button
    }

}
