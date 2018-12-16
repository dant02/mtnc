using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;

namespace android
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
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
        }
    }
}