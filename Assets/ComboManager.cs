using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ComboSmasher
{
    // TODO: Add event listeners for on combo completed, on combo failed, on input reset
    // TODO: ComboEventArgs to pass arround completed combo
    public class ComboManager : MonoBehaviour
    {
        public InputManager inputManager;
        public Text comboText;
        public List<List<char>> combos = new List<List<char>>()
        {
            new List<char>(){ 'A', 'A' },
            new List<char>(){ 'A', 'A', 'A' },
            new List<char>(){ 'B', 'A', 'B' },
            new List<char>(){ 'X', 'A', 'B' },
            new List<char>(){ 'X', 'X', 'X' },
            new List<char>(){ 'A', 'A', 'B', 'Y' },
        };

        bool reset = false;
        public List<bool> possibleCombinations = new List<bool>();
        public List<bool> completedBool = new List<bool>();

        float clearComboRate = 0.25f;
        float nextClearTime = 0;
        List<char> currentInput = new List<char>();

        private void Start()
        {
            // get list of combos from file
            // set completed Combos
            Reset();
        }

        void AddToInput(char input)
        {
            reset = false;
            // add rate to clear time
            nextClearTime = clearComboRate + Time.time;
            currentInput.Add(input);

            // TODO: make loop start from last checked spot - that's why we remove lists that don't apply
            for (int k = 0; k < combos.Count; k++)
            {
                if (!possibleCombinations[k]) continue;
                // check if current input is now greater than current combo
                // if completed combo is 'a', 'a' and it's now 'a', 'a', 'a' we want to use a combo for this instead
                if (currentInput.Count > combos[k].Count)
                {
                    completedBool[k] = false;
                    possibleCombinations[k] = false;
                    continue;
                }

                // we don't even have to loop we just have to check the length of the current input - lit 
                // check only against the length of the current input
                if (currentInput[currentInput.Count - 1] != combos[k][currentInput.Count - 1])
                {
                    possibleCombinations[k] = false;
                }
                else
                {
                    // we've reached the last combo input and completed it
                    if (currentInput.Count - 1 == combos[k].Count - 1)
                    {
                        completedBool[k] = true;
                    }
                }
            }

            
            if (!possibleCombinations.Any(c => c == true))
            {
                comboText.text = "<color=\"#F44336\">" + new string(currentInput.ToArray()) + "</color>";
                // Debug.Log("no more possible combinations");
                Reset();
            }
            else
            {
                comboText.text = new string(currentInput.ToArray());
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (inputManager.Current.Fire1)
            {
                AddToInput('A');
            }

            if (inputManager.Current.Fire2)
            {
                AddToInput('B');
            }

            if (inputManager.Current.Fire3)
            {
                AddToInput('X');
            }

            if (inputManager.Current.JumpInput)
            {
                AddToInput('Y');
            }


            // check time to clear current input and check for completed combo
            if (Time.time > nextClearTime && !reset)
            {
                if (!ExecuteCompletedCombo())
                {
                    comboText.text = "<color=\"#F44336\">" + new string(currentInput.ToArray()) + "</color>";
                }
                Reset();
            }
        }

        private bool ExecuteCompletedCombo()
        {
            // make it happen
            for (int i = 0; i < completedBool.Count; i++)
            {
                if (completedBool[i])
                {
                    comboText.text = "<color=\"#4CAF50\">" + new string(combos[i].ToArray()) + "</color>";
                    return true;
                }
            }
            return false;
        }

        private void Reset()
        {
            reset = true;
            // reset input
            currentInput.Clear(); // clear or set it to null
            possibleCombinations = Enumerable.Repeat(true, combos.Count).ToList();
            completedBool = Enumerable.Repeat(false, combos.Count).ToList();
        }
    }
}
