namespace Mxp.UITests.CrossPlatform.Helpers.Requests
{
    public class EnterValueRequest<T>
    {
        public string Label { get; set; }

        public T Value { get; set; }
    }
}
