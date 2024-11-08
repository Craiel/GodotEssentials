namespace Craiel.Essentials.DB;

using CanineJRPG.Core;
using Godot;

public partial class GameDatabaseLinkNode : Node
{
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public StringGameDataId Id { get; private set; }
    
    public override void _EnterTree()
    {
        base._EnterTree();

        var idLink = this.GetChild<GameDatabaseStringGameDataIdLinkNode>(0);
        this.Id = new StringGameDataId(idLink.Id, idLink.Type);
    }
}