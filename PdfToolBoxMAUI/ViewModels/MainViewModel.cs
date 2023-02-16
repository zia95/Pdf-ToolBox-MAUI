namespace PdfToolBoxMAUI.ViewModels;

public partial class MainViewModel : BaseViewModel
{
	[ObservableProperty]
	FileResult fileResult;

	[RelayCommand]
	public void OnFileResult(object? param)
	{
		var fr = (FileResult)param;

	}
}
