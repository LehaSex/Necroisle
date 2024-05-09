using System.Collections.Generic;

namespace Necroisle.DevConsole
{
    public interface ILogStorage
    {
        int MaxStoredLogs { get; set; }
        IReadOnlyList<ILog> Logs { get; }

        void AddLog(ILog log);
        void RemoveLog();
        void Clear();

        string GetLogString();
    }

}