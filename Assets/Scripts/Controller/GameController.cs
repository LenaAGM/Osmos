using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace assets
{
    public class GameController : MonoBehaviour
    {
        private string gameDataFileName = "game_data.json";

        [SerializeField]
        private GameObject gameObjects;

        [SerializeField]
        private GameObject enemyCircle;
        [SerializeField]
        private GameObject playerCircle;

        [SerializeField]
        private int countEnemies;
        private GameObject[] enemies;

        private GameObject player;

        private int width;
        private int height;

        private float widthScreen;
        private float heightScreen;

        private float timeOnClickDown;
        private float timeOnClickUp;

        private void Awake()
        {
            width = (int)(Camera.main.orthographicSize * Camera.main.aspect);
            height = (int)Camera.main.orthographicSize;

            heightScreen = Camera.main.orthographicSize;
            widthScreen = heightScreen * Camera.main.aspect;

            enemies = new GameObject[height * width];

            Variables.gameData = DataController.LoadGameData(gameDataFileName);
        }

        // Start is called before the first frame update
        void Start()
        {
            DontDestroyOnLoad(gameObject);

            int step = height * width / countEnemies + 1;
            int startVal = 0;

            int playerPosition = height * width / 2;

            player = Instantiate(playerCircle, new Vector2(playerPosition % width * 2 - width + 1, height - playerPosition / width * 2 - 1), Quaternion.identity) as GameObject;
            player.name = "Player";

            float randomScale = Random.Range(5, 10) / 10f;
            player.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
            player.transform.parent = gameObjects.transform;

            Variables.sizePlayer = randomScale;

            player.GetComponent<SpriteRenderer>().color = new Color32(Variables.gameData.user.color.r, Variables.gameData.user.color.g, Variables.gameData.user.color.b, 255); 

            while (startVal < step)
            {
                for (int i = startVal; i < enemies.Length; i += step)
                {
                    if (i != playerPosition)
                    {
                        enemies[i] = Instantiate(enemyCircle, new Vector2(i % width * 2 - width + 1, height - i / width * 2 - 1), Quaternion.identity) as GameObject;
                        enemies[i].GetComponent<EnemyCircle>().offcut = Random.Range(100, 200);
                        enemies[i].name = i.ToString();

                        randomScale = Random.Range(2, 12) / 10f;
                        Variables.allS += Mathf.PI / 4 * randomScale * randomScale;

                        if (Variables.sizeBiggestEnemy < randomScale)
                        {
                            Variables.sizeBiggestEnemy = randomScale;
                        }

                        enemies[i].transform.localScale = new Vector3(randomScale, randomScale, randomScale);
                        enemies[i].transform.parent = gameObjects.transform;

                        if (--countEnemies == 0)
                        {
                            Variables.needRefreshColor = true;
                            return;
                        }
                    }
                }
                ++startVal;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Variables.needRefreshColor)
            {
                refreshColor();
            }
            if (Variables.isDone)
            {
                gameObjects.SetActive(false);
            } else 
            {
                if (player.transform.position.x - player.transform.localScale.x / 2 - 0.3f < -widthScreen)
                {
                    Variables.clickTime = 0;
                    player.transform.position = new Vector3(-widthScreen + player.transform.localScale.x / 2 + 0.32f, player.transform.position.y, player.transform.position.z);
                }
                else if (player.transform.position.x + player.transform.localScale.x / 2 + 0.3f > widthScreen)
                {
                    Variables.clickTime = 0;
                    player.transform.position = new Vector3(widthScreen - player.transform.localScale.x / 2 - 0.32f, player.transform.position.y, player.transform.position.z);
                }
                else if (player.transform.position.y - player.transform.localScale.y / 2 - 0.3f < -heightScreen)
                {
                    Variables.clickTime = 0;
                    player.transform.position = new Vector3(player.transform.position.x, -heightScreen + player.transform.localScale.y / 2 + 0.32f, player.transform.position.z);
                }
                else if (player.transform.position.y + player.transform.localScale.y / 2 + 0.3f > heightScreen)
                {
                    Variables.clickTime = 0;
                    player.transform.position = new Vector3(player.transform.position.x, heightScreen - player.transform.localScale.y / 2 - 0.32f, player.transform.position.z);
                }
            }
        }

        private void refreshColor()
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i] != null)
                {
                    enemies[i].GetComponent<SpriteRenderer>().color = Settings.getColorBySize(enemies[i].transform.localScale.x);
                }
            }
            Variables.needRefreshColor = false;
        }

        private void OnMouseDown()
        {
            timeOnClickDown = Time.time;
            Variables.clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        private void OnMouseUp()
        {
            timeOnClickUp = Time.time;
            Variables.clickTime = (timeOnClickUp - timeOnClickDown + 1f) * 4;
            pushPlayer(Variables.clickTime);
        }

        private void pushPlayer(float clickTime)
        {
            StartCoroutine(FakeAddForceMotion(clickTime, player.transform.position - new Vector3(Variables.clickPosition.x, Variables.clickPosition.y, 0)));
        }

        IEnumerator FakeAddForceMotion(float time, Vector2 force)
        {
            float i = 0.3f;
            while (time > i)
            {
                player.GetComponent<Rigidbody2D>().velocity = force / i / 1.5f;
                i = i + Time.deltaTime;

                Debug.Log("time = " + time + ", v_time = " + Variables.clickTime);

                if (Variables.clickTime != time)
                {
                    player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    StopCoroutine(FakeAddForceMotion(time, force));
                }
                yield return new WaitForEndOfFrame();
            }
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            yield return null;
        }
    }
}