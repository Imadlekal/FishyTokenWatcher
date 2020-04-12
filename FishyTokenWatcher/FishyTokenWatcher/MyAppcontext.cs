using System;
using System.Timers;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace FishyTokenWatcher
{
    public class MyAppcontext : ApplicationContext
    {
        private readonly string noknown = "No known changes";
        public MyAppcontext()
        {
            MenuItem configMenuItem = new MenuItem("Config", new EventHandler(ShowConfig));
            MenuItem exitMenuItem = new MenuItem("Exit", new EventHandler(Exit));

            notifyIcon = new NotifyIcon();
            notifyIcon.Text = noknown;
            notifyIcon.Icon = Properties.Resources.tokenIcon;
            notifyIcon.ContextMenu = new ContextMenu(new MenuItem[]
                { configMenuItem, exitMenuItem });
            notifyIcon.Visible = true;

            StartTimer();
        }

        NotifyIcon notifyIcon;
        Configuration configWindow = new Configuration();
        Timer myTimer = null;
        DateTime lastCheck = DateTime.MinValue;
        string lastChange = "No change.";
        Data mainData = new Data();
        bool isShowingConfig = false;

        private void ShowConfig(object sender, EventArgs e)
        {
            StopTimer();
            // If we are already showing the window, merely focus it.
            isShowingConfig = true;
            if (configWindow.Visible)
            {
                configWindow.Activate();
            }
            else
            {
                configWindow.ShowDialog();
            }
            isShowingConfig = false;
            notifyIcon.Text = noknown;
            StartTimer();
        }

        private void Exit(object sender, EventArgs e)
        {
            // We must manually tidy up and remove the icon before we exit.
            // Otherwise it will be left behind until the user mouses over.
            notifyIcon.Visible = false;
            StopTimer();
            if (isShowingConfig)
            {
                configWindow.Close();
            }
            Application.Exit();
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            DoJob();
        }

        private void DoJob()
        {
            try
            {
                var data = REST.Inst.QUERY();
                if (null != data)
                {
                    lastCheck = DateTime.Now;

                    string change = mainData.CheckAndChange(data);
                    if (!string.IsNullOrWhiteSpace(change))
                    {
                        lastChange = $"{lastCheck.ToString(Properties.Settings.Default.dateFormat)}: {change}";

                        notifyIcon.Visible = true;
                        notifyIcon.ShowBalloonTip(15000,
                            "Amounts have changed",
                            change.Replace(";", Environment.NewLine),
                            ToolTipIcon.Info);
                    }

                    var text = $"{lastChange} (checked: {lastCheck.ToString(Properties.Settings.Default.dateFormat)})";
                    if (text.Length > 63)
                        text = text.Substring(0, 63);

                    notifyIcon.Text = text;
                }
            }
            catch
            {
            }
        }

        private void StartTimer()
        {
            if (myTimer != null)
                throw new Exception("Timer should have been stopped!");

            if ((Properties.Settings.Default.secondsCooldown <= 0)
                || string.IsNullOrWhiteSpace(Properties.Settings.Default.apiKey)
                || string.IsNullOrWhiteSpace(Properties.Settings.Default.ethAddress)
                || string.IsNullOrWhiteSpace(Properties.Settings.Default.dateFormat))
            {
                notifyIcon.Text = "Application is dormant, please review settings.";
                return;
            }
            else
            {
                DoJob();
            }

            myTimer = new Timer(Properties.Settings.Default.secondsCooldown * 1000);
            //myTimer = new Timer(10 * 1000);
            // Hook up the Elapsed event for the timer. 
            myTimer.Elapsed += OnTimedEvent;
            myTimer.AutoReset = true;
            myTimer.Enabled = true;
        }

        private void StopTimer()
        {
            if (myTimer != null)
            {
                myTimer.Stop();
                myTimer.Dispose();
                myTimer = null;
            }
        }
    }
}
