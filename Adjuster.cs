using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NvAPIWrapper;
using WindowsDisplayAPI;
using Monitor = NvAPIWrapper.Display.Display;
using Display = WindowsDisplayAPI.Display;
using NvAPIWrapper.Display;

namespace MonitorAdjuster
{
    public partial class Adjuster : Form
    {
        internal static Monitor[] GetAllNvidiaMonitors()
        {
            return Monitor.GetDisplays();
        }
        internal static Display[] GetAllWindowsDisplays()
        {
            return Display.GetDisplays().ToArray();
        }
        private static Display GetWindowsMainDisplay()
        {
            Display[] displays = Display.GetDisplays().ToArray();
            foreach (Display display in displays)
            {
                if (display.DisplayScreen.IsPrimary)
                {
                    return display;
                }
            }

            return null;
        }

        private static Monitor GetNvidiaMainMonitor(int index = int.MaxValue)
        {
            Monitor[] allDisplays = Monitor.GetDisplays();
            if (index != int.MaxValue)
                return allDisplays[index];
            for (int i = 0; i < PathInfo.GetDisplaysConfig().Length; i++)
            {
                PathInfo info = PathInfo.GetDisplaysConfig()[i];
                if (info.Position.X == 0 && info.Position.Y == 0)   //info.IsGDIPrimary
                {
                    return allDisplays[i];
                }
            }

            return null;
        }

        private static int GetMainMonitorIndex()
        {
            for (int i = 0; i < PathInfo.GetDisplaysConfig().Length; i++)
            {
                PathInfo info = PathInfo.GetDisplaysConfig()[i];
                if (info.Position.X == 0 && info.Position.Y == 0)   //info.IsGDIPrimary
                {
                    return i;
                }
            }

            return int.MaxValue;
        }

        internal static Adjuster instance;
        public Adjuster()
        {
            InitializeComponent();
            instance = this;
            InitNvidia();
        }

        private static bool forceResetUsed = false;
        internal static void Reset(bool force = false)
        {
            forceResetUsed = force;
            int i = 0;
            foreach (var item in monitorsDefault)
            {
                var monitor = item.Key;
                foreach (var config in item.Value)
                {
                    switch (config.Key)
                    {
                        case "hue":
                            monitor.HUEControl.CurrentAngle = config.Value;
                            hueSliders[i].Value = monitor.HUEControl.CurrentAngle;
                            break;
                        case "vibrance":
                            monitor.DigitalVibranceControl.CurrentLevel = config.Value;
                            vibranceSliders[i].Value = monitor.DigitalVibranceControl.CurrentLevel;
                            break;
                        default:
                            break;
                    }
                }
                i++;
            }
            i = 0;
            foreach (var item in displaysDefault)
            {
                item.Key.GammaRamp = forceResetUsed ? new DisplayGammaRamp() : item.Value;
                brightnessSliders[i].Value = 50;
                contrastSliders[i].Value = 50;
                gammaSliders[i].Value = 100;
                i++;
            }
        }

        internal static Dictionary<Monitor, Dictionary<string, int>> monitorsDefault = new Dictionary<Monitor, Dictionary<string, int>>();
        internal static Dictionary<Display, DisplayGammaRamp> displaysDefault = new Dictionary<Display, DisplayGammaRamp>();
        internal static Dictionary<int, TrackBar> hueSliders = new Dictionary<int, TrackBar>();
        internal static Dictionary<int, TrackBar> vibranceSliders = new Dictionary<int, TrackBar>();
        internal static Dictionary<int, TrackBar> brightnessSliders = new Dictionary<int, TrackBar>();
        internal static Dictionary<int, TrackBar> contrastSliders = new Dictionary<int, TrackBar>();
        internal static Dictionary<int, TrackBar> gammaSliders = new Dictionary<int, TrackBar>();

        internal static int displaysCount = 0;

