namespace Craiel.Essentials.Nodes;

using Godot;

public partial class DebugOnlyVisibleNode : Control
{
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public override void _EnterTree()
    {
        base._EnterTree();
        
#if DEBUG
        this.Show();
#else
        this.Hide();
#endif
    }
}