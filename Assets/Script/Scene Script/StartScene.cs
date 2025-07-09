using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    private GameObject menuText;
    private GameObject mainMenu,guideScene,music;
    private static StartScene instance;
    
    
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            
            instance = this;
            
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            
            Destroy(GetComponent<AudioSource>());
        }
        
        
        
        guideScene = GameObject.Find("GuideScene");
        menuText = GameObject.Find("Menutext");
        mainMenu = GameObject.Find("Main menu");
        guideScene.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
            
    }
    public void ComeToSellectCar()
    {
        SceneManager.LoadScene("SelectCar");
    }
    public void Guide()
    {
        guideScene.SetActive(true);
        menuText.SetActive(false);
        mainMenu.SetActive(false);
    
    }
    public void OutGuide()
    {
        guideScene.SetActive(false);
        menuText.SetActive(true);
        mainMenu.SetActive(true);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
