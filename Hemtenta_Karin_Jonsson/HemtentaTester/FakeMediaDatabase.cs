using HemtentaTdd2017;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HemtentaTester
{
    public class FakeMediaDatabase : IMediaDatabase
    {
        public List<ISong> fakeDbSongs { get; set; } = new List<ISong>();

        public bool IsConnected
        { get; set; }

        public void CloseConnection()
        {
            IsConnected = false;
        }

        public List<ISong> FetchSongs(string search)
        {
            if (!IsConnected)
            {
                throw new DatabaseClosedException();
            }

            return fakeDbSongs;
        }

        public void OpenConnection()
        {
            if (IsConnected)
            {
                throw new DatabaseAlreadyOpenException();
            }
            else
            {
                IsConnected = true;
            }
        }
    }
}
