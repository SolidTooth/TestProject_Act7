using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AniEventCtrl : MonoBehaviour
{
    [System.Serializable]
    public class AniEvent
    {
        public string eventTag;
        public UnityEvent aniEvent;
        public bool isEnable;
        [SerializeField]
        private bool isRepeat;
        public void eventCall()
        {
            isEnable = isRepeat;
            aniEvent?.Invoke();
        }
    }
    [SerializeField]
    private List<AniEvent> eventList = new List<AniEvent>();


    public void aniEventCall(string eventTag)
    {
        for (int i = 0; i < eventList.Count; i++)
        {
            if (eventList[i].isEnable && eventList[i].eventTag.Equals(eventTag))
            {
                eventList[i].eventCall();
            }
        }
    }

}