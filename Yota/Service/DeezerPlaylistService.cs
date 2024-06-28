using Newtonsoft.Json;
using Yota.Commons;
using Yota.Model;

namespace Yota.Service;

public class DeezerPlaylistService
{
    private readonly HttpClient _httpClient;

    public DeezerPlaylistService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<Playlist?> GetPlaylistAsync(long playlistId)
    {
        var url = $"{Constants.DeezerApiUrl}{Constants.DeezerPlaylistEndpoint}/{playlistId}";
        HttpResponseMessage response = await _httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var playlist = JsonConvert.DeserializeObject<Playlist>(jsonResponse);
            return playlist is null ? null : playlist;
        }
        else
        {
            Console.WriteLine($"Error fetching playlist: {response.StatusCode}");
            return null;
        }
    }

    public async Task<List<Track>> GetUserFlowAsync(long userId)
    {
        var url = $"{Constants.DeezerApiUrl}{Constants.DeezerUserEndpoint}/{userId}/flow";
        HttpResponseMessage response = await _httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var flow = JsonConvert.DeserializeObject<DeezerFlowResponse>(jsonResponse);
            return flow is null ? [] : flow.Data;
        }
        else
        {
            Console.WriteLine($"Error fetching playlist: {response.StatusCode}");
            return [];
        }
    }

    public async Task<List<Playlist>> GetPlaylistsAsync()
    {
        return
        [
            await GetPlaylistAsync(Constants.ZDCL),
            await GetPlaylistAsync(Constants.Playlistche),
            await GetPlaylistAsync(Constants.TeAlegra),
            await GetPlaylistAsync(Constants.AooooXibaata),
            await GetPlaylistAsync(Constants.RadarPagode),
            await GetPlaylistAsync(Constants.GlobalDanceHits),
            await GetPlaylistAsync(Constants.RemixBrasil),
            await GetPlaylistAsync(Constants.TopHitsInternacionais),
            await GetPlaylistAsync(Constants.ReggaeEssentials)
        ];
    }

    private record DeezerFlowResponse
    {
        public List<Track> Data { get; set; }
    }
}