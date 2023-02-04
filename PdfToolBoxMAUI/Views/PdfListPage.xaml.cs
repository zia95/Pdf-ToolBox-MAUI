namespace PdfToolBoxMAUI.Views;

public partial class PdfListPage : ContentPage
{
	PdfListViewModel ViewModel;

	public PdfListPage(PdfListViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = ViewModel = viewModel;
	}

	protected override async void OnNavigatedTo(NavigatedToEventArgs args)
	{
		base.OnNavigatedTo(args);

		await ViewModel.LoadDataAsync();
	}
}
