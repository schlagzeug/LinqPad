<Query Kind="Program" />

void Main()
{
	var records = new List<Record>();
	var lines = MyUtil.GetListFromFile(@"C:\Work\Cases\02546715\stagnes_pa_20180309_edit.out");
	foreach (var line in lines)
	{
		var r = new Record(line);
		records.Add(r);
	}
	records.Sum(x => x.Amount ).Dump("File Total");

	var sumOfTypes =
	from record in records
	group record by record.Type into recordGroup
	select new
	{
		Type = recordGroup.Key,
		TotalAmount = recordGroup.Sum(x => x.Amount),
	};
	sumOfTypes.Dump();
	
	var z = from y in records
	where y.Amount == 60m && y.Type == "Payment"
	select y;
	z.Dump();
}

// Define other methods and classes here
public class Record
{
	public string Patnum { get; set; }
	public decimal Amount { get; set; }
	public string Type { get; set; }
	public Record(string input)
	{
		var elements = input.Split('|');
		Patnum = elements[0].Trim();
		Type = elements[4].Trim();
		Amount = decimal.Parse(elements[2]);
	}
}