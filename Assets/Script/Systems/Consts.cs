﻿using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Constants
{
    public static class Consts
    {
        //マスの状態
        public const int EMPTY = 0;
        public const int WHITE = -1;
        public const int BLACK = 1;

        public const int WALL = 2;
        public const int BOARD_SIZE = 8;

        //探索の方向
        //以下のようにすることで、ひっくり返せる方向が考えやすくなる
        //「00000000」の8bitの2進数として考えることで、1がたっている方向にはひっくり返せる、となる
        public const int LEFT = 1;
        public const int UPPER_LEFT = 2;
        public const int UPPER = 4;
        public const int UPPER_RIGHT = 8;
        public const int RIGHT = 16;
        public const int LOWER_RIGHT = 32;
        public const int LOWER = 64;
        public const int LOWER_LEFT = 128;

        //入力の表現
        public static readonly char[] INPUT_ALPHABET = new char[] {'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h'};
        public static readonly char[] INPUT_NUMBER = new char[] {'1', '2', '3', '4', '5', '6', '7', '8'};

        /// <summary> 盤面の評価値を示した盤面 </summary>
        public static readonly int[,] EVALUATION_BOARD
            = new int[8, 8]
              { { 9, 1, 5, 3, 3, 5, 1, 9 },
                { 1, 1, 3, 4, 4, 3, 1, 1 },
                { 5, 3, 4, 6, 6, 4, 3, 5 },
                { 3, 4, 6, 6, 6, 6, 4, 3 },
                { 3, 4, 6, 6, 6, 6, 4, 3 },
                { 5, 3, 4, 6, 6, 4, 3, 5 },
                { 1, 1, 3, 4, 4, 3, 1, 1 },
                { 9, 1, 5, 3, 3, 5, 1, 9 } };

        //手数の上限
        public const int MAX_TURNS = 60;

        public const string WHITE_TAG = "White";
        public const string BLACK_TAG = "Black";

        public static readonly Dictionary<SceneNames, string> Scenes = new()
        {
            [SceneNames.TITLE_SCENE] = "Title",
            [SceneNames.GAME_SCENE] = "MainScene",
            [SceneNames.RESULT_SCENE] = "Result",
        };

        public static GameObject FindWithVector(Vector3 pos)
        {
            GameObject find = null;

            foreach (GameObject obj
                     in Object.FindObjectsOfType(typeof(GameObject)).Cast<GameObject>())
            {
                if (pos == obj.transform.position)
                {
                    find = obj;
                    break;
                }
            }
            return find;
        }
    }

    public enum SceneNames
    {
        TITLE_SCENE,
        GAME_SCENE,
        RESULT_SCENE,
    }

    public enum Turns
    {
        WHITE,
        BLACK,
    }

    public enum JudgeResult
    {
        DRAW,
        WHITE_WIN,
        BLACK_WIN,
    }
}