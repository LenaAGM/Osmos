using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace assets
{
    public class Settings
    {
        public static Color32 getColorBySize(float size)
        {
            float r, g, b;

            if (size < Variables.sizePlayer)
            {
                r = Variables.gameData.enemy.color1.r + (Variables.gameData.user.color.r - Variables.gameData.enemy.color1.r) / Variables.sizePlayer * size;
                g = Variables.gameData.enemy.color1.g + (Variables.gameData.user.color.g - Variables.gameData.enemy.color1.g) / Variables.sizePlayer * size;
                b = Variables.gameData.enemy.color1.b + (Variables.gameData.user.color.b - Variables.gameData.enemy.color1.b) / Variables.sizePlayer * size;
            } else
            {
                r = Variables.gameData.user.color.r + (Variables.gameData.enemy.color2.r - Variables.gameData.user.color.r) / (Variables.sizeBiggestEnemy - Variables.sizePlayer) * (size - Variables.sizePlayer);
                g = Variables.gameData.user.color.g + (Variables.gameData.enemy.color2.g - Variables.gameData.user.color.g) / (Variables.sizeBiggestEnemy - Variables.sizePlayer) * (size - Variables.sizePlayer);
                b = Variables.gameData.user.color.b + (Variables.gameData.enemy.color2.b - Variables.gameData.user.color.b) / (Variables.sizeBiggestEnemy - Variables.sizePlayer) * (size - Variables.sizePlayer);
            }

            return new Color32((byte)r, (byte)g, (byte)b, 255);
        }
    }
}