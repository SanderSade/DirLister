namespace Sander.DirLister.Core
{
	/// <summary>
	/// Interface to filename filtering.
	/// Allows custom implementations of filtering logic.
	/// </summary>
	public interface IFilter
	{
		/// <summary>
		///    Filename without the path
		/// </summary>
		/// <param name="filename"></param>
		/// <returns>Should be included to the list</returns>
		bool IsMatch(string filename);
	}
}
