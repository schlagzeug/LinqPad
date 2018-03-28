<Query Kind="Statements" />

/// Provide a file with a list of files to be deleted.
/// The list can be full file paths, or just file names.
/// If the list is just file names, also provide the directory.
/// You can also just provide a directory to remove everything in it.

var directory = @"C:\temp\zip\";
var fileWithList = string.Empty;//@"C:\temp\zip\backup\list.txt";


MyUtil.ShowStarted();

try
{
	if (directory != string.Empty && fileWithList != string.Empty)
	{
		MyUtil.DeleteFilesInDirectory(directory, MyUtil.GetListFromFile(fileWithList));
	}
	else if (directory == string.Empty && fileWithList != string.Empty)
	{
		MyUtil.DeleteListOfFiles(MyUtil.GetListFromFile(fileWithList));
	}
	else if (directory != string.Empty && fileWithList == string.Empty)
	{
		MyUtil.DeleteFilesInDirectory(directory);
	}
}
catch (Exception ex)
{
	ex.Message.Dump();
}

MyUtil.ShowFinished();