using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using mdlr;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;

namespace mtnc.uwp
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.Height = 60;
        }

        private async void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            string sql = await OpenFileAndGenerateSql();

            if (string.IsNullOrWhiteSpace(sql))
                return;

            var picker = new FileSavePicker() { SuggestedStartLocation = PickerLocationId.DocumentsLibrary };

            var encoding = new UTF8Encoding(false, true);

            var bytes = encoding.GetBytes(sql);

            picker.FileTypeChoices.Add("sql files", new List<string>() { ".sql" });

            var sqlFile = await picker.PickSaveFileAsync();
            await FileIO.WriteBytesAsync(sqlFile, bytes);
        }

        private async Task<string> OpenFileAndGenerateSql()
        {
            var picker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.List,
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };

            picker.FileTypeFilter.Add(".xml");

            var xmlFile = await picker.PickSingleFileAsync();

            string sql = null;

            if (xmlFile != null)
            {
                System.Text.EncodingProvider ppp;
                ppp = System.Text.CodePagesEncodingProvider.Instance;
                Encoding.RegisterProvider(ppp);

                IBuffer buffer = await FileIO.ReadBufferAsync(xmlFile);
                byte[] fileData = buffer.ToArray();
                Encoding encoding = Encoding.GetEncoding("Windows-1252");
                string txt = encoding.GetString(fileData, 0, fileData.Length);

                sql = ImportExport.LoadAndGenerateSql(txt);
            }

            return sql;
        }
    }
}