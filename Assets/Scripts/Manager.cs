using System.Collections.Generic;
using UnityEngine;

namespace NeuralNetwork
{
    public class Manager : MonoBehaviour
    {
        public GameObject AgentPrefab;
        public GameObject Target;

        public int PopulationSize;
        //public int[] Layers = null;
        public int Epoch = 0;
        public bool Learning = false;

        public List<NeuralNetwork> Networks = new List<NeuralNetwork>();
        public List<Agent> Agents = new List<Agent>();

        // Use this for initialization
        void Start()
        {
            CreateNetworks();
        }

        // Update is called once per frame
        void Update()
        {
            if (!Learning)
            {
                //float timer - Time.deltaTime
                if (Epoch != 0)
                {
                    Networks.Sort();    // sort ascending by fitness using custom compareto
                    for (int i = 0; i < PopulationSize; i++)
                    {
                        print(Networks[i].Fitness);
                    }

                    for (int i = 0; i < PopulationSize / 2; i++)  //for each lower half of fitness
                    {
                        Networks[i] = new NeuralNetwork(Networks[i + PopulationSize / 2]);    //copy of top half
                        Networks[i].MutateWeights();    //possibly mutated
                    }

                    for (int i = PopulationSize/2; i < PopulationSize; i++)
                    {
                        Networks[i] = new NeuralNetwork(Networks[i]);
                    }

                    for (int i = 0; i < PopulationSize; i++)
                    {
                        Networks[i].Fitness = 0f;   //reset fitness to compare new gen.
                    }
                }

                Epoch++;

                Agents = new List<Agent>();
                CreateAgents();

                Learning = true;

                Invoke("Lifetime", 30f);
            }
        }

        public void CreateNetworks()
        {
            for (int i = 0; i < PopulationSize; i++)
            {
                NeuralNetwork newNet = new NeuralNetwork(new int[4] { 2,3,3,2}); //declare layer info
                Networks.Add(newNet);
            }
        }

        public void CreateAgents()
        {
            for (int i = 0; i < PopulationSize; i++)
            {
                Agent agent = Instantiate(AgentPrefab).GetComponent<Agent>();

                agent.GivePurpose(Networks[i], Target.transform);

                agent.Brain.Fitness = 0;

                //float newX = agent.transform.position.x * UnityEngine.Random.Range(0f, 1f);

                //agent.transform.position = new Vector3(newX, agent.transform.position.y, agent.transform.position.z);

                Agents.Add(agent);
            }
        }

        public void Lifetime()
        {
            for (int i = 0; i < PopulationSize; i++)
            {
                GameObject.Destroy(Agents[i].gameObject);
            }

            Learning = false;
        }
    }
}