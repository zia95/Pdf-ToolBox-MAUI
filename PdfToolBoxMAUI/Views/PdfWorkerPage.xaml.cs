namespace PdfToolBoxMAUI.Views;

public partial class PdfWorkerPage : ContentPage
{
	public PdfWorkerPage(PdfWorkerViewModel pwvm)
	{
		InitializeComponent();

        pwvm.SetDispatcher(this.Dispatcher);
        this.BindingContext = pwvm;
        
    }
}