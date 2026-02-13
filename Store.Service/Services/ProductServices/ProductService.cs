using AutoMapper;
using Store.Data.Entites;
using Store.Repository.Interfaces;
using Store.Repository.Specefication.ProductSpecs;
using Store.Service.Helper;
using Store.Service.Services.ProductServices.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductEntity = Store.Data.Entites.Product;

namespace Store.Service.Services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
           _mapper = mapper;
        }
        public async Task<IReadOnlyList<BrandTypeDetailsDto>> GetAllBrandsAsync()
        {
            var brands = await _unitOfWork.Repository<ProductBrand, int>().GetAllNoTrackingAsync();
            var mappedbrands = _mapper.Map<IReadOnlyList<BrandTypeDetailsDto>>(brands);
          
            return mappedbrands;
        }

        public async Task<PaginatedResultDto<ProductDetailsDto>> GetAllProductsAsync(ProductSpecefication input)
        {
            var specs = new ProductWithSpecefications(input);
            var products = await _unitOfWork.Repository<ProductEntity, int>().GetAllwithspecificationAsync(specs);
           var countspecs = new ProductWithCountSpecefication(input);
            var count = await _unitOfWork.Repository<ProductEntity, int>().GetCountwithSpecificationAsync(countspecs);
            
            var mappedproducts = _mapper.Map<IReadOnlyList<ProductDetailsDto>>(products);
            return new PaginatedResultDto<ProductDetailsDto>(input.PageIndex, input.PageSize, count, mappedproducts);
        }

        public async Task<IReadOnlyList<BrandTypeDetailsDto>> GetAllTypesAsync()
        {
            var types = await _unitOfWork.Repository<ProductType, int>().GetAllNoTrackingAsync();
            var mappedtypes =  _mapper.Map<IReadOnlyList<BrandTypeDetailsDto>>(types);
            return mappedtypes;
        }

        public async Task<ProductDetailsDto> GetProductByIdAsync(int? ProductId)
        {
            if (ProductId is null) throw new Exception("Id is null");
            var specs = new ProductWithSpecefications(ProductId);
            var product = await _unitOfWork.Repository<Product, int>().GetBySpecificatiobIdAsync(specs);
            if (product is null) throw new Exception("Product not found");
            var mappedproduct = _mapper.Map<ProductDetailsDto>(product);
            return mappedproduct;
        }
    }
}
