namespace Craiel.Essentials.Commands;

using System;
using Contracts;
using Utils;

public static class PayloadExtensions
{
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public static T GetTyped<T>(this IGameCommandPayload payload)
        where T: IGameCommandPayload
    {
        if (payload is not T typed)
        {
            throw new InvalidOperationException($"Command payload type invalid, expected {TypeDef<T>.Value} but was {payload?.GetType()}");
        }

        return typed;
    }
}