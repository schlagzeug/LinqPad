<Query Kind="Program" />

/// Used to pull and push changes to the DataManagementToolBox solution

void Main()
{
	MyUtil.ShowStarted();

	//DeployCode();
	PullDownCode();
	
	MyUtil.ShowFinished();
}

public void DeployCode()
{
	var x = 0;
	
	x += MyUtil.CopyFiles(@"C:\Users\sreynolds\Documents\Visual Studio 2015\Projects\DataManagementToolbox\",
		@"\\CRP40PPFS07\RCT_Operations_PHI\DataManagment\sreynolds\Tools\ToolBoxes\Code\DataManagementToolbox\");
	x += MyUtil.CopyFiles(@"C:\Users\sreynolds\Documents\Visual Studio 2015\Projects\SharedTools\",
		@"\\CRP40PPFS07\RCT_Operations_PHI\DataManagment\sreynolds\Tools\ToolBoxes\Code\SharedTools\");
	x += MyUtil.CopyFiles(@"C:\Users\sreynolds\Documents\Visual Studio 2015\Projects\ClaimsManagementTools\",
		@"\\CRP40PPFS07\RCT_Operations_PHI\DataManagment\sreynolds\Tools\ToolBoxes\Code\ClaimsManagementTools\");
	x += MyUtil.CopyFiles(@"C:\Users\sreynolds\Documents\Visual Studio 2015\Projects\ContractManagementTools\",
		@"\\CRP40PPFS07\RCT_Operations_PHI\DataManagment\sreynolds\Tools\ToolBoxes\Code\ContractManagementTools\");
	x += MyUtil.CopyFiles(@"C:\Users\sreynolds\Documents\Visual Studio 2015\Projects\ChargeIntegrityTools\",
		@"\\CRP40PPFS07\RCT_Operations_PHI\DataManagment\sreynolds\Tools\ToolBoxes\Code\ChargeIntegrityTools\");

	$"{x} Files Updated on the Server".Dump();
}
public void PullDownCode()
{
	var x = 0;
	
	x += MyUtil.CopyFiles(@"\\CRP40PPFS07\RCT_Operations_PHI\DataManagment\sreynolds\Tools\ToolBoxes\Code\DataManagementToolbox\",
		@"C:\Users\sreynolds\Documents\Visual Studio 2015\Projects\DataManagementToolbox\");
	x += MyUtil.CopyFiles(@"\\CRP40PPFS07\RCT_Operations_PHI\DataManagment\sreynolds\Tools\ToolBoxes\Code\SharedTools\",
		@"C:\Users\sreynolds\Documents\Visual Studio 2015\Projects\SharedTools\");
	x += MyUtil.CopyFiles(@"\\CRP40PPFS07\RCT_Operations_PHI\DataManagment\sreynolds\Tools\ToolBoxes\Code\ClaimsManagementTools\",
		@"C:\Users\sreynolds\Documents\Visual Studio 2015\Projects\ClaimsManagementTools\");
	x += MyUtil.CopyFiles(@"\\CRP40PPFS07\RCT_Operations_PHI\DataManagment\sreynolds\Tools\ToolBoxes\Code\ContractManagementTools\",
		@"C:\Users\sreynolds\Documents\Visual Studio 2015\Projects\ContractManagementTools\");
	x += MyUtil.CopyFiles(@"\\CRP40PPFS07\RCT_Operations_PHI\DataManagment\sreynolds\Tools\ToolBoxes\Code\ChargeIntegrityTools\",
		@"C:\Users\sreynolds\Documents\Visual Studio 2015\Projects\ChargeIntegrityTools\");

	$"{x} Files Updated Locally".Dump();
}