namespace PdfToolBoxMAUI.Views;

public partial class ProtectPdfPage : ContentPage
{
	public ProtectPdfPage(ProtectPdfViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
