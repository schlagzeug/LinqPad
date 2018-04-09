<Query Kind="Statements" />

/// Script to pull a list of subsegments from an ANSI 837/835 file

var file = string.Empty;
if (MyUtil.OpenFileDialog(out file) == true)
{
	MyUtil.ShowStarted();
	
	var inFileList = MyUtil.ReadFileToList(file);
	var outFileList = new List<string>();
	foreach (var element in inFileList)
	{
		var elements = element.Split('*').ToList();
		if (elements[0] == "CLM" && elements.Count > 5)
		{
			outFileList.Add(elements[1].Dump());
		}
	}

	var outFile = MyUtil.GetOuputFileBasedOnInputFile(file, "Claims");
	MyUtil.WriteListToFile(outFile, outFileList);
	MyUtil.OpenFile(outFile);
	MyUtil.ShowFinished();
}