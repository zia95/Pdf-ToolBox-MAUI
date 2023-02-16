namespace PdfToolBoxMAUI.Views;

public partial class MainPage : ContentPage
{
	public MainPage(MainViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel; 
	}

	/*
	void _log_writeline(string line) => log.Text = $"{line}\n{log.Text}";
	
	private void DragGestureRecognizer_DragStarting(object sender, DragStartingEventArgs e)
	{
		var v = (sender as DragGestureRecognizer).Parent as Label;
		e.Data.Properties.Add("data", v.Text);

		lblstatus.Text = $"started... handled:{e.Handled}, cancel:{e.Cancel}, data:{v.Text}";
		_log_writeline(lblstatus.Text);

	}

	private void DropGestureRecognizer_Drop(object sender, DropEventArgs e)
	{
		var v = (sender as DropGestureRecognizer).Parent as Label;
		v.Text = e.Data.Properties["data"] as string;
		lblstatus.Text = $"dropped... data:{e.Data.Properties["data"]}";
		_log_writeline(lblstatus.Text);
		DropGestureRecognizer_DragLeave(sender, null);

	}

	bool _isin = false;
	private void DropGestureRecognizer_DragOver(object sender, DragEventArgs e)
	{
		if(_isin == false)
		{
			e.AcceptedOperation = DataPackageOperation.Copy;
			var v = (sender as DropGestureRecognizer).Parent as Label;

			v.Background = Color.FromRgb(255, 0, 0);
			
			_isin = true;
			_log_writeline("in..");
		}
		//
		//
	}

	private void DropGestureRecognizer_DragLeave(object sender, DragEventArgs e)
	{
		var v = (sender as DropGestureRecognizer).Parent as Label;

		v.Background = Color.FromRgb(0, 160, 0);
		_isin = false;
		_log_writeline("out..");
	}
	*/
}
