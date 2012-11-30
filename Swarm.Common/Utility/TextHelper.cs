using System.Text;
using Swarm.Common.Extensions;

namespace Swarm.Common.Utility
{
    public class TextHelper
    {
        /// <summary>
        /// Produces URL-friendly version of a title, "like-this-one", hand-tuned for speed.
        /// </summary>
        public string Slugify(string title, int maxlen = 80)
        {
            if (title == null)
            {
                return string.Empty;
            }
            int len = title.Length;
            bool dash = false;
            var sb = new StringBuilder(len);

            for (int i = 0; i < len; i++)
            {
                char c = title[i];
                if ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9'))
                {
                    sb.Append(c);
                    dash = false;
                }
                else if (c >= 'A' && c <= 'Z')
                {
                    // tricky way to convert to lowercase
                    sb.Append((char)(c | 32));
                    dash = false;
                }
                else if (c == ' ' || c == ',' || c == '.' || c == '/' || c == '\\' || c == '-' || c == '_' || c == '=')
                {
                    if (!dash && sb.Length > 0)
                    {
                        sb.Append('-');
                        dash = true;
                    }
                }
                else if (c >= 128)
                {
                    int prevlen = sb.Length;
                    sb.Append(RemapInternationalCharToAscii(c));
                    if (prevlen != sb.Length)
                    {
                        dash = false;
                    }
                }
                if (sb.Length == maxlen)
                {
                    break;
                }
            }

            if (dash)
            {
                return sb.ToString().Substring(0, sb.Length - 1);
            }
            else
            {
                return sb.ToString();
            }
        }

        public string RemapInternationalCharToAscii(char c)
        {
            string s = c.ToInvariantString().ToLowerInvariant();
            if ("àåáâäãåą".Contains(s))
            {
                return "a";
            }
            else if ("èéêëę".Contains(s))
            {
                return "e";
            }
            else if ("ìíîïı".Contains(s))
            {
                return "i";
            }
            else if ("òóôõöøőð".Contains(s))
            {
                return "o";
            }
            else if ("ùúûüŭů".Contains(s))
            {
                return "u";
            }
            else if ("çćčĉ".Contains(s))
            {
                return "c";
            }
            else if ("żźž".Contains(s))
            {
                return "z";
            }
            else if ("śşšŝ".Contains(s))
            {
                return "s";
            }
            else if ("ñń".Contains(s))
            {
                return "n";
            }
            else if ("ýÿ".Contains(s))
            {
                return "y";
            }
            else if ("ğĝ".Contains(s))
            {
                return "g";
            }
            else if (c == 'ř')
            {
                return "r";
            }
            else if (c == 'ł')
            {
                return "l";
            }
            else if (c == 'đ')
            {
                return "d";
            }
            else if (c == 'ß')
            {
                return "ss";
            }
            else if (c == 'Þ')
            {
                return "th";
            }
            else if (c == 'ĥ')
            {
                return "h";
            }
            else if (c == 'ĵ')
            {
                return "j";
            }
            else if (c == '�')
            {
                return " ";
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
