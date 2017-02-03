using HemtentaTdd2017;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HemtentaTester
{
    public class FakeSoundMaker : ISoundMaker
    {

        public int PlayCount { get; set; }

        public int StopCount { get; set; }

        public string NowPlaying { get; set; }

        public void Play(ISong song)
        {
            NowPlaying = song.Title;
            PlayCount++;
        }

        public void Stop()
        {
            NowPlaying = "";
            StopCount++;
        }
    }
}
