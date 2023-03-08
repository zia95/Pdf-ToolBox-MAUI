using CommunityToolkit.Maui.Views;
using Syncfusion.Maui.PdfViewer;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Parsing;
using System.Collections;

namespace PdfToolBoxMAUI.ViewModels;

public partial class MergePdfViewModel : BaseViewModel
{
	[ObservableProperty]
	public ObservableCollection<PdfFile> items = new();


	public bool DisplayHeader { get { return Items?.Count > 0; } }


	private FileHelperService _fhs;
	private PdfProcessingService _pps;
	//private readonly string _img_dir = Path.Combine(FileSystem.Current.AppDataDirectory, "merge_thumbnails");

	public MergePdfViewModel(FileHelperService fileHelperService, PdfProcessingService pps)
	{
		_fhs = fileHelperService;
		_pps = pps;
	}





	[RelayCommand]
	public async void AddDocument()
	{

		var files = await _fhs.PickAndShowPdfMultiAsync();

		if(files is not null)
		{
			foreach(var file in files)
			{
				read_and_add_doc_async(file);
			}
		}
	}
	[RelayCommand]
	public async void MergeDocument()
	{
		await Shell.Current.GoToAsync(nameof(PdfWorkerPage), true, new Dictionary<string, object>
		{
			{ "files", Items.ToArray() },
			{ "workid", PdfWorkerViewModel.WorkIdMerge }
		});
	}



	[RelayCommand]
	public async Task ListItemTapped(PdfFile item)
	{
		bool accept = await Application.Current.MainPage.DisplayAlert("Detail", $"Do you want to remove this document? \nFile: {item.FileName}", "Yes", "No");

		if(accept)
		{
			Items.Remove(item);
		}
	}



	private async Task read_and_add_doc_async(FileResult file)
	{
		var pdf = await _pps.ReadPdfFromResultAsync(file, async () => await Application.Current.MainPage.DisplayPromptAsync("Password", "Document is protected. Please enter password.", placeholder: "Password"));

		if(pdf is null)
		{
			await Application.Current.MainPage.DisplayAlert("Error while reading document", $"Failed to read. Make sure the specified file is valid. \nSource: {file.FullPath}", "OK");

			return;
		}

		pdf.Description = pdf.FileName;

		Items.Add(pdf);
	}
}
