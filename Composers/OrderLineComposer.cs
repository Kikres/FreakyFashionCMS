using FreakyFashion.Migrations;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Notifications;

namespace FreakyFashion.Composers;

public class OrderLineComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        builder.AddNotificationHandler<UmbracoApplicationStartingNotification, RunOrderLineMigration>();
    }
}