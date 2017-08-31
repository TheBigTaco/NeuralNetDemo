using UnityEngine;

namespace NeuralNetwork
{
    public class Agent : MonoBehaviour
    {
        public NeuralNetwork Brain;

        public Transform Target;

        public Rigidbody rigidBody;

        public bool Alive = false;
        public float currentDistance;
        public float lastDistance = 0;

        RaycastHit View;

        // Use this for initialization
        void Start()
        {
            Physics.IgnoreLayerCollision(8, 8);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            /* Limit Movement *///--------------------------------------------------------------------
            if (transform.position.z != -1)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y,-1);
            }

            if (transform.position.x < -4.5f)
            {
                //transform.position = new Vector3(-4.25f, transform.position.y);
                rigidBody.AddForce(new Vector3(0.5f, 0f));
            }
            else if (transform.position.x > 4.5f)
            {
                //transform.position = new Vector3(4.25f, transform.position.y);
                rigidBody.AddForce(new Vector3(-0.5f, 0f));
            }
            else { }

            if (transform.position.y < -4.5f)
            {
                //transform.position = new Vector3(transform.position.x, -4.25f);
                rigidBody.AddForce(new Vector3(0f, 0.5f));
            }
            else if (transform.position.y > 4.5f)
            {
                //transform.position = new Vector3(transform.position.x, 4.25f);
                rigidBody.AddForce(new Vector3(0f, -0.5f));
            }
            else { }

            /* Positioning w/ NeuralNet *///---------------------------------------------
            if (Alive)
            {
                float[] inputs = new float[2]; //init [] for input of position [x,y]
                inputs[0] = transform.position.x;
                inputs[1] = transform.position.y;

                float[] output = Brain.FeedForward(inputs); //query

                float outX = transform.position.x + output[0];
                float outY = transform.position.y + output[1];

                Vector3 dirChange = new Vector3(outX, outY);   //change direction from Brain
                transform.position = Vector3.Lerp(transform.position, dirChange, 0.01f);

                /* Calculate Fitness *///------------------------------------------------------------------------------
                Vector3 origin = transform.position;
                Vector3 dest = Target.position;
                Vector3 direction = dest - origin;

                if (Physics.Raycast(transform.position, direction, out View)) //get distance of target
                {
                    if (View.transform.name == "Target")
                    {
                        currentDistance = View.distance;
                        Brain.AddFitness(lastDistance - currentDistance);   //increase fitness as distance decreases
                    }
                }
            }

            lastDistance = currentDistance;

        }

        public void GivePurpose(NeuralNetwork brain, Transform target) // initialize agent
        {
            Brain = brain;
            Target = target;
            Alive = true;
        }
    }
}