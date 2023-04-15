using Promarkers.Models;
using Promarkers.Models.Enums;

namespace Promarkers;

public partial class BrowsePage : ContentPage
{
	private readonly PromarkerService _promarkerService;
	private List<PromarkerModel> promarkerList = new List<PromarkerModel>();

	public BrowsePage()
	{
		InitializeComponent();
		_promarkerService = new PromarkerService();
		globalFilter.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);
		typePicker.ItemsSource = Enum.GetValues(typeof(MarkerTypeEnum));
		colorPicker.ItemsSource = Enum.GetValues(typeof(RegularColorFamilyEnum));
		typePicker.SelectedIndex = 0;
		colorPicker.SelectedIndex = 0;
		typePicker.SelectedIndexChanged += TypeSelectionChange;
		colorPicker.SelectedIndexChanged += ColorSelectionChange;
		LoadData();
	}

	private void TypeSelectionChange(object sender, EventArgs e)
	{
		var markerType = (MarkerTypeEnum)typePicker.SelectedItem;
		switch (markerType)
		{
			case MarkerTypeEnum.Regular:
				{
					colorPicker.ItemsSource = Enum.GetValues(typeof(RegularColorFamilyEnum));
					colorPicker.IsEnabled = true;
					break;
				}
			case MarkerTypeEnum.Brush:
				{
					colorPicker.ItemsSource = Enum.GetValues(typeof(BrushColorFamilyEnum));
					colorPicker.IsEnabled = true;
					break;
				}
			case MarkerTypeEnum.Metalic:
				{
					colorPicker.ItemsSource = Enum.GetValues(typeof(MetalicColorFamilyEnum));
					colorPicker.IsEnabled = false;
					break;
				}
			case MarkerTypeEnum.Neon:
				{
					colorPicker.ItemsSource = Enum.GetValues(typeof(NeonColorFamilyEnum));
					colorPicker.IsEnabled = false;
					break;
				}
		}

		colorPicker.SelectedIndex = 0;
	}

	private void ColorSelectionChange(object sender, EventArgs e)
	{
		LoadData();
	}

	private async void LoadData()
	{
		MarkerTypeEnum markerType = (MarkerTypeEnum)typePicker.SelectedItem;
		var selectedColor = colorPicker.SelectedIndex;
		
		promarkerList = await _promarkerService.GetMarkers(markerType, selectedColor);
		
        FilterData();
	}

	private void FilterData()
	{
		bool activeOnly = activeOnlySwitch.IsToggled;
		string textFilter = globalFilter.Text;
		var filteredList = promarkerList;
		if (activeOnly)
		{
			filteredList = filteredList.Where(x => x.IsOwned == true).ToList();
		}
		if (!string.IsNullOrWhiteSpace(textFilter))
		{
			filteredList = filteredList.Where(x => x.ColorName.Contains(textFilter) || x.ColorCode.Contains(textFilter)).ToList();
		}
		lvBrowsePage.ItemsSource = filteredList;
    }

    private void globalFilter_TextChanged(object sender, TextChangedEventArgs e)
    {
		FilterData();
    }

    private async void lvBrowsePage_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
		if (e.SelectedItem == null) return;
		var promarkerModel = (PromarkerModel)e.SelectedItem;
        string messageText = promarkerModel.IsOwned ? StringConstants.StatusChangeToNotOwned : StringConstants.StatusChangeToOwned;
        var result = await DisplayAlert("Zmiana statusu", messageText, "Tak", "Nie");
        if (result)
        {
            await _promarkerService.ChangeStatus(promarkerModel);
			LoadData();
        } 
		else
		{
			lvBrowsePage.SelectedItem = null;
		}
    }

    private void Switch_Toggled(object sender, ToggledEventArgs e)
    {
		FilterData();
    }
}