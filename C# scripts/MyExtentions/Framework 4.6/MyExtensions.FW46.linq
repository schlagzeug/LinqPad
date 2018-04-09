<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\WPF\PresentationFramework.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Xaml.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\WPF\WindowsBase.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\WPF\PresentationCore.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Configuration.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\WPF\UIAutomationProvider.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\WPF\UIAutomationTypes.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\WPF\ReachFramework.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\WPF\PresentationUI.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\WPF\System.Printing.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\Accessibility.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Deployment.dll</Reference>
  <Namespace>Microsoft.Win32</Namespace>
</Query>

void Main()
{
	// Write code to test your extensions here. Press F5 to compile and run.
	var x = new string[]{"A", "A", "A"};
	MyUtil.CompareMultiple("A", x).Dump();
}

public static class MyExtensions
{
	// Extend DateTime class
	/// <summary>Returns earliest DateTime for provided DateTime</summary>
	public static DateTime StartOfDay(this DateTime date)
	{
		return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
	}
	/// <summary>Returns latest DateTime for provided DateTime</summary>
	public static DateTime EndOfDay(this DateTime date)
	{
		return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999);
	}

	// Extend StreamWriter class
	/// <summary>Calls Flush() and Close()</summary>
	public static void FlushAndClose(this StreamWriter str)
	{
		str.Flush();
		str.Close();
	}
	/// <summary>Writes a List&lt;string&gt; to a file, one string per line</summary>
	public static void WriteList(this StreamWriter writer, List<string> list)
	{
		foreach (var item in list)
		{
			writer.WriteLine(item);
		}
	}

	// Extend StreamReader class
	/// <summary>Read file contents into a List&lt;string&gt;, one record per line</summary>
	public static List<string> ReadFileToList(this StreamReader reader)
	{
		var list = new List<string>();

		var line = string.Empty;
		while ((line = reader.ReadLine()) != null)
		{
			list.Add(line);
		}

		return list;
	}
}

public class MyUtil
{
	public static string DIVIDER = "----------------------------------------";

