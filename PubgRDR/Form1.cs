using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace PubgTriggr
{
    public partial class Form1 : Form
    {
        //Alerted stage when kill is detection hits.
        private bool alert;

        private LearningHelper learninghelper;

        private OBSRemote obsRemote;

        //Interval to check for kill
        private int iKillCheckInterval = 100;

        //Interval to check for Number as soon as Kill is detected
        private int iNumberCheckInterval = 100;



        //Common
        private int KillCount;
        private int PeopleAlive;


        //Reference colors and thresholds
        //RedKillMessage
        private Color redReference = Color.FromArgb(247, 96, 53);
        private int redLowerThreshold = 19;
        private int redUpperThreshold = 32;

        //RedKillMessage
        private Color whiteReference = Color.FromArgb(231, 239, 234);
        private int whiteLowerThreshold = 4;
        private int whiteUpperThreshold = 50;


        //ColorThreshold
        private int generalColorThreshold = Properties.Settings.Default.ColorThreshold;


        //ScreenInfo Rectangles
        //People Alive
        private static Rectangle rectPeopleAliveSingle = new Rectangle(2405, 50, 27, 35);
        private static Rectangle rectPeopleAliveDouble = new Rectangle(2380, 50, 60, 35);

        private Rectangle rectPeopleAlive = rectPeopleAliveSingle;

        //TopKillCounter
        private static Rectangle rectTopKillNumberSingle = new Rectangle(2210, 50, 27, 35);
        private static Rectangle rectTopKillNumberDouble = new Rectangle(2180, 40, 62, 55);
        //TopKillCounter if less than ten alive
        private static Rectangle rectTopKillNumberSingleLTT = new Rectangle(2235, 50, 27, 35);
        private static Rectangle rectTopKillNumberDoubleLTT = new Rectangle(2200, 50, 60, 35);

        private static Rectangle rectTopKillNumber = Properties.Settings.Default.rectTopKillNumber;

        //CenterKill-"KILL"
        private static Rectangle rectCenterKillSingle = new Rectangle(1250, 1015, 100, 42);
        private static Rectangle rectCenterKillDouble = new Rectangle(1265, 1015, 100, 42);

        private static Rectangle rectCenterKill = Properties.Settings.Default.rectRedKill;


        //CenterKillNumber
        private static Rectangle rectCenterKillNumber = Properties.Settings.Default.rectRedKillNumber;

        //KillMesssage
        private static Rectangle rectKillMessage = Properties.Settings.Default.rectKillMessage;


        //Helper Classes
        private Helpers helpers = Helpers.Instance;

        //GameCapture Class
        private GameCapture gcaptr = GameCapture.Instance;


        public Form1()
        {
            SetDpiAwareness();

            InitializeComponent();

            WriteToLog("PippoFPS PUBG Killtrigger Started!");

            float value = (generalColorThreshold / 250f) * 100f;
            tBColorThreshold.Value = Convert.ToInt16(value);
            tBColorThreshold.Update();

            this.Show();
            this.Refresh();

            //Get OBS Preview Window to capture
            IntPtr windowhandle = IntPtr.Zero;
            while (windowhandle == IntPtr.Zero)
            {
                //Let User Pick Process
                using (var form = new ProcessPicker("obs"))
                {
                    var result = form.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        windowhandle = form.WindowHandle;
                    }
                    else form.Dispose();
                }
            }
            gcaptr.SetWindowHandle = windowhandle;


            //OBS Remote
            obsRemote = OBSRemote.Instance;
            WriteToLog(obsRemote.Connect());

            GetAllScreenComponents();

            string modelpath = @"./learning/KillDetectionModel.mdl";
            if (File.Exists(modelpath))
                learninghelper = new LearningHelper(modelpath);
        }




        private void GetAllScreenComponents()
        {
            //RedReference
            btnPickReferenceRed.BackColor = redReference;

            //WhiteReference
            btnPickReferenceWhite.BackColor = whiteReference;

            //KillMessage
            if (pBKillMessage.Image != null)
                pBKillMessage.Image.Dispose();
            pBKillMessage.Image = gcaptr.MakeScreenShot(rectKillMessage);

            //CenterKillTrigger
            if (pBRedKill.Image != null)
                pBRedKill.Image.Dispose();
            pBRedKill.Image = helpers.CleanForColor(gcaptr.MakeScreenShot(rectCenterKill), redReference);

            //CenterKillCounter
            if (pBCenterKillNumber.Image != null)
                pBCenterKillNumber.Image.Dispose();
            pBCenterKillNumber.Image = helpers.CleanForColor(gcaptr.MakeScreenShot(rectCenterKillNumber), redReference);

            if (pBKillCounterTopRight.Image != null)
                pBKillCounterTopRight.Image.Dispose();
            //TopKillCounter
            pBKillCounterTopRight.Image = helpers.CleanForColor(gcaptr.MakeScreenShot(rectTopKillNumber), whiteReference);

        }


        #region GUI Control Events
        /// <summary>
        /// Start Button 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnStart_Click(object sender, EventArgs e)
        {
            if (!Scanner_RedKill.IsBusy)
            {
                PeopleAlive = 0;
                KillCount = 0;
                tsLblAlive.Text = "Alive: 0";
                tsLblKills.Text = "Kills: 0";

                WriteToLog("Scanning for kills...");
                btnStart.Text = "Stop";
                Scanner_RedKill.RunWorkerAsync();
                btnStart.BackColor = Color.PaleVioletRed;
                toolStripProgressBar1.Style = ProgressBarStyle.Marquee;
                toolStripProgressBar1.MarqueeAnimationSpeed = 100;
            }
            else
            {
                WriteToLog("Stopped scanning for kills...");
                btnStart.Text = "Start";

                Scanner_RedKill.CancelAsync();
                Scanner_KillNumber.CancelAsync();
                Worker_KillMessageCleaner.CancelAsync();


                btnStart.BackColor = Color.LightGreen;

                toolStripProgressBar1.MarqueeAnimationSpeed = 0;
                toolStripProgressBar1.Style = ProgressBarStyle.Blocks;
                toolStripProgressBar1.Value = toolStripProgressBar1.Minimum;
            }
        }

        private void BtnRescanAll_Click(object sender, EventArgs e)
        {
            GetAllScreenComponents();
        }

        /// <summary>
        /// PickReference Red
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPickReferenceRed_Click(object sender, EventArgs e)
        {
            using (var form = new ScreenPixelGrabForm())
            {
                form.Visible = false;
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    redReference = form.Pixel;
                    btnPickReferenceRed.BackColor = redReference;
                }
            }
        }

        /// <summary>
        /// Pick Reference White
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPickReferenceWhite_Click(object sender, EventArgs e)
        {
            using (var form = new ScreenPixelGrabForm())
            {
                form.Visible = false;
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    whiteReference = form.Pixel;
                    btnPickReferenceWhite.BackColor = whiteReference;
                }
            }
        }

        /// <summary>
        /// Select RedKillNumber Rectangle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRedKillNumber_Click(object sender, EventArgs e)
        {
            using (var form = new ScreenGrabForm())
            {
                form.Visible = false;
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    rectCenterKillNumber = form.SelectedRectangle;
                    pBCenterKillNumber.Image = gcaptr.MakeScreenShot(rectCenterKillNumber);
                }
            }
        }

        /// <summary>
        /// Select Kill Message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSelectKillMessage_Click(object sender, EventArgs e)
        {
            using (var form = new ScreenGrabForm())
            {
                form.Visible = false;
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    rectKillMessage = form.SelectedRectangle;
                    pBKillMessage.Image = gcaptr.MakeScreenShot(rectKillMessage);
                }
            }
        }

        /// <summary>
        /// Select red "KILL"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSelRedKill_Click(object sender, EventArgs e)
        {
            using (var form = new ScreenGrabForm())
            {
                form.Visible = false;
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    rectCenterKill = form.SelectedRectangle;
                    pBRedKill.Image = gcaptr.MakeScreenShot(rectCenterKill);
                }
            }
        }

        /// <summary>
        /// Select White Kill Counter on Top Rectangle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnWhiteKillCounter_Click(object sender, EventArgs e)
        {
            using (var form = new ScreenGrabForm())
            {
                form.Visible = false;
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    rectTopKillNumber = form.SelectedRectangle;
                    pBKillCounterTopRight.Image = gcaptr.MakeScreenShot(rectTopKillNumber);
                }
            }
        }

        private void TBColorThreshold_ValueChanged(object sender, EventArgs e)
        {
            generalColorThreshold = 255 / 100 * tBColorThreshold.Value;
            helpers.GeneralColorThreshold = generalColorThreshold;
            GetAllScreenComponents();
        }

        private void BtnSaveSettings_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.redReference = redReference;
            Properties.Settings.Default.whiteReference = whiteReference;

            Properties.Settings.Default.rectRedKill = rectCenterKill;
            Properties.Settings.Default.rectRedKillNumber = rectCenterKillNumber;
            Properties.Settings.Default.rectTopKillNumber = rectTopKillNumber;
            Properties.Settings.Default.rectKillMessage = rectKillMessage;

            Properties.Settings.Default.ColorThreshold = Convert.ToInt32((250f / 100f) * tBColorThreshold.Value);
            Properties.Settings.Default.Save();
        }
        #endregion

        #region Kill Detection
        private int GetRedKillPercentage()
        {
            //Get KillRect SingleLine 
            Rectangle singleLine = new Rectangle(rectCenterKill.X, rectCenterKill.Y + rectCenterKill.Height / 2, rectCenterKill.Width, 1);
            //GetPercentage of red colored parts
            int redPercentage = helpers.GetColorPercentage(gcaptr.MakeScreenShot(singleLine), redReference);
            return redPercentage;
        }


        //NUML TEST
        private ScreenData GetScreenData()
        {
            //Get middle Line Data
            Rectangle singleLineRed = new Rectangle(rectCenterKill.X, rectCenterKill.Y + rectCenterKill.Height / 2, 100, 1);
            //GetPercentage of red colored parts
            int redPercentage = helpers.GetColorPercentage(gcaptr.MakeScreenShot(singleLineRed), redReference);

            Rectangle singleLineWhite = new Rectangle(rectTopKillNumber.X, rectTopKillNumber.Y + rectTopKillNumber.Height / 2, 100, 1);
            //GetPercentage of red colored parts
            int whitePercentage = helpers.GetColorPercentage(gcaptr.MakeScreenShot(singleLineWhite), whiteReference);

            return new ScreenData { Red = redPercentage, White = whitePercentage };

        }

        /// <summary>
        /// KillScanner - Scans for red colored kill message.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Scanner_RedKill_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!Scanner_RedKill.CancellationPending)
            {
                //Check for Kill
                if (learninghelper.CheckForKill(GetScreenData()))
                {
                    Scanner_RedKill.ReportProgress(1, "");
                }
                else
                {
                    Scanner_RedKill.ReportProgress(0, "");
                }
                Thread.Sleep(iKillCheckInterval);
            }
        }


        /// <summary>
        /// Progress of Kill Scanner
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Scanner_RedKill_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 1)
            {
                SetAlert(true);
                if (!Scanner_KillNumber.IsBusy)
                    Scanner_KillNumber.RunWorkerAsync();
            }
            else
            {
                SetAlert(false);
            }
        }


        #endregion

        #region Kill Number Detection
        /// <summary>
        /// Scans for KillNumber 5 times.
        /// </summary>
        /// <returns>Scanned Killnumber. 0 if nothing was found.</returns>
        public int GetKillNumber()
        {
            int timestried = 0;
            int lastKillNumber = 0;
            //Try five times to scan Killnumber.
            while (timestried < 5)
            {
                timestried++;
                Bitmap bNumberTop = helpers.CleanForColor(gcaptr.MakeScreenShot(rectTopKillNumber), whiteReference);
                Bitmap bNumberCenter = helpers.CleanForColor(gcaptr.MakeScreenShot(rectCenterKillNumber), redReference);

                int iNumberTop = helpers.OCRNumber(bNumberTop);
                int iNumberCenter = helpers.OCRNumber(bNumberCenter);
                bNumberTop.Dispose();
                bNumberCenter.Dispose();

                //if (iNumberCenter == iNumberTop)
                //{
                    //number has to be the same in two consecutive scans.
                    if (lastKillNumber != iNumberTop)
                        lastKillNumber = iNumberTop;
                    else
                        return iNumberTop;
                //}
            }
            //If no sucess on getting killnumber, return 0
            return 0;
        }


        /// <summary>
        /// TopKillScanner - Scans for number on top as soon as Kill-Scanner has detected a kill.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Scanner_KillNumber_DoWork(object sender, DoWorkEventArgs e)
        {
            int timestried = 0;
            while (!Scanner_KillNumber.CancellationPending && timestried < 5)
            {
                int newKillNumber = GetKillNumber();
                if (newKillNumber > 0)
                {
                    e.Result = newKillNumber;
                    Scanner_KillNumber.ReportProgress(newKillNumber);
                    Scanner_KillNumber.CancelAsync();
                }
                else
                {
                    Thread.Sleep(iNumberCheckInterval);
                }
            }
        }


        /// <summary>
        /// Progress of TopKillScanner
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Scanner_KillNumber_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //If new KillNumber is bigger than the current Killcount and not bigger than the current Killcount+6, accept it as valid.
            if (e.ProgressPercentage > KillCount && e.ProgressPercentage < KillCount + 20)
            {
                KillCount = e.ProgressPercentage;
                WriteToLog("New Kill detected!" + e.ProgressPercentage);

                //Wait for KillMessageCleaner to end and start it again.
                while (Worker_KillMessageCleaner.IsBusy)
                {
                    Worker_KillMessageCleaner.CancelAsync();
                }
                Worker_KillMessageCleaner.RunWorkerAsync();

                tsLblKills.Text = "Kills: " + e.ProgressPercentage.ToString();
            }
        }

        /// <summary>
        /// Gets called when we're finished with Kill-Number detection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Scanner_KillNumber_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Start Scanner again
            if(!Scanner_RedKill.IsBusy && !Scanner_RedKill.CancellationPending)
                Scanner_RedKill.RunWorkerAsync();
        }

        #endregion

        #region KillMessageCleaner
        /// <summary>
        /// Captures the Killmessage and cleans it. Triggers OBS source based on the string information.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Worker_KillMessageCleaner_DoWork(object sender, DoWorkEventArgs e)
        {
            //Acquire 30 Killmessages over 150ms.
            List<Bitmap> killmessages = new List<Bitmap>();
            int i = 0;
            var myUniqueFileName = $@"{DateTime.Now.Ticks}";
            while (!Worker_KillMessageCleaner.CancellationPending && i < 30)
            {
                Bitmap KillMessage = gcaptr.MakeScreenShot(rectKillMessage);
                killmessages.Add(KillMessage);
                i++;
                Thread.Sleep(5);
            }

            //Clean KillMessage
            Bitmap CleanedKillMessage = helpers.MakeDarkestImage(killmessages, whiteReference);

            //Save it for later improving the OCR
            CleanedKillMessage.Save(@"./learning/KillMessages/" + myUniqueFileName + ".bmp");

            String OCRResult = helpers.OCRText(CleanedKillMessage);
            Worker_KillMessageCleaner.ReportProgress(0, OCRResult);
            e.Result = CleanedKillMessage;
        }


        private void Worker_KillMessageCleaner_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            WriteToLog((String)e.UserState);

            //Trigger OBS Event
            Task.Run(() => TriggerEvent((String)e.UserState));
        }

        private void Worker_KillMessageCleaner_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pBKillMessage.Image = (Bitmap)e.Result;
        }
        #endregion

        #region Learning
        private void Worker_LearnThresholds_DoWork(object sender, DoWorkEventArgs e)
        {
            LearningHelper learninghelper = new LearningHelper();
            int samplescollected = 0;

            while (!Worker_LearnThresholds.CancellationPending)
            {
                while (GetKillNumber() == 0 && !Worker_LearnThresholds.CancellationPending)
                {
                    ScreenData scrData = GetScreenData();
                    if (GetKillNumber() == 0)
                    {
                        scrData.Kill = false;
                        learninghelper.AddData(scrData);
                        samplescollected++;
                    }
                }

                List<int> thisKill = new List<int>();
                while (GetKillNumber() > 0 && !Worker_LearnThresholds.CancellationPending)
                {
                    ScreenData scrData = GetScreenData();
                    if (GetKillNumber() > 0)
                    {
                        scrData.Kill = true;
                        learninghelper.AddData(scrData);
                        samplescollected++;
                    }
                }
            }


            Worker_LearnThresholds.ReportProgress(0, String.Format("Collected a total of {0} samples", samplescollected));

            Worker_LearnThresholds.ReportProgress(0, "Creating model from samples....");

            double accuracy = learninghelper.LearnFromData(5000);

            Worker_LearnThresholds.ReportProgress(0, String.Format("Model created. Accuracy on 20% of testdata is {0}", accuracy));

            learninghelper.SaveModel(@"./learning/KillDetectionModel.mdl");
        }

        private void Worker_LearnThresholds_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            WriteToLog(e.UserState.ToString());
        }

        private void Worker_LearnThresholds_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            WriteToLog("Learning stopped! Press 'Save' to save.");
        }

        public double getStandardDeviation(List<int> values)
        {
            double average = values.Average();
            double sumOfSquaresOfDifferences = values.Select(val => (val - average) * (val - average)).Sum();
            double sd = Math.Sqrt(sumOfSquaresOfDifferences / values.Count());
            return sd;
        }

        public int GetAverageOfIntArray(int[] values)
        {
            int sum = 0;
            int nonNull = 0;
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] > 0)
                {
                    sum += values[i];
                    nonNull++;
                }
            }
            if (nonNull == 0)
                return 0;
            decimal average = (decimal)sum / nonNull;
            return Convert.ToInt16(average);
        }


        private void btnLearnRedThresholds_Click(object sender, EventArgs e)
        {
            if (!Worker_LearnThresholds.IsBusy)
            {
                WriteToLog("Started learning...");
                btnLearnRedThresholds.Text = "Stop";
                Worker_LearnThresholds.RunWorkerAsync();
                btnStart.BackColor = Color.PaleVioletRed;
            }
            else
            {
                WriteToLog("Stopping learning...");
                btnLearnRedThresholds.Text = "Learn Red Thresholds";
                Worker_LearnThresholds.CancelAsync();
                btnLearnRedThresholds.BackColor = SystemColors.Control;
            }
        }
        #endregion

        #region Misc

        private void SetAlert(bool alertstate)
        {
            if (alertstate)
            {
                alert = true;
                tsLblStatus.BackColor = Color.PaleVioletRed;
                tsLblStatus.Text = "ALERT!";


            }
            else
            {
                alert = false;
                tsLblStatus.Text = "";
            }
        }

        /// <summary>
        /// LogWrite helper
        /// </summary>
        /// <param name="message"></param>
        /// <param name="replaceWithlast"></param>
        private void WriteToLog(String message, bool clean = false, bool sameLine = false)
        {
            if (sameLine)
                tBLog.Text = tBLog.Text.Remove(tBLog.Text.LastIndexOf(Environment.NewLine));


            if (clean)
                tBLog.Text += message;
            else
            {
                string tS = DateTime.Now.ToString("HH:mm:ss.F");
                tBLog.AppendText(String.Format("[{0}] : {1}", tS, message));
                tBLog.AppendText(Environment.NewLine);
            }
        }


        private enum ProcessDPIAwareness
        {
            ProcessDPIUnaware = 0,
            ProcessSystemDPIAware = 1,
            ProcessPerMonitorDPIAware = 2
        }

        [System.Runtime.InteropServices.DllImport("shcore.dll")]
        private static extern int SetProcessDpiAwareness(ProcessDPIAwareness value);

        private static void SetDpiAwareness()
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                SetProcessDpiAwareness(ProcessDPIAwareness.ProcessPerMonitorDPIAware);
            }
        }
        #endregion

        

        private void TriggerEvent(String killmessage)
        {
            if(killmessage.ToLower().Contains("shot"))
                WriteToLog(obsRemote.TriggerSceneItem("E_HEADSHOT"));

            if(killmessage.ToLower().Contains("pan"))
                WriteToLog(obsRemote.TriggerSceneItem("E_PAN"));

            if (killmessage.ToLower().Contains("bow"))
                WriteToLog(obsRemote.TriggerSceneItem("E_CROSSBOW"));
        }
    }
}
