using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace
{
    public class PagedData : PagedData<object>
    {
        public PagedData()
            : base()
        {
        }
        public PagedData(List<object> models)
            : base(models, 0)
        {
        }
        public PagedData(List<object> models, int totalCount)
            : base(models, totalCount, 0, 0)
        {
        }
        public PagedData(List<object> models, int totalCount, int currentPage, int pageSize)
            : base(models, totalCount, currentPage, pageSize)
        {
        }

        public PagedData(Pagination paging)
            : base(paging)
        {
        }
        public PagedData(int currentPage, int pageSize)
            : base(currentPage, pageSize)
        {
        }
        public PagedData(int totalCount, int currentPage, int pageSize)
            : base(new List<object>(), totalCount, currentPage, pageSize)
        {
        }
    }

    public class PagedData<T>
    {
        public PagedData()
            : this(new List<T>())
        {
        }
        public PagedData(List<T> models)
            : this(models, 0)
        {
        }
        public PagedData(List<T> models, int totalCount)
            : this(models, totalCount, 0, 0)
        {
        }
        public PagedData(List<T> models, int totalCount, int currentPage, int pageSize)
        {
            this.Models = models;
            this.TotalCount = totalCount;
            this.CurrentPage = currentPage;
            this.PageSize = pageSize;
        }

        public PagedData(Pagination paging)
            : this(paging.Page, paging.PageSize)
        {
        }
        public PagedData(int currentPage, int pageSize)
            : this(new List<T>(), 0, currentPage, pageSize)
        {
        }
        public PagedData(int totalCount, int currentPage, int pageSize)
            : this(new List<T>(), totalCount, currentPage, pageSize)
        {
        }

        public int TotalCount { get; set; }
        public int TotalPage
        {
            get
            {
                if (this.TotalCount > 0)
                {
                    return this.TotalCount % this.PageSize == 0 ? this.TotalCount / this.PageSize : this.TotalCount / this.PageSize + 1;
                }
                else
                {
                    return 0;
                }
            }
            set
            {

            }
        }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public List<T> Models { get; set; }
    }
}
