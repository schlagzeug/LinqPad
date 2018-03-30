<Query Kind="Program" />

/// This script is used to calculate totals on various file types
/// Update the static members of the Flag class to needed values for
/// your desired file type.
/// Ansi 837 files should work automatically without flags
/// The type of file will be determined automatically by the 
/// TotalsFileCreator class.

public static class Flag
{
	// DELIMITED FILE FLAGS
	//   Index list:
	//     32 - UB xmitlog files
	//     38 - HF xmitlog files
	//      3 - DT xmitlog files
	//      2 - PA xmitlog files
	public static readonly int Index = 0;
	public static readonly char Delimiter = '|';

	// PRINT IMAGE FILE FLAGS
	public static readonly int ChargeLineStart = 18;
	public static readonly int MaxChargeLines = 22;
}
/**********************************************************/
void Main()
{
	var file = string.Empty;
	if (MyUtil.OpenFileDialog(out file) == true)
	{
		MyUtil.ShowStarted();
		TotalsFileCreator.CreateTotalsFile(file.Dump("Selected File:"));
		MyUtil.ShowFinished();
	}
}

public static class TotalsFileCreator
{
	public static void CreateTotalsFile(string sourceFile)
	{
		var type = CheckFile(sourceFile);
		type.Dump("Selected Type:");
		
		switch (type)
		{
			case IncomingFileType.Ansi837:
				CreateTotalsFile_Ansi837(sourceFile);
				break;
			case IncomingFileType.Delimited:
				CreateTotalsFile_Delimited(sourceFile);
				break;
			case IncomingFileType.PrintImage:
				CreateTotalsFile_PrintImage(sourceFile);
				break;
			case IncomingFileType.Unspecified:
			default:
				"File Type can't be determined".Dump();
				break;
		}
	}
	private static IncomingFileType CheckFile(string sourceFile)
	{
		using (var reader = new StreamReader(sourceFile))
		{
			var firstLine = reader.ReadLine();
			if (firstLine.ToUpper().StartsWith("ISA")) return IncomingFileType.Ansi837;
			else if (firstLine.ToUpper().Contains(Flag.Delimiter)) return IncomingFileType.Delimited;
			else return IncomingFileType.PrintImage;
		}
	}
	private static void CreateTotalsFile_Ansi837(string sourceFile)
	{
		var totalsFile = MyUtil.GetResultsFile(sourceFile);
		var ansiFile = new Ansi837File(sourceFile);
		
		using (var writer = new StreamWriter(totalsFile))
		{
			foreach (var clm in ansiFile.Claims)
			{
				writer.WriteLine(clm.ToString());
			}

			writer.WriteLine(MyUtil.DIVIDER); writer.WriteLine();
			writer.WriteLine(ansiFile.ToString().Dump());
		}

		MyUtil.OpenFileInNotepad(totalsFile);
	}
	private static void CreateTotalsFile_Delimited(string sourceFile)
	{
		var outFile = MyUtil.GetResultsFile(sourceFile);

		var myFile = new DelimitedFile(sourceFile);

		using (var writer = new StreamWriter(outFile))
		{
			writer.WriteLine($"File: {sourceFile}".Dump());
			writer.WriteLine($"Using Column: {Flag.Index}".Dump());
			writer.WriteLine($"Using Delimiter: {Flag.Delimiter}".Dump());
			writer.WriteLine($"Total: {myFile.Total}".Dump());
		}

		MyUtil.OpenFileInNotepad(outFile);
	}
	private static void CreateTotalsFile_PrintImage(string sourceFile)
	{
		var totalsFile = MyUtil.GetResultsFile(sourceFile);
		var printImageFile = new PrintImageFile(sourceFile);

		using (var writer = new StreamWriter(totalsFile))
		{
			foreach (var clm in printImageFile.Claims)
			{
				writer.WriteLine(clm.ToString());
			}

			writer.WriteLine(MyUtil.DIVIDER); writer.WriteLine();
			writer.WriteLine(printImageFile.ToString().Dump());
		}

		MyUtil.OpenFileInNotepad(totalsFile);
	}
}

public enum IncomingFileType
{
	Ansi837,
	Delimited,
	PrintImage,
	Unspecified
}
public class Record
{
	public decimal Amount { get; set; }
	public Record(string line)
	{
		var x = line.Split(Flag.Delimiter);
		var d = 0m;
		Amount = decimal.TryParse(x[Flag.Index], out d) ? d : 0m;
	}
}
public class DelimitedFile
{
	public List<Record> Lines { get; set; }
	public decimal Total { get; set; }
	
	public DelimitedFile(string filePath)
	{
		Lines = new List<Record>();
		using (var reader = new StreamReader(filePath))
		{
			var skippedRecords = 0;
			var line = string.Empty;
			while ((line = reader.ReadLine()) != null)
			{
				try
				{
					var x = new Record(line);
					Total += x.Amount;
					Lines.Add(x);
				}
				catch
				{
					skippedRecords++;
				}
			}
			
			if (skippedRecords > 0) skippedRecords.Dump("Skipped Records:");
		}
	}
}
public class ClmInfo
{
	public ClmInfo()
	{
		LineAmts = new List<decimal>();
	}

	public string PCN { get; set; }
	public decimal ClmAmount { get; set; }
	public decimal TotalSvn
	{
		get
		{
			var total = 0m;
			foreach (var amt in LineAmts)
			{
				total += amt;
			}
			return total;
		}
	}
	public List<decimal> LineAmts { get; set; }

