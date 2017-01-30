using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HemtentaTdd2017
{
    // Ska testas och implementeras.
    public interface IMusicPlayer
    {
        // Antal sånger som finns i spellistan.
        // Returnerar alltid ett heltal >= 0.
        int NumSongsInQueue { get; }

        // Söker i databasen efter sångtitlar som
        // innehåller "search" och lägger till alla
        // sökträffar i spellistan.
        void LoadSongs(string search);

        // Om ingen låt spelas för tillfället ska
        // nästa sång i kön börja spelas. Om en låt
        // redan spelas har funktionen ingen effekt.
        void Play();

        // Om en sång spelas ska den sluta spelas.
        // Sången ligger kvar i spellistan. Om ingen
        // sång spelas har funktionen ingen effekt.
        void Stop();

        // Börjar spela nästa sång i kön. Om kön är tom
        // har funktionen samma effekt som Stop().
        void NextSong();

        // Returnerar strängen "Tystnad råder" om ingen
        // sång spelas, annars "Spelar <namnet på sången>".
        // Exempel: "Spelar Born to run".
        string NowPlaying();
    }

    // Databasen som har alla sånger.
    // Om man försöker använda databasen när den
    // är stängd, eller öppna den när den redan
    // är öppen, ska funktionen kasta motsvarande
    // exception. Ska inte implementeras.
    public interface IMediaDatabase
    {
        bool IsConnected { get; }

        // Ansluter till databasen
        void OpenConnection();

        // Stänger anslutning till databasen
        void CloseConnection();

        // Returnerar alla sånger i databasen som
        // matchar söksträngen.
        // Tips: använd string.Contains(string)
        List<ISong> FetchSongs(string search);
    }

    // Representerar en ljudfil inklusive metadata.
    // Implementera eller mocka.
    public interface ISong
    {
        string Title { get; }
    }

    // Spelar musik. Implementera eller mocka.
    public interface ISoundMaker
    {
        // Titeln på sången som spelas just nu. Ska vara
        // tom sträng om ingen sång spelas.
        string NowPlaying { get; }

        void Play(ISong song);
        void Stop();
    }

    // Se till att databasklassen kastar dessa exceptions
    // när den ska göra det enligt specen.
    public class DatabaseClosedException : Exception { }
    public class DatabaseAlreadyOpenException : Exception { }
}
