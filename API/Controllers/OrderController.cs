﻿using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace FreakyFashion.API.Controllers;

[Route("api/[controller]")]
public class OrderController : UmbracoApiController
{
    private readonly IContentService _contentService;
    private readonly IUmbracoContextAccessor _umbracoContextAccessor;
    private readonly IMediaService _mediaService;

    public OrderController(IContentService contentService, IMediaService mediaService, IUmbracoContextAccessor umbracoContextAccessor)
    {
        _contentService = contentService;
        _mediaService = mediaService;
        _umbracoContextAccessor = umbracoContextAccessor;
    }

    [HttpPost]
    public IActionResult Create(ProductDto productDto)
    {
        var content = _contentService.Create(productDto.Title, GetProductList().Key, ProductPage.ModelTypeAlias);
        var image = _mediaService.GetById(productDto.ImageId ?? 1063);

        content.SetValue(nameof(ISEoproperties.Title), productDto.SeoTitle);
        content.SetValue(nameof(ISEoproperties.Description), productDto.SeoDescription);
        content.SetValue(nameof(ProductPage.ProductTitle), productDto.Title);
        content.SetValue(nameof(ProductPage.ProductDescription), productDto.Description);
        content.SetValue(nameof(ProductPage.ProductPrice), productDto.Price);
        content.SetValue(nameof(ProductPage.ProductImage), Udi.Create(Constants.UdiEntityType.Media, image.Key));

        _contentService.SaveAndPublish(content);

        return Created("", null);
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var productPageList = GetProductList().Children<ProductPage>();
        if (!productPageList.Any()) return NoContent();

        var productList = productPageList.Select(o => new ProductDto
        {
            Id = o.Id,
            Title = o.ProductTitle,
            Description = o.ProductDescription,
            Price = o.ProductPrice,
            ImageId = o.ProductImage.Id
        });

        return Ok(productList);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var productPage = GetProductList().Children<ProductPage>().Where(o => o.Id == id).FirstOrDefault();
        if (productPage == null) return NotFound();

        return Ok(MapProduct(productPage));
    }

    private ProductDto MapProduct(ProductPage productPage)
    {
        var product = new ProductDto
        {
            Id = productPage.Id,
            Title = productPage.ProductTitle,
            Description = productPage.ProductDescription,
            Price = productPage.ProductPrice,
            ImageId = productPage.ProductImage.Id
        };

        return product;
    }

    private ProductsList? GetProductList()
    {
        if (_umbracoContextAccessor.TryGetUmbracoContext(out IUmbracoContext context))
        {
            return context.Content.GetAtRoot().DescendantsOrSelf<ProductsList>().FirstOrDefault();
        }
        return null;
    }
}

public class ProductDto
{
    public string SeoTitle { get; set; }
    public string SeoDescription { get; set; }
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public int? ImageId { get; set; }
}