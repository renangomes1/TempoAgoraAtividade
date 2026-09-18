using TempoAgoraAtividade.Models;
using TempoAgoraAtividade.Services;

namespace TempoAgoraAtividade.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txt_cidade.Text))
                {
                    lbl_res.Text = "Preencha o nome da cidade.";
                    return;
                }

                if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                {
                    await DisplayAlert(
                        "Sem internet",
                        "Verifique sua conexão com a internet.",
                        "OK");
                    return;
                }

                Tempo? t = await DataService.GetPrevisao(
                    txt_cidade.Text.Trim());

                if (t != null)
                {
                    lbl_res.Text =
                        $"Latitude: {t.lat}\n" +
                        $"Longitude: {t.lon}\n" +
                        $"Nascer do Sol: {t.sunrise}\n" +
                        $"Pôr do Sol: {t.sunset}\n" +
                        $"Temp Máx: {t.temp_max} °C\n" +
                        $"Temp Min: {t.temp_min} °C\n" +
                        $"Descrição: {t.description}\n" +
                        $"Velocidade do Vento: {t.speed} m/s\n" +
                        $"Visibilidade: {t.visibility} metros";
                }
                else
                {
                    lbl_res.Text = "Não foi possível obter a previsão.";
                }
            }
            catch (HttpRequestException ex)
            {
                await DisplayAlert(
                    "Erro detalhado",
                    ex.ToString(),
                    "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert(
                    "Aviso",
                    ex.Message,
                    "OK");
            }
        }
    }
}