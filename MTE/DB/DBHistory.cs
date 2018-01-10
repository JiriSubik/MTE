using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;

namespace MTE.DB
{
    public class DBHistory
    {
        public string Table { get; private set; }

        public string DBname { get; private set; }

        private readonly SQLiteAsyncConnection conn;

        public DBHistory(string db, string table)
        {
            Table = table; // "TBL_History"
            DBname = System.IO.Path.Combine(Config.Folder, db); // "History"

            // Table creating
            try
            {
                conn = new SQLiteAsyncConnection(DBname);
                conn.CreateTableAsync<History>().Wait();
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task AddAsync(History history)
        {
            try
            {
                await conn.InsertAsync(history);
            }
            catch (SQLiteException ex)
            {
                //Console.WriteLine(ex);
            }
        }

        public async Task<List<History>> GetAllAsync()
        {
            List<History> result = new List<History>();
            try
            {
                result = await conn.QueryAsync<History>($"SELECT * FROM {Table}");
                return result.OrderByDescending(r => r.ID).ToList();
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public async Task<History> GetOneAsync(long ID)
        {
            try
            {
                List<History> result = new List<History>();
                result = await conn.QueryAsync<History>($"SELECT * FROM {Table} WHERE ID = {ID}");
                return result.FirstOrDefault();
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public async Task DeleteAsync()
        {
            try
            {
                await conn.ExecuteAsync($"DELETE FROM {Table}");
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task<int> RecordsCountAsync()
        {
            try
            {
                var count = await conn.ExecuteScalarAsync<int>($"SELECT Count(*) FROM {Table}");
                return count;
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex);
                return -1;
            }
        }

    }
}