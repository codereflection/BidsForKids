#rakefile.rb

require 'rake'
require 'albacore'

desc 'Perform a full build'
task :default => [:full]

desc 'Run a full clean, update assembly info, build, xUnit.Net, MSpec, and publish'
task :full => [:clean,:solutionPackageRestore,:assemblyInfo,:build,:xunitTests,:specifications,:publish]

desc 'Clean the build and publish folders'
task :clean do
	FileUtils.rm_rf 'build'
	FileUtils.rm_rf 'publish'
end

exec :solutionPackageRestore do |cmd|
    cmd.command = "src/.nuget/nuget.exe"
    cmd.parameters = "install src/.nuget/packages.config -o src/packages"
end

desc 'Run migrations aginst SQL instance at (local)'
task :migrateLocal => [:clean,:assemblyInfo,:build,:migrate_local]

desc 'Run migrations against SQL Express instance at (local)\SQLExpress'
task :migrateLocalExpress => [:clean,:assemblyInfo,:build,:migrate_local_express]

desc 'Rollback all migrations against SQL Express instance at (local)\SQLExpress'
task :rollbackAllLocalExpress => [:clean,:assemblyInfo,:build,:rollback_All_local_express]

fluentmigrator :migrate_local do |fm|
    fm.command = "src/packages/FluentMigrator.Tools.1.0.3.0/tools/AnyCpu/40/Migrate.exe"
    fm.connection = "Data Source=(local);Initial Catalog=BidForKids;Integrated Security=True"
    fm.provider = "sqlserver2008"
    fm.namespace = "BidsForKids.Database"
    fm.target = "build/BidsForKids.Database.dll"
end

fluentmigrator :migrate_local_express do |fm|
    fm.command = "src/packages/FluentMigrator.Tools.1.0.3.0/tools/AnyCpu/40/Migrate.exe"
    fm.connection = "Data Source=(local)\\SQLEXPRESS;Initial Catalog=BidForKids;Integrated Security=True"
    fm.provider = "sqlserver2008"
    fm.namespace = "BidsForKids.Database"
    fm.target = "build/BidsForKids.Database.dll"
end

fluentmigrator :rollback_All_local_express do |fm|
    fm.command = "src/packages/FluentMigrator.Tools.1.0.3.0/tools/AnyCpu/40/Migrate.exe"
    fm.connection = "Data Source=(local)\\SQLEXPRESS;Initial Catalog=BidForKids;Integrated Security=True"
    fm.provider = "sqlserver2008"
    fm.namespace = "BidsForKids.Database"
    fm.target = "build/BidsForKids.Database.dll"
    fm.task = "rollback:all"
end

desc 'Build the application'
msbuild :build do |msb|
	msb.properties :configuration => :AutomatedRelease
	msb.solution = "src/BidsForKids.sln"
end

desc 'Run all xUnit.net tests'
xunit :xunitTests do |xunit|
	xunit.command = "src/packages/xunit.runners.1.9.1/tools/xunit.console.clr4.x86.exe"
	xunit.assembly = "build/BidsForKids.Tests.dll"
	xunit.html_output = "report/Tests"
end

desc 'Run all Machine.Specification specs'
mspec :specifications do |mspec|
	mspec.command = "src/packages/Machine.Specifications.0.5.8/tools/mspec-clr4.exe"
	mspec.assemblies = "build/BidsForKids.Tests.dll"
	mspec.html_output = "report/Specs"
end


desc 'Assembly Version Info Generator'
assemblyinfo :assemblyInfo do |asm|
	asm.output_file ="src/ProjectVersion.cs"
	asm.title = "Bids For Kids Auction Management System"
	asm.company_name = "Gatewood Elementary PTA"
	asm.product_name = "Bids For Kids Auction Management System"
	asm.version = "1.2.1.7"
	asm.file_version = "1.2.1.7"
	asm.copyright = "Copyright (c)2010-2012 Elementary PTA"
end

desc 'Publish to the publish folder to stage for deployment'
msbuild :publish do |msb|
	msb.targets :ResolveReferences, :_CopyWebApplication
	msb.properties(
		:configuration => :AutomatedRelease,
		:webprojectoutputdir => "../../publish/",
		:outdir => "../../publish/bin/"
	)
	msb.solution = "src/BidForKids/BidsForKids.csproj"
end