        internal static Monitor[] monitors;
        internal static Display[] displays;
        internal static void InitNvidia()
        {
            try
            {
                NVIDIA.Initialize();
            }
            catch (Exception exc)
            {
                MessageBox.Show($"Nvidia API Initialization Failed... \n{exc.StackTrace}");
                return;
            }
            //int nvidiaMainMonitorIndex = GetMainMonitorIndex();
            //Monitor monitor = GetNvidiaMainMonitor(nvidiaMainMonitorIndex);
            //Display display = GetWindowsMainDisplay();
            //MessageBox.Show($"Main nvidia monitor: {PathInfo.GetDisplaysConfig()[nvidiaMainMonitorIndex].ToString()}, main windows display: {display.ToPathDisplayTarget().FriendlyName}");

            monitors = GetAllNvidiaMonitors();
            displays = GetAllWindowsDisplays();
            displaysCount = displays.Count();
            ConfigManager.LoadConfig();
            var tempMonitorConfig = new Dictionary<string, int>();
            var tempDisplayConfig = new Dictionary<string, double>();
            for (int i = 0; i < displaysCount; i++)
            {
                var monitor = monitors[i];
                tempMonitorConfig.Clear();
                tempMonitorConfig["hue"] = monitor.HUEControl.CurrentAngle;

                tempMonitorConfig["vibrance"] = monitor.DigitalVibranceControl.CurrentLevel;
                monitorsDefault[monitor] = tempMonitorConfig;
                var display = displays[i];
                tempDisplayConfig.Clear();
                displaysDefault[display] = display.GammaRamp;
            }

            var font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            for (int i = 0; i < displays.Count(); i++)
            {
                var monitor = monitors[i];
                var display = displays[i];

                //Nvidia API monitor section
                var monitorLabel = new Label();
                var hueLabel = new Label();
                var vibranceLabel = new Label();
                monitorLabel.AutoSize = vibranceLabel.AutoSize = hueLabel.AutoSize = true;
                monitorLabel.Size = vibranceLabel.Size = hueLabel.Size = new Size(60, 25);
                monitorLabel.TabIndex = vibranceLabel.TabIndex = hueLabel.TabIndex = 0;
                monitorLabel.Font = vibranceLabel.Font = hueLabel.Font = font;

                monitorLabel.Location = new Point(25 + (i * 175), 125);
                hueLabel.Location = new Point(25 + (i * 175), 175);
                vibranceLabel.Location = new Point(25 + (i * 175), 300);
                monitorLabel.Name = $"monitorLabel{i + 1}";
                hueLabel.Name = $"hueLabel{i + 1}";
                vibranceLabel.Name = $"vibranceLabel{i + 1}";
                monitorLabel.Text = $"Monitor {i + 1}";
                hueLabel.Text = $"HUE {i + 1}";
                vibranceLabel.Text = $"Vibrance {i + 1}";

                var hueSlider = new TrackBar();
                var vibranceSlider = new TrackBar();
                vibranceSlider.Size = hueSlider.Size = new Size(150, 70);
                vibranceSlider.TabIndex = hueSlider.TabIndex = 1;
                hueSlider.Location = new Point(25 + (i * 175), 225);
                vibranceSlider.Location = new Point(25 + (i * 175), 350);
                hueSlider.Name = $"hueTrackBar{i}";
                vibranceSlider.Name = $"vibranceTrackBar{i}";

                hueSlider.Minimum = 0;
                hueSlider.Maximum = 359;
                hueSlider.Value = monitor.HUEControl.CurrentAngle;

                vibranceSlider.Minimum = monitor.DigitalVibranceControl.MinimumLevel;
                vibranceSlider.Maximum = monitor.DigitalVibranceControl.MaximumLevel;
                vibranceSlider.Value = monitor.DigitalVibranceControl.CurrentLevel;

                hueSlider.Scroll += new EventHandler((object sender, EventArgs e) => { monitor.HUEControl.CurrentAngle = ((TrackBar)sender).Value; });
                vibranceSlider.Scroll += new EventHandler((object sender, EventArgs e) => { monitor.DigitalVibranceControl.CurrentLevel = ((TrackBar)sender).Value; });

                //Windows display section
                var brightnessLabel = new Label();
                var contrastLabel = new Label();
                var gammaLabel = new Label();
                brightnessLabel.AutoSize = contrastLabel.AutoSize = gammaLabel.AutoSize = true;
                brightnessLabel.Size = contrastLabel.Size = gammaLabel.Size = new System.Drawing.Size(60, 25);
                brightnessLabel.TabIndex = contrastLabel.TabIndex = gammaLabel.TabIndex = 0;
                brightnessLabel.Font = contrastLabel.Font = gammaLabel.Font = font;

                brightnessLabel.Location = new Point(25 + (i * 175), 425);
                contrastLabel.Location = new Point(25 + (i * 175), 550);
                gammaLabel.Location = new Point(25 + (i * 175), 675);
                brightnessLabel.Name = $"brightnessLabel{i + 1}";
                contrastLabel.Name = $"contrastLabel{i + 1}";
                gammaLabel.Name = $"gammaLabel{i + 1}";
                brightnessLabel.Text = $"Brightness {i + 1}";
                contrastLabel.Text = $"Contrast {i + 1}";
                gammaLabel.Text = $"Gamma {i + 1}";

                var brightnessSlider = new TrackBar();
                var contrastSlider = new TrackBar();
                var gammaSlider = new TrackBar();
                brightnessSlider.Size = contrastSlider.Size = gammaSlider.Size = new System.Drawing.Size(150, 70);
                brightnessSlider.TabIndex = contrastSlider.TabIndex = gammaSlider.TabIndex = 1;
                brightnessSlider.Location = new Point(25 + (i * 175), 475);
                contrastSlider.Location = new Point(25 + (i * 175), 600);
                gammaSlider.Location = new Point(25 + (i * 175), 725);
                brightnessSlider.Name = $"brightnessTrackBar{i}";
                contrastSlider.Name = $"contrastTrackBar{i}";
                gammaSlider.Name = $"gammaTrackBar{i}";

                brightnessSlider.Minimum = 0;
                brightnessSlider.Maximum = 100;
                brightnessSlider.Value = 50;

                contrastSlider.Minimum = 0;
                contrastSlider.Maximum = 100;
                contrastSlider.Value = 50;

                gammaSlider.Minimum = 30;
                gammaSlider.Maximum = 280;
                gammaSlider.Value = 100;

                brightnessSlider.Scroll += new EventHandler((object sender, EventArgs e) =>  { display.GammaRamp = new DisplayGammaRamp((double)(((TrackBar)sender).Value) / 100,    (double)contrastSlider.Value / 100, (double)gammaSlider.Value / 100); });
                contrastSlider.Scroll += new EventHandler((object sender, EventArgs e) =>    { display.GammaRamp = new DisplayGammaRamp((double)brightnessSlider.Value / 100, (double)(((TrackBar)sender).Value) / 100, (double)gammaSlider.Value / 100); });
                gammaSlider.Scroll += new EventHandler((object sender, EventArgs e) =>       { display.GammaRamp = new DisplayGammaRamp((double)brightnessSlider.Value / 100, (double)contrastSlider.Value / 100, (double)(((TrackBar)sender).Value) / 100); });


                instance.Controls.Add(monitorLabel);
                instance.Controls.Add(hueLabel);
                instance.Controls.Add(vibranceLabel);
                instance.Controls.Add(hueSlider);
                instance.Controls.Add(vibranceSlider);

                hueSliders[i] = hueSlider;
                vibranceSliders[i] = vibranceSlider;

                instance.Controls.Add(brightnessLabel);
                instance.Controls.Add(contrastLabel);
                instance.Controls.Add(gammaLabel);
                instance.Controls.Add(brightnessSlider);
                instance.Controls.Add(contrastSlider);
                instance.Controls.Add(gammaSlider);

                brightnessSliders[i] = brightnessSlider;
                contrastSliders[i] = contrastSlider;
                gammaSliders[i] = gammaSlider;

                foreach (var item in instance.Controls)
                {
                    if (item.GetType() == typeof(TrackBar))
                    {
                        ((ISupportInitialize)(item)).BeginInit();
                    }
                }
            }
        }

        private void resetButton_Click(object sender, EventArgs e) => Reset();

        private void forceReset_Click(object sender, EventArgs e) => Reset(true);

        private void loadButton_Click(object sender, EventArgs e, int num)
        {
            ConfigManager.LoadConfig(num);
        }

        private void saveButton_Click(object sender, EventArgs e, int num)
        {
            ConfigManager.SaveConfig(num);
        }
    }
}
