using Base;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class TimerView : ViewBase
    {
        [SerializeField] private Text time;

        public void Refresh(float value)
        {
            time.text = Mathf.CeilToInt(value).ToString();
        }
    }
}