	/// <summary>Displays 'Started' message for Linqpad</summary>
	public static void ShowStarted()
	{
		DIVIDER.Dump();
		string.Format("Started - {0}", DateTime.Now).Dump();
		DIVIDER.Dump();
		string.Empty.Dump();
	}
	/// <summary>Displays 'Finished' message for Linqpad</summary>
	public static void ShowFinished()
	{
		string.Empty.Dump();
		DIVIDER.Dump();
		string.Format("Finished - {0}", DateTime.Now).Dump();
		DIVIDER.Dump();
	}
	/// <summary>Returns a path to a new "results" file based on the provided file</summary>
	public static string GetResultsFile(string sourceFile)
	{
		return GetOuputFileBasedOnInputFile(sourceFile, "Results");
	}
	/// <summary>Returns a path to a new file based on the provided file. (name.txt --> name_nameExt.txt)</summary>
	public static string GetOuputFileBasedOnInputFile(string sourceFile, string nameExt)
	{
		var resultFileName = Path.GetFileNameWithoutExtension(sourceFile) + "_" + nameExt + Path.GetExtension(sourceFile);
		var resultFile = Path.Combine(Path.GetDirectoryName(sourceFile), resultFileName);
		return resultFile;
	}
	/// <summary>Opens a generic OpenFileDialog window</summary>
	public static bool? OpenFileDialog(out string returnedFileName)
	{
		return OpenFileDialog(@"C:\Work\Cases", "All files (*.*)|*.*", "*", "file.txt", out returnedFileName);
	}
	/// <summary>Opens an OpenFileDialog window</summary>
	public static bool? OpenFileDialog(string directory, string filter, string defaultExt, string fileName, out string returnedFileName)
	{
		var openDiag = new OpenFileDialog();
		openDiag.InitialDirectory = directory;
		openDiag.Multiselect = false;
		openDiag.Filter = filter;
		openDiag.DefaultExt = defaultExt;
		openDiag.FileName = fileName;

		var result = openDiag.ShowDialog();
		returnedFileName = openDiag.FileName;
		return result;
	}
	/// <summary>Opens a directory in Windows Explorer</summary>
	public static void OpenDirectory(string path)
	{
		if (path == string.Empty)
		{
			ShowError("Directory doesn't exist.");
			return;
		}
		DirectoryInfo diDir = new DirectoryInfo(path);
		if (diDir.Exists)
		{
			Process.Start(path);
		}
		else
		{
			ShowError(string.Format("Directory ({0}) doesn't exist.", path));
		}
	}
	/// <summary>Opens a file</summary>
	public static void OpenFile(string path)
	{
		OpenFile(path, false);
	}
	/// <summary>Opens a file in Notepad++, with option to open as read only</summary>
	public static void OpenFile(string path, bool openAsReadOnly)
	{
		if (File.Exists(path))
		{
			var p = new Process();
			p.StartInfo = new ProcessStartInfo()
			{
				UseShellExecute = false,
				FileName = @"C:\Program Files (x86)\Notepad++\notepad++.exe",
				Arguments = string.Format("{0}{1}", openAsReadOnly ? "-ro " : string.Empty, path)
			};
			p.Start();
		}
		else
		{
			ShowError("File doesn't exist.");
		}
	}
	/// <summary>Open a file in Notepad++</summary>
	public static void OpenFileInNotepad(string fileLocation)
	{
		if (File.Exists(fileLocation))
		{
			Process.Start(@"C:\Program Files (x86)\Notepad++\notepad++.exe", fileLocation);
		}
		else
		{
			ShowError("File doesn't exist.");
		}
	}
	/// <summary>Open a webpage, in Internet Explorer or default browser</summary>
	public static void OpenWebPage(string path, bool openInIE)
	{
		if (openInIE)
		{
			Process.Start("IExplore.exe", path);
		}
		else
		{
			Process.Start(path);
		}
	}
	/// <summary>Show Error in System.Windows.MessageBox</summary>
	public static void ShowError(string error)
	{
		ShowMessage(error, "Error.");
	}
	/// <summary>Show Message in System.Windows.MessageBox</summary>
	public static void ShowMessage(string message)
	{
		System.Windows.MessageBox.Show(message);
	}
	/// <summary>Show Message in System.Windows.MessageBox with provided title</summary>
	public static void ShowMessage(string title, string message)
	{
		System.Windows.MessageBox.Show(message, title);
	}
	/// <summary>Copies files from one directory to another replacing files if file is newer and different</summary>
	public static int CopyFiles(string sourceDir, string destDir)
	{
		var retVal = 0;
		// Get our files (recursive and any of them, based on the 2nd param of the Directory.GetFiles() method
		string[] originalFiles = Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories);

