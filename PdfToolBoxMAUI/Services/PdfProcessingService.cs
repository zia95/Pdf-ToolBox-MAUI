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


	public PdfFile ReadPdfFromResult(FileResult file, Func<string> passwordRequest)
	{
		var fs = file.OpenReadAsync();
		fs.Wait();

		var d = ReadPdfFromStream(fs.Result, file.FileName, file.FullPath, passwordRequest);
		if(d != null)
		{
            d.FileResult = file;
        }
		
		fs.Dispose();
		return d;
	}
	public PdfFile ReadPdfFromFile(string fullpath, Func<string> passwordRequest)
	{
		using var fs = File.OpenRead(fullpath);

		var d = ReadPdfFromStream(fs, Path.GetFileName(fullpath), fullpath, passwordRequest);

		return d;
	}
	
	private PdfLoadedDocument open_doc(PdfFile pdf)
	{
		if (pdf.FileResult is not null)
		{
			var strm = pdf.FileResult.OpenReadAsync();
			strm.Wait();

			var res = open_doc(strm.Result, pdf.Password);
			return res.doc;
		}
		else
		{
			if (File.Exists(pdf.FilePath))
			{
				var res = open_doc(File.OpenRead(pdf.FilePath), pdf.Password);
				return res.doc;
			}
		}
		return null;
	}
	private (PdfLoadedDocument doc, string password) open_doc(Stream fs, string password = null, Func<string> passwordRequest = null)
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
					return open_doc(fs, passwordRequest(), passwordRequest);
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

	public PdfFile ReadPdfFromStream(Stream fs, string filename, string filepath, Func<string> passwordRequest)
	{
		PdfFile pdfFile = new PdfFile();

		pdfFile.Id = Guid.NewGuid().ToString();
		pdfFile.FileResult = null;
		pdfFile.FilePath = filepath;
		pdfFile.FileName = filename;
		


		var pdf_doc = open_doc(fs, passwordRequest: passwordRequest);
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

	public enum ProgressState
	{
		Merging,
		Opening,
		OpeningFailed,
		Closing,

		Done,
	}

	public struct ProgressTracker
	{
		public string OutputFile;
		public PdfFile[] Files;
		public int CurrentFileIndex;


		public int CurrentFileTotalPages;
		public int CurrentFileTotalProcessedPages;
		public float CurrentFileProgressPercentage;

		public ProgressState State;

		public List<PdfFile> ErrorFiles;

	}



	public ProgressTracker MergePdfFiles(string outfile, PdfFile[] files, IProgress<ProgressTracker> tracker)
	{
		//Create a new document.
		using PdfDocument document = new PdfDocument();
		using Stream outstream = File.OpenWrite(outfile);

		var e = new ProgressTracker();
		e.OutputFile = outfile;
		e.Files = files;

		for(int i = 0; i <  files.Length; i++)
		{
			e.CurrentFileIndex = i;
			e.CurrentFileTotalPages = 0;
			e.CurrentFileTotalProcessedPages = 0;
			e.CurrentFileProgressPercentage = -1;
			//Load the PDF document.
			e.State = ProgressState.Opening;
			tracker?.Report(e);
			PdfLoadedDocument doc = open_doc(files[i]);

			

			if (doc is null)
			{
				e.ErrorFiles ??= new List<PdfFile>();
				e.ErrorFiles.Add(files[i]);
				e.State = ProgressState.OpeningFailed;
				tracker?.Report(e);
				continue;
			}

			e.CurrentFileTotalPages = doc.PageCount;




			for (int j = 0; j < doc.PageCount; j++) 
			{
				//Imports the page at j from the doc.
				document.ImportPage(doc, j);

				e.CurrentFileTotalProcessedPages++;

				float progress = ((float)e.CurrentFileTotalProcessedPages / (float)e.CurrentFileTotalPages) * 100.0f;
				
				if(((int)progress) > e.CurrentFileProgressPercentage)
				{
					e.CurrentFileProgressPercentage = progress;
					e.State = ProgressState.Merging;
					tracker?.Report(e);
				}


				
			}

			e.State = ProgressState.Closing;
			tracker?.Report(e);

			//Save the document and imported pages
			document.Save(outstream);


			doc.Close();
			doc.Dispose();
		}

		
		//Close the document.
		document.Close();

		e.State = ProgressState.Done;
		tracker?.Report(e);
		return e;
	}
}
