namespace Craiel.Essentials;

using System;
using Contracts;

[Serializable]
public class GameDataRuntimeRefBase
{
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public string RefGuid;

    public bool IsValid()
    {
        return !string.IsNullOrEmpty(this.RefGuid);
    }

    public void Reset()
    {
        this.RefGuid = null;
    }
    
    public bool Validate(object owner, IGameDataRuntimeValidationContext context, bool isOptional = true, bool warnIfMissing = false)
    {
        if (!this.IsValid())
        {
            if (!isOptional)
            {
                context.ErrorFormat(owner, this, null, "Static Data Ref is not optional: {0}", this.GetType().Name);
                return false;
            }

            if (warnIfMissing)
            {
                context.WarningFormat(owner, this, null, "Static Data Ref is recommended to be set: {0}", this.GetType().Name);
                return false;
            }

            return false;
        }

        return true;
    }
}