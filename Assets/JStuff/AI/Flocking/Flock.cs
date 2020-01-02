using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JStuff.AI.Flocking
{
    public class Flock : MonoBehaviour
    {
        [SerializeField]
        private WeightedBehavior[] behaviors;

        //[SerializeField]
        //private WeightedBehavior behavior;

        [SerializeField]
        private Boid[] boids;

        [SerializeField]
        private float maxSpeed;

        void Start()
        {
            if (boids == null || boids.Length == 0)
            {
                boids = this.gameObject.GetComponentsInChildren<Boid>();
            }
        }

        void FixedUpdate()
        {
            foreach(Boid boid in boids)
            {
                Vector2 v = Vector2.zero;
        
                foreach(WeightedBehavior b in behaviors)
                {
                    v += b.Behavior.VelocityChange(boids, b.Context, boid) * b.Weight;
                }
        
                boid.velocity = v.normalized * maxSpeed * Time.fixedDeltaTime;
            }
        
            foreach(Boid boid in boids)
            {
                boid.UpdatePosition();
            }
        }



        public static (Boid, float) NearestBoid(Boid[] flock, Boid boid)
        {
            Boid retval = null;
            float distance = float.MaxValue;
            foreach (Boid b in flock)
            {
                //float d = b.transform.position.magnitude - boid.transform.position.magnitude;
                Vector2 v = b.transform.position - boid.transform.position;
                if (v.magnitude < distance
                    && b != boid)
                {
                    retval = b;
                    distance = v.magnitude;
                }
            }
            //Debug.Log(retval + ":" + boid);
            return (retval, distance);
        }

        public static Boid[] BoidsInRadius(Boid[] flock, Vector2 point, float radius)
        {
            List<Boid> retval = new List<Boid>();
            foreach (Boid boid in flock)
            {
                if ((boid.transform.position - (Vector3)point).magnitude <= radius)
                    retval.Add(boid);
            }
            return retval.ToArray();
        }
    }
}

