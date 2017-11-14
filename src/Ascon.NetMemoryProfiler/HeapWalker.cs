using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Diagnostics.Runtime;

namespace Ascon.NetMemoryProfiler
{
    delegate bool HandleObjectDelegate(ulong address, Func<string> getRetentionPath);

    class HeapWalker
    {
        private readonly ClrHeap _heap;

        public HeapWalker(ClrHeap heap)
        {
            _heap = heap;
        }

        public void Traverse(HandleObjectDelegate handleObject)
        {
            var visited = new HashSet<ulong>();
            foreach (var gcRoot in _heap.EnumerateRoots())
            {
                var stack = new Stack<TreeItem>();
                var currentPath = new List<TreeItem>();
                var top = new TreeItem { Address = gcRoot.Object, Level = 0 };
                stack.Push(top);

                while (stack.Any())
                {
                    var previousLevel = top.Level;
                    top = stack.Pop();
                    if (top.Level <= previousLevel)
                        currentPath = currentPath.Where(x => x.Level < top.Level).ToList();

                    currentPath.Add(top);

                    var type = _heap.GetObjectType(top.Address);

                    var localTop = top;
                    var localCurrentPath = currentPath;
                    type?.EnumerateRefsOfObject(top.Address, (addr, offset) =>
                    {
                        if (addr != 0 && !visited.Contains(addr))
                        {
                            var handled = handleObject(addr, () =>
                            {
                                StringBuilder retention = new StringBuilder();
                                retention.AppendLine(gcRoot.Name);

                                for (var i = 0; i < localCurrentPath.Count; i++)
                                {
                                    var item = localCurrentPath[i];
                                    var ptrType = _heap.GetObjectType(item.Address);
                                    if (ptrType == null)
                                        continue;

                                    retention.AppendLine(ptrType.Name);
                                }
                                return retention.ToString();
                            });

                            if (!handled)
                            {
                                stack.Push(new TreeItem { Address = addr, Level = localTop.Level + 1 });
                            }
                        }
                    });
                    visited.Add(top.Address);
                }
            }
        }

        struct TreeItem
        {
            public ulong Address;
            public uint Level;
        }
    }
}
