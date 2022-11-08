using FreakyFashion.Models;
using NPoco;
using Umbraco.Cms.Infrastructure.Migrations;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;

namespace FreakyFashion.Migrations
{
    public class AddOrderLineTable : MigrationBase
    {
        public AddOrderLineTable(IMigrationContext context) : base(context)
        {
        }

        protected override void Migrate()
        {
            Logger.LogDebug("Running Migration {MigrationStep}", "AddOrderLineTable");

            if (!TableExists("MemberOrderLine"))
            {
                Create.Table<OrderLineSchema>().Do();
            }
            else
            {
                Logger.LogDebug("Table {DbTable} already exists, skipping", "MemberOrderLine");
            }
        }

        [TableName("MemberOrderLine")]
        [PrimaryKey("Id", AutoIncrement = true)]
        [ExplicitColumns]
        public class OrderLineSchema
        {
            [PrimaryKeyColumn(AutoIncrement = true, IdentitySeed = 1)]
            [Column("Id")]
            public int Id { get; set; }

            [Column("OrderId")]
            public int OrderId { get; set; }

            [Column("ProductId")]
            public int ProductId { get; set; }

            [Column("Quantity")]
            public int Quantity { get; set; }

            [Column("Price")]
            public double Price { get; set; }
        }
    }
}