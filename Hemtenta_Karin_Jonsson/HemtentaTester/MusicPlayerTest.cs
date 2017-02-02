using System;
using NUnit.Framework;
using HemtentaTdd2017.music;
using HemtentaTdd2017;
using Moq;
using System.Collections.Generic;

namespace HemtentaTester
{
    [TestFixture]
    public class MusicPlayerTest
    {
        MusicPlayer mp;

        [SetUp]
        public void SetUp()
        {
            mp = new MusicPlayer();
        }


        [Test]
        public void LoadSongs_GetSongsFromDbCorrectInputSuccess()
        {
            string song = "Fire";

            List<ISong> inDb = new List<ISong>() { new FakeSong("Ring of Fire"), new FakeSong("Hearts on Fire") };

            var mockMediaDb = new Mock<IMediaDatabase>();
            mockMediaDb.Setup(x => x.FetchSongs(song)).Returns(inDb);
            mp.SetDb(mockMediaDb.Object);

            int expected = mp.NumSongsInQueue + inDb.Count;

            mp.LoadSongs(song);

            Assert.That(expected, Is.EqualTo(mp.NumSongsInQueue));
            mockMediaDb.Verify(x => x.FetchSongs(song), Times.Once);
        }

        [TestCase(null)]
        [TestCase("")] //bestämde att en tom sträng inte är giltig
        public void LoadSongs_InvalidInputThrowsException(string search)
        {
            Assert.That(()=>mp.LoadSongs(search), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void LoadSongs_ThrowsExceptionIfDbConnectionAlreadyOpen()
        {
            IMediaDatabase fakeDb = new FakeMediaDatabase();
            fakeDb.OpenConnection();
            mp.SetDb(fakeDb);

            Assert.That(() => mp.LoadSongs("search"), Throws.TypeOf<DatabaseAlreadyOpenException>());
        }

        [Test]
        public void LoadSongs_ThrowsExceptionIfDbConnectionNotOpen() 
        {
            IMediaDatabase fakeDb = new FakeMediaDatabase();

            mp.SetDb(fakeDb);

            Assert.That(() => mp.LoadSongs("search"), Throws.TypeOf<DatabaseClosedException>());

        }

        [Test]
        public void Play_PlaysNextSongFromQueue()
        {
            var mockSoundMaker = new Mock<ISoundMaker>();
            mockSoundMaker.Setup(x => x.NowPlaying).Returns("");
            mp.SetSoundMaker(mockSoundMaker.Object);

            //använd ISoundMaker, om den skickar tillbaka en tom sträng
            //betyder det att ingen låt spelas (mocka att den skickar det svaret)
            //spela då en låt
            Assert.Fail();
        }

        [Test]
        public void Play_DoesNothingIfASongIsAlreadyPlaying()
        {
            var mockSoundMaker = new Mock<ISoundMaker>();
            mockSoundMaker.Setup(x => x.NowPlaying).Returns("Everything is Awesome");
            mp.SetSoundMaker(mockSoundMaker.Object);

            //använd ISoundMaker, om den skickar tillbaka en sträng som inte är tom 
            //betyder det att en låt redan spelas (mocka att den skickar det svaret)

            // Titeln på sången som spelas just nu. Ska vara
            // tom sträng om ingen sång spelas.
            //string NowPlaying { get; }
            Assert.Fail();
        }
        
        [Test]
        public void Stop_StopsPlayingCurrentSongAndLeavesItInList()
        {
            Assert.Fail();
        }

        [Test]
        public void Stop_DoesNothingIfNoSongIsPlaying()
        {
            Assert.Fail();
        }

        [Test]
        public void NextSong_StartsPlayingNextSongInQueue()
        {
            Assert.Fail();
        }
        

        [Test]
        public void NextSong_StopsPlaybackIfQueueIsEmpty()
        {
            Assert.Fail();
        }

        [Test]
        public void NowPlaying_ReturnsCorrectStringWhenNothingIsPlaying()
        {
            var mockSoundMaker = new Mock<ISoundMaker>();
            mockSoundMaker.Setup(x => x.NowPlaying).Returns("");
            mp.SetSoundMaker(mockSoundMaker.Object);

            string returned = mp.NowPlaying();

            Assert.That(returned, Is.EqualTo("Tystnad råder"));
        }

        [Test]
        public void NowPlaying_ReturnsCorrectStringWhenSongIsPlaying()
        {
            string song = "Life On Mars";

            var mockSoundMaker = new Mock<ISoundMaker>();
            mockSoundMaker.Setup(x => x.NowPlaying).Returns(song);
            mp.SetSoundMaker(mockSoundMaker.Object);

            string returned = mp.NowPlaying();

            Assert.That(returned, Is.EqualTo("Spelar " + song));
        }
        /*

        // Börjar spela nästa sång i kön. Om kön är tom
        // har funktionen samma effekt som Stop().
        void NextSong();

        // Returnerar strängen "Tystnad råder" om ingen
        // sång spelas, annars "Spelar <namnet på sången>".
        // Exempel: "Spelar Born to run".
        string NowPlaying();

         // Antal sånger som finns i spellistan.
        // Returnerar alltid ett heltal >= 0.
        int NumSongsInQueue { get; }
         
         */
    }
}
