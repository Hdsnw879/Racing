using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<GameObject> car ,carEnemy;
    private GameObject carSpawnStart, player,winCheck,speedoMeter,gameInPause;
    AudioListener cameraMan;
    private TextMeshProUGUI countDown;
    
    private float rayCastDistance = 50f;
    private bool enemyWin,playerWin;
    private TextMeshProUGUI timerText ;

    private float remainingTime =75f ;

    private GameObject gamePanel;
    private TextMeshProUGUI gameOver;
    
    float timer =3;
    private bool start,end;
    Rigidbody playerCar;
    private Rigidbody enemyCar;
    private float maxSpeed = 290f;
    private float speed = 0.0f;

    private float minSpeedArrowAngle = -1.875f;
    private float maxSpeedArrowAngle = -182.712f;

    private TextMeshProUGUI speedLabel; // The label that displays the speed;
    private RectTransform arrow; // The arrow in the speedometer
    void Parameter()
    {
        speed = playerCar.linearVelocity.magnitude * 3.6f;
        if (speedLabel != null)
            speedLabel.text = ((int)speed) + " km/h";
        if (arrow != null)
            arrow.localEulerAngles =
                    new Vector3(0, 0, Mathf.Lerp(minSpeedArrowAngle, maxSpeedArrowAngle, speed / maxSpeed));
    }
    void Awake()
    {
        
    }
    
    void Start()
    {
        SpawnEmeny();
        winCheck = GameObject.Find("CheckWin");
        enemyWin=false;
        playerWin=false; 
        end = true;
        start = false;
        StartCoroutine(GetObject());
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        // if(start == true) return;
        WhoWin();
        WinOrLose();
        TimeRemaining();
        Parameter();
    }
    void GetMoreComponent()
    {
        InputManager input = player.GetComponent<InputManager>();
        AudioSource audio1 = player.GetComponents<AudioSource>()[0];
        AudioSource audio2 = player.GetComponents<AudioSource>()[1];
        if(PlayerPrefs.GetInt("car") == 0)
        {
            XeClassic carController = player.GetComponent<XeClassic>();
            carController.enabled = true;
        }else if(PlayerPrefs.GetInt("car")==1)
        {
            Xe1 carController = player.GetComponent<Xe1>();
            carController.enabled = true;
        }else if(PlayerPrefs.GetInt("car") == 2)
        {
            Xe2 carController = player.GetComponent<Xe2>();
            carController.enabled = true;
        }else if(PlayerPrefs.GetInt("car")==3)
        {
            Xe3 carController = player.GetComponent<Xe3>();
            carController.enabled = true;
        }
        
        input.enabled = true;
        audio1.enabled = true;
        audio2.enabled = true;
    }
    IEnumerator GetObject()
    {
        gameInPause = GameObject.Find("GameInPause");
        cameraMan = GameObject.Find("camera").GetComponent<AudioListener>();
        speedoMeter = GameObject.Find("Speedometer");
        countDown = GameObject.Find("CountDown").GetComponent<TextMeshProUGUI>();
        gamePanel = GameObject.Find("Panel");
        gameOver = GameObject.Find("GameOver").GetComponent<TextMeshProUGUI>();
        timerText = GameObject.Find("Time").GetComponent<TextMeshProUGUI>();
        player = GameObject.FindWithTag("Player");
        playerCar = GameObject.FindWithTag("Player").GetComponent<Rigidbody>();
        enemyCar = GameObject.FindWithTag("Enemy1").GetComponent<Rigidbody>();
        speedLabel = GameObject.Find("Km/h").gameObject.GetComponent<TextMeshProUGUI>();
        arrow = GameObject.Find("Arrow").gameObject.GetComponent<RectTransform>();
        enemyCar.isKinematic = true;
        playerCar.isKinematic = true;
        GetMoreComponent();
        Time.timeScale = 1f;
        cameraMan.enabled = true;
        gameInPause.SetActive(false);
        gamePanel.SetActive(false);
        yield return new WaitForSeconds(1f);
        StartCoroutine(CountDownTime());    

        
    }
    public void PauseGame()
    {
        gameInPause.SetActive(true);
        speedoMeter.SetActive(false);
        cameraMan.enabled = false;
        Time.timeScale = 0f;
    }
    public void Resume()
    {
        gameInPause.SetActive(false);
        speedoMeter.SetActive(true);
        cameraMan.enabled = true;
        Time.timeScale = 1;
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene("StartScene");
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void SpawnEmeny()
    {
        carSpawnStart = GameObject.Find("StartPoint");
        GameObject enemyCarSpawn = GameObject.Find("EnemyStartPoint");
        int dom = Random.Range(0,4);
        Instantiate(carEnemy[dom],enemyCarSpawn.transform.position,enemyCarSpawn.transform.rotation);
        Instantiate(car[PlayerPrefs.GetInt("car")],carSpawnStart.transform.position,carSpawnStart.transform.rotation);
        
    }
    void WhoWin()
    {
        Ray ray = new Ray(winCheck.transform.position,winCheck.transform.right * rayCastDistance);
        Debug.DrawRay(winCheck.transform.position,winCheck.transform.right * rayCastDistance,Color.red);
        if(Physics.Raycast(ray,out RaycastHit hit, rayCastDistance))
        {
            if(hit.collider.tag == "PlayerWin")
            {   
                playerWin =true;
            }
            if(hit.collider.tag == "Enemy")
            {
                enemyWin = true;
            }
        }
    }
    public void PlayAgain()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void Menu()
    {
        SceneManager.LoadScene("StartScene");
    }
    void TimeRemaining()    
    {
        if(remainingTime > 0 && start == true){
         
            remainingTime -= Time.deltaTime; 
        }
        else if(remainingTime < 0){
            remainingTime = 0;
            gamePanel.gameObject.SetActive(true);
            end = false;
        }
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    void WinOrLose()
    {
        if(enemyWin == true && end == true)
        {
            gameOver.text = "YOU LOSE";
            gamePanel.gameObject.SetActive(true);
            end=false;
        }else if(playerWin && end == true)
        {
            gameOver.text = "YOU WIN";
            gamePanel.gameObject.SetActive(true);
            end = false;
        }

    }
    
    IEnumerator CountDownTime()
    {               
            while(timer>0)
            {
                timer -=Time.deltaTime;
                countDown.text = Mathf.Round(timer).ToString();
                yield return null;
            }
            start = true;
            enemyCar.isKinematic = false;
            playerCar.isKinematic = false;
            
            countDown.gameObject.SetActive(false);
    }
    
}


