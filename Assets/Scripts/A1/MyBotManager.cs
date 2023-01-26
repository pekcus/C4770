using System.Collections.Generic;
using System.Linq;
using EasyAI;
using Unity.Mathematics;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace A1
{
    /// <summary>
    /// Extension of AgentManager to handle floor tile generation.
    /// </summary>
    [DisallowMultipleComponent]
    public class MyBotManager : Manager
    {
        /// <summary>
        /// All floors.
        /// </summary>
        public static List<MyFloor> Floors => BotSingleton._floors;
        
        /// <summary>
        /// Getter to cast the AgentManager singleton into a FloorManager.
        /// </summary>
        private static MyBotManager BotSingleton => Singleton as MyBotManager;

        /// <summary>
        /// All floors.
        /// </summary>
        private readonly List<MyFloor> _floors = new();

        [Header("Tile Parameters")] [Tooltip("How many floor sections will be generated.")] [SerializeField]
        private Vector2 floorSize = new(3, 1);

        [Tooltip("How many units wide will each floor section be generated as.")] [SerializeField] [Min(1)]
        private int floorScale = 1;

        /*[Tooltip("The percentage chance that any floor section during generation will be likely to light up.")]
        [Range(0, 1)]
        [SerializeField]
        private float likelyToBeLit;*/

        [Tooltip("How many seconds between every time tiles light up.")] [Min(0)] [SerializeField]
        private float timeBetweenLights = 5;

        [Tooltip("How many seconds the tiles remain lit.")] [Min(0)] [SerializeField]
        private float timeToDim = 5;

        [Tooltip("The percentage chance that a floor section will light up during lighting.")]
        [Range(0, 1)]
        [SerializeField]
        private float chanceLit;

        [Header("Prefabs")] [Tooltip("The prefab for the first bot agent that will be spawned in.")] [SerializeField]
        private GameObject BotAgentPrefab1;

        [Tooltip("The prefab for the second bot agent that will be spawned in.")] [SerializeField]
        private GameObject BotAgentPrefab2;


        [Header("Floor Materials")]
        /*[Tooltip("The material applied to the middle (base) floor section.")]
        [SerializeField]
        private Material materialBase;*/
        [Tooltip("The material applied to normal floor sections when they are not lit.")]
        [SerializeField]
        private Material materialDark;

        /*[Tooltip("The material applied to like to get dirty floor sections when they are clean.")]
        [SerializeField]
        private Material materialCleanLikelyToGetDirty;*/

        [Tooltip("The material applied to a floor section when it is lit to L1.")] [SerializeField]
        private Material materialL1;

        [Tooltip("The material applied to a floor section when it is lit to L2.")] [SerializeField]
        private Material materialL2;

        [Tooltip("The material applied to a floor section when it is lit to L3.")] [SerializeField]
        private Material materialL3;

        /// <summary>
        /// The root game object of the cleaner agent.
        /// </summary>
        private GameObject _BotAgent1;

        private GameObject _BotAgent2;

        /// <summary>
        /// Keep track of how much time has passed since the last time floor tiles were lit.
        /// </summary>
        private float _elapsedTime;

        /// <summary>
        /// Keeps track of if the tiles were turned off or not.
        /// </summary>
        private bool tilesReset = false;

        /// <summary>
        /// Generate the floor.
        /// </summary>
        private static void GenerateFloor()
        {
            /*// Destroy the previous agent.
            if (BotSingleton._BotAgent != null)
            {
                Destroy(BotSingleton._BotAgent.gameObject);
            }*/

            // Destroy all previous floors.
            foreach (MyFloor floor in BotSingleton._floors)
            {
                Destroy(floor.gameObject);
            }

            BotSingleton._floors.Clear();

            // Generate the floor tiles.
            for (int i = 1; i < 3; i++)
            {
                Vector2 offsets = new Vector2((BotSingleton.floorSize.x - 1) / 2f * i,
                                      (BotSingleton.floorSize.y - 1) / 2f) *
                                  BotSingleton.floorScale;
                for (int x = 0; x < BotSingleton.floorSize.x; x++)
                {
                    for (int y = 0; y < BotSingleton.floorSize.y; y++)
                    {
                        GenerateFloorTile(i, new(x, y), offsets);
                    }
                }
            }


            // Add the two bots.
            BotSingleton._BotAgent1 = Instantiate(BotSingleton.BotAgentPrefab1, Vector3.zero, quaternion.identity);
            BotSingleton._BotAgent1.name = "Bot 1";
            BotSingleton._BotAgent2 =
                Instantiate(BotSingleton.BotAgentPrefab2, (Vector3.left * 18), quaternion.identity);
            BotSingleton._BotAgent2.name = "Bot 2";

            // Reset elapsed time.
            BotSingleton._elapsedTime = 0;
        }

        /// <summary>
        /// Generate a floor tile.
        /// </summary>
        /// <param name="position">Its position relative to the rest of the floor tiles.</param>
        /// <param name="offsets">How much to offset the floor tile so all floors are centered around the origin.</param>
        private static void GenerateFloorTile(int i, Vector2 position, Vector2 offsets)
        {
            // Create a quad, then position, rotate, size, and name it.
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Quad);
            go.transform.position = new(position.x * BotSingleton.floorScale - offsets.x * i, 0,
                position.y * BotSingleton.floorScale - offsets.y);
            go.transform.rotation = Quaternion.Euler(90, 0, 0);
            go.transform.localScale = new(BotSingleton.floorScale, BotSingleton.floorScale, 1);
            go.name = $"Floor{i} {position.x} {position.y}";

            // Its collider is not needed.
            Destroy(go.GetComponent<Collider>());

            // Add and setup its floor component.
            MyFloor floor = go.AddComponent<MyFloor>();
            //bool likelyToBeLit = Random.value < BotSingleton.likelyToBeLit;
            //floor.Setup(likelyToBeLit, likelyToBeLit ? BotSingleton.materialCleanLikelyToGetDirty : BotSingleton.materialDark, BotSingleton.materialL1, BotSingleton.materialL2, BotSingleton.materialL3);
            bool isBase = (go.transform.position == Vector3.zero);
            floor.Setup( /*isBase, BotSingleton.materialBase,*/ BotSingleton.materialDark, BotSingleton.materialL1,
                BotSingleton.materialL2, BotSingleton.materialL3);
            BotSingleton._floors.Add(floor);
        }

        /// <summary>
        /// Update the states of floor tiles.
        /// </summary>
        private static void UpdateFloor()
        {
            // Increment how much time has passed and return if it has not been long enough since the last light generation.
            BotSingleton._elapsedTime += Time.deltaTime;
            // Tiles are reset before new tiles light up.
            if (BotSingleton._elapsedTime < BotSingleton.timeToDim)
                return;
            if (!BotSingleton.tilesReset)
            {
                ResetFloorTiles();
                return;
            }

            if (BotSingleton._elapsedTime < BotSingleton.timeBetweenLights)
                return;

            // Reset elapsed time.
            BotSingleton._elapsedTime = 0;
            SetFloorTiles();
            BotSingleton.tilesReset = false;
        }

        /// <summary>
        /// Light floor tiles.
        /// </summary>
        private static void SetFloorTiles()
        {
            /* Unused because tiles turn off after a certain amount of time.
             * // If all floor tiles are already at max dirt level return as there is nothing more which can be updated.
            if (BotSingleton._floors.Count(f => f.State != Floor.DirtLevel.ExtremelyDirty) == 0)
            {
                return;
            }*/

            // Get the chance that any tile will become dirty.
            float currentLitChance = Mathf.Max(BotSingleton.chanceLit, float.Epsilon);

            // We will loop until at least a single tile has been made dirty.
            int addedLit = 0;
            do
            {
                // Loop through all floor tiles that are not lit.
                foreach (MyFloor floor in BotSingleton._floors.Where(f => f.State == MyFloor.LightLevel.Dark))
                {
                    /*// Double the chance to get dirty of the current floor tile is likely to get dirty.
                    float dirtChance = floor.LikelyToGetDirty ? currentLitChance * 2 : currentLitChance;*/
                    // 40% chance to make it more likely that the tile will be lit
                    float lightChance = Random.value > 0.6f ? currentLitChance * 2 : currentLitChance;

                    // Attempt to light each tile three times to set the light level to one of the three light levels.
                    for (int i = 0; i < 3; i++)
                    {
                        if (Random.value <= lightChance)
                        {
                            floor.Light();
                            addedLit++;
                            // Increase the chance of the tile lighting if it has already been lit.
                            lightChance = lightChance * 1.5f;
                        }
                    }
                }

                // Double the chances of tiles getting lighting up for the next loop so we are not infinitely looping.
                currentLitChance *= 2;
            } while (addedLit < 10);
        }

        /// <summary>
        /// Reset floor tiles.
        /// </summary>
        private static void ResetFloorTiles()
        {
            foreach (MyFloor floor in BotSingleton._floors.Where(f => f.State > MyFloor.LightLevel.Dark))
            {
                floor.Hit();
            }

            BotSingleton.tilesReset = true;
        }

        protected override void Start()
        {
            base.Start();
            GenerateFloor();
            SetFloorTiles();
        }

        protected override void Update()
        {
            base.Update();
            UpdateFloor();
        }

        /// <summary>
        /// Render buttons to regenerate the floor or change its size.
        /// </summary>
        /// <param name="x">X rendering position. In most cases this should remain unchanged.</param>
        /// <param name="y">Y rendering position. Update this with every component added and return it.</param>
        /// <param name="w">Width of components. In most cases this should remain unchanged.</param>
        /// <param name="h">Height of components. In most cases this should remain unchanged.</param>
        /// <param name="p">Padding of components. In most cases this should remain unchanged.</param>
        /// <returns>The updated Y position after all custom rendering has been done.</returns>
        protected override float CustomRendering(float x, float y, float w, float h, float p)
        {
            // Regenerate the floor button.
            if (GuiButton(x, y, w, h, "Reset"))
            {
                ClearMessages();
                GenerateFloor();
                SetFloorTiles();
            }

            // Increase the floor width.
            if (floorSize.x < 5)
            {
                y = NextItem(y, h, p);
                if (GuiButton(x, y, w, h, "Increase Size X"))
                {
                    floorSize.x++;
                    GenerateFloor();
                }
            }

            // Decrease the floor width.
            if (floorSize.x > 1)
            {
                y = NextItem(y, h, p);
                if (GuiButton(x, y, w, h, "Decrease Size X"))
                {
                    floorSize.x--;
                    GenerateFloor();
                }
            }

            // Increase the floor height.
            if (floorSize.y < 5)
            {
                y = NextItem(y, h, p);
                if (GuiButton(x, y, w, h, "Increase Size Y"))
                {
                    floorSize.y++;
                    GenerateFloor();
                }
            }

            // Decrease the floor height.
            if (floorSize.y > 1)
            {
                y = NextItem(y, h, p);
                if (GuiButton(x, y, w, h, "Decrease Size Y"))
                {
                    floorSize.y--;
                    GenerateFloor();
                }
            }

            return NextItem(y, h, p);
        }
    }
}