namespace PdfToolBoxMAUI.Views;

public partial class CompressPdfPage : ContentPage
{
	public CompressPdfPage(CompressPdfViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
