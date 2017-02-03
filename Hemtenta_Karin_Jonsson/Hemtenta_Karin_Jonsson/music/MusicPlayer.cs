using System;
using System.Collections.Generic;

namespace HemtentaTdd2017.music
{
    public class MusicPlayer : IMusicPlayer
    {
        IMediaDatabase mdb;
        ISoundMaker soundMaker;
        List<ISong> playlist = new List<ISong>();

        public void SetDb(IMediaDatabase db)
        {
            mdb = db;
        }

        public void SetSoundMaker(ISoundMaker sm)
        {
            soundMaker = sm;
        }

        public int NumSongsInQueue
        {
            get
            {
                return playlist.Count;
            }
        }
        

        public void LoadSongs(string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                throw new ArgumentException();
            }

            mdb.OpenConnection();

            playlist.AddRange(mdb.FetchSongs(search));

            mdb.CloseConnection();

        }

        public void NextSong()
        {
            if (NumSongsInQueue <= 1)
            {
                soundMaker.Stop();
                playlist.Clear();
            }
            else
            {
                playlist.RemoveAt(0);
                soundMaker.Play(playlist[0]);
            }

        }

        public string NowPlaying()
        {
            string nowPlaying = soundMaker.NowPlaying;

            if (string.IsNullOrEmpty(nowPlaying))
            {
                return "Tystnad råder";
            }
            else
            {
                string returnString = "Spelar " + nowPlaying;
                return returnString;
            }

        }

        public void Play()
        {
            if (NumSongsInQueue != 0 && soundMaker.NowPlaying == string.Empty)
                soundMaker.Play(playlist[0]);
        }

        public void Stop()
        {
            if (soundMaker.NowPlaying != string.Empty)
                soundMaker.Stop();
        }

    }
}
