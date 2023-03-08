using Syncfusion.Maui.PdfViewer;

namespace PdfToolBoxMAUI.Views;

public partial class EditPagePdfPage : ContentPage
{
	public EditPagePdfPage(EditPagePdfViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;

	}
}
