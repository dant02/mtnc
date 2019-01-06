using System;
using System.Collections.Generic;
using System.IO;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using mtnc.android.Database;
using SQLite;

namespace mtnc.android
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private static readonly List<string> phoneNumbers = new List<string>();
        protected string DbPath { get { return Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "mydb.db3"); } }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            var btn = FindViewById<Button>(Resource.Id.btn1);
            btn.Click += (src, arg) =>
            {
                Toast.MakeText(this.ApplicationContext, "Clicked!", ToastLength.Long).Show();
            };

            var dbDefaultsBtn = this.FindViewById<Button>(Resource.Id.dbDefaults);
            dbDefaultsBtn.Click += (src, arg) =>
            {
                try
                {
                    using (var db = new SQLiteConnection(this.DbPath))
                    {
                        db.BeginTransaction();

                        db.CreateTable<Valuable>();
                        db.CreateTable<Bill>();

                        db.InsertOrReplace(new Valuable() { Id = 0, Symbol = "CZK" });
                        db.InsertOrReplace(new Valuable() { Id = 1, Symbol = "EUR" });

                        db.InsertOrReplace(new Bill() { CreatedOnUtc = DateTime.UtcNow, ModifiedOnUtc = DateTime.UtcNow, Price = 30.3m, IdValuable = 1 });

                        db.Commit();
                    }

                    Toast.MakeText(this, "Database defaults inserted", ToastLength.Long).Show();
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, "Error when setting database defaults", ToastLength.Long).Show();
                }
            };

            var dbGetBtn = this.FindViewById<Button>(Resource.Id.dbGet);
            dbGetBtn.Click += (src, arg) =>
            {
                using (var db = new SQLiteConnection(this.DbPath))
                {
                    db.BeginTransaction();

                    var valuable = db.Get<Valuable>(0);

                    //db.Table<Bill>()

                    var bill = db.Get<Bill>(0);

                    db.Commit();
                }
            };

            var dbGetTablesBtn = this.FindViewById<Button>(Resource.Id.dbGetTables);
            dbGetTablesBtn.Click += (src, arg) =>
            {
                using (var db = new SQLiteConnection(this.DbPath))
                {
                    db.BeginTransaction();

                    var items = db.Query<Sqlite_Master>("SELECT * FROM sqlite_master");

                    //var cmd = db.CreateCommand("SELECT * FROM sqlite_master");

                    if (items.Count > 0)
                    { }

                    db.Commit();
                }
            };

            var callHistoryBtn = this.FindViewById<Button>(Resource.Id.callHistory);
            callHistoryBtn.Click += (src, arg) =>
            {
                var intent = new Intent(this, typeof(TablesActivity));
                intent.PutStringArrayListExtra("phone_numbers", phoneNumbers);
                StartActivity(intent);
            };
        }
    }
}