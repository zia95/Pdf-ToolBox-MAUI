namespace PdfToolBoxMAUI.Views;

public partial class PdfWorkerPage : ContentPage
{
	public PdfWorkerPage(PdfWorkerViewModel pwvm)
	{
		InitializeComponent();
		this.BindingContext = pwvm;
	}
}