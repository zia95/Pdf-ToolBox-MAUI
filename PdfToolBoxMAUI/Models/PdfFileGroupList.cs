using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfToolBoxMAUI.Models;

public class PdfFileGroupList : List<PdfFile>
{
	public string Name { get; set; }

	public PdfFileGroupList(string groupName, params PdfFile[] files) : base(files)
	{
		Name = groupName;
	}
}
