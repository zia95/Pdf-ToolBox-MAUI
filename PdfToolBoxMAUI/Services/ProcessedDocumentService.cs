namespace PdfToolBoxMAUI.Services;

public class ProcessedDocumentService
{
	


	public static string BasePath { get; } = Path.Combine(FileSystem.Current.AppDataDirectory, "out_docs");

	public static string MergePath { get; } = Path.Combine(BasePath, "merge");

	public static string SplitPath { get; } = Path.Combine(BasePath, "split");

	public static void SetupFileStructure()
	{
		if(!Directory.Exists(BasePath))
		{
			Directory.CreateDirectory(BasePath);
		}

		if (!Directory.Exists(MergePath))
		{
			Directory.CreateDirectory(MergePath);
		}

		if (!Directory.Exists(SplitPath))
		{
			Directory.CreateDirectory(SplitPath);
		}
	}



	PdfProcessingService _pps;
	public ProcessedDocumentService(PdfProcessingService pps) 
	{
		_pps = pps;

		SetupFileStructure();
	}


	public string[] GetProcessedSplitDoc() => GetProcessedDocs(SplitPath);
	public string[] GetProcessedMergeDoc() => GetProcessedDocs(MergePath);
	public string[] GetProcessedDocs(string path) => Directory.GetFiles(path);


	public bool CheckIfMergeDocumentExist(string fileName) => CheckIfDocumentExist(Path.Combine(MergePath, fileName));
	public bool CheckIfDocumentExist(string fileName) => File.Exists(fileName);


	public string GetNewMergeDocument(string fileName)
	{
		if(!CheckIfMergeDocumentExist(fileName))
		{
			return Path.Combine(MergePath, fileName);
		}
		return null;
	}
}
