using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace CommonOfDamini.Lives
{
    public class Manager_Lives : MonoBehaviour
    {
        public static Manager_Lives instance;
        #region VARIABLES
        #region INSPECTOR_VARIABLES
        [Space(10)]
        [Header("<========= REFERENCES =======>")]
        [Space(3)]
        public int i;
        #endregion

        #region PUBLIC_VARIABLES
        public float lifeAddInDuration = 15;
        int maxLives = 5;
        #endregion

        #region PRIVATE_VARIABLES
        #endregion
        
        #region PROTECTED_VARIABLES
        #endregion

        #region GETTER/SETTER
        int _lives;
        int lives
        {
            get
            {
                return PlayerPrefs.GetInt("lives", maxLives);
            }
            set
            {
                if (value < 0)
                    value = 0;
                if (value >= maxLives)
                {
                    value = maxLives;
                }
                PlayerPrefs.SetInt("lives", value);
                UpdatelivesUI();
            }

        }

        bool _livesAddTimerStarted;
        public bool livesAddTimerStarted
        {
            get
            {
                Debug.Log("Get lives = " + _livesAddTimerStarted);
                if (SaveLoad.CheckIfFileExists("_livesAddTimerStarted"))
                {
                    _livesAddTimerStarted = SaveLoad.LoadData(_livesAddTimerStarted, "_livesAddTimerStarted");
                }
                else
                {
                    livesAddTimerStarted = false;
                }

                if (_livesAddTimerStarted && CheckIfTimerfinished())
                {
                    Debug.Log(" Timer complete on lives get = " + _livesAddTimerStarted);

                    OnTimercompleteOfLivesAdd();
                }
                return _livesAddTimerStarted;
            }
            set
            {
                if (_livesAddTimerStarted && !value)
                {
                    //OnTimercompleteOfLivesAdd();
                }
                _livesAddTimerStarted = value;
                SaveLoad.SaveData(_livesAddTimerStarted, "_livesAddTimerStarted");
           }
        }


        System.DateTime _startLivesAddTimer;
        public System.DateTime startLivesAddTimer
        {
            get
            {
                return SaveLoad.LoadData(_startLivesAddTimer, "_startLivesAddTimer");
            }
            set
            {
                _startLivesAddTimer = value;
                SaveLoad.SaveData(_startLivesAddTimer, "_startLivesAddTimer");
            }
        }
        #endregion

        #region ENUMS
        #endregion

        #region CLASS
        #endregion

        #region STRUCT
        #endregion

        #region DELEGATE/EVENT
        public delegate void UpdatelivesAction();
        public event UpdatelivesAction updatelivesEvent;

        public delegate void LivesDecreasedAction(System.DateTime dateTime);
        public event LivesDecreasedAction startTimerEvent;

        #endregion
        #endregion

        #region METHODS
        #region UNITY_METHODS
        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(this);

            lifeAddInDuration = 15f;
            maxLives = 5;

        }

        private void Start()
        {
            //Loadlives();
        }
        #endregion

        #region INIT_METHODS
        /// <summary>
        /// Add all values which will be initalized for once only
        /// </summary>
        void InitValues()
        {
        }

        /// <summary>
        /// Reset values at each game start, over
        /// </summary>
        void ResetValues()
        {
        }

        /// <summary>
        /// On Play Mode Start
        /// </summary>
        public void OnPlayModeStart()
        {
            ResetValues();
        }

        /// <summary>
        /// On GameOver
        /// </summary>
        public void OnGameOver()
        {
            ResetValues();
        }
        #endregion

        #region ON_CLICK_METHODS
        #endregion

        #region SET_UI
        #endregion

        #region LOGIC_BASED_METHODS
        public int GetCurrentlives()
        {
            return lives;
        }

        public bool GetCurrentTimer(ref System.DateTime dateTime)
        {
            if (lives >= maxLives)
                return false;
            dateTime = startLivesAddTimer;
            return livesAddTimerStarted;
        }

        public void OnTimercompleteOfLivesAdd()
        {
            livesAddTimerStarted = false;
            Addlives(1);
           
        }

        void Loadlives()
        {
            UpdatelivesUI();
            CheckAndStartTimer();
        }

        void CheckAndStartTimer()
        {
            if (livesAddTimerStarted)
            {
                CallStartTimerEvent(startLivesAddTimer);
            }
        }

        public bool IsLivesAvailable()
        {
            return lives > 0;
        }

        public bool CheckIfLivesAreFull()
        {
            return lives >= maxLives;
        }

        public void Addlives(int value)
        {
            Debug.Log("Addlives = " + value);
            

            lives += value;
            if (!CheckIfLivesAreFull())
            {
                StartTimer();
            }
            else
            {
                livesAddTimerStarted = false;
            }
        }

        bool CheckIfTimerfinished()
        {
            TimeSpan timeSpan = (DateTime.Now - startLivesAddTimer);
            double currentTime = timeSpan.TotalSeconds;
            Debug.Log("timeDiff = " + currentTime);


            double timeDiff = (lifeAddInDuration * 60f) - currentTime;
            Debug.Log("timeDiff = " + timeDiff);

            return timeDiff <= 0;
           // return ((System.DateTime.Now - startLivesAddTimer).Minutes >= lifeAddInDuration);
        }

        public bool Removelives(int value)
        {
            if (lives - value < 0)
                return false;

            Debug.Log("Removelives = " + value);

          //  ManagerEffects.Instance.PlayUiCoinPulse();
            lives -= value;
            StartTimer();
            return true;
        }

        void StartTimer()
        {
            if (livesAddTimerStarted == true && CheckIfTimerfinished())
            {
                Debug.Log("----------------------------Timer Finished---------------");
                startLivesAddTimer = System.DateTime.Now;
            }else if (!livesAddTimerStarted)
            {
                startLivesAddTimer = System.DateTime.Now;
            }
            livesAddTimerStarted = true;

            CallStartTimerEvent(startLivesAddTimer);
        }

        void CallStartTimerEvent(System.DateTime dateTime)
        {
            startTimerEvent?.Invoke(dateTime);
        }

        void UpdatelivesUI()
        {
            updatelivesEvent?.Invoke();
        }
        #endregion
        #endregion

        #region COROUTINES
        #endregion
    }
}
