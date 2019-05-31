using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoad : LevelManager
{
   

    public override void Awake()
    {
        base.Awake();
        if (!GameManager.Instance) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

    }
    public override void Update()
    {
        base.Update();

    }


}
