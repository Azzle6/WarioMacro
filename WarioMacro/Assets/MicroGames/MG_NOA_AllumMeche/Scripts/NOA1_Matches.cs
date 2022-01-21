using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NOA1_Matches : MonoBehaviour
{
   
   private Vector3 AngleVelocity;
   [SerializeField]private Vector2 AngleRange;
   [SerializeField] private Rigidbody rb;
   [SerializeField] private GameObject fire;
   public bool isOnFire = true;

   private void Start()
   {
      AngleVelocity.z = Random.Range(AngleRange.x, AngleRange.y);
   }

   private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.name == "Drop(Clone)")
      {
         fire.SetActive(false);
         isOnFire = false;
      }
   }

   private void OnCollisionEnter(Collision other)
   {
      AngleVelocity = Vector3.zero;
   }

   private void Update()
   {
      Quaternion deltaRotation = Quaternion.Euler(AngleVelocity * Time.fixedDeltaTime);
      rb.MoveRotation(rb.rotation * deltaRotation);
   }
}
