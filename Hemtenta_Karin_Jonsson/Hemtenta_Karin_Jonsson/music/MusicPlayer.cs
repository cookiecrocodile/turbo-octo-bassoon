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
            // Börjar spela nästa sång i kön. Om kön är tom
            // har funktionen samma effekt som Stop().
            throw new NotImplementedException();
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
            // Om ingen låt spelas för tillfället ska
            // nästa sång i kön börja spelas. Om en låt
            // redan spelas har funktionen ingen effekt.
            throw new NotImplementedException();
        }

        public void Stop()
        {
            // Om en sång spelas ska den sluta spelas.
            // Sången ligger kvar i spellistan. Om ingen
            // sång spelas har funktionen ingen effekt.
            throw new NotImplementedException();
        }

        /*
         ISoundMaker har följande metoder:
         
        // Titeln på sången som spelas just nu. Ska vara
        // tom sträng om ingen sång spelas.
        string NowPlaying { get; }

        void Play(ISong song);
        void Stop();
         
         */
    }
}
