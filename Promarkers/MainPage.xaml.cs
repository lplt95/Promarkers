using Promarkers.Database;

namespace Promarkers;

public partial class MainPage : ContentPage
{
	private readonly PromarkerService promarkerService;

	public MainPage()
	{
		InitializeComponent();
		promarkerService = new PromarkerService();
	}

	private async void NavigateToBrowse(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new BrowsePage());
	}

	private async void ImportFromFile(object sender, EventArgs e)
	{
		var updateExisting = await DisplayAlert("Tryb importowania", "Czy chcesz zaaktualizować istniejące rekordy przy imporcie?", "Tak", "Nie");
		activity.IsRunning = true;
		var result = await promarkerService.ImportDataFromFile(updateExisting);
		activity.IsRunning = false;
		await DisplayAlert("Sukces", result, "OK");
	}
}

