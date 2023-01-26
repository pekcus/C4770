using System.Linq;
using EasyAI;
using UnityEngine;

namespace A1.Scripts
{
    public class BotSensor2 : Sensor
    {
        public override object Sense()
        {
            // Find all floor tiles in the scene.
            // Constantly finding objects is inefficient, in actual use look for ways to store values.
            /* Note: due to my "game design", since all tiles turn off every little while (i.e. they're not persistent)
                saving them in a permanent array and clearing it every time is a bit more complex than it's worth for
                only 2 bots, so I left this as is and focused on the other components!                                  */

            // Just like BotSensor1, except each light level is stored in a different array.
            Transform[] tiles1 = FindObjectsOfType<Transform>().Where(t =>
                t.name.Contains("Floor2") && t.GetComponent<MyFloor>().State == MyFloor.LightLevel.Light1).ToArray();
            Transform[] tiles2 = FindObjectsOfType<Transform>().Where(t =>
                t.name.Contains("Floor2") && t.GetComponent<MyFloor>().State == MyFloor.LightLevel.Light2).ToArray();
            Transform[] tiles3 = FindObjectsOfType<Transform>().Where(t =>
                t.name.Contains("Floor2") && t.GetComponent<MyFloor>().State == MyFloor.LightLevel.Light3).ToArray();

            // Return null if there are no tiles.
            if (tiles1.Length == 0 && tiles2.Length == 0 && tiles3.Length == 0)
            {
                Log("No tiles lit.");
                return null;
            }

            // Return the nearest tile otherwise.
            // Prioritize targeting tiles with highest level worth, then second-highest, then lowest.
            Log("Targeting nearest tile.");
            if (tiles3.Length > 0)
                return tiles3.OrderBy(b => Vector3.Distance(Agent.transform.position, b.transform.position)).First();
            if (tiles2.Length > 0)
                return tiles2.OrderBy(b => Vector3.Distance(Agent.transform.position, b.transform.position)).First();
            return tiles1.OrderBy(b => Vector3.Distance(Agent.transform.position, b.transform.position)).First();
        }
    }
}