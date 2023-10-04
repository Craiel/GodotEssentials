using Craiel.Essentials.Contracts;

namespace Craiel.Essentials.TweenLite;

using EngineCore;
using Enums;
using Godot;

public abstract class TweenLiteNode : ITicketData
{
    // -------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------
    protected TweenLiteNode()
    {
        this.Ticket = TweenLiteTicket.Next();
    }
    
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    
    // Fields for performance
    public float Duration;
    public TweenLiteEasingMode Easing;
    
    public TweenLiteTicket Ticket { get; }
    public bool IsFinished { get; protected set; }
    public double Runtime { get; protected set; }

    public void Update(double delta)
    {
        this.Runtime += delta;
        
        float percentage = TweenLiteSystem.ApplyEasing(this.Easing, delta, 0, 1, this.Duration);
        this.DoUpdate(percentage);

        if (this.Runtime >= this.Duration)
        {
            this.IsFinished = true;
            this.DoUpdate(1f);
            this.Finish();
        }
    }

    public abstract bool HasValidTarget();

    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    protected abstract void DoUpdate(float percentage);
    protected abstract void Finish();
}