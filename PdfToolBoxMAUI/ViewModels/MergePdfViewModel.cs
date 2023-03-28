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
	IDispatcher _disp;
	public MergePdfViewModel(FileHelperService fileHelperService, PdfProcessingService pps)
	{
		_fhs = fileHelperService;
		_pps = pps;
	}

	public void SetDispatcher(IDispatcher dispatcher)
	{
		_disp = dispatcher;
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
		if(items.Count <= 1)
		{
			await Application.Current.MainPage.DisplayAlert("Detail", $"At minimum 2 documents can be merged. Add more documents and try again.", "OK");
			return;
		}
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
		var pdf = _pps.ReadPdfFromResult(file, () =>
		{
			var t = Shell.Current.DisplayPromptAsync("Password", "Document is protected. Please enter password.", placeholder: "Password");
			//t.Start();
			//t.Wait();
			return t.Result;
		});

		if(pdf is null)
		{
			await Application.Current.MainPage.DisplayAlert("Error while reading document", $"Failed to read. Make sure the specified file is valid. \nSource: {file.FullPath}", "OK");

			return;
		}

		pdf.Description = pdf.FileName;

		await _disp.DispatchAsync(() => Items.Add(pdf));
		
	}
}
