namespace PdfToolBoxMAUI.Views;

public partial class PdfListDetailPage : ContentPage
{
	public PdfListDetailPage(PdfListDetailViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
