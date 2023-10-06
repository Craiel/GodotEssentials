namespace Craiel.Essentials.GameData;

using System;
using Contracts;

[Serializable]
public class GameDataRefBase
{
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public uint Id;

    public bool IsValid()
    {
        return this.Id != GameDataId.InvalidId;
    }

    public void Reset()
    {
        this.Id = GameDataId.InvalidId;
    }
    
    public bool Validate(object owner, IGameDataValidationContext context, bool isOptional = true, bool warnIfMissing = false)
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