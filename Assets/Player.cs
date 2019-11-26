using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    public Weapon weapon;
    public float moveSpeed;
    [SyncVar]
    public float currentLife;
    public float maxLife;
    public CharacterController controller;
    public float rotationSpeed;
    public bool collided = false;

    void Start()
    {
        currentLife = maxLife;
        controller = GetComponent<CharacterController>();
        
    }

    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Move(input);

        LookAtMouse();

        if (Input.GetMouseButtonDown(0))
        {
            weapon.canShoot = true;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            weapon.canShoot = false;
        }

        if(collided)
        {
            CmdBulletCollidePlayer();
            collided = false;
        }

        weapon.timer += Time.deltaTime;
        if (weapon.timer > weapon.fireInterval && weapon.canShoot)
        {
            Shoot();
            weapon.timer = 0;

        }

    }

    public void TakeDamage(float amount)
    {
        currentLife -= amount;
        Debug.Log(transform.name + " tem: " + currentLife + "de vida");
    }

    public void Move(Vector2 axis)
    {
        Vector3 translate = new Vector3(axis.x, 0, axis.y);
        controller.Move(translate * moveSpeed * Time.deltaTime);
    }
    void LookAtMouse()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, new Vector3(0, transform.position.y, 0));
        float rayLenght;
        if (groundPlane.Raycast(cameraRay, out rayLenght))
        {
            Vector3 pointToLook = new Vector3(cameraRay.GetPoint(rayLenght).x, cameraRay.GetPoint(rayLenght).y, cameraRay.GetPoint(rayLenght).z - 0.5f);

            Vector3 direction = (pointToLook - transform.position).normalized;

            LookAt(direction);

        }
    }

    public void LookAt(Vector3 direction)
    {
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }
   

    [Command]
    public void CmdBulletCollidePlayer()
    {
        currentLife -= 10;
        Debug.Log(currentLife);
    }

    [Command]
    public void CmdBulletCollideScene(string name)
    {
        Debug.Log("tiro colidiu com uma" + name);
    }

    public void Shoot()
    {
        GameObject bullet = Instantiate(weapon.bulletPrefab, weapon.firePosition.position, weapon.transform.rotation);
        bullet.GetComponent<Bullet>().speed = weapon.bulletSpeed;
        bullet.GetComponent<Bullet>().damage = weapon.damage;
        NetworkServer.Spawn(bullet);
    }
}
