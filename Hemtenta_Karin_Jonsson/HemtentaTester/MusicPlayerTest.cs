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
            Assert.That(() => mp.LoadSongs(search), Throws.TypeOf<ArgumentException>());
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
        public void Play_PlaysNextFromQueueIfNothingIsPlaying()
        {
            FakeSoundMaker fsm = new FakeSoundMaker();
            fsm.NowPlaying = "";
            mp.SetSoundMaker(fsm);

            string songtitle = "Mrs Robinson";

            FakeMediaDatabase db = new FakeMediaDatabase();
            db.fakeDbSongs = new List<ISong>() { new FakeSong(songtitle), new FakeSong("Satellit") };
            mp.SetDb(db);

            mp.LoadSongs("whatever");

            mp.Play();

            Assert.That(mp.NowPlaying, Is.EqualTo("Spelar " + songtitle));
            Assert.That(fsm.PlayCount, Is.EqualTo(1));
        }

        [Test]
        public void Play_DoesNothingIfSongIsAlreadyPlaying()
        {
            FakeSoundMaker fsm = new FakeSoundMaker();
            fsm.NowPlaying = "Everything is Awesome";
            mp.SetSoundMaker(fsm);

            FakeMediaDatabase db = new FakeMediaDatabase();
            db.fakeDbSongs = new List<ISong>() { new FakeSong("Banankontakt") };
            mp.SetDb(db);

            mp.LoadSongs("whatever");

            mp.Play();

            Assert.That(fsm.PlayCount, Is.EqualTo(0));
        }

        [Test]
        public void Play_DoesNothingIfQueueIsEmpty()
        {
            FakeSoundMaker fsm = new FakeSoundMaker();
            fsm.NowPlaying = "";
            mp.SetSoundMaker(fsm);

            mp.Play();

            Assert.That(fsm.PlayCount, Is.EqualTo(0));

        }

        [Test]
        public void Stop_StopsPlayingCurrentSongAndLeavesItInList()
        {
            FakeSong playing = new FakeSong("Bä bä vita lamm");
            FakeSong nextInQueue = new FakeSong("Imse vimse spindel");

            List<ISong> inDb = new List<ISong>()
            {
                playing, nextInQueue
            };

            FakeSoundMaker fsm = new FakeSoundMaker();
            fsm.NowPlaying = playing.Title;
            mp.SetSoundMaker(fsm);

            FakeMediaDatabase db = new FakeMediaDatabase();
            db.fakeDbSongs = inDb;
            mp.SetDb(db);

            mp.LoadSongs("whatever");

            mp.Stop();

            Assert.That(mp.NowPlaying, Is.EqualTo("Tystnad råder"));
            Assert.That(mp.NumSongsInQueue, Is.EqualTo(inDb.Count));
            Assert.That(fsm.StopCount, Is.EqualTo(1));

        }

        [Test]
        public void Stop_DoesNothingIfNoSongIsPlaying()
        {
            FakeSoundMaker fsm = new FakeSoundMaker();
            fsm.NowPlaying = "";
            mp.SetSoundMaker(fsm);

            mp.Stop();

            Assert.That(fsm.StopCount, Is.EqualTo(0));
        }

        [Test]
        public void NextSong_PlaysNextSongInQueue()
        {
            FakeSong playing = new FakeSong("Purple Haze");
            FakeSong nextInQueue = new FakeSong("Smoke on the Water");

            List<ISong> inDb = new List<ISong>()
            {
                playing, nextInQueue
            };

            FakeSoundMaker fsm = new FakeSoundMaker();
            fsm.NowPlaying = playing.Title;
            mp.SetSoundMaker(fsm);

            FakeMediaDatabase db = new FakeMediaDatabase();
            db.fakeDbSongs = inDb;
            mp.SetDb(db);

            mp.LoadSongs("stuff");

            int originallyInQueue = mp.NumSongsInQueue;
            mp.NextSong();

            Assert.That(mp.NowPlaying, Is.EqualTo("Spelar " + nextInQueue.Title));
            Assert.That(mp.NumSongsInQueue, Is.EqualTo(originallyInQueue - 1));

        }

        [Test]
        public void NextSong_StopsPlaybackIfQueueIsEmpty()
        {
            FakeSoundMaker fsm = new FakeSoundMaker();
            mp.SetSoundMaker(fsm);

            mp.NextSong();

            Assert.That(fsm.StopCount, Is.EqualTo(1));
            Assert.That(mp.NowPlaying, Is.EqualTo("Tystnad råder"));
        }

        [Test]
        public void NowPlaying_ReturnsCorrectStringWhenNothingIsPlaying()
        {
            FakeSoundMaker fsm = new FakeSoundMaker();
            fsm.NowPlaying = "";
            mp.SetSoundMaker(fsm);

            string returned = mp.NowPlaying();

            Assert.That(returned, Is.EqualTo("Tystnad råder"));
        }

        [Test]
        public void NowPlaying_ReturnsCorrectStringWhenSongIsPlaying()
        {
            string song = "Life On Mars";
            FakeSoundMaker fsm = new FakeSoundMaker();
            fsm.NowPlaying = song;
            mp.SetSoundMaker(fsm);

            string expected = "Spelar " + song;

            Assert.That(mp.NowPlaying(), Is.EqualTo(expected));
        }

    }
}
