using System;
using System.Threading;
using System.Windows.Forms;

namespace ThredadWatch
{
    public partial class TaskWatch : Form
    {
        private Thread clockThread; // 스레드
        private volatile bool systemState = false; // volatile 뜻 즉 언제든지 값이 변경될수 있으니 항상 메모리에서 대기해라! 라고 컴파일러에게 알려준다.

        public TaskWatch()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Start(sender, e); // 시작
        }

        private void UpdateClock()
        {
            while (systemState)
            {
                try
                {
                    string nowTime = DateTime.Now.ToString("HH : mm : ss : fff");

                    if (dashBord.IsHandleCreated)
                    {
                        dashBord.BeginInvoke((MethodInvoker)(() => dashBord.Text = nowTime));
                    }

                    Thread.Sleep(16);

                }
                catch (Exception ex)                {
                    MessageBox.Show(ex.ToString());
                    break;
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            systemState = false;

            if (clockThread != null && clockThread.IsAlive)
            {
                clockThread.Join(200);
            }
        }

        /// <summary>
        /// 시작
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Start(object sender, EventArgs e)
        {
            if (systemState)
            {
                this.button1.Enabled = false;
                this.button2.Enabled = true;
                return;
            }

            systemState = true;
            clockThread = new Thread(UpdateClock)
            {
                IsBackground = true,
                Name = "ClockThread"
            };
            clockThread.Start();

            this.button1.Enabled = false;
            this.button2.Enabled = true;
        }

        /// <summary>
        /// 정지
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Stop(object sender, EventArgs e)
        {
            systemState = false;

            if (clockThread != null && clockThread.IsAlive)
            {
                clockThread.Join(200);
                clockThread = null;
            }
            Console.WriteLine(dashBord.Text);


            this.button1.Enabled = true;
            this.button2.Enabled = false;


        }
    }
}
