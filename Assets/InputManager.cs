using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using ComboSmasher.Utility.Constants;

namespace ComboSmasher
{
    public struct Input
    {
        public Vector3 MoveInput;
        public bool JumpInput;
        public bool Fire1;
        public bool Fire2;
        public bool Fire3;
    }

    public class InputManager : MonoBehaviour
    {
        bool active = true;
        public Input Current;
        public bool Active
        {
            get
            {
                return active;
            }
            set
            {
                active = value;
            }
        }

        float Horizontal
        {
            get
            {
                return CrossPlatformInputManager.GetAxis(Axes.Horizontal);
            }
        }

        float Vertical
        {
            get
            {
                return CrossPlatformInputManager.GetAxis(Axes.Vertical);
            }
        }

        private void Update()
        {
            if (active)
            {
                Current = new Input()
                {
                    MoveInput = new Vector3(Horizontal, 0, Vertical),
                    JumpInput = CrossPlatformInputManager.GetButtonDown(Axes.Jump),
                    Fire1 = CrossPlatformInputManager.GetButtonDown(Axes.Fire1),
                    Fire2 = CrossPlatformInputManager.GetButtonDown(Axes.Fire2),
                    Fire3 = CrossPlatformInputManager.GetButtonDown(Axes.Fire3),
                };
            }
            else
            {
                Current = new Input()
                {
                    MoveInput = Vector3.zero,
                    JumpInput = false,
                    Fire1 = false,
                    Fire2 = false,
                    Fire3 = false
                };
            }
        }
    }
}
