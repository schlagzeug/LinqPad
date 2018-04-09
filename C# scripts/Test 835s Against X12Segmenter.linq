<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

/// Provide a folder containing 835s and the FacID that they are for,
/// and the script will run them through the x12segmenter.pl script,
/// moving the good files to a "Good" subfolder, and the bad files
/// to a "Bad" subfolder. It will leave a results file in the provided
/// folder.

private string RunFolder = @"C:\Work\Cases\02559103\835s";
private int FacID = 1728;


public List<string> Files { get; set; }
public List<RunResult> Results = new List<RunResult>();
public string GoodFolder
{
	get { return "Good"; }
}
public string BadFolder
{
	get { return "Bad"; }
}

void Main()
{
	MyUtil.ShowStarted();
	
	var resultFile = Path.Combine(RunFolder, "Results.txt");
	var resultText = new List<string>();
	
	Files = Directory.GetFiles(RunFolder).ToList();
	var tasks = new List<Task>();
	
	for (int i = 0; i < Files.Count; i++)
	{
		var task = new Task(RunNext);
		tasks.Add(task);
		task.Start();
	}

	Task.WaitAll(tasks.ToArray());

	foreach (var result in Results)
	{
		resultText.Add(result.ToString());
		resultText.Add("\r\n" + MyUtil.DIVIDER + "\r\n");
	}
	
	MyUtil.WriteListToFile(resultFile, resultText);
	
	MyUtil.ShowFinished();
}


public RunResult RunX12Segmenter(string filePath)
{
	var runResult = new RunResult();
	runResult.File = filePath;
	var tempFile = MyUtil.CreateTempCopy(filePath);
	
	// move temp file to its own subfolder to prevent collisions
	tempFile = MyUtil.MoveToSubFolder(tempFile, Guid.NewGuid().ToString());
	runResult.Output = $"{filePath} copied to {tempFile} for testing.\r\n";
	
	var perlStartInfo = new ProcessStartInfo(@"C:\Perl\bin\perl.exe");
	perlStartInfo.Arguments = @"-s P:\perl\Imacs\x12Segmenter.pl  -f=" + FacID.ToString() + " -i=" + tempFile;
	perlStartInfo.UseShellExecute = false;
	perlStartInfo.RedirectStandardOutput = true;
	perlStartInfo.RedirectStandardError = true;
	perlStartInfo.CreateNoWindow = true;

	var perl = new Process();
	perl.StartInfo = perlStartInfo;
	perl.Start();
	perl.WaitForExit();

	runResult.Output += perl.StandardError.ReadToEnd();
	runResult.ReturnValue = perl.ExitCode;
	MyUtil.DeleteDirectory(Path.GetDirectoryName(tempFile));
	
	return runResult;
}

public void RunNext()
{
	var fileToRun = string.Empty;
	
	lock(Files)
	{
		fileToRun = Files[0];
		Files.RemoveAt(0);
	}
	
	var result = RunX12Segmenter(fileToRun);
	if (result.ReturnValue == 0)
	{
		MyUtil.MoveToSubFolder(result.File, GoodFolder);
	}
	else
	{
		MyUtil.MoveToSubFolder(result.File, BadFolder);
	}
	
	lock(Results)
	{
		Results.Add(result.Dump());
	}
}

public class RunResult
{
	public string File {get;set;}
	public string Output {get;set;}
	public int ReturnValue {get;set;}
	
	public override string ToString()
	{
		if (ReturnValue == 0)
		{
			return $"File {Path.GetFileName(File)} is good\r\n";
		}
		else
		{
			return $"File {Path.GetFileName(File)} is bad\r\n" + Output;
		}
	}
}