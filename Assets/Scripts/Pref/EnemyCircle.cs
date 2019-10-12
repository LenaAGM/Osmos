using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace assets {

    enum Side
    {
        UpLeft,
        UpRight,
        DownLeft,
        DownRight
    }

    public class EnemyCircle : MonoBehaviour
    {
        [HideInInspector]
        public int offcut = 30;
        private int currentPartOfOffcut = 0;

        private Side randomSide;

        [SerializeField]
        private float speed;

        private float width;
        private float height;

        private void Awake()
        {
            Array values = Enum.GetValues(typeof(Side));
            randomSide = (Side)values.GetValue(Random.Range(0, values.Length));

            height = Camera.main.orthographicSize;
            width = height * Camera.main.aspect;
        }

        // Update is called once per frame
        void Update()
        {
            if (!Variables.isDone)
            {
                float distance = speed * Time.deltaTime;

                switch (randomSide)
                {
                    case Side.UpLeft:
                        transform.position += new Vector3(-distance, distance, 0f);
                        break;
                    case Side.UpRight:
                        transform.position += new Vector3(distance, distance, 0f);
                        break;
                    case Side.DownLeft:
                        transform.position += new Vector3(-distance, -distance, 0f);
                        break;
                    case Side.DownRight:
                        transform.position += new Vector3(distance, -distance, 0f);
                        break;
                }

                if (transform.position.x - transform.localScale.x / 2 < -width)
                {
                    switch (randomSide)
                    {
                        case Side.UpLeft:
                            currentPartOfOffcut = 0;
                            randomSide = Side.UpRight;
                            break;
                        case Side.DownLeft:
                            currentPartOfOffcut = 0;
                            randomSide = Side.DownRight;
                            break;
                    }
                }
                else if (transform.position.x + transform.localScale.x / 2 > width)
                {
                    switch (randomSide)
                    {
                        case Side.UpRight:
                            currentPartOfOffcut = 0;
                            randomSide = Side.UpLeft;
                            break;
                        case Side.DownRight:
                            currentPartOfOffcut = 0;
                            randomSide = Side.DownLeft;
                            break;
                    }
                }
                else if (transform.position.y - transform.localScale.y / 2 < -height)
                {
                    switch (randomSide)
                    {
                        case Side.DownRight:
                            currentPartOfOffcut = 0;
                            randomSide = Side.UpRight;
                            break;
                        case Side.DownLeft:
                            currentPartOfOffcut = 0;
                            randomSide = Side.UpLeft;
                            break;
                    }
                }
                else if (transform.position.y + transform.localScale.y / 2 > height)
                {
                    switch (randomSide)
                    {
                        case Side.UpRight:
                            currentPartOfOffcut = 0;
                            randomSide = Side.DownRight;
                            break;
                        case Side.UpLeft:
                            currentPartOfOffcut = 0;
                            randomSide = Side.DownLeft;
                            break;
                    }
                }

                if (currentPartOfOffcut < offcut)
                {
                    ++currentPartOfOffcut;
                }
                else
                {
                    currentPartOfOffcut = 0;
                    Array values = Enum.GetValues(typeof(Side));
                    randomSide = (Side)values.GetValue(Random.Range(0, values.Length));
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.name.Equals("GameController"))
            {
                if (!other.name.Equals("Player") && (transform.localScale.x > other.transform.localScale.x || (transform.localScale.x == other.transform.localScale.x && int.Parse(name) > int.Parse(other.name))))
                {
                    float s = Mathf.PI / 4 * transform.localScale.x * transform.localScale.x;
                    float sOther = Mathf.PI / 4 * other.transform.localScale.x * other.transform.localScale.x;

                    float scale = Mathf.Sqrt((s + sOther) / Mathf.PI) * 2f;

                    if (Variables.sizeBiggestEnemy < scale)
                    {
                        Variables.sizeBiggestEnemy = scale;
                        Variables.needRefreshColor = true;
                    }

                    transform.localScale = new Vector3(scale, scale, scale);

                    GetComponent<SpriteRenderer>().color = Settings.getColorBySize(transform.localScale.x);

                    Destroy(other.gameObject);
                }
            }
        }
    }
}