<Query Kind="Program" />

void Main()
{
	var file = string.Empty;
	if (MyExtensions.OpenFileDialog(out file) == true)
	{
		GenerateResult(file);
	}
}

// Define other methods and classes here

public void GenerateResult(string inFile)
{
	MyExtensions.ShowStarted();
	
	var reader = new StreamReader(inFile);
	var outFile = MyExtensions.GetOuputFileBasedOnInputFile(inFile, "Claims");
	var writer = new StreamWriter(outFile);

	var line = string.Empty;
	while ((line = reader.ReadLine()) != null)
	{
		var elements = line.Split('*').ToList();
		if (elements[0] == "CLM" && elements.Count > 5)
		{
			writer.WriteLine(elements[1].Dump());
		}
	}

	reader.Close();
	writer.Flush();
	writer.Close();
	
	MyExtensions.OpenFile(outFile);
	
	MyExtensions.ShowFinished();
}