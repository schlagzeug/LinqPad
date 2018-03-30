<Query Kind="Statements" />

/// Script that takes a path and outputs a file that contains all the MD5
/// hash values for each file in the provided directory

var path = @"C:\Work\Cases\02452430";


MyUtil.ShowStarted();

var rand = new Random();
var resultFileName = string.Format("Results_{0}_{1}.txt", DateTime.Now.ToString("yyyyMMdd"), rand.Next());
var resultFile = Path.Combine(path, resultFileName);

var di = new DirectoryInfo(path);
var files = di.GetFiles();

using (var writer = new StreamWriter(resultFile))
{
	writer.WriteLine("FILE NAME | MD5 HASH".Dump());
	foreach (var file in files)
	{
		writer.WriteLine(string.Format("{0} | {1}", file.Name, MyUtil.CalculateMD5Hash(file.FullName)).Dump());
	}
}
MyUtil.OpenFileInNotepad(resultFile);
MyUtil.ShowFinished();