using System.Windows.Forms;

namespace FishyTokenWatcher
{
    public partial class Configuration : Form
    {
        public Configuration()
        {
            InitializeComponent();

            numericUpDownSeconds.Maximum = int.MaxValue;
            numericUpDownSeconds.Minimum = 1;
            numericUpDownSeconds.DecimalPlaces = 0;

            this.Load += Configuration_Load;
        }

        private void Configuration_Load(object sender, System.EventArgs e)
        {
            try
            {
                tbAPIKey.Text = Properties.Settings.Default.apiKey;
                tbAddr.Text = Properties.Settings.Default.ethAddress;
                numericUpDownSeconds.Value = Properties.Settings.Default.secondsCooldown;
                tbDate.Text = Properties.Settings.Default.dateFormat;
            }
            catch { }
        }

        private void btnOK_Click(object sender, System.EventArgs e)
        {
            try
            {
                Properties.Settings.Default.apiKey = tbAPIKey.Text;
                Properties.Settings.Default.ethAddress = tbAddr.Text;
                Properties.Settings.Default.secondsCooldown = (int)numericUpDownSeconds.Value;
                Properties.Settings.Default.dateFormat = tbDate.Text;

                Properties.Settings.Default.Save();
            }
            catch
            { }

            this.Close();
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
    }
}
