using System;
using System.Collections.Generic;
using System.Text;

namespace PdfToolBoxMAUI.Models;

public class PdfFile
{
    public string Id { get; set; }

    public string FileName { get; set; }
    public string FilePath { get; set; }

    public FileResult File { get; set; }

    public string Description { get; set; }

    public ImageSource Thumbnail { get; set; }

}
