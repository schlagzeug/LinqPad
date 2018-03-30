<Query Kind="Statements" />

/// Provide two files containing lists. The script will generate a list in a
/// file that conatins all items that are in the 'fullListFile' but are not 
/// in the 'excludeListFile'.

var fullListFile = @"C:\Work\Cases\02074020\missing_SortedUnique.txt";
var excludeListFile = @"C:\Work\Cases\02074020\inXmitlog_SortedUnique.txt";

MyUtil.ShowStarted();
var resultFile = Path.Combine(Path.GetDirectoryName(fullListFile), "Results.txt");
var fullList = MyUtil.ReadFileToList(fullListFile);
var excludeList = MyUtil.ReadFileToList(excludeListFile);

using (var writer = new StreamWriter(resultFile))
{
	foreach (var element in fullList)
	{
		if (excludeList.Contains(element))
			continue;
		writer.WriteLine(element);
	}
}
MyUtil.OpenFileInNotepad(resultFile);
MyUtil.ShowFinished();