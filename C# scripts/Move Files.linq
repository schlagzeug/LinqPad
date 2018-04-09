<Query Kind="Program" />

/// Moves files from the FromDirectory to the ToDirectory if a certain condition
/// is met. 

private static readonly string FromDirectory = @"C:\Work\Cases\02545198\Bad";
private static readonly string ToDirectory = @"C:\Work\Cases\02545198\Bad\same";


void Main()
{
	MyUtil.ShowStarted();
	var di = new DirectoryInfo(FromDirectory);
	var files = di.GetFiles();
	var movedFiles = new List<string>();

	foreach (var file in files)
	{
		if (IsContentMatch(file)) // switch out for different options
		{
			File.Move(Path.Combine(FromDirectory, file.Name), Path.Combine(ToDirectory, file.Name));
			movedFiles.Add(file.Name);
		}
	}

	movedFiles.Dump("Files Moved");
	MyUtil.ShowFinished();
}
// ============================================================
private static bool HasMultiISA(FileInfo file)
{
	var contents = File.ReadAllText(file.FullName);
	if (contents.Contains(@"ISA$") && contents.Contains(@"ISA*"))
	{
		return true;
	}

	return false;
}
// ============================================================
private static string MatchString = "837i";
private static bool MatchFileName(FileInfo file)
{
	if (file.Name.Contains(MatchString)) return true;
	return false;
}
// ============================================================
private static readonly List<string> FileList = MyUtil.GetListFromFile(@"C:\Work\Cases\02437120\Errors\TrueErrors.txt");
private static bool IsInList(FileInfo file)
{
	if (FileList.Contains(file.Name)) return true;
	return false;
}
// ============================================================
private static bool IsContentMatch(FileInfo file)
{
	var fi = @"C:\Work\Cases\02545198\Bad\same\rb180321075969.unkn.ahcc";
	if (MyUtil.CalculateMD5Hash(fi) == MyUtil.CalculateMD5Hash(file.FullName)) return true;
	return false;
}
