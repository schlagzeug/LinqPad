<Query Kind="Statements" />

/// Given a delimited file (inFile), the file separator character (separator),
/// and a list of elements to compare (indexes), this script compares the listed
/// columns to each other and writes a file containing all records where the 
/// indexes are not the same.

var inFile = @"C:\Work\Cases\02419095\cd171215.sfm";
var separator = '|';
var indexes = new int[]{4,5,6,7,8,10,11}; // 1 based index


MyUtil.ShowStarted();

var extension = "_MismatchResults(";
foreach (var num in indexes)
{
	extension += num.ToString();
	extension += ",";
}
extension = extension.TrimEnd(',');
extension += ").txt";

var resultFile = inFile + extension;

using (var writer = new StreamWriter(resultFile))
{
	foreach (var line in File.ReadLines(inFile))
	{
		var elements = line.Split(separator);
		var firstItem = elements[indexes[0]-1];
		var otherItems = new List<string>();
		for (int i = 1; i < indexes.Count(); i++)
		{
			otherItems.Add(elements[indexes[i]-1]);
		}
		if (!MyUtil.CompareMultiple(firstItem, otherItems.ToArray()))
		{
			writer.WriteLine(line);
		}
	}
}

MyUtil.OpenFile(resultFile);
MyUtil.ShowFinished();