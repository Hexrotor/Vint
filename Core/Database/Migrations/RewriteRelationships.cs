using System.Data;
using FluentMigrator;

namespace Vint.Core.Database.Migrations;

[Migration(20250308)]
public class RewriteRelationships : Migration {
    #region Constants
    const string UserId = "UserId";
    const string FriendId = "FriendId";

    const string BlockerId = "BlockerId";
    const string BlockedId = "BlockedId";

    const string ReporterId = "ReporterId";
    const string ReportedId = "ReportedId";

    const string CreatedAt = "CreatedAt";

    const byte NoneType = 0;
    const byte BlockedType = 1;
    const byte ReportedType = 2;
    const byte FriendType = 4;
    const byte IncomingRequestType = 8;
    const byte OutgoingRequestType = 16;
    #endregion

    public override void Up() {
        #region CreateNewTables
        Create.Table(DbConstants.Blocks)
            .WithColumn(BlockerId).AsInt64().NotNullable()
            .WithColumn(BlockedId).AsInt64().NotNullable()
            .WithColumn(CreatedAt).AsDateTime().NotNullable();

        Create.Table(DbConstants.Reports)
            .WithColumn(ReporterId).AsInt64().NotNullable()
            .WithColumn(ReportedId).AsInt64().NotNullable()
            .WithColumn("InteractionSource").AsByte().NotNullable()
            .WithColumn(CreatedAt).AsDateTime().NotNullable();

        Create.Table(DbConstants.Friends)
            .WithColumn(UserId).AsInt64().NotNullable()
            .WithColumn(FriendId).AsInt64().NotNullable()
            .WithColumn("RequestedAt").AsDateTime().NotNullable()
            .WithColumn("AcceptedAt").AsDateTime().NotNullable();

        Create.Table(DbConstants.FriendRequests)
            .WithColumn(DbConstants.Id).AsInt32().PrimaryKey().Identity().NotNullable()
            .WithColumn("SenderId").AsInt64().NotNullable()
            .WithColumn(FriendId).AsInt64().NotNullable()
            .WithColumn(CreatedAt).AsDateTime().NotNullable();
        #endregion

        #region CreateConstraints
        Create.PrimaryKey().OnTable(DbConstants.Blocks)
            .Columns(BlockerId, BlockedId);

        Create.PrimaryKey().OnTable(DbConstants.Reports)
            .Columns(ReporterId, ReportedId);

        Create.PrimaryKey().OnTable(DbConstants.Friends)
            .Columns(UserId, FriendId);

        Create.UniqueConstraint().OnTable(DbConstants.FriendRequests)
            .Columns("SenderId", FriendId);
        #endregion

        #region CreateForeignKeys
        Create.ForeignKey()
            .FromTable(DbConstants.Blocks).ForeignColumn(BlockerId)
            .ToTable(DbConstants.Players).PrimaryColumn(DbConstants.Id)
            .OnDelete(Rule.Cascade);

        Create.ForeignKey()
            .FromTable(DbConstants.Blocks).ForeignColumn(BlockedId)
            .ToTable(DbConstants.Players).PrimaryColumn(DbConstants.Id)
            .OnDelete(Rule.Cascade);

        Create.ForeignKey()
            .FromTable(DbConstants.Reports).ForeignColumn(ReporterId)
            .ToTable(DbConstants.Players).PrimaryColumn(DbConstants.Id)
            .OnDelete(Rule.Cascade);

        Create.ForeignKey()
            .FromTable(DbConstants.Reports).ForeignColumn(ReportedId)
            .ToTable(DbConstants.Players).PrimaryColumn(DbConstants.Id)
            .OnDelete(Rule.Cascade);

        Create.ForeignKey()
            .FromTable(DbConstants.Friends).ForeignColumn(UserId)
            .ToTable(DbConstants.Players).PrimaryColumn(DbConstants.Id)
            .OnDelete(Rule.Cascade);

        Create.ForeignKey()
            .FromTable(DbConstants.Friends).ForeignColumn(FriendId)
            .ToTable(DbConstants.Players).PrimaryColumn(DbConstants.Id)
            .OnDelete(Rule.Cascade);

        Create.ForeignKey()
            .FromTable(DbConstants.FriendRequests).ForeignColumn("SenderId")
            .ToTable(DbConstants.Players).PrimaryColumn(DbConstants.Id)
            .OnDelete(Rule.Cascade);

        Create.ForeignKey()
            .FromTable(DbConstants.FriendRequests).ForeignColumn(FriendId)
            .ToTable(DbConstants.Players).PrimaryColumn(DbConstants.Id)
            .OnDelete(Rule.Cascade);
        #endregion

        #region TransformData
        Execute.Sql($"""
            INSERT INTO {DbConstants.Blocks} (BlockerId, BlockedId, CreatedAt)
            SELECT SourcePlayerId, TargetPlayerId, SYSDATE()
            FROM Relations
            WHERE ((Types & {BlockedType}) = {BlockedType});

            INSERT INTO {DbConstants.Reports} (ReporterId, ReportedId, InteractionSource, CreatedAt)
            SELECT SourcePlayerId, TargetPlayerId, 0, SYSDATE()
            FROM Relations
            WHERE ((Types & {ReportedType}) = {ReportedType});

            INSERT INTO {DbConstants.Friends} (UserId, FriendId, RequestedAt, AcceptedAt)
            SELECT SourcePlayerId, TargetPlayerId, SYSDATE(), SYSDATE()
            FROM Relations
            WHERE ((Types & {FriendType}) = {FriendType});

            INSERT INTO {DbConstants.FriendRequests} (SenderId, FriendId, CreatedAt)
            SELECT SourcePlayerId, TargetPlayerId, SYSDATE()
            FROM Relations
            WHERE ((Types & {OutgoingRequestType}) = {OutgoingRequestType});
        """);
        #endregion

        #region DeleteOldTable
        Delete.Table(DbConstants.Relations);
        #endregion
    }