/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JStuff.AI
{
    public class Flock : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField]
        private GameObject target;
        [SerializeField]
        private Vector2 targetOffset;

        [Header("Weights")]
        [SerializeField]
        private float pullWeight = 0.5f;
        [SerializeField]
        private float pushWeight = 0.6f;
        [SerializeField]
        private float randomWeight = 0.3f;
        [SerializeField]
        private float directionWeight = 0.3f;
        [SerializeField]
        private float separationWeight = 0.3f;
        [SerializeField]
        private float alignmentWeight = 0.3f;
        [SerializeField]
        private float cohesionWeight = 0.3f;
        [SerializeField]
        private float avoidanceWeight = 0.3f;

        [Header("Attributes")]
        [SerializeField]
        private float pushDistance = 1f;
        [SerializeField]
        private float separationDistance = 1f;

        [SerializeField]
        private float speed = 1;
        [SerializeField]
        private float distanceSpeed = 1;

        private Boid[] boids;

        [SerializeField]
        private float radius;

        [SerializeField]
        private Transform[] obstacles;

        void Start()
        {
            if (boids == null)
            {
                boids = this.gameObject.GetComponentsInChildren<Boid>();
            }
        }

        void FixedUpdate()
        {
            //Vector2 v = this.transform.position;
            //Vector2 tar = target.transform.position;
            //float distance = (v - tar).magnitude;

            foreach(Boid boid in boids)
            {
                Vector2 v2 = Vector2.zero;

                if (pullWeight != 0)
                    v2 += Pull(boid) * pullWeight;
                if (pushWeight != 0)
                    v2 += Push(boid) * pushWeight;
                if (randomWeight != 0)
                    v2 += Random(boid) * randomWeight;
                v2 += boid.velocity.normalized * directionWeight;

                if (separationWeight != 0)
                    v2 += Separation(boid) * separationWeight;
                if (alignmentWeight != 0)
                    v2 += Alignment(boid) * alignmentWeight;
                if (cohesionWeight != 0)
                    v2 += Cohesion(boid) * cohesionWeight;

                if (directionWeight != 0)
                    v2 += Direction(boid) * directionWeight;

                if (avoidanceWeight != 0)
                    v2 += Avoidance(boid, obstacles) * avoidanceWeight;

                v2.Normalize();
                //boid.velocity = v2 * distance * distanceSpeed * Time.fixedDeltaTime
                //    + v2 * speed * Time.fixedDeltaTime;
                boid.velocity = v2 * speed * Time.fixedDeltaTime;
            }
            foreach(Boid boid in boids)
            {
                boid.UpdatePosition();
            }
        }

        Vector2 Direction(Boid boid)
        {
            return boid.velocity.normalized;
        }

        Vector2 Separation(Boid boid)
        {
            (Boid b, float distance) = Flock.NearestBoid(boids, boid);
            Vector2 v = b.transform.position - boid.transform.position;
            float r = 1 - 2 * separationDistance / (v.magnitude + separationDistance);
            //return r * v.normalized;
            //if (r > 0)
            //    r = 0;
            return (v.x != 0 && v.y !=0) ? r * v.normalized : r * Random(boid);
        }

        Vector2 Alignment(Boid boid)
        {
            Vector2 v = Vector2.zero;
            Boid[] bb = Flock.BoidsInRadius(boids, boid.transform.position, radius);
            foreach (Boid b in bb)
            {
                v += b.velocity;
            }
            return v/bb.Length;
        }

        Vector2 Cohesion(Boid boid)
        {
            Vector2 v = new Vector2(0, 0);
            Boid[] bb = Flock.BoidsInRadius(boids, boid.transform.position, radius);
            foreach (Boid b in bb)
            {
                Vector2 v2 = b.transform.position;
                v += v2;
            }
            v /= bb.Length;
            Vector2 v3 = boid.transform.position;
            v -= v3;
            return (v.magnitude > 0) ? v.normalized : new Vector2(0, 0);
        }

        Vector2 Avoidance(Boid boid, Transform[] obstacles)
        {
            Vector2 retval = Vector2.zero;
            foreach(Transform t in obstacles)
            {
                Vector2 delta = t.position - boid.transform.position;
                if ((delta).magnitude < radius)
                {
                    retval -= delta.normalized;
                }
            }
            return retval.normalized;
        }

        Vector2 Pull(Boid boid)
        {
            if (target == null)
                return Vector2.zero;
            Vector2 v = target.transform.position;
            Vector2 v2 = boid.transform.position;
            return (v + targetOffset - v2).normalized;
        }

        Vector2 Push(Boid boid)
        {
            if (target == null)
                return Vector2.zero;
            Vector2 v = target.transform.position;
            Vector2 v2 = boid.transform.position;
            return ((v + targetOffset - v2).magnitude < pushDistance) ? -(v + targetOffset - v2).normalized : new Vector2();
        }

        Vector2 Random(Boid boid)
        {
            float random = UnityEngine.Random.Range(0f, 260f);
            return new Vector2(Mathf.Cos(random), Mathf.Sin(random));
            //return new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)).normalized;
        }

        public static (Boid,float) NearestBoid(Boid[] flock, Boid boid)
        {
            Boid retval = null;
            float distance = float.MaxValue;
            foreach(Boid b in flock)
            {
                //float d = b.transform.position.magnitude - boid.transform.position.magnitude;
                Vector2 v = b.transform.position - boid.transform.position;
                if (v.magnitude < distance
                    && b != boid)
                {
                    retval = b;
                    distance = v.magnitude;
                }
            }
            return (retval,distance);
        }

        public static Boid[] BoidsInRadius(Boid[] flock, Vector2 point, float radius)
        {
            List<Boid> retval = new List<Boid>();
            foreach(Boid boid in flock)
            {
                if ((boid.transform.position - (Vector3)point).magnitude <= radius)
                    retval.Add(boid);
            }
            return retval.ToArray();
        }
    }
}
 */
