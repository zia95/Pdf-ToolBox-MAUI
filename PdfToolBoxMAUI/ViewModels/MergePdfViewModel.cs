using Syncfusion.Pdf;
using Syncfusion.Pdf.Parsing;
using System.Collections;

namespace PdfToolBoxMAUI.ViewModels;

public partial class MergePdfViewModel : BaseViewModel
{
	[ObservableProperty]
	public ObservableCollection<PdfFile> items = new();


	public bool DisplayHeader { get { return Items?.Count > 0; } }


	private FileHelperService _fhs;
	private readonly string _img_dir = Path.Combine(FileSystem.Current.AppDataDirectory, "merge_thumbnails");

	public MergePdfViewModel(FileHelperService fileHelperService)
	{
		_fhs = fileHelperService;

		if (!Directory.Exists(_img_dir))
		{
			Directory.CreateDirectory(_img_dir);
		}
		else
		{
			//delete img cache
			foreach(var f in Directory.EnumerateFiles(_img_dir))
			{
				File.Delete(f);
			}
		}
	}





	[RelayCommand]
	public async void AddDocument()
	{

		var files = await _fhs.PickAndShowPdfMultiAsync();

		if(files is not null)
		{
			foreach(var file in files)
			{
				read_and_add_doc_async(file);
			}
		}
	}



	[RelayCommand]
	public async Task ListItemTapped(PdfFile item)
	{
		bool accept = await Application.Current.MainPage.DisplayAlert("Detail", $"Do you want to remove this document? \nFile: {item.FileName}", "Yes", "No");

		if(accept)
		{
			Items.Remove(item);
		}
	}



	private async Task read_and_add_doc_async(FileResult file)
	{
		using Stream fs = await file.OpenReadAsync();

		if (fs is null) return;

		PdfFile pdfFile = new PdfFile();

		pdfFile.Id = Guid.NewGuid().ToString();
		pdfFile.File = file;
		pdfFile.FilePath = file.FullPath;
		pdfFile.FileName = file.FileName;
		pdfFile.Thumbnail = "iconblank.png";

		try
		{
			using var pdf_doc = new PdfLoadedDocument(fs);

			
			var title = pdf_doc.DocumentInformation.Title;

			pdfFile.Description = string.IsNullOrWhiteSpace(title) ? pdfFile.FileName : title;


		}
		catch (PdfException ex) 
		{
			await Application.Current.MainPage.DisplayAlert("Error while reading document", $"File: {file.FullPath}\nError: {ex.Message}" , "OK");
			return;
		}
		



		using Syncfusion.Maui.PdfToImageConverter.PdfToImageConverter pdftoimg = new(fs);

		using Stream img_strm = await pdftoimg.ConvertAsync(0, new Size(96, 128));

		



		if (img_strm is not null)
		{

			string img_file = Path.Combine(_img_dir, pdfFile.Id);

			using var img_fs = new FileStream(img_file, FileMode.Create);

			img_strm.CopyTo(img_fs);
			img_fs.Close();

			pdfFile.Thumbnail = img_file;
		}

		Items.Add(pdfFile);
	}
}
