using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Sander.DirLister.Core
{
	/// <summary>
	/// Filter to apply when listing
	/// </summary>
	public sealed class Filter
	{
		private readonly string[] _wildcards;
		private readonly Regex _regex;
		private readonly bool _isWildcard;

		/// <summary>
		/// No-filtering constructor
		/// </summary>
		internal Filter()
		{
			_isWildcard = true;
		}

		/// <summary>
		/// Apply wildcard filter
		/// </summary>
		/// <param name="wildcards"></param>
		public Filter(string[] wildcards)
		{
			_wildcards = wildcards;
			_isWildcard = true;
		}

		/// <summary>
		/// Apply regex filter
		/// </summary>
		/// <param name="regex"></param>
		public Filter(string regex)
		{
			_regex = new Regex(regex, RegexOptions.CultureInvariant);
		}

		/// <summary>
		/// Send in filename without the path
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		internal bool IsMatch(string filename)
		{
			if (_isWildcard)
			{
				if (_wildcards == null || _wildcards.Length == 0)
					return true;

				foreach (var wildcard in _wildcards)
				{
					if (LikeOperator.LikeString(filename, wildcard, CompareMethod.Text))
						return true;
				}

				return false;
			}

			return _regex.IsMatch(filename);
		}
	}
}
