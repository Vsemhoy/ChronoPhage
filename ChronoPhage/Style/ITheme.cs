using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronoPhage.Style
{
    interface ITheme
    {
        public string Name { get; set; }

        public Shadow CardShadow { get; set; }

        public int CardCorner {  get; set; }

        public int MiniCardHeightMinHeight { get; set; }
    }
}
