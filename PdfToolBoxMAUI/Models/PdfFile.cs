using System;
using System.Collections.Generic;
using System.Text;

namespace PdfToolBoxMAUI.Models;

public class PdfFile
{
	public string Id { get; set; }

	public string FileName { get; set; }
	public string FilePath { get; set; }
	public string Description { get; set; }

	public string Password { get; set; }


	public FileResult FileResult { get; set; }

	

	public ImageSource Thumbnail { get; set; }

}
