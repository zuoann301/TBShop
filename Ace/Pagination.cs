using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace
{
    /// <summary>
    /// 分页信息
    /// </summary>
    public class Pagination
    {
        int _page = 1;
        int _pageSize = 20;
        public Pagination()
        {
        }
        public Pagination(int page, int pageSize)
        {
            this.Page = page;
            this.PageSize = pageSize;
        }
        /// <summary>
        /// 当前页
        /// </summary>
        public int Page { get { return this._page; } set { this._page = value; } }
        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize { get { return this._pageSize; } set { this._pageSize = value; } }

        public PagedData ToPagedData()
        {
            PagedData pageData = new PagedData(this);
            return pageData;
        }
        public PagedData<T> ToPagedData<T>()
        {
            PagedData<T> pageData = new PagedData<T>(this);
            return pageData;
        }
    }
}
