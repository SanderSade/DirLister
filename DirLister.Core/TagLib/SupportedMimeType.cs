using System;
using System.Collections.Generic;

namespace Sander.DirLister.Core.TagLib
{

	/// <summary>
	///
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public sealed class SupportedMimeType : Attribute
	{
		/// <summary>
		///    Contains the registered <see cref="SupportedMimeType" />
		///    objects.
		/// </summary>
		private static readonly List<SupportedMimeType> mimetypes =
			new List<SupportedMimeType>();


		/// <summary>
		///    Constructs and initializes the <see
		///    cref="SupportedMimeType" /> class by initializing the
		///    <see cref="FileTypes" /> class.
		/// </summary>
		static SupportedMimeType()
		{
			//to init the static file
			var init = FileTypes.AvailableTypes.Count == 1;
		}


		/// <summary>
		///    Constructs and initializes a new instance of the <see
		///    cref="SupportedMimeType" /> attribute for a specified
		///    mime-type.
		/// </summary>
		/// <param name="mimetype">
		///    A <see cref="string" /> object containing a standard
		///    mime-type.
		/// </param>
		/// <remarks>
		///    <para>Standard practice is to use <see
		///    cref="SupportedMimeType(string)" /> to register standard
		///    mime-types, like "audio/mp3" and "video/mpeg" and to use
		///    <see cref="SupportedMimeType(string,string)" /> strictly
		///    to register extensions, using "taglib/ext" for the mime
		///    type. Eg. <c>SupportedMimeType("taglib/mp3",
		///    "mp3")</c>.</para>
		/// </remarks>
		public SupportedMimeType(string mimetype)
		{
			MimeType = mimetype;
			mimetypes.Add(this);
		}


		/// <summary>
		///    Constructs and initializes a new instance of the <see
		///    cref="SupportedMimeType" /> attribute for a specified
		///    mime-type and extension.
		/// </summary>
		/// <param name="mimetype">
		///    A <see cref="string" /> object containing a standard
		///    mime-type.
		/// </param>
		/// <param name="extension">
		///    A <see cref="string" /> object containing a file
		///    extension.
		/// </param>
		/// <remarks>
		///    <para>Standard practice is to use <see
		///    cref="SupportedMimeType(string)" /> to register standard
		///    mime-types, like "audio/mp3" and "video/mpeg" and to use
		///    <see cref="SupportedMimeType(string,string)" /> strictly
		///    to register extensions, using "taglib/ext" for the mime
		///    type. Eg. <c>SupportedMimeType("taglib/mp3",
		///    "mp3")</c>.</para>
		/// </remarks>
		public SupportedMimeType(string mimetype, string extension)
			: this(mimetype)
		{
			Extension = extension;
		}


		/// <summary>
		///    Gets the mime-type registered by the current instance.
		/// </summary>
		/// <value>
		///    A <see cref="string" /> object containing the mime-type
		///    registered by the current instance.
		/// </value>
		/// <remarks>
		///    <para>The value is in the format "generic/specific". For
		///    example, "video/mp4".</para>
		/// </remarks>
		public string MimeType { get; }

		/// <summary>
		///    Gets the extension registered by the current instance.
		/// </summary>
		/// <value>
		///    A <see cref="string" /> object containing the extension
		///    registered by the current instance, or <see
		///    langword="null" /> if not specified.
		/// </value>
		/// <remarks>
		///    <para>The value is the file extension minus the preceding
		///    ".". For example, "m4v".</para>
		/// </remarks>
		public string Extension { get; }

		/// <summary>
		///    Gets all the mime-types that have been registered with
		///    <see cref="SupportedMimeType" />.
		/// </summary>
		/// <value>
		///    A <see cref="T:System.Collections.Generic.IEnumerable`1" /> object containing all the
		///    mime-types that have been registered with <see
		///    cref="SupportedMimeType" />.
		/// </value>
		/// <remarks>
		///    <para>These values are used by <see
		///    cref="TagLib.File.Create(string,string,ReadStyle)" /> to
		///    match file types.</para>
		/// </remarks>
		public static IEnumerable<string> AllMimeTypes
		{
			get
			{
				foreach (var type in mimetypes)
					yield return type.MimeType;
			}
		}

		/// <summary>
		///    Gets all the extensions that have been registered with
		///    <see cref="SupportedMimeType" />.
		/// </summary>
		/// <value>
		///    A <see cref="T:System.Collections.Generic.IEnumerable`1" /> object containing all the
		///    extensions that have been registered with <see
		///    cref="SupportedMimeType" />.
		/// </value>
		/// <remarks>
		///    <para>These values are currently not used in file type
		///    recognition.</para>
		/// </remarks>
		public static IEnumerable<string> AllExtensions
		{
			get
			{
				foreach (var type in mimetypes)
					if (type.Extension != null)
						yield return type.Extension;
			}
		}
	}
}
