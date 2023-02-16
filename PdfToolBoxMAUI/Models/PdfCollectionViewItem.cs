using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfToolBoxMAUI.Models;

public class PdfCollectionViewItem
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public Image DisplayImage { get; set; }
}
