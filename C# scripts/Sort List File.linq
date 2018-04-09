<Query Kind="Statements" />

/// Takes a file and sorts the lines, creating two output files, one with all original entries in it, and 
/// one with only unique entries

var file = string.Empty;
if (MyUtil.OpenFileDialog(out file) == true)
{
	MyUtil.ShowStarted();

	var list = MyUtil.GetListFromFile(file.Dump("Source File:"));

	list.Sort();
	MyUtil.WriteListToFile(MyUtil.GetOuputFileBasedOnInputFile(file, "Sorted"), list.Dump("Sorted List"));

	list = list.Distinct().ToList();
	MyUtil.WriteListToFile(MyUtil.GetOuputFileBasedOnInputFile(file, "SortedUnique"), list.Dump("Sorted List (Unique)"));

	MyUtil.ShowFinished();
}