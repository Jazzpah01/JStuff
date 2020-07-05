using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JStuff.Utilities;
using JStuff.Collection;

/// <summary>
/// Based on: https://www.roystan.net/articles/character-controller-2d.html
/// and: https://www.youtube.com/watch?v=7KiK0Aqtmzc
/// </summary>
public class Body2d : MonoBehaviour
{
    [Header("Flags and stuff")]
    [SerializeField, Tooltip("Collider flags that should collide with this.")]
    List<string> ColliderFlags;
    [SerializeField, Tooltip("Collider flags that should collide with this as a platform.")]
    List<string> PlatformFlags;
    [SerializeField]
    private BoxCollider2D boxCollider;

    [Header("Physics variables")]
    [SerializeField, Tooltip("Max speed, in units per second, that the character moves.")]
    float speed = 9;

    [SerializeField, Tooltip("Acceleration while grounded.")]
    float walkAcceleration = 75;

    [SerializeField, Tooltip("Acceleration while in the air.")]
    float airAcceleration = 30;

    [SerializeField, Tooltip("Deceleration applied when character is grounded and not attempting to move.")]
    float groundDeceleration = 70;

    [SerializeField, Tooltip("Max height the character will jump regardless of gravity")]
    float jumpHeight = 4;

    [SerializeField]
    float gravityAcceleration = -9.81f;

    float initialGravityAcceleration;

    [Header("Advanced physics")]
    [SerializeField, Tooltip("Gravitymultiplier on descend.")]
    float GMDescend = 1;
    [SerializeField, Tooltip("Gravity multiplier on FallInput().")]
    float GMFall = 1;
    [SerializeField, Tooltip("Instant drop on FallInput().")]
    bool dropOnFall;
    [SerializeField, Tooltip("When hitting a wall, apply the inverse velocity x multiplied with the bounce factor.")]
    float horizontalBounce;
    [SerializeField, Tooltip("When hitting the ground, apply the inverse velocity y multiplied with the bounce factor.")]
    float verticalBounce;
    [SerializeField, Tooltip("The amount of frames where you can still jump after leaving ground.")]
    float jackalFrames;
    float activeJackalFrames;
    bool fall;


    [Header("Other")]
    [SerializeField]
    private bool groundOnSpawn = false;
    [SerializeField]
    private string landingSound;

    private Vector2 velocity;

    public bool grounded;
    private bool ceiled;

    private float horizontalInput;
    private float verticalInput;
    private bool jumpInput;

    private bool ascending = false;
    private bool descending = false;

    private bool oldGrounded = false;

    private Vector2 oldPosition;

    [SerializeField]
    private bool collideWithPlatform = false;

    public bool GroundOnSpawn
    {
        set { groundOnSpawn = value; }
    }

    public bool CollideWithPlatform
    {
        get { return collideWithPlatform; }
        set { collideWithPlatform = value; }
    }

    private MultiPopper<string, Vector2> velocityStack =
        new MultiPopper<string, Vector2>((x, y) => x == y, (x, y) => x + y, "");

    public bool Ascending
    {
        get { return ascending; }
    }

    public bool Descending
    {
        get { return descending; }
    }

    public float GravetyAcceleration
    {
        set { this.gravityAcceleration = value; }
        get { return this.gravityAcceleration; }
    }

    public bool Grounded
    {
        get { return grounded; }
        private set { 
            grounded = value;
            if (value)
            {
                fall = (gravityAcceleration < 0) ? false : true;
                velocity.y = verticalBounce * -velocity.y;
                if (gravityAcceleration < 0)
                    activeJackalFrames = jackalFrames;
                Debug.Log("BOUNCE");
            }
        }
    }

    public bool Ceiled
    {
        get { return ceiled; }
        set
        {
            ceiled = value;
            if (ceiled)
            {
                fall = (gravityAcceleration > 0) ? false : true;
                velocity.y = verticalBounce * -velocity.y;
                if (gravityAcceleration > 0)
                    activeJackalFrames = jackalFrames;
            }
        }
    }

    public float JumpHeight
    {
        get { return jumpHeight; }
    }

