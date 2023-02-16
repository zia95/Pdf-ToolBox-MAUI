using System.Windows.Input;

namespace PdfToolBoxMAUI.Views.Controls;

public partial class FilePickerView : ContentView
{
	public static readonly BindableProperty CommandProperty =
		BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(FilePickerView), null, BindingMode.OneWay);

	public static readonly BindableProperty ResultProperty =
		BindableProperty.Create(nameof(Result), typeof(FileResult), typeof(FilePickerView), null, BindingMode.OneWayToSource);

	


	public enum FileType
	{
		Pdf,
		Images,
		Videos,
		Any
	}
	public enum FileDisplayType
	{
		Name,
		FullPath
	}
	public string DialogTitle { get; set; }
	public FileType DialogFileTypeFilter { get; set; }
	public FileDisplayType DisplayType { get; set; }
	public ICommand Command
	{
		get => (ICommand)GetValue(CommandProperty);
		set => SetValue(CommandProperty, value);
	}

	public FileResult Result 
	{ 
		get => (FileResult)GetValue(ResultProperty); 
		private set {
			SetValue(ResultProperty, value);
			_entrypath.Text = DisplayType is FileDisplayType.Name ? value?.FileName :  value?.FullPath;
		}
	}

	public FilePickerView()
	{
		InitializeComponent();
		
	}

	private async void OnFilePickerClicked(object sender, EventArgs e)
	{
		var f = await FilePicker.PickAsync(
			new PickOptions()
			{
				PickerTitle = DialogTitle,
				FileTypes = DialogFileTypeFilter switch
				{
					FileType.Pdf => FilePickerFileType.Pdf,
					FileType.Images => FilePickerFileType.Images,
					FileType.Videos => FilePickerFileType.Videos,
					_ => null
				}
			});

		if(f is not null)
		{
			Result = f;
			if (Command is not null)
			{
				if (Command.CanExecute(Result))
				{
					Command.Execute(Result);
				}
			}
		}
		
	}
}