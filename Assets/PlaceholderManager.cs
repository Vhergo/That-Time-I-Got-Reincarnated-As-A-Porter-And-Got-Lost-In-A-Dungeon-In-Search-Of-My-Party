using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceholderManager : MonoBehaviour
{
    [Header("Torch")]
    [SerializeField] private int torchNumber;
    [SerializeField] private float torchDuration;

    [Header("Item Strength")]
    [SerializeField] private float goldStrength;
    [SerializeField] private float gemStrength;
    [SerializeField] private float weaponStrength;
    [SerializeField] private float armorStrength;
    [SerializeField] private float monsterDropStrength;

    public Dictionary<int, Tuple<GameObject, float>> torches = new Dictionary<int, Tuple<GameObject, float>>();
    public Dictionary<String,float> item_book = new Dictionary<String,float>();
    public static PlaceholderManager instance;
    private int nextTorch;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        item_book.Add("Gold", goldStrength);
        item_book.Add("Gem", gemStrength);
        item_book.Add("Weapon", weaponStrength);
        item_book.Add("Armor", armorStrength);
        item_book.Add("MonsterDrop", monsterDropStrength);

        setupTorches();
    }

    

    // Function places a torch at any placeholder and changes the strength of torch
    public void setTorch(GameObject gameObject)
    {
        if (torches[nextTorch].Item1 != null)
        {
            GameObject previousTorch = torches[nextTorch].Item1;
            float previousTorchStrength = previousTorch.GetComponent<Placeholder>().strength;
            torches[nextTorch] = new Tuple<GameObject, float>(gameObject, previousTorchStrength);
            gameObject.GetComponent<Placeholder>().strength = previousTorchStrength;
            previousTorch.GetComponent<Placeholder>().strength = 0;
        }
        else
        {
            float nextTorchStrength = torches[nextTorch].Item2;
            torches[nextTorch] = new Tuple<GameObject, float>(gameObject, nextTorchStrength);
            gameObject.GetComponent<Placeholder>().strength = nextTorchStrength;
        }
        checkTorch();
        updateNextTorch();
    }

    public void addTorchStrength(String itemTag)
    {
        
        foreach (int keys in torches.Keys)
        {
            Debug.Log(item_book[itemTag].ToString());
            if (torches[keys].Item1 != null)
            {
                float newStrength = torches[keys].Item1.GetComponent<Placeholder>().strength + item_book[itemTag];
                if(newStrength > 100)
                    newStrength = 100;
                torches[keys].Item1.GetComponent<Placeholder>().strength = 0;
                torches[keys].Item1.GetComponent<Placeholder>().strength = newStrength;
            }
        }
    }

    //Helper function to reset torches when reaching checkpoint
    public void resetTorch()
    {
        foreach(int keys in torches.Keys) {
            if(torches[keys].Item1 != null)
                torches[keys].Item1.GetComponent<Placeholder>().strength = 0;
        }
        setupTorches();
    }

    // Helper function to set up torches at start of game and when reset
    private void setupTorches()
    {
        torches.Clear();
        nextTorch = 1;

        //Dictionary Key is the torch number, game object is reference to the placeholder that is currently being used, second int in tuple represents the strength of torch (currently at 100%)
        for (int i = 0; i < torchNumber; i++)
        {
            torches.Add(i + 1, new Tuple<GameObject, float>(null, 100));
        }
    }

    // Helper function to check if all the torches are still lit
    private void checkTorch()
    {
        int torchCount = 0;
        foreach(KeyValuePair<int, Tuple<GameObject,float>> entry in torches)
        {
            if(entry.Value.Item2 == 0)
                torchCount++;
        }
        if(torchCount == torches.Count)
        {
            Debug.Log("Game Over");
        }
    }

    // Helper function to list the placeholder names and their strength
    private void printDict()
    {
        foreach (KeyValuePair<int, Tuple<GameObject, float>> entry in torches)
        {
            if (entry.Value.Item1 != null)
                Debug.Log(entry.Value.Item1.name + " is at strength " + entry.Value.Item2);
        }
    }

    // Helper function to change to next torch and skip torches that have 0 strength
    private void updateNextTorch()
    {

        if(nextTorch == torches.Count)
        {
            nextTorch = 1;
        }
        else
        {
            nextTorch++;
        }

        if(torches[nextTorch].Item2 == 0)
        {
            updateNextTorch();
        }
        else
        {
            return;
        }
    }


}
