using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ImagineCup2012
{
    public enum MouseKeys { LEFT_BUTTON, MIDDLE_BUTTON, RIGHT_BUTTON };

    public class Input
    {
        private static readonly Input instance = new Input();

        private KeyboardState newKeyboardState;
        private KeyboardState oldKeyboardState;
        private MouseState newMouseState;
        private MouseState oldMouseState;

        private Dictionary<string, Keys> keyboardKeys;
        private Dictionary<string, MouseKeys> mouseKeys;

        private Input()
        {
            newKeyboardState = new KeyboardState();
            oldKeyboardState = new KeyboardState();
            newMouseState = new MouseState();
            oldMouseState = new MouseState();

            keyboardKeys = new Dictionary<string, Keys>();
            mouseKeys = new Dictionary<string, MouseKeys>();
        }

        public static Input getInstance
        {
            get
            {
                return instance;
            }
        }

        public Dictionary<string, Keys> getkeyboardKeys()
        {
            return keyboardKeys;
        }

        public Dictionary<string, MouseKeys> getMouseKeys()
        {
            return mouseKeys;
        }

        public void start()
        {
            Console.WriteLine("Input.start");

            keyboardKeys.Add("right", Keys.Right);
            keyboardKeys.Add("left", Keys.Left);
            keyboardKeys.Add("down", Keys.Down);
            keyboardKeys.Add("up", Keys.Up);
            keyboardKeys.Add("d", Keys.D);
            keyboardKeys.Add("a", Keys.A);
            keyboardKeys.Add("s", Keys.S);
            keyboardKeys.Add("w", Keys.W);
            keyboardKeys.Add("q", Keys.Q);
            keyboardKeys.Add("e", Keys.E);
            keyboardKeys.Add("r", Keys.R);
            keyboardKeys.Add("t", Keys.T);
            keyboardKeys.Add("y", Keys.Y);
            keyboardKeys.Add("u", Keys.U);
            keyboardKeys.Add("i", Keys.I);
            keyboardKeys.Add("o", Keys.O);
            keyboardKeys.Add("p", Keys.P);
            keyboardKeys.Add("f", Keys.F);
            keyboardKeys.Add("g", Keys.G);
            keyboardKeys.Add("h", Keys.H);
            keyboardKeys.Add("j", Keys.J);
            keyboardKeys.Add("k", Keys.K);
            keyboardKeys.Add("l", Keys.L);
            keyboardKeys.Add("z", Keys.Z);
            keyboardKeys.Add("x", Keys.X);
            keyboardKeys.Add("c", Keys.C);
            keyboardKeys.Add("v", Keys.V);
            keyboardKeys.Add("b", Keys.B);
            keyboardKeys.Add("n", Keys.N);
            keyboardKeys.Add("m", Keys.M);
            keyboardKeys.Add("enter", Keys.Enter);
            keyboardKeys.Add("space", Keys.Space);
            keyboardKeys.Add("escape", Keys.Escape);
            keyboardKeys.Add("pause", Keys.Pause); //Funker ikke
            keyboardKeys.Add("delete", Keys.Delete);
            keyboardKeys.Add("back", Keys.Back);

            mouseKeys.Add("mouseleft", MouseKeys.LEFT_BUTTON);
            mouseKeys.Add("mouseright", MouseKeys.RIGHT_BUTTON);
            mouseKeys.Add("mousemiddle", MouseKeys.MIDDLE_BUTTON);

            SettingsManager.getInstance.getControllerSettings();
        }

        public void update()
        {
            oldKeyboardState = newKeyboardState;
            oldMouseState = newMouseState;

            newKeyboardState = Keyboard.GetState();
            newMouseState = Mouse.GetState();
        }

        /*
         * Gets the mouse position relative to local coordinates
         * */
        public Vector2 getMousePosRelativeToScreen()
        {
            Vector2 mousePos = new Vector2(newMouseState.X, newMouseState.Y);

            return mousePos;
        }

        /*
         * Gets the mouse position relative to world coordinates
         * */
        public Vector2 getMousePosRelativeToWorld()
        {
            Vector2 mousePos = -SceneManager.getInstance.getCamera().getPosition() + getMousePosRelativeToScreen();

            return mousePos;
        }

        /*
         * Gets the mouse position relative to previous local coordinates
         * */
        public Vector2 getMousePosRelativeToOld()
        {
            Vector2 mousePos = new Vector2(newMouseState.X - oldMouseState.X, newMouseState.Y - oldMouseState.Y);

            return mousePos;
        }

        public bool keyDown(Keys key)
        {
            return (newKeyboardState.IsKeyDown(key)) && !(oldKeyboardState.IsKeyDown(key));
        }

        public bool keyStillDown(Keys key)
        {
            return (newKeyboardState.IsKeyDown(key)) && (oldKeyboardState.IsKeyDown(key));
        }

        public bool keyUp(Keys key)
        {
            return !(newKeyboardState.IsKeyDown(key)) && (oldKeyboardState.IsKeyDown(key));
        }

        public bool keyStillUp(Keys key)
        {
            return !(newKeyboardState.IsKeyDown(key)) && !(oldKeyboardState.IsKeyDown(key));
        }

        public bool mouseDown(MouseKeys mouseKey)
        {
            if (mouseKey == MouseKeys.LEFT_BUTTON)
            {
                return (newMouseState.LeftButton == ButtonState.Pressed) && !(oldMouseState.LeftButton == ButtonState.Pressed);
            }
            else if (mouseKey == MouseKeys.MIDDLE_BUTTON)
            {
                return (newMouseState.MiddleButton == ButtonState.Pressed) && !(oldMouseState.MiddleButton == ButtonState.Pressed);
            }
            else
            {
                return (newMouseState.RightButton == ButtonState.Pressed) && !(oldMouseState.RightButton == ButtonState.Pressed);
            }
        }

        public bool mouseStillDown(MouseKeys mouseKey)
        {
            if (mouseKey == MouseKeys.LEFT_BUTTON)
            {
                return (newMouseState.LeftButton == ButtonState.Pressed) && (oldMouseState.LeftButton == ButtonState.Pressed);
            }
            else if (mouseKey == MouseKeys.MIDDLE_BUTTON)
            {
                return (newMouseState.MiddleButton == ButtonState.Pressed) && (oldMouseState.MiddleButton == ButtonState.Pressed);
            }
            else
            {
                return (newMouseState.RightButton == ButtonState.Pressed) && (oldMouseState.RightButton == ButtonState.Pressed);
            }
        }

        public bool mouseUp(MouseKeys mouseKey)
        {
            if (mouseKey == MouseKeys.LEFT_BUTTON)
            {
                return !(newMouseState.LeftButton == ButtonState.Pressed) && (oldMouseState.LeftButton == ButtonState.Pressed);
            }
            else if (mouseKey == MouseKeys.MIDDLE_BUTTON)
            {
                return !(newMouseState.MiddleButton == ButtonState.Pressed) && (oldMouseState.MiddleButton == ButtonState.Pressed);
            }
            else
            {
                return !(newMouseState.RightButton == ButtonState.Pressed) && (oldMouseState.RightButton == ButtonState.Pressed);
            }
        }

        public bool mouseStillUp(MouseKeys mouseKey)
        {
            if (mouseKey == MouseKeys.LEFT_BUTTON)
            {
                return !(newMouseState.LeftButton == ButtonState.Pressed) && !(oldMouseState.LeftButton == ButtonState.Pressed);
            }
            else if (mouseKey == MouseKeys.MIDDLE_BUTTON)
            {
                return !(newMouseState.MiddleButton == ButtonState.Pressed) && !(oldMouseState.MiddleButton == ButtonState.Pressed);
            }
            else
            {
                return !(newMouseState.RightButton == ButtonState.Pressed) && !(oldMouseState.RightButton == ButtonState.Pressed);
            }
        }
    }
}