using SrtShifter.Controllers;
using SrtShifter.Views;

namespace SrtShifter
{
    public partial class frmMain : Form, IMainView
    {
        private readonly MainController _controller;

        public frmMain()
        {
            InitializeComponent();
            _controller = new MainController(this);
        }

        private void btnSelectVideoFile_Click(object sender, EventArgs e)
        {
            _controller.SelectVideoFile();
        }

        private void btnSelectSrtFile_Click(object sender, EventArgs e)
        {
            _controller.SelectSrtFile();
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            _controller.ProcessFiles();
        }

        string IMainView.VideoFilePath
        {
            get => txtVideoFilePath.Text;
            set => txtVideoFilePath.Text = value;
        }

        string IMainView.SrtFilePath
        {
            get => txtSrtFilePath.Text;
            set => txtSrtFilePath.Text = value;
        }

        void IMainView.AppendLog(string text)
        {
            txtLogText.AppendText(text + Environment.NewLine);
        }
    }
}
