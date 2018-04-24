<Query Kind="Statements" />

var fileList = MyUtil.GetListFromFile(@"C:\temp\AutoImportFailedFiles_SortedUnique.txt");
var exists = new List<string>();
var missing = new List<string>();

foreach (var file in fileList)
{
	if (File.Exists(file))
	{
		exists.Add(file);
	}
	else
	{
		missing.Add(file);
	}
}

exists.Dump("Exists");
missing.Dump("Missing");