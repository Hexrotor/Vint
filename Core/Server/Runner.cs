using FluentMigrator.Runner;
using Vint.Core.Server.API;
using Vint.Core.Server.Game;
using Vint.Core.Server.Static;

namespace Vint.Core.Server;

public class Runner(
    ApiServer apiServer,
    StaticServer staticServer,
    GameServer gameServer,
    IMigrationRunner migrationRunner
) {
    public async Task Run() {
        migrationRunner.MigrateUp();

        await await Task.WhenAny( // Task.WhenAny returns Task<Task>, so we need to await both tasks
            staticServer.Start(),
            apiServer.Start(),
            gameServer.Start()
        );
    }
}
