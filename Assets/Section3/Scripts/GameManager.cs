using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Section3
{
    public enum GameState
    {
        Home,
        Gameplay,
        Pause,
        Gameover
    }

    public class GameManager : MonoBehaviour
    {
        private static GameManager m_Instance;
        public static GameManager Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = FindObjectOfType<GameManager>();
                return m_Instance;
            }
        }

        public Action<int> onScoreChanged;

        [SerializeField] private HomePanel m_HomePanel;
        [SerializeField] private GameplayPanel m_GameplayPanel;
        [SerializeField] private PausePanel m_PausePanel;
        [SerializeField] private GameoverPanel m_GameoverPanel;
        [SerializeField] private WaveData[] m_Waves;

        //private AudioManager m_AudioManager;
        //private SpawnManager m_SpawnManager;
        private GameState m_GameState;
        private bool m_Win;
        private int m_Score;
        private int m_CurWaveIndex;

        private void Awake()
        {
            if (m_Instance == null)
                m_Instance = this;
            else if (m_Instance != this)
                Destroy(gameObject);
        }

        // Start is called before the first frame update
        void Start()
        {
            //m_AudioManager = FindObjectOfType<AudioManager>();
            //m_SpawnManager = FindObjectOfType<SpawnManager>();
            m_HomePanel.gameObject.SetActive(false);
            m_GameplayPanel.gameObject.SetActive(false);
            m_PausePanel.gameObject.SetActive(false);
            m_GameoverPanel.gameObject.SetActive(false);
            SetState(GameState.Home);
        }

        private void SetState(GameState state)
        {
            m_GameState = state;
            m_HomePanel.gameObject.SetActive(m_GameState == GameState.Home);
            m_GameplayPanel.gameObject.SetActive(m_GameState == GameState.Gameplay);
            m_PausePanel.gameObject.SetActive(m_GameState == GameState.Pause);
            m_GameoverPanel.gameObject.SetActive(m_GameState == GameState.Gameover);

            if (m_GameState == GameState.Pause)
                Time.timeScale = 0;
            else
                Time.timeScale = 1;

            if (m_GameState == GameState.Home)
                AudioManager.Instance.PlayHomeMusic();
            else
                AudioManager.Instance.PlayBattleMusic();
        }

        public bool IsActive()
        {
            return m_GameState == GameState.Gameplay;
        }

        public void Play()
        {
            m_CurWaveIndex = 0;
            WaveData wave = m_Waves[m_CurWaveIndex];
            SpawnManager.Instance.StartBattle(wave, true);
            SetState(GameState.Gameplay);
            m_Score = 0;
            if (onScoreChanged != null)
                onScoreChanged(m_Score);
        }

        public void Pause()
        {
            SetState(GameState.Pause);
        }

        public void Home()
        {
            SetState(GameState.Home);
            SpawnManager.Instance.Clear();
        }

        public void Continue()
        {
            SetState(GameState.Gameplay);
        }

        public void Gameover(bool win)
        {
            int curHighScore = PlayerPrefs.GetInt("HighScore");
            if (curHighScore < m_Score)
            {
                PlayerPrefs.SetInt("HighScore", m_Score);
                curHighScore = m_Score;
            }

            m_Win = win;
            SetState(GameState.Gameover);
            m_GameoverPanel.DisplayResult(m_Win);
            m_GameoverPanel.DisplayHighScore(curHighScore);
        }

        public void AddScore(int value)
        {
            m_Score += value;
            if (onScoreChanged != null)
                onScoreChanged(m_Score);

            if (SpawnManager.Instance.IsClear())
            {
                m_CurWaveIndex++;
                if (m_CurWaveIndex >= m_Waves.Length)
                    Gameover(true);
                else
                {
                    WaveData wave = m_Waves[m_CurWaveIndex];
                    SpawnManager.Instance.StartBattle(wave, false);
                }
            }
        }
    }
}