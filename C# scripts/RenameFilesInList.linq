<Query Kind="Statements" />

/// Provided a list in the format of 'CURRENTNAME,NEWNAME' one record per line, renames all listed files 
/// in the provided directory

MyUtil.ShowStarted();
var listLocation = @"C:\Work\Cases\02169249\List.txt";
var directory = @"C:\Work\Cases\02169249";
var renameList = new Dictionary<string, string>();

using (var reader = File.OpenText(listLocation))
{
    while (!reader.EndOfStream)
    {
        var pair = reader.ReadLine().Split(',');
        renameList.Add(pair[0], pair[1]);
    }
}

foreach (var pair in renameList.Dump())
{
    if (File.Exists(Path.Combine(directory, pair.Key)))
    {
        File.Move(Path.Combine(directory, pair.Key), Path.Combine(directory, pair.Value));
    }
}

MyUtil.ShowFinished();