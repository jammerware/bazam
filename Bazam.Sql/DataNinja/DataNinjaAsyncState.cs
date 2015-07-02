using System;
using System.Data.SqlClient;

namespace Bazam.DataNinja
{
    public class DataNinjaAsyncState
    {
        public AsyncCallback Callback { get; set; }
        public SqlCommand Command { get; set; }

        public DataNinjaAsyncState(AsyncCallback callback, SqlCommand command)
        {
            Callback = callback;
            Command = command;
        }
    }
}
