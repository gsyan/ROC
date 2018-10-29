

public static class SceneLoad
{
    private static string _nextScene = "";
    private static string _beforScene = "";
    public static string spawnPoint = "";//GameEscape 에서 사용함

    public static string nextScene
    {
        get
        {
            return _nextScene;
        }
        set
        {
            _beforScene = "";
            _nextScene = value;
        }
    }
}
