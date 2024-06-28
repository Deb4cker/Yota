using CommunityToolkit.Maui.Core.Primitives;
namespace Yota;

public partial class PlayerPage : ContentPage
{
	public PlayerPage()
	{
		InitializeComponent();
	}

    private void OnPreviousClicked(object sender, EventArgs e)
    {
        MainPage.PlayPrevious();
    }

    private void OnPlayPauseClicked(object sender, EventArgs e)
    {
        MainPage.PlayAndPause();
    }

    private void OnNextClicked(object sender, EventArgs e)
    {
        MainPage.PlayNext();
    }
}