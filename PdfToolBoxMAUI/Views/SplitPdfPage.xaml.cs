namespace PdfToolBoxMAUI.Views;

public partial class SplitPdfPage : ContentPage
{
	public SplitPdfPage(SplitPdfViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
