using HemtentaTdd2017;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HemtentaTester
{
    public class FakeSong : ISong
    {
        public FakeSong (string title)
        {
            Title = title;
        }

        public string Title { get; set; }
    }
}
