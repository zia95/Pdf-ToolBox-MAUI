namespace PdfToolBoxMAUI.ViewModels;

[QueryProperty(nameof(Item), "Item")]
public partial class PdfListDetailViewModel : BaseViewModel
{
	[ObservableProperty]
	SampleItem item;
}
