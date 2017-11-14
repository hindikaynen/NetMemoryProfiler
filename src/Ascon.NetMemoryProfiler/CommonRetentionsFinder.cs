using System.Collections.Generic;

namespace Ascon.NetMemoryProfiler
{
    static class CommonRetentionsFinder
    {
        public static void AddOrReplace(List<string> retentionPaths, string addingRetentionPath)
        {
            bool replaced = false;
            for (int i = 0; i < retentionPaths.Count; i++)
            {
                var current = retentionPaths[i];
                string commonRetention;
                if (TryGetCommonRetention(current, addingRetentionPath, out commonRetention))
                {
                    retentionPaths[i] = commonRetention;
                    replaced = true;
                    break;
                }
            }
            if (!replaced)
                retentionPaths.Add(addingRetentionPath);
        }

        private static bool TryGetCommonRetention(string retention1, string retention2, out string commonRetention)
        {
            if (retention1.Length == retention2.Length)
            {
                commonRetention = retention1;
                return string.Equals(retention1, retention2);
            }
            if (retention1.Length > retention2.Length)
            {
                commonRetention = retention2;
                return retention1.Contains(retention2);
            }
            if (retention2.Length > retention1.Length)
            {
                commonRetention = retention1;
                return retention2.Contains(retention1);
            }
            commonRetention = null;
            return false;
        }
    }
}
