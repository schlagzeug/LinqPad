<Query Kind="Statements" />

/// Script that take a file that is too large to be opened by NotePad++ and trims it down by removing duplicated lines

var file = @"\\CRP40PPFS07\RCT_Operations_PHI\DataManagment\sreynolds\largeLog\AutoImportSvc.2018_02_15.old.log";


MyUtil.ShowStarted();

using (var reader = new StreamReader(file))
{
	using (var writer = new StreamWriter(MyUtil.GetOuputFileBasedOnInputFile(file, "EDIT")))
	{
		var line = string.Empty;
		var previousLine = string.Empty;

		while ((line = reader.ReadLine()) != null)
		{
			if (line != previousLine)
			{
				writer.WriteLine(line);
			}
			previousLine = line;
		}
	}
}

MyUtil.ShowFinished();