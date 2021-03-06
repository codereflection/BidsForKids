#rakefile.rb

require 'rake'
require 'albacore'

task :default => [:full]


task :full => [:clean,:solutionPackageRestore,:assemblyInfo,:build,:xunitTests,:specifications,:publish]

task :clean do
	FileUtils.rm_rf 'build'
	FileUtils.rm_rf 'publish'
end

exec :solutionPackageRestore do |cmd|
    cmd.command = "src/.nuget/nuget.exe"
    cmd.parameters = "install src/.nuget/packages.config -o src/packages"
end

msbuild :build do |msb|
	msb.properties :configuration => :AutomatedRelease
	msb.solution = "src/BidsForKids.sln"
end


xunit :xunitTests do |xunit|
	xunit.command = "src/packages/xunit.runners.1.9.1/tools/xunit.console.clr4.x86.exe"
	xunit.assembly = "build/BidsForKids.Tests.dll"
	xunit.html_output = "report/Tests"
end

mspec :specifications do |mspec|
	mspec.command = "src/packages/Machine.Specifications.0.5.8/tools/mspec-clr4.exe"
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
