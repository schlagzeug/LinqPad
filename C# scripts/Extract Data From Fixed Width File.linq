<Query Kind="Program" />

void Main()
{
	var dir = @"C:\Work\Cases\1406";
	var outputLines = new List<string>();
	
	foreach (var file in Directory.GetFiles(dir))
	{
		var lines = MyUtil.GetListFromFile(file);
		var ccFile = new CCFile(Path.GetFileName(file));

		foreach (var line in lines)
		{
			if (line.StartsWith("RUN")) continue;
			if (!Char.IsWhiteSpace(line[72]) || !Char.IsWhiteSpace(line[79]))
			{
				ccFile.Accounts.Add(new AccountChange(line));
			}
		}

		if (ccFile.Accounts.Count > 0)
		{
			outputLines.Add(ccFile.ToString());
		}
	}
	
	MyUtil.WriteListToFile(@"C:\Work\Cases\1406\output\output.txt", outputLines);
}

public class AccountChange
{
	public AccountChange(string data)
	{
		AccountNumber = data.Substring(0, 10);
		Code = data.Substring(66, 7);
		Code2 = data.Substring(73, 7);
	}

	string AccountNumber { get; set; }
	string Code { get; set; }
	string Code2 { get; set; }

	public override string ToString()
	{
		return $"{AccountNumber} -- {Code} -- {Code2}";
	}
}
public class CCFile
{
	public CCFile(string fileName)
	{
		FileName = fileName;
	}
	
	public string FileName { get; set; }
	public List<AccountChange> Accounts = new List<UserQuery.AccountChange>();

	public override string ToString()
	{
		var output = FileName + "\r\n";

		foreach (var account in Accounts)
		{
			output += "\t";
			output += account.ToString();
			output += "\r\n";
		}

		return output;
	}
}