using System;

namespace Swarm.Common.Extensions
{
    public static class Int32Extensions
    {
        public static string ToFancyString(this Int32 number)
        {
            if (number < 0)
            {
                return string.Concat("-", (number * -1).ToFancyString());
            }
            if (number < 1000)
            {
                return number.ToInvariantString();
            }
            double d = number / 1000.0;
            if (d < 1000)
            {
                return d.ToString("#.#k");
            }
            return d.ToString("#.#m");
        }

        public static string ToFancyLabel(this Int32 number, string label)
        {
            if (number < 0 || number > 1000)
            {
                return string.Format("{0} {1}", number, label);
            }
            return label;
        }

        public static bool IsEven(this Int32 number)
        {
            return number % 2 == 0;
        }
    }
}
