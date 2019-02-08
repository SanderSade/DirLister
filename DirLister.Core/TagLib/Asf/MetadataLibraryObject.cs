using System;
using System.Collections;
using System.Collections.Generic;

namespace Sander.DirLister.Core.TagLib.Asf
{
	/// <summary>
	///    This class extends <see cref="Object" /> to provide a
	///    representation of an ASF Metadata Library object which can be
	///    read from and written to disk.
	/// </summary>
	public class MetadataLibraryObject : Object,
		IEnumerable<DescriptionRecord>
	{
		/// <summary>
		///    Contains the description records.
		/// </summary>
		private readonly List<DescriptionRecord> records =
			new List<DescriptionRecord>();


		/// <summary>
		///    Constructs and initializes a new instance of <see
		///    cref="MetadataLibraryObject" /> by reading the contents
		///    from a specified position in a specified file.
		/// </summary>
		/// <param name="file">
		///    A <see cref="Asf.File" /> object containing the file from
		///    which the contents of the new instance are to be read.
		/// </param>
		/// <param name="position">
		///    A <see cref="long" /> value specify at what position to
		///    read the object.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///    <paramref name="file" /> is <see langword="null" />.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///    <paramref name="position" /> is less than zero or greater
		///    than the size of the file.
		/// </exception>
		/// <exception cref="CorruptFileException">
		///    The object read from disk does not have the correct GUID
		///    or smaller than the minimum size.
		/// </exception>
		public MetadataLibraryObject(File file, long position)
			: base(file, position)
		{
			if (!Guid.Equals(Asf.Guid.AsfMetadataLibraryObject))
				throw new CorruptFileException(
					"Object GUID incorrect.");

			if (OriginalSize < 26)
				throw new CorruptFileException(
					"Object size too small.");

			var count = file.ReadWord();

			for (ushort i = 0; i < count; i++)
			{
				var rec = new DescriptionRecord(
					file);
				AddRecord(rec);
			}
		}


		/// <summary>
		///    Constructs and initializes a new instance of <see
		///    cref="MetadataLibraryObject" /> with no contents.
		/// </summary>
		public MetadataLibraryObject()
			: base(Asf.Guid.AsfMetadataLibraryObject)
		{
		}


		/// <summary>
		///    Gets whether or not the current instance is empty.
		/// </summary>
		/// <value>
		///    <see langword="true" /> if the current instance doesn't
		///    contain any <see cref="DescriptionRecord" /> objects.
		///    Otherwise <see langword="false" />.
		/// </value>
		public bool IsEmpty => records.Count == 0;


		/// <summary>
		///    Gets an enumerator for enumerating through the
		///    description records.
		/// </summary>
		/// <returns>
		///    A <see cref="T:System.Collections.IEnumerator`1" /> for
		///    enumerating through the description records.
		/// </returns>
		public IEnumerator<DescriptionRecord> GetEnumerator()
		{
			return records.GetEnumerator();
		}


		IEnumerator
			IEnumerable.GetEnumerator()
		{
			return records.GetEnumerator();
		}


		/// <summary>
		///    Renders the current instance as a raw ASF object.
		/// </summary>
		/// <returns>
		///    A <see cref="ByteVector" /> object containing the
		///    rendered version of the current instance.
		/// </returns>
		public override ByteVector Render()
		{
			var output = new ByteVector();
			ushort count = 0;

			foreach (var rec in records)
			{
				count++;
				output.Add(rec.Render());
			}

			return Render(RenderWord(count) + output);
		}


		/// <summary>
		///    Removes all records with a given language, stream, and
		///    name from the current instance.
		/// </summary>
		/// <param name="languageListIndex">
		///    A <see cref="ushort" /> value containing the language
		///    list index of the records to be removed.
		/// </param>
		/// <param name="streamNumber">
		///    A <see cref="ushort" /> value containing the stream
		///    number of the records to be removed.
		/// </param>
		/// <param name="name">
		///    A <see cref="string" /> object containing the name of the
		///    records to be removed.
		/// </param>
		public void RemoveRecords(ushort languageListIndex,
			ushort streamNumber,
			string name)
		{
			for (var i = records.Count - 1; i >= 0; i--)
			{
				var rec = records[i];
				if (rec.LanguageListIndex == languageListIndex &&
				    rec.StreamNumber == streamNumber &&
				    rec.Name == name)
					records.RemoveAt(i);
			}
		}


		/// <summary>
		///    Gets all records with a given language, stream, and any
		///    of a collection of names from the current instance.
		/// </summary>
		/// <param name="languageListIndex">
		///    A <see cref="ushort" /> value containing the language
		///    list index of the records to be retrieved.
		/// </param>
		/// <param name="streamNumber">
		///    A <see cref="ushort" /> value containing the stream
		///    number of the records to be retrieved.
		/// </param>
		/// <param name="names">
		///    A <see cref="T:string[]" /> containing the names of the
		///    records to be retrieved.
		/// </param>
		/// <returns>
		///    A <see cref="T:System.Collections.Generic.IEnumerable`1" /> object enumerating
		///    through the <see cref="DescriptionRecord" /> objects
		///    retrieved from the current instance.
		/// </returns>
		public IEnumerable<DescriptionRecord> GetRecords(ushort languageListIndex,
			ushort streamNumber,
			params string[] names)
		{
			foreach (var rec in records)
			{
				if (rec.LanguageListIndex != languageListIndex ||
				    rec.StreamNumber != streamNumber)
					continue;

				foreach (var name in names)
					if (rec.Name == name)
						yield return rec;
			}
		}


		/// <summary>
		///    Adds a record to the current instance.
		/// </summary>
		/// <param name="record">
		///    A <see cref="DescriptionRecord" /> object to add to the
		///    current instance.
		/// </param>
		public void AddRecord(DescriptionRecord record)
		{
			records.Add(record);
		}


		/// <summary>
		///    Sets the a collection of records for a given language,
		///    stream, and name, removing the existing matching records.
		/// </summary>
		/// <param name="languageListIndex">
		///    A <see cref="ushort" /> value containing the language
		///    list index of the records to be added.
		/// </param>
		/// <param name="streamNumber">
		///    A <see cref="ushort" /> value containing the stream
		///    number of the records to be added.
		/// </param>
		/// <param name="name">
		///    A <see cref="string" /> object containing the name of the
		///    records to be added.
		/// </param>
		/// <param name="records">
		///    A <see cref="T:DescriptionRecord[]" /> containing records
		///    to add to the new instance.
		/// </param>
		/// <remarks>
		///    All added entries in <paramref name="records" /> should
		///    match <paramref name="languageListIndex" />, <paramref
		///    name="streamNumber" /> and <paramref name="name" /> but
		///    it is not verified by the method. The records will be
		///    added with their own values and not those provided in
		///    this method, which are used for removing existing values
		///    and determining where to position the new object.
		/// </remarks>
		public void SetRecords(ushort languageListIndex,
			ushort streamNumber, string name,
			params DescriptionRecord[] records)
		{
			var position = this.records.Count;
			for (var i = this.records.Count - 1; i >= 0; i--)
			{
				var rec = this.records[i];
				if (rec.LanguageListIndex == languageListIndex &&
				    rec.StreamNumber == streamNumber &&
				    rec.Name == name)
				{
					this.records.RemoveAt(i);
					position = i;
				}
			}

			this.records.InsertRange(position, records);
		}
	}
}
