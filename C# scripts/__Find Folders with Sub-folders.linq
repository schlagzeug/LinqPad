<Query Kind="Statements" />

var directory = @"C:\Work\Cases";
DirectoryInfo di = new DirectoryInfo(directory);

foreach (var item in Directory.GetDirectories(directory))
{
    var subs = Directory.GetDirectories(item).ToList();
    if (subs.Count > 0)
    {
		item.Dump();
		foreach (var sub in subs)
		{
			$"	{sub}".Dump();
		}
		//subs.Dump();
    }
}