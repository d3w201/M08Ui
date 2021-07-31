/*
The MIT License

Copyright (c) 2020 DoublSB
https://github.com/DoublSB/UnityDialogAsset

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using Entity.Dialog;
using Enumeral;
using UnityEngine;
using UnityEngine.UI;

namespace Controller.Dialog
{
    public class DialogController : RootController
    {
        //Public-props
        [Header("Game Objects")] public GameObject Printer, Characters;
        [Header("UI Objects")] public Text Printer_Text;
        [Header("Audio Objects")] public AudioSource SEAudio, CallAudio;
        [Header("Preference")] public float Delay = 0.1f;
        [Header("Selector")] public GameObject Selector, SelectorItem;
        [Header("Text")] public Text SelectorItemText;

        [HideInInspector] public State state;
        [HideInInspector] public string Result;

        //Private-props
        private DialogData _currentData;
        private float _currentDelay;
        private float _lastDelay;
        private Coroutine _textingRoutine;
        private Coroutine _printingRoutine;

        //Start
        private new void Start()
        {
            base.Start();
            Hide();
        }

        //Public Method
        public void CustomShow(DialogData data)
        {
            if (!GameStatus.interact.Equals(GameController.GetStatus())) GameController.SetStatus(GameStatus.interact);
            _currentData = data;
            _textingRoutine = StartCoroutine(Activate());
        }

        public void CustomShow(List<DialogData> data)
        {
            StartCoroutine(CustomActivate_List(data));
        }

        public void Click_Window()
        {
            switch (state)
            {
                case State.Active:
                    StartCoroutine(_skip());
                    break;

                case State.Wait:
                    if (_currentData.SelectList.Count <= 0) Hide();
                    break;
            }
        }

        public void Hide()
        {
            if (_textingRoutine != null)
                StopCoroutine(_textingRoutine);

            if (_printingRoutine != null)
                StopCoroutine(_printingRoutine);

            Printer.SetActive(false);
            Characters.SetActive(false);
            Selector.SetActive(false);

            state = State.Deactivate;

            GameController.SetStatus(GameStatus.play);

            if (_currentData?.Callback != null)
            {
                _currentData.Callback.Invoke();
                _currentData.Callback = null;
            }
        }

        #region

        public void Select(int index)
        {
            Result = _currentData.SelectList.GetByIndex(index).Key;
            Hide();
        }

        #endregion

        #region Speed

        public void Set_Speed(string speed)
        {
            switch (speed)
            {
                case "up":
                    _currentDelay -= 0.25f;
                    if (_currentDelay <= 0) _currentDelay = 0.001f;
                    break;

                case "down":
                    _currentDelay += 0.25f;
                    break;

                case "init":
                    _currentDelay = Delay;
                    break;

                default:
                    _currentDelay = float.Parse(speed);
                    break;
            }

            _lastDelay = _currentDelay;
        }

        #endregion

        //Private-methods
        private void _initialize()
        {
            _currentDelay = Delay;
            _lastDelay = 0.1f;
            Printer_Text.text = string.Empty;
            Printer.SetActive(true);
            foreach (Transform item in Characters.transform) item.gameObject.SetActive(false);
        }

        private void _init_selector()
        {
            _clear_selector();

            if (_currentData.SelectList.Count > 0)
            {
                Selector.SetActive(true);

                for (int i = 0; i < _currentData.SelectList.Count; i++)
                {
                    _add_selectorItem(i);
                }
            }

            else Selector.SetActive(false);
        }

        private void _clear_selector()
        {
            for (int i = 1; i < Selector.transform.childCount; i++)
            {
                Destroy(Selector.transform.GetChild(i).gameObject);
            }
        }

        private void _add_selectorItem(int index)
        {
            SelectorItemText.text = _currentData.SelectList.GetByIndex(index).Value;

            var newItem = Instantiate(SelectorItem, Selector.transform);
            newItem.GetComponent<Button>().onClick.AddListener(() => Select(index));
            newItem.SetActive(true);
            if (!EventSystem.currentSelectedGameObject)
            {
                EventSystem.SetSelectedGameObject(newItem);
            }
        }

        #region Show Text

        private IEnumerator CustomActivate_List(List<DialogData> DataList)
        {
            state = State.Active;

            foreach (var Data in DataList)
            {
                CustomShow(Data);
                _init_selector();

                while (state != State.Deactivate)
                {
                    yield return null;
                }
            }
        }

        private IEnumerator Activate()
        {
            _initialize();

            state = State.Active;

            foreach (var item in _currentData.Commands)
            {
                switch (item.Command)
                {
                    case Command.print:
                        yield return _printingRoutine = StartCoroutine(_print(item.Context));
                        break;

                    case Command.color:
                        _currentData.Format.Color = item.Context;
                        break;

                    case Command.size:
                        _currentData.Format.Resize(item.Context);
                        break;

                    case Command.speed:
                        Set_Speed(item.Context);
                        break;

                    case Command.click:
                        yield return _waitInput();
                        break;

                    case Command.close:
                        Hide();
                        yield break;

                    case Command.wait:
                        yield return new WaitForSeconds(float.Parse(item.Context));
                        break;
                }
            }

            state = State.Wait;
        }

        private IEnumerator _waitInput()
        {
            while (!UnityEngine.Input.GetMouseButtonDown(0)) yield return null;
            _currentDelay = _lastDelay;
        }

        private IEnumerator _print(string Text)
        {
            _currentData.PrintText += _currentData.Format.OpenTagger;

            for (int i = 0; i < Text.Length; i++)
            {
                _currentData.PrintText += Text[i];
                Printer_Text.text = _currentData.PrintText + _currentData.Format.CloseTagger;

                if (_currentDelay != 0) yield return new WaitForSeconds(_currentDelay);
            }

            _currentData.PrintText += _currentData.Format.CloseTagger;
        }

        private IEnumerator _skip()
        {
            if (_currentData.isSkippable)
            {
                _currentDelay = 0;
                while (state != State.Wait) yield return null;
                _currentDelay = Delay;
            }
        }

        #endregion
    }
}