    public override void Down() {
        #region CreateOldTable
        Create.Table(DbConstants.Relations)
            .WithColumn("SourcePlayerId").AsInt64().NotNullable()
            .WithColumn("TargetPlayerId").AsInt64().NotNullable()
            .WithColumn("Types").AsInt32().NotNullable();
        #endregion

        #region CreateConstraints
        Create.PrimaryKey().OnTable(DbConstants.Relations)
            .Columns("SourcePlayerId", "TargetPlayerId");
        #endregion

        #region CreateForeignKeys
        Create.ForeignKey()
            .FromTable(DbConstants.Relations).ForeignColumn("SourcePlayerId")
            .ToTable(DbConstants.Players).PrimaryColumn(DbConstants.Id)
            .OnDelete(Rule.Cascade);

        Create.ForeignKey()
            .FromTable(DbConstants.Relations).ForeignColumn("TargetPlayerId")
            .ToTable(DbConstants.Players).PrimaryColumn(DbConstants.Id)
            .OnDelete(Rule.Cascade);
        #endregion

        #region TransformData
        Execute.Sql($"""
            INSERT INTO Relations (SourcePlayerId, TargetPlayerId, Types)
            SELECT BlockerId, BlockedId, {BlockedType}
            FROM {DbConstants.Blocks};

            INSERT INTO Relations (SourcePlayerId, TargetPlayerId, Types)
            SELECT ReporterId, ReportedId, {ReportedType}
            FROM {DbConstants.Reports};

            INSERT INTO Relations (SourcePlayerId, TargetPlayerId, Types)
            SELECT UserId, FriendId, {FriendType}
            FROM {DbConstants.Friends};

            INSERT INTO Relations (SourcePlayerId, TargetPlayerId, Types)
            SELECT FriendId, SenderId, {IncomingRequestType}
            FROM {DbConstants.FriendRequests};

            INSERT INTO Relations (SourcePlayerId, TargetPlayerId, Types)
            SELECT SenderId, FriendId, {OutgoingRequestType}
            FROM {DbConstants.FriendRequests};
        """);
        #endregion

        #region DeleteNewTables
        Delete.Table(DbConstants.FriendRequests);
        Delete.Table(DbConstants.Friends);
        Delete.Table(DbConstants.Reports);
        Delete.Table(DbConstants.Blocks);
        #endregion
    }
}
