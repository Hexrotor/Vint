using System.Data;
using FluentMigrator;
using Vint.Core.Utils;

namespace Vint.Core.Database.Migrations;

[Migration(20250306)]
public class AddDailyRewards : Migration {
    const string DailyBonusCycle = "DailyBonusCycle";
    const string DailyBonusZone = "DailyBonusZone";
    const string LastDailyBonusReceivingTime = "LastDailyBonusReceivingTime";

    public override void Up() {
        Alter.Table(DbConstants.Players)
            .AddColumn(LastDailyBonusReceivingTime).AsDateTime().NotNullable()
            .AddColumn(DailyBonusCycle).AsInt32().NotNullable().WithDefaultValue(0)
            .AddColumn(DailyBonusZone).AsInt32().NotNullable().WithDefaultValue(0);

        Create.Table(DbConstants.DailyBonusRedemptions)
            .WithColumn(DbConstants.Id).AsInt64().NotNullable()
            .WithColumn(DbConstants.PlayerId).AsInt64().NotNullable()
            .WithColumn("Code").AsInt32().NotNullable()
            .WithColumn("RedeemedAt").AsDateTime().NotNullable()
            .WithColumn("Zone").AsInt32().NotNullable()
            .WithColumn("Cycle").AsInt32().NotNullable();

        Create.Table(DbConstants.Details)
            .WithColumn(DbConstants.Id).AsInt64().NotNullable()
            .WithColumn(DbConstants.PlayerId).AsInt64().NotNullable()
            .WithColumn("Count").AsInt32().NotNullable();

        Create.PrimaryKey().OnTable(DbConstants.DailyBonusRedemptions)
            .Columns(DbConstants.Id, "Code", DbConstants.PlayerId);

        Create.PrimaryKey().OnTable(DbConstants.Details)
            .Columns(DbConstants.Id, DbConstants.PlayerId);

        Create.ForeignKey()
            .FromTable(DbConstants.DailyBonusRedemptions).ForeignColumn(DbConstants.PlayerId)
            .ToTable(DbConstants.Players).PrimaryColumn(DbConstants.Id)
            .OnDelete(Rule.Cascade);

        Create.ForeignKey()
            .FromTable(DbConstants.Details).ForeignColumn(DbConstants.PlayerId)
            .ToTable(DbConstants.Players).PrimaryColumn(DbConstants.Id)
            .OnDelete(Rule.Cascade);

        Alter.Column(DbConstants.Id)
            .OnTable(DbConstants.DailyBonusRedemptions)
            .AsInt64().Identity().NotNullable();
    }

    public override void Down() {
        Delete.Table(DbConstants.Details);
        Delete.Table(DbConstants.DailyBonusRedemptions);

        Delete.Column(DailyBonusZone).FromTable(DbConstants.Players);
        Delete.Column(DailyBonusCycle).FromTable(DbConstants.Players);
        Delete.Column(LastDailyBonusReceivingTime).FromTable(DbConstants.Players);
    }
}
