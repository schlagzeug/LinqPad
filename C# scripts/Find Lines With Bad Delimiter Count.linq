<Query Kind="Statements" />

/// Provide the path to a delimited file, the delimiter,
/// and the expected count of delimiters in the file, and 
/// the script will tell you which lines don't have that count

var filePath = @"C:\Work\Cases\02566721\Backlog\MMCNTHRIVECHARGEDETAIL_20180401_003600.txt";
var delimiter = '|';
var expectedCount = 16;


MyUtil.ShowStarted();
var lineNum = 0;
foreach (var line in File.ReadLines(filePath))
{
	lineNum++;
	var count = line.Count(x => x == delimiter);
	if (count != expectedCount)
	{
		$"Line {lineNum} - expected {expectedCount}, found {count}".Dump();
	}
}
MyUtil.ShowFinished();