namespace PdfToolBoxMAUI.Views;

public partial class MergePdfPage : ContentPage
{
	public MergePdfPage(MergePdfViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

}
