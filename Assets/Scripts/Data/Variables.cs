using UnityEngine;

namespace assets
{
    public class Variables
    {
        public static GameData gameData = null;
        public static float width = 0;
        public static float height = 0;

        public static float sizePlayer = 0;
        public static float sizeBiggestEnemy = 0;

        public static float allS = 0;

        public static bool needRefreshColor = false;

        public static bool isDone = false;

        public static Vector2 clickPosition = new Vector2();
        public static float clickTime = 0;
    }
}