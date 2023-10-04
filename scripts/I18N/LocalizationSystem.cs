using Craiel.Essentials.Contracts;

namespace Craiel.Essentials.I18N;

using System.Globalization;
using EngineCore;

public class LocalizationSystem : IGameModule
{
    private const float AutoSaveInterval = 5 * 60;
    
    private LocalizationProvider provider;

    private float lastAutoSave;
    
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public void Update(double delta)
    {
#if DEBUG
        if (EssentialCore.GameTime > this.lastAutoSave + AutoSaveInterval)
        {
            EssentialCore.Logger.Info($"Saving Localization: {this.provider.Root}");

            this.lastAutoSave = EssentialCore.GameTime;

            // TODO: Localization
            this.provider.SaveDictionary();
        }
#endif
    }

    public void Initialize()
    {
        this.provider = new LocalizationProvider();

#if DEBUG
        this.provider.SetRoot(EssentialCore.PersistentDataPath);
#endif
        
        Localization.Initialize(this.provider);
    }

    public void Load()
    {
        this.provider.ReloadDictionary();
    }

    public void Destroy()
    {
    }

    public void SetCulture(CultureInfo culture)
    {
        Localization.CurrentCulture = culture;
        this.provider.ReloadDictionary();
    }
}
