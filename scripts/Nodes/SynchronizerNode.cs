namespace Craiel.Essentials.Nodes;

using System.Collections.Generic;
using Godot;

public partial class SynchronizerNode : SingletonNode<SynchronizerNode>
{
    private readonly IList<ISynchronizedNode> nodes = new List<ISynchronizedNode>();

    private bool resetRequired;
    
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public void Register(ISynchronizedNode node)
    {
        this.nodes.Add(node);
        this.resetRequired = true;
    }

    public void Unregister(ISynchronizedNode node)
    {
        this.nodes.Remove(node);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        for (var i = 0; i < this.nodes.Count; i++)
        {
            if (this.resetRequired)
            {
                this.nodes[i]._ResetSynchronized();
            }
            
            this.nodes[i]._ProcessSynchronized(delta);
        }
        
        this.resetRequired = false;
    }
}