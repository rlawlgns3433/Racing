using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/CarData", fileName = "Car Data")]
public class CarData : ScriptableObject
{
    public float power           = 400f;
    public float rot              = 45f;
    public float axiss            = 0f;
    public double velocity         = 0;
    public float currentVelocity  = 0;
    public float maxSpeed         = 400f;
    public float car_durability = 100f;
}
