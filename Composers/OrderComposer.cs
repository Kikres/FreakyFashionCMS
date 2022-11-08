using FreakyFashion.Migrations;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Notifications;

namespace FreakyFashion.Composers;

public class OrderComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        builder.AddNotificationHandler<UmbracoApplicationStartingNotification, RunOrderMigration>();
    }
}