using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.Maui.PdfToImageConverter;
using CommunityToolkit.Mvvm.DependencyInjection;

namespace PdfToolBoxMAUI.Services;

public class PdfProcessingService
{

	private readonly string PdfThumbnailBaseDir = Path.Combine(FileSystem.Current.CacheDirectory, "pdf_thumbnails");


	public async Task<PdfFile> ReadPdfFromResultAsync(FileResult file, Func<Task<string>> passwordRequest)
	{
		using var fs = await file.OpenReadAsync();

		var d = await ReadPdfFromStreamAsync(fs, file.FileName, file.FullPath, passwordRequest);
		d.FileResult = file;
		return d;
	}
	public async Task<PdfFile> ReadPdfFromFileAsync(string fullpath, Func<Task<string>> passwordRequest)
	{
		using var fs = File.OpenRead(fullpath);

		var d = await ReadPdfFromStreamAsync(fs, Path.GetFileName(fullpath), fullpath, passwordRequest);

		return d;
	}
	
	private async Task<PdfLoadedDocument> open_doc(PdfFile pdf)
	{
		
		if (pdf.FileResult is not null)
		{
			var res = await open_doc(await pdf.FileResult.OpenReadAsync(), pdf.Password);
			return res.doc;
		}
		else
		{
			if (File.Exists(pdf.FilePath))
			{
				var res = await open_doc(File.OpenRead(pdf.FilePath), pdf.Password);
				return res.doc;
			}
		}
		return null;
	}
	private async Task<(PdfLoadedDocument doc, string password)> open_doc(Stream fs, string password = null, Func<Task<string>> passwordRequest = null)
	{
		try
		{
			if(password is not null)
			{
				var pdf_doc = new PdfLoadedDocument(fs, password);

				return (pdf_doc, password);
			}
			else
			{
				var pdf_doc = new PdfLoadedDocument(fs);

				return (pdf_doc, null);
			}
			
		}
		catch (PdfException ex)
		{
			bool is_protected = ex.Message.ToLower().Contains("password");
			if (is_protected)
			{
				if (passwordRequest != null)
				{
					return await open_doc(fs, await passwordRequest(), passwordRequest);
				}
			}
			return (null, null);
		}
	}

	private string get_doc_thumbnail(Stream fs, string filepath, string password)
	{
		if(!Directory.Exists(PdfThumbnailBaseDir))
		{
			Directory.CreateDirectory(PdfThumbnailBaseDir);
		}


		string thumbnail = Path.Combine(PdfThumbnailBaseDir, filepath.GetHashCode().ToString());
		if(File.Exists(thumbnail))
		{
			return thumbnail;
		}




		//generate
		PdfToImageConverter pdftoimg = null;
		if (password is not null)
		{
			if(DeviceInfo.Current.Platform != DevicePlatform.Android)
			{
				pdftoimg = new PdfToImageConverter(fs, (string?) password);
			}
		}
		else
		{
			pdftoimg = new PdfToImageConverter(fs);
		}

		if (pdftoimg is null) return null;

		Stream img_strm = pdftoimg.Convert(0, new Size(96, 128));


		if (img_strm is not null)
		{
			using var img_fs = new FileStream(thumbnail, FileMode.Create);


			
			img_strm.CopyTo(img_fs);
			
			img_fs.Close();





			img_strm.Dispose();
			pdftoimg.Dispose();
			


			return thumbnail;
		}
		else
		{
			pdftoimg.Dispose();
		}

		return null;
	}

	public async Task<PdfFile> ReadPdfFromStreamAsync(Stream fs, string filename, string filepath, Func<Task<string>> passwordRequest)
	{
		PdfFile pdfFile = new PdfFile();

		pdfFile.Id = Guid.NewGuid().ToString();
		pdfFile.FileResult = null;
		pdfFile.FilePath = filepath;
		pdfFile.FileName = filename;
		


		var pdf_doc = await open_doc(fs, passwordRequest: passwordRequest);
		if (pdf_doc.doc is null)
		{
			return null;
		}

		pdfFile.Password = pdf_doc.password;
		
		pdf_doc.doc.Dispose();



		pdfFile.Thumbnail = get_doc_thumbnail(fs, filepath, pdfFile.Password);

		pdfFile.Thumbnail ??= "iconblank.png";

		

		return pdfFile;
	}



	public struct ProgressTracker
	{
		public string OutputFile;
		public PdfFile[] Files;
		public int CurrentFileIndex;


		public int CurrentFileTotalPages;
		public int CurrentFileTotalProcessedPages;
		public int CurrentFileProgressPercentage;

		

		public List<PdfFile> ErrorFiles;

		public bool Done;
	}



	public async Task MergePdfFilesAsync(string outfile, PdfFile[] files, Action<ProgressTracker> tracker)
	{
		//Create a new document.
		using PdfDocument document = new PdfDocument();
		using Stream outstream = File.OpenWrite(outfile);

		var e = new ProgressTracker();
		e.OutputFile = outfile;
		e.Files = files;
		e.Done = false;

		for(int i = 0; i <  files.Length; i++)
		{
			e.CurrentFileIndex = i;
			//Load the PDF document.
			PdfLoadedDocument doc = await open_doc(files[i]);

			e.CurrentFileTotalPages = 0;
			e.CurrentFileTotalProcessedPages = 0;
			e.CurrentFileProgressPercentage = 0;

			if (doc is null)
			{
				e.ErrorFiles ??= new List<PdfFile>();
				e.ErrorFiles.Add(files[i]);
				tracker?.Invoke(e);
				continue;
			}

			e.CurrentFileTotalPages = doc.PageCount;




			for (int j = 0; j < doc.PageCount; j++) 
			{
				//Imports the page at j from the doc.
				document.ImportPage(doc, j);

				e.CurrentFileTotalProcessedPages++;

				e.CurrentFileProgressPercentage = (e.CurrentFileTotalProcessedPages / e.CurrentFileTotalPages) * 100;

				tracker?.Invoke(e);
			}


			//Save the document and imported pages
			document.Save(outstream);


			doc.Close();
			doc.Dispose();
		}

		
		//Close the document.
		document.Close();


		e.Done = true;
		tracker?.Invoke(e);
	}
}
