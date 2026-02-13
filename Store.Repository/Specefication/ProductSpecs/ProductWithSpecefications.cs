using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Store.Data.Entites;

namespace Store.Repository.Specefication.ProductSpecs
{
    public class ProductWithSpecefications : BaseSpecefication<Product>
    {
        public ProductWithSpecefications(ProductSpecefication specs) : base(product => (!specs.BrandId.HasValue || product.BrandId == specs.BrandId.Value) &&
                                                                           (!specs.TypeId.HasValue || product.TypeId == specs.TypeId.Value) &&
                                                                           (string.IsNullOrEmpty(specs.Search) || EF.Functions.Like(product.Name.ToLower(), $"%{specs.Search}%"))
        )
        {
            AddInclude(x => x.Brand);
            AddInclude(x => x.Type);
            AddOrderBy(x => x.Name);
            ApplyPagination(specs.PageSize * (specs.PageIndex - 1), specs.PageSize);




            if (!string.IsNullOrEmpty(specs.Sort))
            {
                switch (specs.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(p => p.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDescending(p => p.Price);
                        break;
                        default:
                        AddOrderBy(n => n.Name);
                        break;

                }
            }
        }
        public ProductWithSpecefications(int? id) : base(product => product.Id == id)
                                                                                      
        {
            AddInclude(x => x.Brand);
            AddInclude(x => x.Type);
         
        }
    }
}
