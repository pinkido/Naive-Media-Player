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
using System.Web;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.Media.Core;
using Windows.Web.Http;
using Windows.UI.Popups;



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
            ApplicationView.PreferredLaunchViewSize = new Size(1280,720);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            
        }



        private async void FileButton_Click(object sender, RoutedEventArgs e)
        {
            if (MPE.Source != null){
                MPE.MediaPlayer.Pause();
            }
             
            var picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = PickerLocationId.VideosLibrary;
            picker.FileTypeFilter.Add(".mp3");
            picker.FileTypeFilter.Add(".mp4");

            StorageFile MediaFile = await picker.PickSingleFileAsync();

            if (MediaFile != null)
            {
                MPE.Source = MediaSource.CreateFromStorageFile(MediaFile);
                MPE.AreTransportControlsEnabled = true;
                TB001.Text = MediaFile.Name;
                MPE.MediaPlayer.Play();
                if (MediaFile.FileType == ".mp3")
                {
                    musicIcon.Visibility = Visibility.Visible;
                }
                else { musicIcon.Visibility = Visibility.Collapsed; }
                
            }
          
        }

        private async void PlayOnlineMusic(object sender, RoutedEventArgs e)
        {
            if (MPE.Source != null)
            {
                MPE.MediaPlayer.Pause();
            }
            await content_dialog001.ShowAsync();

        }

        private async void content_dialog001_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            try {
                MPE.Source = MediaSource.CreateFromUri(new Uri(textbox001.Text));
                MPE.AreTransportControlsEnabled = true;
                MPE.MediaPlayer.Play();
                textbox001.Text = "";
                musicIcon.Visibility = Visibility.Visible;
            }
            catch {
                MessageDialog message_dialog = new MessageDialog("Invalid Link!! Try agian.") {
                       
                };
                await message_dialog.ShowAsync();
                textbox001.Text = "";
            }       
        }

        private async void content_dialog002_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
           try
            {
                ring.IsActive = true;
                ring.Visibility = Visibility.Visible;
                onlineButton.IsEnabled = false;
                openButton.IsEnabled = true;


                var httpClient = new HttpClient();
                var buffer = await httpClient.GetBufferAsync(new Uri(textbox002.Text));

                if (buffer != null && buffer.Length > 0u)
                {

                var musicCache = await KnownFolders.MusicLibrary.CreateFileAsync("Cache00.mp3", CreationCollisionOption.GenerateUniqueName);
                
                    using (var stream = await musicCache.OpenAsync(FileAccessMode.ReadWrite))
                    {

                        await stream.WriteAsync(buffer);
                        await stream.FlushAsync();

                    }
                    /*await FileIO.WriteBufferAsync(musicCache, buffer);*/
                    ring.IsActive = false;
                    ring.Visibility = Visibility.Collapsed;
                    onlineButton.IsEnabled = true;
                    openButton.IsEnabled = true;

                    MPE.Source = MediaSource.CreateFromStorageFile(musicCache);
                    MPE.AreTransportControlsEnabled = true;
                    MPE.MediaPlayer.Play();
                    textbox002.Text = "";

                    musicIcon.Visibility = Visibility.Visible;
                }

            }
            catch
            {
                MessageDialog message_dialog = new MessageDialog("Invalid Link!! Try agian.")
                {

                };
                await message_dialog.ShowAsync();
                textbox002.Text = "";
            }
            ring.IsActive = true;
            ring.Visibility = Visibility.Visible;
        }

        private async void DownloadandPlay(object sender, RoutedEventArgs e)
        {
            if (MPE.Source != null)
            {
                MPE.MediaPlayer.Pause();
            }
            await content_dialog002.ShowAsync();

        }
    }
}


