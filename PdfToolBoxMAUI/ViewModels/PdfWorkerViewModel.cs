using Microsoft.Maui.Dispatching;
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

	readonly PdfProcessingService _pps;
	readonly ProcessedDocumentService _pds;


	Progress<PdfProcessingService.ProgressTracker> _progress;

	IDispatcher _disp;



	public PdfWorkerViewModel(PdfProcessingService pps, ProcessedDocumentService pds)
	{
		_pps = pps;
		_pds = pds;
		

		
	}

	public void SetDispatcher(IDispatcher dispatcher)
	{
        _disp = dispatcher;
		_progress = new Progress<PdfProcessingService.ProgressTracker>();
        _progress.ProgressChanged += (s, e) =>
        {
			_disp.Dispatch(() => 
			{ 
				if(e.State == PdfProcessingService.ProgressState.Merging)
				{
                    ProgressText = $"Source: {e.Files[e.CurrentFileIndex].FileName.Truncate(50)} ({e.CurrentFileIndex + 1}/{e.Files.Length}), Progress: {e.CurrentFileProgressPercentage.ToString("0.00")}%";
                }
				else if(e.State == PdfProcessingService.ProgressState.Closing)
				{
                    ProgressText = $"Source: {e.Files[e.CurrentFileIndex].FileName.Truncate(50)} ({e.CurrentFileIndex + 1}/{e.Files.Length}), Closing...";

                }
                else if (e.State == PdfProcessingService.ProgressState.Opening)
                {
                    ProgressText = $"Source: {e.Files[e.CurrentFileIndex].FileName.Truncate(50)} ({e.CurrentFileIndex + 1}/{e.Files.Length}), Opening...";
                }
            });
            
        };
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
			Shell.Current.GoToAsync("..");
			return;
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

        _successful = await Task.Run<bool>(() => do_work(outfile));

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

	bool do_work(string outfile)
	{

		if(WorkId == WorkIdMerge)
		{
			var tracker = _pps.MergePdfFiles(outfile, Files, _progress);

			if(File.Exists(outfile))
			{
				string message = "Merge process was successfully.";
				if (tracker.ErrorFiles?.Count > 0)
				{
					message += " But there were error in " + tracker.ErrorFiles.Count + " file(s).";
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
