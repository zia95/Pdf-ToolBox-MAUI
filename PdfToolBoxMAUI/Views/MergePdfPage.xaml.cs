namespace PdfToolBoxMAUI.Views;

public partial class MergePdfPage : ContentPage
{
	public MergePdfPage(MergePdfViewModel viewModel)
	{
		InitializeComponent();
		viewModel.SetDispatcher(this.Dispatcher);
		BindingContext = viewModel;
	}

}