	public override string ToString()
	{
		var retVal =
			string.Format("PCN - {0}\r\n", PCN) +
			string.Format("CLM Total - {0}\r\n", ClmAmount.ToString("n2")) +
			string.Format("SV Line Total - {0}\r\n", TotalSvn.ToString("n2"));
		if (ClmAmount != TotalSvn)
		{
			retVal += string.Format("ERROR - Amount Mismatch - {0}\r\n", (ClmAmount - TotalSvn).ToString("n2"));
		}

		return retVal;
	}
}
public class Ansi837File
{
	public string SourceFile { get; set; }
	public List<ClmInfo> Claims { get; set; }
	public decimal ClaimLineTotal { get; set; }
	public decimal SvcLineTotal { get; set; }
	
	public Ansi837File(string sourceFile)
	{
		SourceFile = sourceFile;
		Claims = new List<ClmInfo>();
		var splitChar = MyUtil.GetCharacterSeparatorFromANSI(sourceFile).Dump("Separator Char:");

		using (var reader = new StreamReader(sourceFile))
		{
			var clminfo = new ClmInfo();
			var line = string.Empty;
			while ((line = reader.ReadLine()) != null)
			{
				if (line.StartsWith(string.Format("CLM{0}", splitChar)))
				{
					if (clminfo.PCN != null && clminfo.PCN != string.Empty)
					{
						Claims.Add(clminfo);
						ClaimLineTotal += clminfo.ClmAmount;
						SvcLineTotal += clminfo.TotalSvn;
					}
					clminfo = new ClmInfo();
					var elements = line.Split(splitChar).ToList();
					if (elements.Count >= 3)
					{
						clminfo.PCN = elements[1];
						clminfo.ClmAmount = decimal.Parse(elements[2]);
					}
				}
				else if (line.StartsWith(string.Format("SV1{0}", splitChar)))
				{
					var elements = line.Split(splitChar).ToList();
					if (elements.Count >= 3)
					{
						clminfo.LineAmts.Add(decimal.Parse(elements[2]));
					}
				}
				else if (line.StartsWith(string.Format("SV2{0}", splitChar)))
				{
					var elements = line.Split(splitChar).ToList();
					if (elements.Count >= 4)
					{
						clminfo.LineAmts.Add(decimal.Parse(elements[3]));
					}
				}
			}
			Claims.Add(clminfo); // adds the final claim
			ClaimLineTotal += clminfo.ClmAmount;
			SvcLineTotal += clminfo.TotalSvn;
		}
	}
	public override string ToString()
	{
		var retVal = $"For File {Path.GetFileName(SourceFile)}:\r\n" +
					 $"\tCLM segment Count - {Claims.Count}\r\n" +
					 $"\tCLM segment Total - {ClaimLineTotal.ToString("n2")}\r\n" +
					 $"\tSV segment Total - {SvcLineTotal.ToString("n2")}";
		return retVal;
	}
}
public class PrintImageFile
{
	public string SourceFile { get; set; }
	public List<ClmInfo> Claims { get; set; }
	public decimal ClaimLineTotal { get; set; }
	public decimal SvcLineTotal { get; set; }

	public PrintImageFile(string sourceFile)
	{
		SourceFile = sourceFile;
		Claims = new List<ClmInfo>();

		using (var reader = new StreamReader(sourceFile))
		{
			var currentClm = "XXXXXXXXXXXXXX";
			var line = string.Empty;
			ClmInfo clminfo = null;
			do
			{
				if (line != string.Empty && line[0] == (char)12 && line.Length > 1)
				{
					if (!line.Contains(currentClm))
					{
						if (clminfo != null)
						{
							Claims.Add(clminfo);
							ClaimLineTotal += clminfo.ClmAmount;
							SvcLineTotal += clminfo.TotalSvn;
						}
						clminfo = new ClmInfo();

						line = line.Substring(1).Trim();

						currentClm = line.Substring(0, line.IndexOf(" "));
						clminfo.PCN = currentClm;
					}

					for (int i = 0; i < Flag.ChargeLineStart; i++)
					{
						line = reader.ReadLine();
					}
					for (int i = 0; i < Flag.MaxChargeLines; i++)
					{
						if (line != string.Empty)
						{
							line = line.Substring(62, 10).Trim();
							clminfo.LineAmts.Add(decimal.Parse(line) / 100);
						}
						line = reader.ReadLine();
					}

					if (line.StartsWith("  0001"))
					{
						line = line.Substring(62, 10).Trim();
						clminfo.ClmAmount = decimal.Parse(line) / 100;
					}
				}
				line = reader.ReadLine();
			} while (line == string.Empty || ((line != string.Empty) && line[line.Length - 1] != (char)12));

			Claims.Add(clminfo); // adds the final claim
			ClaimLineTotal += clminfo.ClmAmount;
			SvcLineTotal += clminfo.TotalSvn;
		}
	}
	public override string ToString()
	{
		var retVal = $"For File {Path.GetFileName(SourceFile)}:\r\n" +
					 $"\tClaims Count - {Claims.Count}\r\n" +
					 $"\tClaim Amount Total - {ClaimLineTotal.ToString("n2")}\r\n" +
					 $"\tLine Item Total - {SvcLineTotal.ToString("n2")}";
		return retVal;
	}
}