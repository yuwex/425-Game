using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "Wave")]
public class Wave : ScriptableObject
{
    public List<SpawnInstance> group1;
    public List<SpawnInstance> group2;
    public List<SpawnInstance> group3;
    public List<SpawnInstance> group4;
    public int nextRoundDelay;
}
