using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ascon.NetMemoryProfiler;
using NUnit.Framework;

namespace LeakingApp.Tests
{
    [TestFixture]
    public class MemoryLeakTests
    {
        [Test]
        public void EventHandlerLeakTest()
        {
            // Before running the test, do steps below:
            // 1. Run Leaking App
            // 2. Click "EventHandler Leak" button
            // 3. Close appeared window
            using (var session = Profiler.AttachToProcess("LeakingApp"))
            {
                var objects = session.GetAliveObjects(x => x.Type.EndsWith("EventHandlerLeakViewModel")).ToList();
                if (objects.Any())
                {
                    var retentions = session.FindRetentions(objects);
                    Assert.Fail(DumpRetentions(retentions));
                }
            }
        }

        [Test]
        public void BindingLeakTest()
        {
            // Before running the test, do steps below:
            // 1. Run Leaking App
            // 2. Click "Binding Leak" button
            // 3. Close appeared window
            using (var session = Profiler.AttachToProcess("LeakingApp"))
            {
                var objects = session.GetAliveObjects(x => x.Type.EndsWith("BindingLeakViewModel")).ToList();
                if (objects.Any())
                {
                    var retentions = session.FindRetentions(objects);
                    Assert.Fail(DumpRetentions(retentions));
                }
            }
        }

        [Test]
        public void StaticRefLeakTest()
        {
            // Before running the test, do steps below:
            // 1. Run Leaking App
            // 2. Click "Static Ref Leak" button
            // 3. Close appeared window
            using (var session = Profiler.AttachToProcess("LeakingApp"))
            {
                var objects = session.GetAliveObjects(x => x.Type.EndsWith("StaticRefLeakViewModel")).ToList();
                if (objects.Any())
                {
                    var retentions = session.FindRetentions(objects);
                    Assert.Fail(DumpRetentions(retentions));
                }
            }
        }

        [Test]
        public void CollectionLeakTest()
        {
            // Before running the test, do steps below:
            // 1. Run Leaking App
            // 2. Click "Collection Leak" button
            // 3. Close appeared window
            using (var session = Profiler.AttachToProcess("LeakingApp"))
            {
                var objects = session.GetAliveObjects(x => x.Type.EndsWith("MyCollectionItem")).ToList();
                if (objects.Any())
                {
                    var retentions = session.FindRetentions(objects);
                    Assert.Fail(DumpRetentions(retentions));
                }
            }
        }

        static string DumpRetentions(IEnumerable<RetentionsInfo> retentions)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var group in retentions.GroupBy(x => x.Instance.TypeName))
            {
                sb.AppendLine($"Found {group.Count()} instances of {group.Key}");
                var instances = group.ToList();
                for (int i = 1; i <= instances.Count; i++)
                {
                    var instance = instances[i - 1];
                    sb.AppendLine($"Instance {i}:");
                    foreach (var retentionPath in instance.RetentionPaths)
                    {
                        sb.AppendLine(retentionPath);
                        sb.AppendLine("----------------------------");
                    }
                }
            }
            return sb.ToString();
        }
    }
}
