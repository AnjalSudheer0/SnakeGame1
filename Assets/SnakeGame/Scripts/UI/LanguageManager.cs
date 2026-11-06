
using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LanguageManager : MonoBehaviour
{
    [Header("Language UI")]
    [SerializeField] private GameObject languagePanel;

    public void OpenLanguagePanel()
    {
        languagePanel.SetActive(true);
    }

    public void CloseLanguagePanel()
    {
        languagePanel.SetActive(false);
    }

    public void SelectEnglish()
    {
        ChangeLanguage("en-IN");
    }

    public void SelectMalayalam()
    {
        ChangeLanguage("ml-IN");
    }

    private void ChangeLanguage(string localeCode)
    {
        StartCoroutine(ChangeLanguageRoutine(localeCode));
    }

    private IEnumerator ChangeLanguageRoutine(string localeCode)
    {
        yield return LocalizationSettings.InitializationOperation;

        Locale selectedLocale =
            LocalizationSettings.AvailableLocales.GetLocale(
                new LocaleIdentifier(localeCode)
            );

        if (selectedLocale == null)
        {
            Debug.LogError("Language not found: " + localeCode);
            yield break;
        }

        LocalizationSettings.SelectedLocale = selectedLocale;

        languagePanel.SetActive(false);

        Debug.Log("Language changed: " + localeCode);
    }
}
