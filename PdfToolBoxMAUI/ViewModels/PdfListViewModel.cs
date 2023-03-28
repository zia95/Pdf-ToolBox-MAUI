namespace PdfToolBoxMAUI.ViewModels;

public partial class PdfListViewModel : BaseViewModel
{
	readonly ProcessedDocumentService dataService;
	readonly PdfProcessingService _pps;

	[ObservableProperty]
	bool isRefreshing;

	[ObservableProperty]
	ObservableCollection<PdfFileGroupList> items;

	public PdfListViewModel(ProcessedDocumentService service, PdfProcessingService pps)
	{
		dataService = service;
		_pps = pps;
	}

	[RelayCommand]
	private async void OnRefreshing()
	{
		IsRefreshing = true;

		try
		{
			await LoadDataAsync();
		}
		finally
		{
			IsRefreshing = false;
		}
	}

	public async Task LoadDataAsync()
	{
		Items = new();


		var mf = dataService.GetProcessedMergeDoc();


		var sf = dataService.GetProcessedSplitDoc();


		PdfFileGroupList group_merge = new PdfFileGroupList("MERGE");

		var merge_docs = dataService.GetProcessedMergeDoc();

		if(merge_docs is not null)
		{
			foreach(var f in merge_docs)
			{
				var pdf = _pps.ReadPdfFromFile(f, null);
				if(pdf is not null)
				{
					group_merge.Add(pdf);
				}
				else
				{
					await Application.Current.MainPage.DisplayAlert("Error", $"Unexpected error while reading from file.\nSource:{f}", "OK");
				}
			}
		}


		PdfFileGroupList group_split = new PdfFileGroupList("SPLIT");


		Items.Add(group_merge);
		Items.Add(group_split);
	}

	[RelayCommand]
	private async void GoToDetails(SampleItem item)
	{
		await Shell.Current.GoToAsync(nameof(PdfListDetailPage), true, new Dictionary<string, object>
		{
			{ "Item", item }
		});
	}
}
