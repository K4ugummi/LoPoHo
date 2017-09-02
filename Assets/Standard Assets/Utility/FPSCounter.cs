using System;
using UnityEngine;
using UnityEngine.UI;

namespace UnityStandardAssets.Utility
{
    [RequireComponent(typeof (Text))]
    public class FPSCounter : MonoBehaviour
    {
        const float fpsMeasurePeriod = 0.5f;
        private int m_FpsAccumulator = 0;
        private float m_FpsNextPeriod = 0;
        private int m_CurrentFps;
        [SerializeField]
        private Text m_Text;


        private void Start()
        {
            m_FpsNextPeriod = Time.realtimeSinceStartup + fpsMeasurePeriod;
        }


        private void Update()
        {
            // measure average frames per second
            m_FpsAccumulator++;
            if (Time.realtimeSinceStartup > m_FpsNextPeriod)
            {
                m_CurrentFps = (int) (m_FpsAccumulator/fpsMeasurePeriod);
                m_FpsAccumulator = 0;
                m_FpsNextPeriod += fpsMeasurePeriod;
                m_Text.text = "FPS: " + GetFPSColored(m_CurrentFps);
            }
        }

        string GetFPSColored(int _fps) {
            string _result = "<color=#";
            if (_fps < 8) {
                _result += "B40404";
            }
            else if (_fps < 16) {
                _result += "B43104"; 
            }
            else if (_fps < 24) {
                _result += "B45F04";
            }
            else if (_fps < 32) {
                _result += "B18904"; 
            }
            else if (_fps < 40) {
                _result += "AEB404"; 
            }
            else if (_fps < 48) {
                _result += "86B404";
            }
            else if (_fps < 56) {
                _result += "5FB404"; 
            }
            else if (_fps < 62) {
                _result += "31B404";
            }
            else {
                _result += "04B404";
            }
            return _result + ">" + _fps.ToString() + "</color>";
        }
    }
}
