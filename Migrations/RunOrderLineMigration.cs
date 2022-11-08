using FreakyFashion.Models;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Migrations;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Migrations;
using Umbraco.Cms.Infrastructure.Migrations.Upgrade;

namespace FreakyFashion.Migrations
{
    public class RunOrderLineMigration : INotificationHandler<UmbracoApplicationStartingNotification>
    {
        private readonly IMigrationPlanExecutor _migrationPlanExecutor;
        private readonly IRuntimeState _runtimeState;
        private readonly ICoreScopeProvider _coreScopeProvider;
        private readonly IKeyValueService _keyValueService;

        public RunOrderLineMigration(IRuntimeState runtimeState,
                                              ICoreScopeProvider coreScopeProvider,
                                              IKeyValueService keyValueService,
                                              IMigrationPlanExecutor migrationPlanExecutor)
        {
            _runtimeState = runtimeState;
            _coreScopeProvider = coreScopeProvider;
            _keyValueService = keyValueService;
            _migrationPlanExecutor = migrationPlanExecutor;
        }

        public void Handle(UmbracoApplicationStartingNotification notification)
        {
            if (_runtimeState.Level < RuntimeLevel.Run) return;

            var migrationPlan = new MigrationPlan("MemberOrderLine");

            migrationPlan.From(string.Empty).To<AddOrderLineTable>("MemberOrderLine");

            var upgrader = new Upgrader(migrationPlan);
            upgrader.Execute(_migrationPlanExecutor, _coreScopeProvider, _keyValueService);
        }
    }
}