		// Dealing with a string array, so let's use the actionable Array.ForEach() with a anonymous method
		Array.ForEach(originalFiles, (originalFileLocation) =>
		{
			// Get the FileInfo for both of our files
			FileInfo originalFile = new FileInfo(originalFileLocation);
			FileInfo destFile = new FileInfo(originalFileLocation.Replace(sourceDir, destDir));
			// ^^ We can fill the FileInfo() constructor with files that don't exist...

			if (!originalFile.FullName.Contains(@"\bin\") && !originalFile.FullName.Contains(@"\obj\"))
			{
				// ... because we check it here
				if (destFile.Exists)
				{
					// Logic for files that exist applied here; if the original is different, replace the updated files...
					if (originalFile.LastWriteTime >= destFile.LastWriteTime && CalculateMD5Hash(originalFile.FullName) != CalculateMD5Hash(destFile.FullName))
					{
						originalFile.CopyTo(destFile.FullName, true);
						originalFile.FullName.Dump(); //***ADD THIS
						retVal++;
					}
				}
				else // ... otherwise create any missing directories and copy the folder over
				{
					Directory.CreateDirectory(destFile.DirectoryName); // Does nothing on directories that already exist
					originalFile.CopyTo(destFile.FullName, false); // Copy but don't over-write  
					originalFile.FullName.Dump(); //***ADD THIS
					retVal++;
				}
			}

		});
		return retVal;
	}
	/// <summary>Returns MD5 hash of provided file (input)</summary>
	public static string CalculateMD5Hash(string input)
	{
		var reader = new StreamReader(input);
		var fileContents = reader.ReadToEnd();

		reader.Close();
		reader.Dispose();

		// step 1, calculate MD5 hash from input
		System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
		byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(fileContents);
		byte[] hash = md5.ComputeHash(inputBytes);

		// step 2, convert byte array to hex string
		StringBuilder sb = new StringBuilder();
		for (int i = 0; i < hash.Length; i++)
		{
			sb.Append(hash[i].ToString("X2"));
		}

		return sb.ToString();
	}
	/// <summary>Returns true if all values match</summary>
	public static bool CompareMultiple(string data, params string[] compareValues)
	{
		foreach (string s in compareValues)
		{
			if (data != s)
			{
				return false;
			}
		}

		return true; // returns true only if all the same
	}
	/// <summary>Returns a List&lt;string&gt; of lines in a file</summary>
	public static List<string> GetListFromFile(string filePath)
	{
		using (var readstream = new StreamReader(filePath))
		{
			return readstream.ReadFileToList();
		}
	}
	/// <summary>Returns earliest DateTime for provided DateTime</summary>
	public static DateTime StartOfDay(DateTime date)
	{
		return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
	}
	/// <summary>Returns latest DateTime for provided DateTime</summary>
	public static DateTime EndOfDay(DateTime date)
	{
		return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999);
	}
	/// <summary>Writes a List&lt;string&gt; to a file</summary>
	public static void WriteListToFile(string filePath, List<string> list)
	{
		using (var writer = new StreamWriter(filePath))
		{
			writer.WriteList(list);
		}
	}
	/// <summary>Reads the 4th character of the file, which should be the separator (ISA*)</summary>
	public static char GetCharacterSeparatorFromANSI(string filePath)
	{
		using (var reader = new StreamReader(filePath))
		{
			return reader.ReadLine()[3];
		}
	}
	/// <summary>Returns a list of strings from the provided file</summary>
	public static List<string> ReadFileToList(string filePath)
	{
		var list = new List<string>();
		foreach (var line in File.ReadLines(filePath))
		{
			list.Add(line);
		}
		return list;
	}
	/// <summary>Deletes all files is provided directory</summary>
	public static void DeleteFilesInDirectory(string directory)
	{
		var list = Directory.GetFiles(directory).ToList();
		DeleteFilesInDirectory(directory, list);
	}
	/// <summary>Deletes all files in provided directory that are in the provided list</summary>
	public static void DeleteFilesInDirectory(string directory, List<string> filesToDelete)
	{
		foreach (var file in filesToDelete)
		{
			if (File.Exists(Path.Combine(directory, file)))
			{
				File.Delete(Path.Combine(directory, file));
			}
		}
	}
	/// <summary>Deletes all files in the provided list</summary>
	public static void DeleteListOfFiles(List<string> filesToDelete)
	{
		foreach (var file in filesToDelete)
		{
			if (File.Exists(file))
			{
				File.Delete(file);
			}
			else
			{
				var tempFile = file.Replace("\"", string.Empty);
				if (File.Exists(tempFile))
				{
					File.Delete(tempFile);
				}
			}
		}
	}
	/// <summary>Creates a temp file that is a copy of the provided file</summary>
	public static string CreateTempCopy(string filePath)
	{
		var tempFile = Path.GetTempFileName();

		using (var writer = new StreamWriter(tempFile))
		{
			using (var reader = new StreamReader(filePath))
			{
				writer.Write(reader.ReadToEnd());
			}
		}

		return tempFile;
	}
	/// <summary>Moves a file to a sub folder. Deletes the file if it already exists</summary>
	public static string MoveToSubFolder(string filePath, string subFolderName)
	{
		var subfolder = Path.Combine(Path.GetDirectoryName(filePath), subFolderName);
		if (!Directory.Exists(subfolder)) Directory.CreateDirectory(subfolder);
		var destinationPath = Path.Combine(subfolder, Path.GetFileName(filePath));
		if (File.Exists(destinationPath))
		{
			File.Delete(destinationPath);
		}
		File.Move(filePath, destinationPath);

		return destinationPath;
	}
	/// <summary>Deletes a directory and all files in it</summary>
	public static void DeleteDirectory(string directoryPath)
	{
		var di = new DirectoryInfo(directoryPath);

		foreach (var file in di.GetFiles())
		{
			file.Delete();
		}
		foreach (var dir in di.GetDirectories())
		{
			dir.Delete(true);
		}
		di.Delete();
	}


	/// <summary></summary>
	private static void Function_name()
	{
		// template
	}
}