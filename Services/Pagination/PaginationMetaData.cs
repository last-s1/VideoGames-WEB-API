namespace VideoGamesAPI.Services
{
	/// <summary>
	/// Класс для хранения метаданных пагинации
	/// </summary>
	/// <typeparam name="T"></typeparam>
    public class PaginationMetaData<T>
    {
		public int CurrentPage { get; private set; }
		public int TotalPages { get; private set; }
		public int PageSize { get; private set; }
		public int TotalCount { get; private set; }
		public bool HasPrevious { get; private set; }
		public bool HasNext { get; private set; }

		public PaginationMetaData(PagedList<T> pagedList)
        {
			CurrentPage = pagedList.CurrentPage;
			TotalPages = pagedList.TotalPages;
			PageSize = pagedList.PageSize;
			TotalCount = pagedList.TotalCount;
			HasPrevious = pagedList.HasPrevious;
			HasNext = pagedList.HasNext;
        }
	}
}
