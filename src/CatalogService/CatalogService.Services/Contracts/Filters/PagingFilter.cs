namespace CatalogService.Services.Contracts.Filters
{
    public abstract class PagingFilter
    {
        public int? Take { get; set; }

        public int? Skip { get; set; }
    }
}