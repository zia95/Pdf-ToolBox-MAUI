using System.Reflection;

namespace PdfToolBoxMAUI.ViewModels;

public partial class EditPagePdfViewModel : BaseViewModel
{
	FileHelperService _fhs;
	public EditPagePdfViewModel(FileHelperService fileHelperService)
	{
		_fhs = fileHelperService;
	}

	[ObservableProperty]
	private Stream _pdfDocStream;

	[RelayCommand]
	public async void OpenPdfToDisplay()
	{
		var f = (await _fhs.PickAndShowPdfMultiAsync())?.FirstOrDefault();
		if (f is not null)
		{
			this.PdfDocStream = await f.OpenReadAsync();
		}
	}
}
