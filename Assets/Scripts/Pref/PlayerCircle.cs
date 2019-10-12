using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace assets
{
    public class PlayerCircle : MonoBehaviour
    {
        private GameObject panelPause;

        private void Awake()
        {
            panelPause = GameObject.FindGameObjectWithTag("PanelResult");
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            float playerScale = transform.localScale.x;
            float enemyScale = other.transform.localScale.x;

            if (playerScale > enemyScale)
            {
                float s = Mathf.PI / 4 * playerScale * playerScale;
                float sOther = Mathf.PI / 4 * other.transform.localScale.x * other.transform.localScale.x;

                float scale = Mathf.Sqrt((s + sOther) / Mathf.PI) * 2f;

                Variables.sizePlayer = scale;
                Variables.needRefreshColor = true;

                transform.localScale = new Vector3(scale, scale, scale);

                Variables.allS -= sOther;
                Destroy(other.gameObject);

                if (Variables.allS < s)
                {
                    panelPause.transform.position = new Vector3(panelPause.transform.position.x, panelPause.transform.position.y, -9f);
                    panelPause.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "WIN!";
                    Variables.isDone = true;
                }
            } else
            {
                panelPause.transform.position = new Vector3(panelPause.transform.position.x, panelPause.transform.position.y, -9f);
                panelPause.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "LOSE!";
                Variables.isDone = true;
            }
        }
    }
}