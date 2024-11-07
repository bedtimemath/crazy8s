if ($args.Count -eq 0) 
{
	dotnet ef -p .\Libraries\C8S.Database.EFCore -s .\Utilities\C8S.EFCoreSetup migrations list
}
else
{
	dotnet ef -p .\Libraries\C8S.Database.EFCore -s .\Utilities\C8S.EFCoreSetup migrations list $args
}


