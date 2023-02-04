namespace PdfToolBoxMAUI.Views;

public partial class WatermarkPdfPage : ContentPage
{
	public WatermarkPdfPage(WatermarkPdfViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
