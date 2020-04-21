using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour {


    [SerializeField] GameObject ExplosionSpot;
    [SerializeField] GameObject SunSpot;

    private GameObject ParticleToSpawn;
    private GameObject PlaceToSpawn;

    public void CreateParticle(string attackName, Entity target, string FolderName) // Creates the particle found with the same name as the attack
    {
        List<Entity> Targets = new List<Entity>() { target };

        ParticleToSpawn = null;

        switch (attackName)
        {
            case "Water Flow":
            case "Blood Stained":
            case "Hollow":
            case "Spook Up":
                Targets = GameObject.Find("Game Manager Battle").GetComponent<EntityCreation>().GetNPCs();
                break;

            case "Bestow Life":
            case "Optimization":
            case "Purgatory":
            case "Inflammation":
                Targets = GameObject.Find("Game Manager Battle").GetComponent<EntityCreation>().GetPlayers();
                break;

            case "Kabe Don":
                if (target is Priestress)
                { attackName = "Kabe Don Priestress"; }
                else
                {
                    attackName = "Kabe Don Self";
                    Targets = GameObject.Find("Game Manager Battle").GetComponent<EntityCreation>().GetPlayers();
                    foreach (Entity tmp in Targets)
                    {
                        if (tmp is Aramusha)
                        { Targets = new List<Entity>() { tmp }; }
                    }
                }
                break;

            case "Hayawan":
                Targets = GameObject.Find("Game Manager Battle").GetComponent<EntityCreation>().GetPlayers();
                foreach (Entity tmp in Targets)
                {
                    if (tmp is Mage)
                    { Targets = new List<Entity>() { tmp }; }
                }
                break;

            case "EXPLOSION":
                Instantiate((GameObject)Resources.Load("Particles/Persona/EXPLOSION"), ExplosionSpot.transform.position, Quaternion.identity);
                return;

            case "Pillar of Light":
                Instantiate((GameObject)Resources.Load("Particles/Persona/Pillar of Light"), SunSpot.transform.position, Quaternion.identity);
                return;
        }

        ParticleToSpawn = (GameObject)Resources.Load("Particles/" + FolderName + "/" + attackName);

        if (!ParticleToSpawn)
        { Debug.Log("Missinng Particle"); return; }

        foreach (Entity tmp in Targets)
        {
            PlaceToSpawn = GameObject.Find("Game Manager Battle").GetComponent<EntityCreation>().FindTargetPosition(tmp);
            Instantiate(ParticleToSpawn, PlaceToSpawn.transform.position, Quaternion.identity);
        } 
    }
}
