namespace PdfToolBoxMAUI.ViewModels;

public partial class SampleViewModel : BaseViewModel
{




	public SampleViewModel()
	{
		/*
		 items = new List<Tools>()
			{
				new Tools { Id = Tools.Ids.Generated, Image = ImageSource.FromResource("PDF_ToolBox.Resources.Images.tool_savedpdfs.png", typeof(ToolsDataStore).Assembly), Text = "Generated Pdfs", Description="Output pdf files." },
				new Tools { Id = Tools.Ids.Split, Image = ImageSource.FromResource("PDF_ToolBox.Resources.Images.tool_split.png", typeof(ToolsDataStore).Assembly), Text = "Split", Description="Split pdf file." },
				new Tools { Id = Tools.Ids.Merge, Image = ImageSource.FromResource("PDF_ToolBox.Resources.Images.tool_merge.png", typeof(ToolsDataStore).Assembly), Text = "Merge", Description="Merge pdf file." },
				new Tools { Id = Tools.Ids.RemovePage, Image = ImageSource.FromResource("PDF_ToolBox.Resources.Images.tool_remove.png", typeof(ToolsDataStore).Assembly), Text = "Remove Page(s)", Description="Remove pages from pdf file." },
				new Tools { Id = Tools.Ids.Lock, Image = ImageSource.FromResource("PDF_ToolBox.Resources.Images.tool_lock.png", typeof(ToolsDataStore).Assembly), Text = "Lock", Description="Lock pdf file." },
				new Tools { Id = Tools.Ids.Unlock, Image = ImageSource.FromResource("PDF_ToolBox.Resources.Images.tool_unlock.png", typeof(ToolsDataStore).Assembly), Text = "Unlock", Description="Remove password from pdf file." },
				new Tools { Id = Tools.Ids.Watermark, Image = ImageSource.FromResource("PDF_ToolBox.Resources.Images.tool_watermark.png", typeof(ToolsDataStore).Assembly), Text = "Watermark", Description="Add watermark on pdf file." },
				new Tools { Id = Tools.Ids.Compress, Image = ImageSource.FromResource("PDF_ToolBox.Resources.Images.tool_compress.png", typeof(ToolsDataStore).Assembly), Text = "Compress", Description="Compress pdf file." },
				new Tools { Id = Tools.Ids.RotatePage, Image = ImageSource.FromResource("PDF_ToolBox.Resources.Images.tool_rotate.png", typeof(ToolsDataStore).Assembly), Text = "Rotate", Description="Rotate page(s) from pdf file." },
				new Tools { Id = Tools.Ids.ImagesToPdf, Image = ImageSource.FromResource("PDF_ToolBox.Resources.Images.tool_imagestopdf.png", typeof(ToolsDataStore).Assembly), Text = "Images To Pdf", Description="Generate pdf file from images." },
				new Tools { Id = Tools.Ids.ViewInformation, Image = ImageSource.FromResource("PDF_ToolBox.Resources.Images.tool_viewinformation.png", typeof(ToolsDataStore).Assembly), Text = "View Info", Description="View or Edit pdf information." }
			};
		 */
	}



	int count = 0;

	[ObservableProperty]
	public string message = "Click me";

	[RelayCommand]
	private void OnCounterClicked()
	{
		count++;

		if (count == 1)
			Message = $"Clicked {count} time";
		else
			Message = $"Clicked {count} times";

		SemanticScreenReader.Announce(Message);
	}
}
