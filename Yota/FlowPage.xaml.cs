using Yota.Commons;
using Yota.Model;
using Yota.Service;

namespace Yota;

public partial class FlowPage : ContentPage
{
	private List<Track> _tracks;
	private DeezerPlaylistService _service;

	public FlowPage()
	{
		_service = new DeezerPlaylistService();
		_tracks = new List<Track>();
		InitializeComponent();
	}

	protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_tracks.Count == 0) await LoadData();

        FlowView.ItemsSource = _tracks;
    }

	private async Task LoadData()
    {
        _tracks = await _service.GetUserFlowAsync(Constants.TestUserId);
    }
}