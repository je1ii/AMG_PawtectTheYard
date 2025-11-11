using UnityEngine;

public class TimeController : MonoBehaviour
{
    public void SetTimeToNormal()
    {
        Time.timeScale = 1;
    }
    
    public void SetTimeToFast()
    {
        Time.timeScale = 2;
    }
    
    public void SetTimeToDoubleFast()
    {
        Time.timeScale = 4;
    }
}
