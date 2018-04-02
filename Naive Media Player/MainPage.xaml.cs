using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.ViewManagement;
using Windows.UI;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.Media.Core;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace Naive_Media_Player
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            CustomTitleBar();
        }

        void CustomTitleBar()
        {
            var view = ApplicationView.GetForCurrentView();
            view.TitleBar.BackgroundColor = Color.FromArgb(255,0,120,215);
            view.TitleBar.ButtonBackgroundColor= Color.FromArgb(255, 0, 120, 215);
        }



        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            var picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = PickerLocationId.VideosLibrary;
            picker.FileTypeFilter.Add(".mp3");
            picker.FileTypeFilter.Add(".mp4");

            StorageFile MediaFile = await picker.PickSingleFileAsync();

            if (MediaFile != null)
            {
                MPE.Source = MediaSource.CreateFromStorageFile(MediaFile);
                MPE.MediaPlayer.Play();
                
            }
            else
            {
                TB001.Text = "Something went wrong.";
            }
        }
    }
}
