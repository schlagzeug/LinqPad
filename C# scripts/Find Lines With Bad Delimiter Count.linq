<Query Kind="Statements" />

/// Provide the path to a delimited file, the delimiter,
/// and the expected count of delimiters in the file, and 
/// the script will tell you which lines don't have that count

var filePath = @"\\medassets.com\crp\tibco\PR\DataIngress\IFM\Archive\2018\3\28\4vze4wba.dnl0\MMCNTHRIVECHARGEDETAIL_20180328_002300.txt";
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