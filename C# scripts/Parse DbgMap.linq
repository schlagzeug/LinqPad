<Query Kind="Statements" />

/// Script that parses out he DbgMap.txt files that is produced when testing a print image, and 
/// gives you the line number(s) to check to find missaligned data.


MyUtil.ShowStarted();
using (var reader = new StreamReader(@"C:\DbgMap.txt"))
{
	var line = string.Empty;
	var lineNum = 0;

	while ((line = reader.ReadLine()) != null)
	{
		lineNum++;
		if (line == @"            1         2         3         4         5         6         7         8         9" ||
			line == @" + 123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890" ||
			line == @"---------------------------------------------------------------------------------------------" ||
			line.StartsWith(@"Image #"))
			continue;
		else
		{
			line = line.Substring(3);
			line = line.Replace("|", string.Empty);
			line = line.Replace(" ", string.Empty);
			if (line != string.Empty)
			{
				$"Line {lineNum}:{line}".Dump();
			}
		}
	}
}
MyUtil.ShowFinished();