namespace Craiel.Essentials.I18N;

using System;
using System.Collections.Generic;
using System.Globalization;
using Enums;
using Resource;

public static class LocaleConstants
{
    private static readonly IDictionary<CultureInfo, ResourceKey> LocalizationMasterFiles = new Dictionary<CultureInfo, ResourceKey>();

    private static readonly ResourceKey LocalizationFallbackMasterFile = ResourceKey.Invalid;
    
    // -------------------------------------------------------------------
    // Public
    // -------------------------------------------------------------------
    public static readonly CultureInfo LocaleEnglish = new CultureInfo("en-US");
    public static readonly CultureInfo LocaleFrench = new CultureInfo("fr-FR");
    public static readonly CultureInfo LocaleGerman = new CultureInfo("de-DE");
    public static readonly CultureInfo LocaleSpanish = new CultureInfo("es-ES");
    public static readonly CultureInfo LocalePortuguese = new CultureInfo("pt-PT");
    public static readonly CultureInfo LocaleRussian = new CultureInfo("ru-RU");
    
    public static ResourceKey GetLocalizationMasterFile(CultureInfo culture)
    {
        ResourceKey key;
        if(LocalizationMasterFiles.TryGetValue(culture, out key))
        {
            return key;
        }

        if (LocalizationFallbackMasterFile == ResourceKey.Invalid)
        {
            throw new InvalidOperationException();
        }

        return LocalizationFallbackMasterFile;
    }
    
    public static readonly CultureInfo[] AllCultures = {
        LocaleEnglish,
        LocaleFrench, 
        LocaleGerman, 
        LocaleSpanish,
        LocalePortuguese,
        LocaleRussian
    };
    
    public static CultureInfo GetCulture(GameLanguage language)
    {
        switch (language)
        {
            case GameLanguage.English:
            {
                return LocaleEnglish;
            }

            case GameLanguage.French:
            {
                return LocaleFrench;
            }

            case GameLanguage.German:
            {
                return LocaleGerman;
            }

            case GameLanguage.Spanish:
            {
                return LocaleSpanish;
            }

            case GameLanguage.Portugese:
            {
                return LocalePortuguese;
            }

            case GameLanguage.Russian:
            {
                return LocaleRussian;
            }

            default:
            {
                return LocaleEnglish;
            }
        }
    }
}
