﻿using Serilog;
using Vint.Core.Server.Game.Protocol.Codecs.Buffer;
using Vint.Core.Server.Game.Protocol.Commands;
using Vint.Core.Utils;

namespace Vint.Core.Server.Game.Protocol.Codecs.Impl;

public class CommandCodec : Codec {
    ILogger Logger { get; } = Log.Logger.ForType<CommandCodec>();
    Dictionary<Type, CommandCode> CommandToCode { get; } = new();
    Dictionary<CommandCode, Type> CodeToCommand { get; } = new();

    public override void Encode(ProtocolBuffer buffer, object value) {
        Type type = value.GetType();
        CommandCode code = CommandToCode[type];

        Protocol
            .GetCodec(new TypeCodecInfo(typeof(CommandCode)))
            .Encode(buffer, code);

        Protocol
            .GetCodec(new TypeCodecInfo(type))
            .Encode(buffer, value);
    }

    public override object Decode(ProtocolBuffer buffer) {
        CommandCode code = (CommandCode)Protocol
            .GetCodec(new TypeCodecInfo(typeof(CommandCode)))
            .Decode(buffer);

        Type type = CodeToCommand[code];

        //Logger.Debug("Decoding command of type {Type}", code);

        return Protocol
            .GetCodec(new TypeCodecInfo(type))
            .Decode(buffer);
    }

    public CommandCodec Register<T>(CommandCode code) where T : ICommand {
        CommandToCode[typeof(T)] = code;
        CodeToCommand[code] = typeof(T);

        return this;
    }
}
