using NPoco;
using Umbraco.Cms.Infrastructure.Migrations;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;

namespace FreakyFashion.Migrations;

public class AddOrderTable : MigrationBase
{
    public AddOrderTable(IMigrationContext context) : base(context)
    {
    }

    protected override void Migrate()
    {
        Logger.LogDebug("Running Migration {MigrationStep}", "AddOrderTable");

        if (!TableExists("MemberOrder"))
        {
            Create.Table<OrderSchema>().Do();
        }
        else
        {
            Logger.LogDebug("Table {DbTable} already exists, skipping", "MemberOrder");
        }
    }

    [TableName("MemberOrder")]
    [PrimaryKey("Id", AutoIncrement = true)]
    [ExplicitColumns]
    public class OrderSchema
    {
        [PrimaryKeyColumn(AutoIncrement = true, IdentitySeed = 1)]
        [Column("Id")]
        public int Id { get; set; }

        [Column("UmbracoMemberId")]
        public int UmbracoMemberId { get; set; }

        [Column("OrderDate")]
        public DateTime OrderDate { get; set; }
    }
}