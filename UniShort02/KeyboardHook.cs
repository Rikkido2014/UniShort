using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace UniShort02
{
    public class KeyboardHook : IDisposable
    {
        [DllImport("User32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifires, uint Vkey);

        [DllImport("User32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private Window _window = new Window();
        private int _currentId;
        public event EventHandler<KeyPressedEventArgs> KeyPressed;

        private class Window : NativeWindow, IDisposable
        {
            private static int WH_HOTKEY = 0x0312;
            public event EventHandler<KeyPressedEventArgs> KeyPressed;
           
            public Window()
            {
                this.CreateHandle(new CreateParams());
            }

            protected override void WndProc(ref Message m)
            {
                base.WndProc(ref m);

                if(m.Msg == WH_HOTKEY)
                {
                    Keys key = (Keys)(((int)m.LParam >> 16)& 0xFFFF);

                    Modifires modifier = (Modifires)((int)m.LParam & 0xFFFF);

                    KeyPressed?.Invoke(this, new KeyPressedEventArgs(modifier, key));
                }
            }

            public void Dispose()
            {
                this.DestroyHandle();
            }
        }
        public KeyboardHook()
        {
            _window.KeyPressed += _window_KeyPressed;
        }

        private void _window_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            KeyPressed?.Invoke(this, e);
        }

        public void RegisterHotkey(Modifires modifier,Keys Key)
        {
            _currentId++;

            if (!RegisterHotKey(_window.Handle, _currentId, (uint)modifier, (uint)Key)) throw new InvalidOperationException("Coudn't register the hot key");
            
        }

        public void Dispose()
        {
            for(int i = _currentId;i > 0;i--)
            {
                UnregisterHotKey(_window.Handle, i);
            }
            _window.Dispose();
        }
       
    }

    public class KeyPressedEventArgs : EventArgs
    {
        private Modifires _modifier;
        private Keys _key;

        internal KeyPressedEventArgs(Modifires modifier,Keys key)
        {
            _modifier = modifier;
            _key = key;
            
        }
        public Modifires Modifier
        {
            get { return _modifier; }
        }
        public Keys Key
        {
            get { return _key; }
        }

    }

    public enum Modifires
    {
        ALT = 0x0001,
        CTRL = 0x0002,
        SHIFT = 0x0004,

    }
}
