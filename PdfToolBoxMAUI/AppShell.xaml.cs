namespace PdfToolBoxMAUI;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(PdfListDetailPage), typeof(PdfListDetailPage));
	}
}
