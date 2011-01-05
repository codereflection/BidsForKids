#rakefile.rb

require 'rake'
gem 'albacore', '=0.2.0.preview2'
require 'albacore'

task :default => [:full]


task :full => [:clean,:build,:test]

task :clean do
	FileUtils.rm_rf 'build'
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
