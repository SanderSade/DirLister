using System;
using System.Diagnostics;

namespace Sander.DirLister.UI.DTO
{
	public sealed class LogEntry
	{
		public LogEntry(TraceLevel level, string message)
		{
			Level = level;
			Message = message;
			Timestamp = DateTimeOffset.UtcNow;
		}


		internal DateTimeOffset Timestamp { get; set; }
		internal TraceLevel Level { get; set; }
		internal string Message { get; set; }


		public override string ToString()
		{
			return $"[{Timestamp.ToLocalTime():G}] {Level}: {Message}";
		}
	}
}
