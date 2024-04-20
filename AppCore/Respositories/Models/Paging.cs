using System;
namespace TestsService.AppCore.Respositories.Models
{
	public class Paging
	{
		public int CurrentPage { get; set; }

		public int PageSize { get; set;}

        public long TotalRecords { get; set; }
    }
}

