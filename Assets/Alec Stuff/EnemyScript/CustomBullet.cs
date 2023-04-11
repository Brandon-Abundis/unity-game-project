using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomBullet : MonoBehaviour
{

    // Assignables
    public Rigidbody rb;
    public GameObject explosion;
    public GameObject whatisEnemies;

    // Stats
    [Range(0f,1f)]
    public float bounciness;
    public bool useGravity;

    // Damage
    public int explosionDamage;
    public float explosionRange;

    // Lifetime
    public int maxCollisions;
    public float maxLifetime;
    public bool explosionOnTouch = true;

    int collisions;
    PhysicMaterial physics_mat;

    private void Start() 
    {
        Setup();
    }

    private void Update()
    {
        // When to explode
        if (collisions > maxCollisions) Explode();

        maxLifetime -= Time.deltaTime;
        if (maxLifetime <= 0) Explode();

    }

    private void Explode() 
    {
        if (explosion != null) Instantiate(explosion, transform.position, Quaternion.identify);

        // Check for enemies
        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange, whatisEnemies);

        
    }

    private void OnCollisionEnter(Collision collision)
    {

    }

    private void Setup()
    {
        // create a Physic material
        physics_mat = new PhysicMaterial();
        physics_mat.bounciness = bounciness;
        physics_mat.frictionCombine = PhysicMaterialCombine.Minimum;
        physics_mat.bounceCombine = PhysicMaterialCombine.Maximum;

        // Allign material collider
        GetComponent<SphereCollider>().material = physics_mat;

        // Set Gravity
        rb.useGravity = useGravity;
    }
}
