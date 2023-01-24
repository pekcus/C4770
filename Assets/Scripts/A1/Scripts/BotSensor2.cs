using System.Linq;
using EasyAI;
using UnityEngine;

namespace A1.Scripts
{
    public class BotSensor2 : Sensor
    {
        public override object Sense()
        {
            // todo: Instead of finding all floor tiles, it should only find tiles that are lit.
            // Find all floor tiles in the scene.
            // Constantly finding objects is inefficient, in actual use look for ways to store values.
            Transform[] tiles1 = FindObjectsOfType<Transform>().Where(t => t.name.Contains("Floor") && t.GetComponent<MyFloor>().State == MyFloor.LightLevel.Light1).ToArray();
            Transform[] tiles2 = FindObjectsOfType<Transform>().Where(t => t.name.Contains("Floor") && t.GetComponent<MyFloor>().State == MyFloor.LightLevel.Light2).ToArray();
            Transform[] tiles3 = FindObjectsOfType<Transform>().Where(t => t.name.Contains("Floor") && t.GetComponent<MyFloor>().State == MyFloor.LightLevel.Light3).ToArray();
            
            // Return null if there are no tiles.
            if (tiles1.Length == 0 && tiles2.Length == 0 && tiles3.Length == 0)
            {
                Log("No tiles lit.");
                return null;
            }

            // Return the nearest tile otherwise.
            Log("Targeting nearest tile.");
            if (tiles3.Length > 0)
                return tiles3.OrderBy(b => Vector3.Distance(Agent.transform.position, b.transform.position)).First();
            else if (tiles2.Length > 0)
                return tiles2.OrderBy(b => Vector3.Distance(Agent.transform.position, b.transform.position)).First();
            else 
                return tiles2.OrderBy(b => Vector3.Distance(Agent.transform.position, b.transform.position)).First();
        }
    }
}