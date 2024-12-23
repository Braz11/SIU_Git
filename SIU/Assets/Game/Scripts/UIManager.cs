using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] List<GameObject> screens = new List<GameObject>();

    private void Awake() {
        EventsManager.OnShowScreen += ShowScreen;
    }

    private void OnDestroy() {
        EventsManager.OnShowScreen -= ShowScreen;
    }
    
    private void Update() 
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began)
            {
                EventsManager.OnUserClickedAtScreen?.Invoke();
            }
        }
    }
    

    public void ShowScreen(string screenName)
    {
        HideAllScreens();
        screens.Find(screen => screen.name == screenName).SetActive(true);
    }

    public void HideAllScreens()
    {
        screens.ForEach(screen => screen.SetActive(false));
    }
}