    public void AddVelocity(string key, Vector2 velocity)
    {
        velocityStack.Add(key, velocity);
    }
    public void AddVelocity(Vector2 velocity)
    {
        velocityStack.Add("input", velocity);
    }

    public void VerticalInput(float factor)
    {
        verticalInput = factor;
    }

    public void HorizontalInput(float factor)
    {
        horizontalInput = factor;
    }

    public void JumpInput(float factor)
    {
        if ((Grounded && gravityAcceleration < 0) || activeJackalFrames > 0)
        {
            AddVelocity("jump", new Vector2(0, Mathf.Sqrt(2 * Mathf.Abs(JumpHeight) * factor * Mathf.Abs(GravetyAcceleration))));
            activeJackalFrames = 0;
            Grounded = false;
        } else if ((Ceiled && gravityAcceleration > 0) || activeJackalFrames > 0)
        {
            AddVelocity("jump", -1 * new Vector2(0, Mathf.Sqrt(2 * Mathf.Abs(JumpHeight) * factor * Mathf.Abs(GravetyAcceleration))));
            activeJackalFrames = 0;
            Ceiled = false;
        }
            
    }

    public void FallInput()
    {
        fall = true;
        if (dropOnFall && ascending)
            velocity.y = 0;
    }

    public Vector2 Velocity
    {
        get { return new Vector2(velocity.x, velocity.y); }
    }

