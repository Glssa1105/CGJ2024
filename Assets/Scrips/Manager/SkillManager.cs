using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public GameObject skillPrefab;

    public Dictionary<ITriggerComponent, Image> triggerMap = new Dictionary<ITriggerComponent, Image>();
    public Dictionary<IHoldComponent, Image> holdMap = new Dictionary<IHoldComponent, Image>();


    private void Update()
    {
        List<ITriggerComponent> removeList = new List<ITriggerComponent>();
        List<IHoldComponent> removeHoldList = new List<IHoldComponent>();
        foreach (var kvp in triggerMap)
        {
            kvp.Value.color = Color.Lerp(Color.red, Color.green, kvp.Key.EnergyProgress);
            Debug.Log(kvp.Value.color);
            if (kvp.Key.EnergyProgress <= 0)
            {
                removeList.Add(kvp.Key);
            }
        }
        foreach (var kvp in holdMap)
        {
            kvp.Value.color = Color.Lerp(Color.red, Color.green, kvp.Key.EnergyProgress);
            if (kvp.Key.EnergyProgress <= 0)
            {
                removeHoldList.Add(kvp.Key);
            }
        }

        foreach (var item in removeList)
        {
            Destroy(triggerMap[item].gameObject);
            triggerMap.Remove(item);
        }
        foreach (var item in removeHoldList)
        {
            Destroy(holdMap[item].gameObject);
            holdMap.Remove(item);
        }
    }


    public void SpawnSkill<T>(Item item,KeyCode keyName, T obj)
    {
        var skill = Instantiate(skillPrefab, transform.position, Quaternion.identity,transform);
        
        var back = skill.GetComponent<Image>();
        
        var image =skill.transform.GetChild(0).GetComponent<Image>();
        image.sprite = item.icon;
        image.SetNativeSize();
        var text = skill.transform.GetChild(1).GetComponent<TMP_Text>();
        text.text = keyName.ToString();


        if (obj is ITriggerComponent trigger)
        {
            //var trigger = tri ?? throw new InvalidOperationException();
            triggerMap.Add(trigger,back);
            Debug.Log(triggerMap.Count);

        }
        else if(obj is IHoldComponent hold)
        {
            holdMap.Add(hold,back);

        }

        

    }
}
