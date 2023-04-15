namespace Promarkers.Models.Templates;

public class PromarkerDataTemplateSelector : DataTemplateSelector
{
    public DataTemplate OwnedMarker { get; set; }

    public DataTemplate NotOwnedMarker { get; set; }

    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
        return ((PromarkerModel)item).IsOwned ? OwnedMarker : NotOwnedMarker;
    }
}
