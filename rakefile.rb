#rakefile.rb

require 'rake'
#gem 'albacore', '=0.2.0.preview2'
require 'albacore'

task :default => [:full]


task :full => [:clean,:assemblyInfo,:build,:test,:publish]

task :clean do
	FileUtils.rm_rf 'build'
	FileUtils.rm_rf 'publish'
end


msbuild :build do |msb|
	msb.properties :configuration => :AutomatedRelease
	msb.solution = "src/BidsForKids.sln"
end


xunit :test do |xunit|
	xunit.command = "lib/xunit-1.6.1/xunit.console.x86.exe"
	xunit.assembly = "build/BidsForKids.Tests.dll"
	xunit.html_output = "report"
end


desc "Assembly Version Info Generator"
assemblyinfo :assemblyInfo do |asm|
	asm.output_file ="src/ProjectVersion.cs"
	asm.title = "Bids For Kids Auction Management System"
	asm.company_name = "Gatewood Elementary PTA"
	asm.product_name = "Bids For Kids Auction Management System"
	asm.version = "1.2.1.2"
	asm.file_version = "1.2.1.2"
	asm.copyright = "Copyright (c)2010-2011 Gatewood Elementary PTA"
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