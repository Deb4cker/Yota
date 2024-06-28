using Yota.Model;
using Yota.Service;
using Yota.Commons;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Maui.Core.Primitives;

namespace Yota
{
    public partial class MainPage : ContentPage
    {
        public static MediaElement GlobalMedia { get; private set; }
        public static List<string>? SrcQueue = [];
        public static int CurrentIndex = 0;

        private List<Playlist> _playlists;
        private readonly DeezerPlaylistService _deezerPlaylistService;

        public MainPage()
        {
            _deezerPlaylistService = new DeezerPlaylistService();
            InitializeComponent();
            GlobalMedia = GlobalMediaElement;
            LoadData();

            PlaylistView.SelectionChanged += OnPlaylistSelected;

            SizeChanged += MainPage_SizeChanged;
        }

        private async Task LoadData()
        {
            _playlists = await _deezerPlaylistService.GetPlaylistsAsync();
            PlaylistView.ItemsSource = _playlists;

            var preloadFlow = await _deezerPlaylistService.GetUserFlowAsync(Constants.TestUserId);
            RestartQueue(preloadFlow);
        }

        private async void OnPlaylistSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Playlist selectedPlaylist)
            {
                await Navigation.PushAsync(new PlaylistPage(selectedPlaylist));
                ((CollectionView)sender).SelectedItem = null;
            }


        }

        private void MainPage_SizeChanged(object sender, EventArgs e)
        {
            int columns = (int)(Width / 150);
            if (columns < 1)
                columns = 1;

            ((GridItemsLayout)PlaylistView.ItemsLayout).Span = columns;
        }

        public static void PlayNext()
        {
            if (SrcQueue!.Count > 0)
            {
                if (CurrentIndex < SrcQueue.Count - 1)
                {
                    CurrentIndex++;
                }
                else
                {
                    CurrentIndex = 0;
                }

                GlobalMedia.Source = SrcQueue[CurrentIndex];
                GlobalMedia.Play();
            }
        }

        public static void PlayPrevious()
        {
            if (SrcQueue!.Count > 0)
            {
                if (CurrentIndex > 0)
                {
                    CurrentIndex--;
                }
                else
                {
                    CurrentIndex = SrcQueue.Count - 1;
                }

                GlobalMedia.Source = SrcQueue[CurrentIndex];
                GlobalMedia.Play();
            }
        }

        public static void PlayAndPause()
        {
            if (GlobalMedia.CurrentState == MediaElementState.Playing)
            {
                GlobalMedia.Pause();
            }
            else
            {
                GlobalMedia.Play();
            }

        }

        public static void RestartQueue(List<Track> src)
        {
            CurrentIndex = 0;
            SrcQueue!.Clear();

            foreach (var item in src)
            {
                SrcQueue.Add(item.Preview);
            }
        }

        public static void Play(string url)
        {
            GlobalMedia.Source = url;
            GlobalMedia.Play();
        }


    }
}
