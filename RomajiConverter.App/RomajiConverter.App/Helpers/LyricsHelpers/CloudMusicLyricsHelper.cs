using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RomajiConverter.App.Models;

namespace RomajiConverter.App.Helpers.LyricsHelpers;

public class CloudMusicLyricsHelper : LyricsHelper
{
    public static async Task<List<MultilingualLrc>> GetLrc(string songId)
    {
        try
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://music.163.com/");
            var jpnLrcResponse = await client.GetAsync($"api/song/media?id={songId}");
            var content = JObject.Parse(await jpnLrcResponse.Content.ReadAsStringAsync());
            var jpnLrcText = content["lyric"].ToString();

            var chnLrcResponse = await client.GetAsync($"api/song/lyric?os=pc&id={songId}&tv=-1");
            content = JObject.Parse(await chnLrcResponse.Content.ReadAsStringAsync());

            var chnLrcText = content["tlyric"]["lyric"].ToString();

            return ParseLrc(jpnLrcText, chnLrcText);
        }
        catch
        {
            throw new Exception("GetLyricsError");
        }
    }
}