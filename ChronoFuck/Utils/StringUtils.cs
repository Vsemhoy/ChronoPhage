using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronoFuck.Utils
{
    class StringUtils
    {


        public static List<string> Generate256HexColors()
        {
            List<string> hexColors = new List<string>();

            // Generate 216 web-safe colors
            for (int r = 0; r <= 5; r++)
            {
                for (int g = 0; g <= 5; g++)
                {
                    for (int b = 0; b <= 5; b++)
                    {
                        hexColors.Add(
                            $"#{r * 51:X2}{g * 51:X2}{b * 51:X2}"
                        );
                    }
                }
            }

            // Add 40 shades of gray to reach 256
            for (int i = 0; i < 40; i++)
            {
                int gray = i * 6 + 8;
                hexColors.Add($"#{gray:X2}{gray:X2}{gray:X2}");
            }

            return hexColors;
        }
    }
}
