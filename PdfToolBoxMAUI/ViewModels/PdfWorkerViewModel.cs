using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfToolBoxMAUI.ViewModels;


[QueryProperty(nameof(WorkId), "workid")]
[QueryProperty(nameof(Files), "files")]
public partial class PdfWorkerViewModel : BaseViewModel
{
	[ObservableProperty]
	PdfFile[] files;

	[ObservableProperty]
	int workId;


	[ObservableProperty]
	bool displayOutputFileName = true;

	[ObservableProperty]
	string outputFileName;


	[ObservableProperty]
	bool isWorking;

	[ObservableProperty]
	bool displayMainButton = true;

	[ObservableProperty]
	string progressText = "Enter new file name";

	[ObservableProperty]
	string mainButtonText = "Begin";



	public const int WorkIdMerge = 1;
	public const int WorkIdSplit = 2;


	bool _workdone = false;
	bool _successful;

	PdfProcessingService _pps;
	ProcessedDocumentService _pds;
	public PdfWorkerViewModel(PdfProcessingService pps, ProcessedDocumentService pds)
	{
		_pps = pps;
		_pds = pds;
	}


	[RelayCommand]
	public async Task MainButtonClicked()
	{
		string outfile = get_out_file();


		if(_workdone)
		{
			if(_successful)
			{
				//go to processed doc list
			}
			else
			{
				//go back to previous page
			}
			await Shell.Current.GoToAsync("..");
		}

		if(outfile is null)
		{
			ProgressText = "File with the same name already exists. Name must be unique.";
			return;
		}

		//do work...
		DisplayOutputFileName = false;
		DisplayMainButton = false;
		IsWorking = true;

		_successful = await do_work(outfile);

		MainButtonText = _successful ? "Go to Merged File" : "Go Back";

		
		_workdone = true;
		IsWorking = false;
		DisplayMainButton = true;
	}



	string get_out_file()
	{
		if (WorkId == WorkIdMerge)
		{
			string f = get_out_file_name();
			return f is null ? null : _pds.GetNewMergeDocument(f);
		}
		else
		{
			throw new NotImplementedException($"Workid:{WorkId} is not implemented.");
		}
	}
	string get_out_file_name()
	{
		if (string.IsNullOrWhiteSpace(OutputFileName)) return null;

		if (OutputFileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase)) return OutputFileName;

		return OutputFileName + ".pdf";
	}

	async Task<bool> do_work(string outfile)
	{

		if(WorkId == WorkIdMerge)
		{
			PdfProcessingService.ProgressTracker? tracker = null;
			await _pps.MergePdfFilesAsync(outfile, Files, t => 
			{ 
				tracker = t;
				ProgressText = $"Source: {t.Files[t.CurrentFileIndex].FileName.Truncate(10)} ({t.CurrentFileIndex + 1}/{t.Files.Length}), Progress: {t.CurrentFileProgressPercentage}";
			});

			if(File.Exists(outfile))
			{
				string message = "Merge process was successfully.";
				if (tracker.Value.ErrorFiles?.Count > 0)
				{
					message += " But there were error in " + tracker.Value.ErrorFiles.Count + " file(s).";
				}
				ProgressText = message;

				return true;
			}
			else
			{
				ProgressText = "Failed to merge files. Errors encountered.";
				return false;
			}
		}
		else
		{
			throw new NotImplementedException($"Workid:{WorkId} is not implemented.");
		}

		return false;
	}
}
