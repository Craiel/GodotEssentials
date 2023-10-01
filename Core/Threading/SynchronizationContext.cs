namespace Craiel.Essentials.Runtime.Threading;

using System.Threading;

public class SynchronizationContext : System.Threading.SynchronizationContext
{
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public override void Post(SendOrPostCallback d, object state)
    {
        EssentialCore.Synchronization.InvokeLater(() => { d.Invoke(state); });
    }

    public override void Send(SendOrPostCallback d, object state)
    {
        EssentialCore.Synchronization.InvokeLater(() => { d.Invoke(state); });
    }
}