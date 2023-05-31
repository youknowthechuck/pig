using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BankConfig")]
public class BankConfig : ScriptableObject
{
    public GameObject bankPrefab;
    public GameObject bankTextPrefab;
}