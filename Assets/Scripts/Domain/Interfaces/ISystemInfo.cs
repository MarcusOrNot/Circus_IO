public interface ISystemInfo
{
    public PlatformType GetPlatformType();
    public ControlType GetControlType();
    public LangType GetSystemLang(LangType defaultLanguage);
    public SystemPrefsModel GetSystemPrefs();
}
