namespace Craiel.Essentials.Commands;

using Craiel.Essentials.Contracts;

public struct BoolPayload : IGameCommandPayload
{
// -------------------------------------------------------------------
// Constructor
// -------------------------------------------------------------------
    public BoolPayload(bool value)
    {
        this.Value = value;
    }

// -------------------------------------------------------------------
// Public
// -------------------------------------------------------------------
    public bool Value;
}