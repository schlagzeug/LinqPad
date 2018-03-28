<Query Kind="Program" />

void Main()
{
	CreateMD5List(@"C:\Work\Cases\02452430");
}

static void CreateMD5List(string path)
{
	MyExtensions.ShowStarted();
	
	var rand = new Random();
    var resultFileName = string.Format("Results_{0}_{1}.txt", DateTime.Now.ToString("yyyyMMdd"), rand.Next());
    var resultFile = Path.Combine(path, resultFileName);

    var di = new DirectoryInfo(path);
	var files = di.GetFiles();

	var writer = new StreamWriter(resultFile);

	writer.WriteLine("FILE NAME | MD5 HASH".Dump());
	foreach (var file in files)
    {
		writer.WriteLine(string.Format("{0} | {1}", file.Name, MyExtensions.CalculateMD5Hash(file.FullName)).Dump());
    }

    writer.Flush();
    writer.Close();
	MyExtensions.OpenFileInNotepad(resultFile);
	MyExtensions.ShowFinished();
}