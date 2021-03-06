﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Sander.DirLister.Core.Application.Writers
{
	internal sealed class JsonWriter : BaseWriter
	{
		public JsonWriter(Configuration configuration, DateTimeOffset endDate, List<FileEntry> entries) : base(configuration, endDate, entries)
		{
		}


		[SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
		protected internal override string Write()
		{
			var fileName = GetFilename(OutputFormat.Json);
			using (var file = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Read, 2 << 16))
			{
				using (var writer = JsonReaderWriterFactory.CreateJsonWriter(
					file, Encoding.UTF8, true, true, "\t"))
				{
					var serializer = new DataContractJsonSerializer(typeof(List<FileEntry>),
						new DataContractJsonSerializerSettings { SerializeReadOnlyTypes = true });

					serializer.WriteObject(writer, Entries);
					writer.Flush();
				}
			}

			return fileName;
		}
	}
}
