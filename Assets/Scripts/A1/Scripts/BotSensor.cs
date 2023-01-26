using System.Linq;
using System.Runtime.CompilerServices;
using EasyAI;
using UnityEngine;

namespace A1.Scripts
{
    public class BotSensor : Sensor
    {
        public override object Sense()
        {
            // todo: Instead of finding all floor tiles, it should only find tiles that are lit.
            // Find all floor tiles in the scene.
            // Constantly finding objects is inefficient, in actual use look for ways to store values.
            Transform[] tiles = FindObjectsOfType<Transform>()
                .Where(t => t.name.Contains("Floor1") && t.GetComponent<MyFloor>().IsLit).ToArray();

            // Return null if there are no tiles.
            if (tiles.Length == 0)
            {
                Log("No tiles lit.");
                return null;
            }

            // Return the nearest tile otherwise.
            Log("Targeting nearest tile.");
            return tiles.OrderBy(b => Vector3.Distance(Agent.transform.position, b.transform.position)).First();
        }
    }
}