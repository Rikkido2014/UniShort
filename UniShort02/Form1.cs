using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniShort02
{
    public partial class UniShort : Form
    {
        KeyboardHook HotKey;
        Routine _routine;
        const string Address = "http://kisu.me/";
        string beforeAddress = null;
        string beforeShotedAdrs = null;

        public UniShort()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedDialog;
            
            HotKey = new KeyboardHook();
            _routine = new Routine();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            HotKey.KeyPressed += HotKey_KeyPressed;
            HotKey.RegisterHotkey(Modifires.CTRL | Modifires.SHIFT, Keys.V);

        }

        private async void HotKey_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            string clipboardText = Clipboard.GetText();

            string ShortedStr = null;
            Console.WriteLine("短縮元アドレス:{0}",clipboardText);

            if (beforeAddress != clipboardText & beforeShotedAdrs != clipboardText)
            {
                ShortedStr = await Task.Run(() => _routine.GetJson(clipboardText));
            }

            beforeAddress = clipboardText;
            Console.WriteLine($"短縮結果:{ShortedStr}{Isnull(ShortedStr)}");

            ShortedStr = _routine.ShapeJson(ShortedStr);

            Console.WriteLine($"情報取り出し操作結果:{ShortedStr}{Isnull(ShortedStr)}");

            if(ShortedStr != null)
            {
                beforeShotedAdrs = Address + ShortedStr;
                Clipboard.SetText(beforeShotedAdrs);
                Console.WriteLine($"貼り付けアドレス:{beforeShotedAdrs}");
                this.Text = $"UniShort:{ShortedStr}";
                return;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            HotKey.Dispose();
        }

        public string Isnull(string text)
        {
            return text == null ? "失敗" : null;
        }
    }
}
