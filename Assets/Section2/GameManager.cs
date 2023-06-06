using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Section2
{
    [Serializable]
    public class QuestionData
    {
        public string question;
        public string answerA;
        public string answerB;
        public string answerC;
        public string answerD;
        public string correctAnswer;
    }
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_TxtQuestion, m_TxtAnswerA, m_TxtAnswerB, m_TxtAnswerC, m_TxtAnswerD;

        [SerializeField] private QuestionData[] m_QuestionDatas;
        [SerializeField] private Image m_ImgAnswerA, m_ImgAnswerB, m_ImgAnswerC, m_ImgAnswerD;
        [SerializeField] private TextMeshProUGUI m_CountQuestion;
        private int m_Live;
        private int m_indexCurrent;
        private bool m_TrueAnswer = false;
        // Start is called before the first frame update
        void Start()
        {
            m_Live = 3;
            m_indexCurrent = 0;
            InitQuestion(m_indexCurrent);
        }

        // Update is called once per frame
        void Update()
        {

        }
        public void InitQuestion(int index)
        {
            if (index >= m_QuestionDatas.Length && index <= 0) return;
            m_TxtQuestion.text = m_QuestionDatas[index].question;
            m_TxtAnswerA.text = "A" + m_QuestionDatas[index].answerA;
            m_TxtAnswerB.text = "B" + m_QuestionDatas[index].answerB;
            m_TxtAnswerC.text = "C" + m_QuestionDatas[index].answerC;
            m_TxtAnswerD.text = "D" + m_QuestionDatas[index].answerD;
            m_CountQuestion.text = m_indexCurrent.ToString();
            m_ImgAnswerA.color = Color.white;
            m_ImgAnswerB.color = Color.white;
            m_ImgAnswerC.color = Color.white;
            m_ImgAnswerD.color = Color.white;
        }
        public void BtnAnswer_Pressed(string pSelect)
        {
            if (pSelect == m_QuestionDatas[m_indexCurrent].correctAnswer)
            {
                Debug.Log("dung");
                m_TrueAnswer = true;
                m_indexCurrent++;
                InitQuestion(m_indexCurrent);
            }
            else
            {
                Debug.Log("sai");
                m_TrueAnswer = false;
                m_Live--;
            }


            switch (pSelect)
            {
                case "a":
                    m_ImgAnswerA.color = (m_TrueAnswer) ? Color.green : Color.red;
                    break;
                case "b":
                    m_ImgAnswerB.color = (m_TrueAnswer) ? Color.green : Color.red;
                    break;
                case "c":
                    m_ImgAnswerC.color = (m_TrueAnswer) ? Color.green : Color.red;
                    break;
                case "d":
                    m_ImgAnswerD.color = (m_TrueAnswer) ? Color.green : Color.red;
                    break;

            



            }
            if (m_indexCurrent >= m_QuestionDatas.Length && m_Live > 0)
            {
                Debug.Log("Dang chay >=");
            }
        }
    }
}