    void Start()
    {
        Cleanup();
        initialGravityAcceleration = GravetyAcceleration;

        if (boxCollider == null)
            boxCollider = gameObject.GetComponent<BoxCollider2D>();
        if (groundOnSpawn && boxCollider == null)
            throw new System.Exception("Cannot ground a body that has no box collider!");

        // Ground Body!
        if (!groundOnSpawn)
            return;
        grounded = false;
        oldGrounded = true;
        for (int i = 0; (i < 10 && !grounded && groundOnSpawn); i++)
        {
            // Materialize velocity
            transform.Translate(Vector2.down);

            // Detect collision
            Collider2D[] hits = ColliderController.OverlapBoxAllWithFlags(transform.position, boxCollider.size, ColliderFlags);
            Collider2D[] hitsPlatform = ColliderController.OverlapBoxAllWithFlags(transform.position, boxCollider.size, PlatformFlags);
            grounded = false;
            ceiled = false;

            // Handle collision
            foreach (Collider2D hit in hits)
            {
                if (hit == boxCollider)
                    continue;

                ColliderDistance2D colliderDistance = hit.Distance(boxCollider);

                if (colliderDistance.isOverlapped)
                {
                    transform.Translate(colliderDistance.pointA - colliderDistance.pointB);
                }

                // Is grounded
                if (Vector2.Angle(colliderDistance.normal, Vector2.up) < 90 && velocity.y < 0 &&
                    Mathf.Abs(oldPosition.y - boxCollider.transform.position.y) < 0.0001f)
                {
                    Grounded = true;
                }

                // Is ceiled
                if (Vector2.Angle(colliderDistance.normal, Vector2.down) < 90 && velocity.y > 0 &&
                    Mathf.Abs(oldPosition.y - boxCollider.transform.position.y) < 0.0001f)
                {
                    ceiled = true;
                }
            }

            // Collision hit for platform
            // TODO: Make this work!!!
            foreach (Collider2D hit in hitsPlatform)
            {
                if (hit == boxCollider || collideWithPlatform)
                    continue;

                BoxCollider2D h = hit as BoxCollider2D;
                if ((boxCollider.transform.position.x + boxCollider.size.x / 2 >
                    hit.transform.position.x - h.size.x / 2 ||
                    boxCollider.transform.position.x - boxCollider.size.x / 2 <
                    hit.transform.position.x + h.size.x / 2) &&
                    (boxCollider.transform.position.y - boxCollider.size.y / 2 >
                    hit.transform.position.y + h.size.y / 2) && descending)
                {
                    ColliderDistance2D colliderDistance = hit.Distance(boxCollider);

                    if (colliderDistance.isOverlapped)
                    {
                        transform.Translate(colliderDistance.pointA - colliderDistance.pointB);
                        Grounded = true;
                    }
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (activeJackalFrames > 0)
            activeJackalFrames--;

        velocity += velocityStack.MultiPop(new Vector2());

        // Applying gravety acceleration
        velocity.y += gravityAcceleration * Time.fixedDeltaTime;
        if ((descending && gravityAcceleration < 0) || (ascending && gravityAcceleration > 0))
            velocity.y += gravityAcceleration * Time.fixedDeltaTime * (GMDescend - 1);
        if (fall)
            velocity.y += gravityAcceleration * Time.fixedDeltaTime * (GMFall - 1);

        // Apply movement
        if (horizontalInput != 0)
        {
            if ((Grounded && gravityAcceleration < 0) || (Ceiled && gravityAcceleration > 0))
            {
                velocity.x = Mathf.MoveTowards(velocity.x, speed * horizontalInput, walkAcceleration * Time.fixedDeltaTime);
            } else
            {
                velocity.x = Mathf.MoveTowards(velocity.x, speed * horizontalInput, airAcceleration * Time.fixedDeltaTime);
            }
            
        }
        else
        {
            if ((Grounded && gravityAcceleration < 0) || (Ceiled && gravityAcceleration > 0))
            {
                velocity.x = Mathf.MoveTowards(velocity.x, 0, groundDeceleration * Time.fixedDeltaTime);
            } else
            {
                velocity.x = Mathf.MoveTowards(velocity.x, 0, airAcceleration * Time.fixedDeltaTime);
            }
        }

        // Materialize velocity
        transform.Translate(velocity * Time.fixedDeltaTime);

        if (boxCollider == null)
            return;

        // Detect collision
        Vector3 boxOffset = new Vector2(boxCollider.offset.x, boxCollider.offset.y);
        Collider2D[] hits = ColliderController.OverlapBoxAllWithFlags(boxCollider.transform.position, boxCollider.size, ColliderFlags);
        Collider2D[] hitsPlatform = ColliderController.OverlapBoxAllWithFlags(boxCollider.transform.position, boxCollider.size, PlatformFlags);
        grounded = false;
        ceiled = false;

        Vector3 posBeforeCollision = boxCollider.transform.position;
        bool walled = false;

        // Handle collision
        foreach (Collider2D hit in hits)
        {
            if (hit == boxCollider)
                continue;

            //Debug.Log("collission");

            ColliderDistance2D colliderDistance = hit.Distance(boxCollider);

            if (colliderDistance.isOverlapped)
            {
                transform.Translate(colliderDistance.pointA - colliderDistance.pointB);
            }
        }

        Vector3 change = boxCollider.transform.position - posBeforeCollision;
        if (hits != null && hits.Length != 0 && change != Vector3.zero)
        {
            // Is grounded
            if (Vector2.Angle(change, Vector2.up) == 0 && velocity.y < 0)
            {
                if (!oldGrounded && landingSound != "")
                    AudioPlay.PlaySound(landingSound, 1);
                // If grounded
                Grounded = true;
            } else if (Vector2.Angle(change, Vector2.down) == 0 && velocity.y > 0)
            {
                // If ceiled
                Ceiled = true;
            } else if (Vector2.Angle(change, Vector2.left) == 0 && velocity.x > 0)
            {
                // If walled
                velocity.x = horizontalBounce * -velocity.x;
            } else if (Vector2.Angle(change, Vector2.right) == 0 && velocity.x < 0)
            {
                // If walled
                velocity.x = horizontalBounce * -velocity.x;
            } else
            {
                // If walled + (grounded or ceiled)
                velocity.x = horizontalBounce * -velocity.x;
                if (Vector2.Angle(change, Vector2.up) < 5)
                {
                    Grounded = true;
                } else
                {
                    Ceiled = true;
                }
            }
        }

        ascending = false;
        descending = false;

        if (!grounded)
        {
            ascending = (oldPosition.y < boxCollider.transform.position.y) ? true : false;
            descending = (oldPosition.y > boxCollider.transform.position.y) ? true : false;
        }

        // Collision hit for platform
        foreach (Collider2D hit in hitsPlatform)
        {
            if (hit == boxCollider || !collideWithPlatform)
                continue;
        
            BoxCollider2D h = hit as BoxCollider2D;
        
            if ((boxCollider.transform.position.x + boxCollider.size.x / 2 >
                hit.transform.position.x - (h.size.x / 2) * h.transform.lossyScale.x ||
                boxCollider.transform.position.x - boxCollider.size.x / 2 <
                hit.transform.position.x + (h.size.x / 2) * h.transform.lossyScale.x) &&
        
                (oldPosition.y - boxCollider.size.y / 2 >
                hit.transform.position.y + (h.size.y / 2) * h.transform.lossyScale.y) && descending)
            {
                ColliderDistance2D colliderDistance = hit.Distance(boxCollider);
        
                if (colliderDistance.isOverlapped)
                {
                    transform.Translate(colliderDistance.pointA - colliderDistance.pointB);
        
                    if (!oldGrounded && landingSound != "")
                        AudioPlay.PlaySound(landingSound, 1);

                    Grounded = true;
                    descending = false;
                    ascending = false;
                }
            }
        }

        oldPosition = boxCollider.transform.position;
        oldGrounded = grounded;

        Cleanup();
    }

    void Cleanup()
    {
        horizontalInput = 0;
        verticalInput = 0;
        jumpInput = false;
    }
}



/*
        Vector3 change = boxCollider.transform.position - posBeforeCollision;
        if (hits != null && hits.Length != 0 && change != Vector3.zero)
        {
            // Is grounded
            if (Vector2.Angle(change, Vector2.up) == 0 && velocity.y < 0 &&
                Mathf.Abs(oldPosition.y - boxCollider.transform.position.y) < 0.000001f && !Grounded)
            {
                if (!oldGrounded && landingSound != "")
                    AudioPlay.PlaySound(landingSound, 1);
                // If grounded
                Grounded = true;
            } else if (Vector2.Angle(change, Vector2.down) == 0 && velocity.y > 0 &&
                Mathf.Abs(oldPosition.y - boxCollider.transform.position.y) < 0.000001f && !Ceiled)
            {
                // If ceiled
                Ceiled = true;
            } else if (Vector2.Angle(change, Vector2.left) == 0 && !walled && oldPosition.x < posBeforeCollision.x)
            {
                // If walled
                walled = true;
                velocity.x = horizontalBounce * -velocity.x;
            } else if (Vector2.Angle(change, Vector2.right) == 0 && !walled && oldPosition.x > posBeforeCollision.x)
            {
                // If walled
                walled = true;
                velocity.x = horizontalBounce * -velocity.x;
            } else
            {
                // If walled + (grounded or ceiled)
                velocity.x = horizontalBounce * -velocity.x;
                if (Vector2.Angle(change, Vector2.up) < 5)
                {
                    Grounded = true;
                } else
                {
                    Ceiled = true;
                }
            }
        }

 */
/*
            // Is grounded
            if (Vector2.Angle(colliderDistance.normal, Vector2.up) < 90 && velocity.y < 0 &&
                Mathf.Abs(oldPosition.y - boxCollider.transform.position.y) < 0.000001f)
            {
                if (!oldGrounded  && landingSound != "")
                    AudioPlay.PlaySound(landingSound, 1);
                Grounded = true;
                Debug.Log("grounded");
            }

            // Is ceiled
            if (Vector2.Angle(colliderDistance.normal, Vector2.down) < 90 && velocity.y > 0 &&
                Mathf.Abs(oldPosition.y - boxCollider.transform.position.y) < 0.000001f)
            {
                Ceiled = true;
                Debug.Log("Ceiled");
            }

            if (Vector2.Angle(colliderDistance.normal, Vector2.left) < 90 &&
                Mathf.Abs(oldPosition.x - boxCollider.transform.position.x) < 0.000001f)
            {
                velocity.x = bounce * -velocity.x;
                Debug.Log("WALLED");
            }
            if (Vector2.Angle(colliderDistance.normal, Vector2.right) < 90 &&
                Mathf.Abs(oldPosition.x - boxCollider.transform.position.x) < 0.000001f)
            {
                velocity.x = bounce * -velocity.x;
                Debug.Log("WALLED");
            }
*/
