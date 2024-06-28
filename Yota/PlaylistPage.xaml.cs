using Yota.Commons;
using Yota.Model;
using Yota.Service;

namespace Yota;

public partial class PlaylistPage : ContentPage
{
    private Playlist? _playlist;
    private List<Track> _tracks = [];
    private readonly DeezerPlaylistService _playlistService;

	public PlaylistPage(Playlist playlist)
	{
        _playlistService = new DeezerPlaylistService();
        _playlist = playlist;
        _tracks = playlist.Tracks.Data;
        InitializeComponent();
    }

	protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_playlist is null) await LoadData();

        if (_playlist != null)
        {
            Title = _playlist.Title;
            PlaylistName.Text = _playlist.Title;
            PlaylistDescription.Text = _playlist.Description;
            TotalDuration.Text = $"⌚ {TimeSpan.FromSeconds(_playlist.Duration).ToString(@"hh\:mm\:ss")}";
            PlaylistView.ItemsSource = _tracks;
            PlaylistPicture.HeightRequest = 120;
            PlaylistPicture.WidthRequest = 120;
            PlaylistPicture.Source = _playlist.Picture;
        } 
        else
        {
            await DisplayAlert("Erro", "Não foi possível carregar a playlist", "OK");
        }
    }

	private async Task LoadData()
    {
        _playlist = await _playlistService.GetPlaylistAsync(Constants.ZDCL);
        _tracks = _playlist.Tracks.Data;
    }

    private void OnPlayButtonClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var previewUrl = button?.CommandParameter as string;

        if (!string.IsNullOrEmpty(previewUrl))
        {
            PlayPreview(previewUrl);
            MainPage.RestartQueue(_tracks);
        }
    }

    private void PlayPreview(string url)
    {
        MainPage.Play(url);
    }
}