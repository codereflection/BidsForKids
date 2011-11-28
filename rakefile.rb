#rakefile.rb

require 'rake'
require 'albacore'

task :default => [:full]


task :full => [:clean,:assemblyInfo,:build,:xunitTests,:specifications,:publish]

task :clean do
	FileUtils.rm_rf 'build'
	FileUtils.rm_rf 'publish'
end

task :migrateLocal => [:clean,:assemblyInfo,:build,:migrate_local]

fluentmigrator :migrate_local do |fm|
    fm.command = "src/packages/FluentMigrator.1.0.1.0/tools/Migrate.exe"
    fm.connection = "Data Source=(local);Initial Catalog=BidForKids;Integrated Security=True"
    fm.provider = "sqlserver2000"
    fm.namespace = "BidsForKids.Database"
    fm.target = "build/BidsForKids.Database.dll"
end

msbuild :build do |msb|
	msb.properties :configuration => :AutomatedRelease
	msb.solution = "src/BidsForKids.sln"
end


xunit :xunitTests do |xunit|
	xunit.command = "lib/xunit-1.6.1/xunit.console.x86.exe"
	xunit.assembly = "build/BidsForKids.Tests.dll"
	xunit.html_output = "report/Tests"
end

mspec :specifications do |mspec|
	mspec.command = "packages/Machine.Specifications.0.4.24.0/tools/mspec.exe"
	mspec.assemblies = "build/BidsForKids.Tests.dll"
	mspec.html_output = "report/Specs"
end


desc "Assembly Version Info Generator"
assemblyinfo :assemblyInfo do |asm|
	asm.output_file ="src/ProjectVersion.cs"
	asm.title = "Bids For Kids Auction Management System"
	asm.company_name = "Gatewood Elementary PTA"
	asm.product_name = "Bids For Kids Auction Management System"
	asm.version = "1.2.1.7"
	asm.file_version = "1.2.1.7"
	asm.copyright = "Copyright (c)2010-2012 Elementary PTA"
end

msbuild :publish do |msb|
	msb.targets :ResolveReferences, :_CopyWebApplication
	msb.properties(
		:configuration => :AutomatedRelease,
		:webprojectoutputdir => "../../publish/",
		:outdir => "../../publish/bin/"
	)
	msb.solution = "src/BidForKids/BidsForKids.csproj"
end
