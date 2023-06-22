using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using CommonOfDamini;
using System;

namespace CommonOfDamini.Lives
{
    public class UILife : MonoBehaviour
    {
        #region VARIABLES
        #region INSPECTOR_VARIABLES
        [SerializeField] Transform lifeTransform;
        [SerializeField] Transform withTimerEnablePositionOfLife;
        [SerializeField] Transform withoutTimerEnablePositionOfLife;

        [SerializeField] GameObject lifeUpdateTimerHolder;
        [SerializeField] TextMeshProUGUI livesCounterText;
        [SerializeField] TextMeshProUGUI lifeUpdatetimerText;
        #endregion

        #region PUBLIC_VARIABLES
        #endregion

        #region PRIVATE_VARIABLES
        RectTransform _parentRectTransform;
        Coroutine _timerCoroutine;
        #endregion

        #region PROTECTED_VARIABLES
        #endregion

        #region GETTER/SETTER
        #endregion

        #region ENUMS
        #endregion

        #region CLASS
        #endregion

        #region STRUCT
        #endregion

        #region DELEGATE/EVENT
        #endregion
        #endregion

        #region METHODS
        #region UNITY_METHODS
        private void Awake()
        {
            _parentRectTransform = transform.parent.GetComponent<RectTransform>();
        }
        private void OnEnable()
        {
            Manager_Lives.instance.updatelivesEvent += UpdatelifeUI;
            Manager_Lives.instance.startTimerEvent += OnLifeDecreased;
            UpdatelifeUI();
        }
        private void OnDisable()
        {
            Manager_Lives.instance.updatelivesEvent -= UpdatelifeUI;
            Manager_Lives.instance.startTimerEvent -= OnLifeDecreased;
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
        void UpdatelifeUI()
        {
            Debug.Log("livesCounterText = " + livesCounterText + "  " + Manager_Lives.instance.GetCurrentlives());
            Debug.Log("Manager_Lives.instance = " + Manager_Lives.instance);

            livesCounterText.text = Manager_Lives.instance.GetCurrentlives() + "";
            DateTime dateTime = DateTime.Now;
            if (Manager_Lives.instance.GetCurrentTimer(ref dateTime))
            {
                OnLifeDecreased(Manager_Lives.instance.startLivesAddTimer);
            }else
            {
                Debug.Log(" ************&&&&&&&&&&&&&&&&&&&&&&& = ");

                StopTimer();
               
            }
           
            LayoutRebuilder.ForceRebuildLayoutImmediate(_parentRectTransform);
        }

        void OnLifeDecreased(DateTime time)
        {
            Debug.Log("OnLifeDecreased");
            StartTimer(time);
        }

        void OpenTimerHolder()
        {
            lifeUpdateTimerHolder.gameObject.Open();
            lifeTransform.position = withTimerEnablePositionOfLife.position;
        }

        void CloseTimerHolder()
        {
            lifeUpdateTimerHolder.gameObject.Close();
            lifeTransform.position = withoutTimerEnablePositionOfLife.position;
        }

        void StartTimer(DateTime time)
        {
            StopTimer();
            OpenTimerHolder();
            _timerCoroutine = StartCoroutine(CommoanForAllGame.instance.TimerUpdate(60f * Manager_Lives.instance.lifeAddInDuration, time, TimerUpdateAction, onCompleteAction: OnTimerOutOfTime));
        }

        void TimerUpdateAction(string timerString, double timer, System.TimeSpan time)
        {
            lifeUpdatetimerText.text = timerString;
        }
        public float TotalSec;

        void OnTimerOutOfTime()
        {
            StopTimer();
            Manager_Lives.instance.OnTimercompleteOfLivesAdd();
        }

        void StopTimer()
        {
            CommoanForAllGame.instance.StopPassedCoroutine(_timerCoroutine);
            if (Manager_Lives.instance.CheckIfLivesAreFull())
            {
                Debug.Log("&&&&&&&&&&&&&&&&&&&&&&& = ");
                OpenTimerHolder();
                lifeUpdatetimerText.text = "FULL";
            }
            else
            {
                CloseTimerHolder();
            }
        }
        #endregion
        #endregion

        #region COROUTINES
        #endregion
    }
}
