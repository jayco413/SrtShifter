using SrtShifter.Controllers;
using SrtShifter.Views;

namespace SrtShifter
{
    /// <summary>
    /// Windows Forms implementation of the main application view.
    /// </summary>
    public partial class frmMain : Form, IMainView
    {
        private readonly MainController _controller;

        /// <summary>
        /// Initializes a new instance of the <see cref="frmMain"/> class.
        /// </summary>
        public frmMain()
        {
            InitializeComponent();
            _controller = new MainController(this);
        }

        /// <summary>
        /// Handles the click event for the Select Video File button.
        /// </summary>
        private void btnSelectVideoFile_Click(object sender, EventArgs e)
        {
            _controller.SelectVideoFile();
        }

        /// <summary>
        /// Handles the click event for the Select SRT File button.
        /// </summary>
        private void btnSelectSrtFile_Click(object sender, EventArgs e)
        {
            _controller.SelectSrtFile();
        }

        /// <summary>
        /// Handles the click event for the Process button.
        /// </summary>
        private void btnProcess_Click(object sender, EventArgs e)
        {
            _controller.ProcessFiles();
        }

        /// <inheritdoc/>
        string IMainView.VideoFilePath
        {
            get => txtVideoFilePath.Text;
            set => txtVideoFilePath.Text = value;
        }

        /// <inheritdoc/>
        string IMainView.SrtFilePath
        {
            get => txtSrtFilePath.Text;
            set => txtSrtFilePath.Text = value;
        }

        /// <inheritdoc/>
        void IMainView.AppendLog(string text)
        {
            txtLogText.AppendText(text + Environment.NewLine);
        }
    }
}
