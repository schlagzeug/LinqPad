<Query Kind="Program" />

void Main()
{
	MyExtensions.ShowStarted();
	FileMover.MoveFiles();
	MyExtensions.ShowFinished();
}

// Define other methods and classes here
public static class FileMover
{
	private static readonly string FromDirectory = @"C:\Work\Cases\02437120\Errors\TRUE_ERRORS";
	private static readonly string ToDirectory = @"C:\Work\Cases\02437120\Errors\TRUE_ERRORS\new";

	private static readonly List<string> FileList = MyExtensions.GetFileList(@"C:\Work\Cases\02437120\Errors\TrueErrors.txt");

	public static void MoveFiles()
	{
		var di = new DirectoryInfo(FromDirectory);
		var files = di.GetFiles();
		var movedFiles = new List<string>();

		foreach (var file in files)
		{
			if (IsInList(file)) // switch out for different options
			{
				File.Move(Path.Combine(FromDirectory, file.Name), Path.Combine(ToDirectory, file.Name));
				movedFiles.Add(file.Name);
			}
		}

		movedFiles.Dump("Files Moved");
	}
	
	// ============================================================
	private static bool HasMultiISA(FileInfo file)
	{
		var reader = new StreamReader(file.FullName);
		var contents = reader.ReadToEnd();
		reader.Close();
		if (contents.Contains(@"ISA$") && contents.Contains(@"ISA*"))
		{
			return true;
		}

		return false;
	}
	// ============================================================
	private static bool MatchFileName(FileInfo file, string matchString)
	{
		if (file.Name.Contains(matchString)) return true;
		return false;
	}
	// ============================================================
	private static bool IsInList(FileInfo file)
	{
		if (FileList.Contains(file.Name)) return true;
		return false;
	}
}