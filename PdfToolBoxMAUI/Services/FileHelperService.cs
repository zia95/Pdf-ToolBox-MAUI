using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfToolBoxMAUI.Services;

public class FileHelperService
{
	public async Task<FileResult> PickAndShowPdfAsync(string title = "Select Pdf File") => await PickAndShowAsync(
			new PickOptions() { PickerTitle = title, FileTypes = FilePickerFileType.Pdf }
			);

	public async Task<IEnumerable<FileResult>> PickAndShowPdfMultiAsync(string title = "Select Pdf Files") => await PickAndShowMultiAsync(
		new PickOptions() { PickerTitle = title, FileTypes = FilePickerFileType.Pdf }
		);

	public async Task<FileResult> PickAndShowJpegAsync(string title = "Select Jpeg File") => await PickAndShowAsync(
		new PickOptions() { PickerTitle = title, FileTypes = FilePickerFileType.Images }
		);

	public async Task<IEnumerable<FileResult>> PickAndShowJpegMultiAsync(string title = "Select Jpeg Files") => await PickAndShowMultiAsync(
		new PickOptions() { PickerTitle = title, FileTypes = FilePickerFileType.Images }
		);



	public async Task<FileResult> PickAndShowAsync(PickOptions options) => await FilePicker.PickAsync(options);
	public async Task<IEnumerable<FileResult>> PickAndShowMultiAsync(PickOptions options) => await FilePicker.PickMultipleAsync(options);
}
