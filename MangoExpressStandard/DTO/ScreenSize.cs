using System;
using System.Text.RegularExpressions;
using MangoExpressStandard.Extension;

namespace MangoExpressStandard.DTO
{
    public class BrowserSize
    {
        public int W { get; }
        public int H { get; }
        public bool IsFullScreen { get; } = false;

        private Regex regex = new Regex("[0-9]{1,4}[xX][0-9]{1,4}");

        public BrowserSize(int w, int h)
        {
            W = w;
            H = h;
        }

        public BrowserSize(string whString)
        {
            if (whString.IsNullOrEmpty())
            {
                IsFullScreen = true;
                return;
            }

            var match = regex.Match(whString);
            if (match.Success)
            {
                W = int.Parse(match.Value.ToLower().Split('x')[0]);
                H = int.Parse(match.Value.ToLower().Split('x')[1]);
                IsFullScreen = false;
            }
            else
            {
                throw new ArgumentException($"BrowserSize does not match <int>x<int> pattern!");
            }
        }
    }
}
