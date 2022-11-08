using Umbraco.Cms.Core.Models.PublishedContent;

namespace FreakyFashion.Models.ViewModels;

public class ComposedViewModel<TPage, TViewModel> where TPage : PublishedContentModel
{
    public TPage Model { get; set; }
    public TViewModel ViewModel { get; set; }
}