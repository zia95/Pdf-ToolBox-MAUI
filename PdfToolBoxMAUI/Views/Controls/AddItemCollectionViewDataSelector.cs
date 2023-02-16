using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfToolBoxMAUI.Views.Controls
{
	public class PdfCollectionViewDataSelector : DataTemplateSelector
	{
		public DataTemplate DefaultElement { get; set; }
		public DataTemplate AddElement { get; set; }
		protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
		{
			return DefaultElement;
		}
	}
}
