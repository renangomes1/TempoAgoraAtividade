using TempoAgoraAtividade.Models;
using Newtonsoft.Json.Linq;
using System.Net;

namespace TempoAgoraAtividade.Services
{
    public class DataService
    {
        private static readonly HttpClient client = new();

        public static async Task<Tempo?> GetPrevisao(string cidade)
        {
            string chave = "INSIRA_AQUI_A_SUA_CHAVE";

            string url =
                "https://api.openweathermap.org/data/2.5/weather?" +
                $"q={Uri.EscapeDataString(cidade)}" +
                $"&units=metric&lang=pt_br&appid={chave}";

            HttpResponseMessage resp;

            try
            {
                resp = await client.GetAsync(url);
            }
            catch (HttpRequestException ex)
            {
                throw new Exception(
                    $"Falha na conexão: {ex.Message}", ex);
            }

            if (resp.StatusCode == HttpStatusCode.NotFound)
            {
                throw new Exception("Cidade não encontrada.");
            }

            if (!resp.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Erro na API: {resp.StatusCode}",
                    null,
                    resp.StatusCode);
            }

            string json = await resp.Content.ReadAsStringAsync();

            JObject dados = JObject.Parse(json);

            long nascer = (long)dados["sys"]!["sunrise"]!;
            long por = (long)dados["sys"]!["sunset"]!;

            return new Tempo
            {
                lat = (double?)dados["coord"]?["lat"],
                lon = (double?)dados["coord"]?["lon"],

                description =
                    (string?)dados["weather"]?[0]?["description"],

                main =
                    (string?)dados["weather"]?[0]?["main"],

                temp_min = (double?)dados["main"]?["temp_min"],
                temp_max = (double?)dados["main"]?["temp_max"],

                speed = (double?)dados["wind"]?["speed"],
                visibility = (int?)dados["visibility"],

                sunrise = DateTimeOffset
                    .FromUnixTimeSeconds(nascer)
                    .ToLocalTime()
                    .ToString("HH:mm"),

                sunset = DateTimeOffset
                    .FromUnixTimeSeconds(por)
                    .ToLocalTime()
                    .ToString("HH:mm")
            };
        }
    }
}