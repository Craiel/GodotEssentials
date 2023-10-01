namespace Craiel.Essentials.Runtime.I18N;

using System.Globalization;
using Godot;
using IO;
using Singletons;

public class LocalizationSystem : GodotSingleton<LocalizationSystem>
{
    private const float AutoSaveInterval = 5 * 60;
    
    private LocalizationProvider provider;

    private float lastAutoSave;
    
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public void Update()
    {
#if DEBUG
        if (EssentialsCore.GameTime > this.lastAutoSave + AutoSaveInterval)
        {
            EssentialsCore.Logger.Info($"Saving Localization: {this.provider.Root}");

            this.lastAutoSave = EssentialsCore.GameTime;

            // TODO: Localization
            this.provider.SaveDictionary();
        }
#endif
    }

    public override void Initialize()
    {
        base.Initialize();

        this.provider = new LocalizationProvider();

#if DEBUG
        this.provider.SetRoot(EssentialsCore.PersistentDataPath);
#endif

        Localization.Initialize(this.provider);
    }

    public void SetCulture(CultureInfo culture)
    {
        Localization.CurrentCulture = culture;
        this.provider.ReloadDictionary();
    }
}
