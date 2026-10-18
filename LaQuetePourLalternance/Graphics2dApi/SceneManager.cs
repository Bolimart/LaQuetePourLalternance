namespace LaQuetePourLalternance.Graphics2dApi;

public class SceneManager
{
    private int _currentScene = 0;
    public int CurrentScene
    {
        get => _currentScene;
        set
        {
            _currentScene = value;
            GameEnvironement.Window.ChangeScene(Scenes[value]);
        }
    }
    public List<List<GameObject>> Scenes;

    public SceneManager(List<List<GameObject>> scenes, int currentScene = 0)
    {
        Scenes = scenes;
        CurrentScene = currentScene;
    }

    public void NextScene() => CurrentScene++;
    public void PreviousScene() => CurrentScene--;
    public void GotoScene(int index) => CurrentScene = index;
}

public interface ISceneInitializer
{
    public void LoadScene();

    public void UnloadScene();
}