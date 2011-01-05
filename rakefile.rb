#rakefile.rb

require 'rake'
require 'albacore'

task :default => [:full]


task :full => [:clean,:build,:test]

task :clean do
	FileUtils.rm_rf 'build'
end


msbuild :build do |msb|
	msb.properties :configuration => :AutomatedRelease
	msb.targets :Build
	msb.solution = "src/BidsForKids.sln"
end


xunit :test do |xunit|
	xunit.path_to_command = "lib/xunit-1.6.1/xunit.console.x86.exe"
	xunit.assembly = "build/BidsForKids.Tests.dll"
	xunit.html_output = "../report/testReport.html"
end
