namespace Application.Common.Models
{
    public class PagingParams
    {
        private const int MaxPageSize = 50;
        private int _pageSize = 2;

        public int PageNumber = 1;
        public int Pagesize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }

}
