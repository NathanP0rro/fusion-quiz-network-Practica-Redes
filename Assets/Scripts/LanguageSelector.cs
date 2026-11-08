using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LanguageSelector : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;

    private const string PrefKey = "SelectedLocale";

    private IEnumerator Start()
    {
 
        yield return LocalizationSettings.InitializationOperation;

        var locales = LocalizationSettings.AvailableLocales.Locales;

        string saved = PlayerPrefs.GetString(PrefKey, string.Empty);
        if (!string.IsNullOrEmpty(saved))
        {
            foreach (Locale l in locales)
            {
                if (l.Identifier.Code == saved)
                {
                    LocalizationSettings.SelectedLocale = l;
                    break;
                }
            }
        }

        var options = new List<string>();
        int selectedIndex = 0;
        for (int i = 0; i < locales.Count; i++)
        {
            options.Add(GetDisplayName(locales[i]));
            if (locales[i] == LocalizationSettings.SelectedLocale)
                selectedIndex = i;
        }

        dropdown.ClearOptions();
        dropdown.AddOptions(options);
        dropdown.SetValueWithoutNotify(selectedIndex);
        dropdown.onValueChanged.AddListener(OnLanguageChanged);
    }

    private void OnLanguageChanged(int index)
    {
        var locales = LocalizationSettings.AvailableLocales.Locales;
        if (index < 0 || index >= locales.Count)
            return;

        LocalizationSettings.SelectedLocale = locales[index];
        PlayerPrefs.SetString(PrefKey, locales[index].Identifier.Code);
        PlayerPrefs.Save();
    }

    private static string GetDisplayName(Locale locale)
    {
        var culture = locale.Identifier.CultureInfo;
        if (culture == null)
            return locale.name;

        string name = culture.NativeName;
        return char.ToUpper(name[0]) + name.Substring(1);
    }
}
