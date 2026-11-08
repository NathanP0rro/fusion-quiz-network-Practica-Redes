using UnityEngine.Localization.Settings;

public static class UiText
{
    private const string TableName = "UI";

    public static string Get(string key, params object[] args)
    {
        return LocalizationSettings.StringDatabase.GetLocalizedString(TableName, key, args);
    }
}
