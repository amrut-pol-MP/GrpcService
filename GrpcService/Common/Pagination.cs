namespace GrpcService.Common
{
    public class Pagination
    {
        public int Total { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

        public Pagination(int total, int page, int pageSize)
        {
            Total = total;
            Page = page;
            PageSize = pageSize;
        }
    }
}
