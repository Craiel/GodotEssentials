namespace Craiel.Essentials.DB;

using CanineJRPG.Core;
using Godot;

[GlobalClass]
public partial class GameDatabaseLinkNode : Resource
{
    private StringGameDataId dataId = StringGameDataId.Unset;
    
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    [Export] public string Id;
    [Export] public GameDataType Type;
    
    public StringGameDataId GetId()
    {
        if (this.dataId == StringGameDataId.Unset)
        {
            this.dataId = new StringGameDataId(this.Id, this.Type);
        }

        return this.dataId;
    }
}