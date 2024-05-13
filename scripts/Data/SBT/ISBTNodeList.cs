namespace Craiel.Essentials.Data.SBT;

using Enums;

public interface ISBTNodeList : ISBTNode, ISBTNodeCollection
{
    public ISBTNode AddEntry(SBTType type, object? data = null, SBTFlags flags = SBTFlags.None, string? note = null);
}