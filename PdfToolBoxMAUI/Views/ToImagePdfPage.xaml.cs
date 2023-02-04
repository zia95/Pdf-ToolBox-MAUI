namespace PdfToolBoxMAUI.Views;

public partial class ToImagePdfPage : ContentPage
{
	public ToImagePdfPage(ToImagePdfViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
