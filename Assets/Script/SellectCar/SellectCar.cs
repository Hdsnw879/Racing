using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SellectCar : MonoBehaviour
{
     int carList;
    private GameObject carPosition;
    public List<GameObject>  car;
    
    
    // Start is called before the first frame update    
    void Awake()
    {   
        carPosition = GameObject.Find("Show Position");
    }
    void Start()
    {
        
        float randomRotate = Random.Range(130f,230f);
        PlayerPrefs.SetInt("car",0);
        Quaternion carRotation = Quaternion.Euler(carPosition.transform.rotation.x,randomRotate,carPosition.transform.rotation.z);
        Instantiate(car[PlayerPrefs.GetInt("car")],carPosition.transform.position,carRotation);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
    public void RightClick()
    {
        if(carList >= 0 && carList < car.Count - 1)
        {
            
            Destroy(GameObject.FindGameObjectWithTag("Player"));
            carList++;
            SpawnCar();
        }
    }
    public void LeftClick()
    {
        
        if(carList > 0)
        {
                    
            Destroy(GameObject.FindGameObjectWithTag("Player"));
            carList--;
            SpawnCar();
        }
    }
    
    void SpawnCar()
    {
        
        float randomRotate = Random.Range(130f,230f);
        Quaternion carRotation = Quaternion.Euler(carPosition.transform.rotation.x,randomRotate,carPosition.transform.rotation.z);
        if(carList >=0 && carList <=car.Count)
        {
            Instantiate(car[carList],carPosition.transform.position,carRotation);
        }
    }
    public void StartGame()
    {
        PlayerPrefs.SetInt("car",carList);
        SceneManager.LoadScene("MainScene");
    }
}
