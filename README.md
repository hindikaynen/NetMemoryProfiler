# Ascon.NetMemoryProfiler
Ascon.NetMemoryProfiler is a set of APIs for memory analysis of .NET processes. It allows you to attach to live processes or open crash dumps to inspect which .NET objects are hold in memory, and what prevents these objects from being collected by GC. Below you'll find a minimalistic sample of using Ascon.NetMemoryProfiler:

```cpp
// Attaching profiler to the process "MyApp"
// After being attached, profiler injects into process and forces garbage collection in it
using (var session = Profiler.AttachToProcess("MyApp"))
{
	// Finds alive instances of "MyApp.Foo"
	var objects = session.GetAliveObjects(x => x.Type == "MyApp.Foo");
	// Finds retention paths (paths from GC roots which prevent instances from being collected)
	var retentions = session.FindRetentions(objects);
}
```
Available at https://www.nuget.org/packages/Ascon.NetMemoryProfiler

# Requirements:
- .NET Framework 4 or later
- Microsoft visual C++ 2010 Redistributable installed
