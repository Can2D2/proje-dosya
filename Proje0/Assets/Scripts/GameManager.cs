using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    public GameObject obstacle;
    public Transform spawnPoint;
    int score = 0;//-1 ziplama art icin 0 saniye art icin

    public TextMeshProUGUI scoreText;
    public GameObject playButton;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnObstacles() 
    {

        while (true) 
        {

            float waitTime = Random.Range(1f, 3f);

            yield return new WaitForSeconds(waitTime);
            
            Instantiate(obstacle, spawnPoint.position, Quaternion.identity);
            //ScoreUp();//atladikca score art
            yield return new WaitForSeconds(4f-waitTime);

            Instantiate(obstacle, spawnPoint.position, Quaternion.identity);
            //ScoreUp();//atladikca score art

        }

    }
    void ScoreUp()
    {
        score++;
        scoreText.text = score.ToString();
    }
    public void GameStart()
    {

        scoreText.text = "0";
        player.SetActive(true);
        playButton.SetActive(false);

        StartCoroutine("SpawnObstacles");
        InvokeRepeating("ScoreUp", 2f, 1f); //saniyelik score arttirma

    }

}
