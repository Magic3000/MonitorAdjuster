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
        private static Monitor[] GetAllNvidiaMonitors()
        {
            return Monitor.GetDisplays();
        }
        private static Display[] GetAllWindowsDisplays()
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

        internal static void Reset()
        {
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
                var display = item.Key;
                double brightness = 0;
                double contrast = 0;
                double gamma = 0;
                foreach (var config in item.Value)
                {
                    switch (config.Key)
                    {
                        case "brightness":
                            brightness = config.Value;
                            break;
                        case "contrast":
                            contrast = config.Value;
                            break;
                        case "gamma":
                            gamma = config.Value;
                            break;
                        default:
                            break;
                    }
                }
                display.GammaRamp = new DisplayGammaRamp(brightness, contrast, gamma); ;
            }
        }

        internal static Dictionary<Monitor, Dictionary<string, int>> monitorsDefault = new Dictionary<Monitor, Dictionary<string, int>>();
        internal static Dictionary<Display, Dictionary<string, double>> displaysDefault = new Dictionary<Display, Dictionary<string, double>>();
        internal static Dictionary<int, TrackBar> hueSliders = new Dictionary<int, TrackBar>();
        internal static Dictionary<int, TrackBar> vibranceSliders = new Dictionary<int, TrackBar>();
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

            var monitors = GetAllNvidiaMonitors();
            var displays = GetAllWindowsDisplays();
            var tempMonitorConfig = new Dictionary<string, int>();
            var tempDisplayConfig = new Dictionary<string, double>();
            for (int i = 0; i < displays.Count(); i++)
            {
                var monitor = monitors[i];
                tempMonitorConfig.Clear();
                tempMonitorConfig["hue"] = monitor.HUEControl.CurrentAngle;
                //MessageBox.Show($"Cached hue {tempMonitorConfig["hue"]} for {i + 1} monitor");

                tempMonitorConfig["vibrance"] = monitor.DigitalVibranceControl.CurrentLevel;
                monitorsDefault[monitor] = tempMonitorConfig;
                var display = displays[i];
                tempDisplayConfig.Clear();
                tempDisplayConfig["brightness"] = 0.5;
                tempDisplayConfig["contrast"] = 0.5;
                tempDisplayConfig["gamma"] = 1;
                displaysDefault[display] = tempDisplayConfig;
            }

            var font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            for (int i = 0; i < displays.Count(); i++)
            {
                var monitor = monitors[i];
                var display = displays[i];
                var monitorLabel = new System.Windows.Forms.Label();
                var hueLabel = new System.Windows.Forms.Label();
                var vibranceLabel = new System.Windows.Forms.Label();
                monitorLabel.AutoSize = vibranceLabel.AutoSize = hueLabel.AutoSize = true;
                monitorLabel.Size = vibranceLabel.Size = hueLabel.Size = new System.Drawing.Size(60, 25);
                monitorLabel.TabIndex = vibranceLabel.TabIndex = hueLabel.TabIndex = 0;
                monitorLabel.Font = vibranceLabel.Font = hueLabel.Font = font;

                monitorLabel.Location = new System.Drawing.Point(25 + (i * 175), 75);
                hueLabel.Location = new System.Drawing.Point(25 + (i * 175), 125);
                vibranceLabel.Location = new System.Drawing.Point(25 + (i * 175), 250);
                monitorLabel.Name = $"monitor{i + 1}";
                hueLabel.Name = $"hueLabel{i}";
                vibranceLabel.Name = $"vibranceLabel{i}";
                monitorLabel.Text = $"Monitor {i + 1}";
                vibranceLabel.Text = $"Vibrance {i + 1}";
                hueLabel.Text = $"HUE {i + 1}";

                var hueSlider = new TrackBar();
                var vibranceSlider = new TrackBar();
                vibranceSlider.Size = hueSlider.Size = new System.Drawing.Size(150, 70);
                vibranceSlider.TabIndex = hueSlider.TabIndex = 1;
                hueSlider.Location = new System.Drawing.Point(25 + (i * 175), 175);
                vibranceSlider.Location = new System.Drawing.Point(25 + (i * 175), 300);
                hueSlider.Name = $"vibranceTrackBar{i}";
                vibranceSlider.Name = $"hueTrackBar{i}";
                hueSlider.Minimum = 0;
                hueSlider.Maximum = 359;
                hueSlider.Value = monitor.HUEControl.CurrentAngle;
                vibranceSlider.Minimum = monitor.DigitalVibranceControl.MinimumLevel;
                vibranceSlider.Maximum = monitor.DigitalVibranceControl.MaximumLevel;
                vibranceSlider.Value = monitor.DigitalVibranceControl.CurrentLevel;
                hueSlider.Scroll += new System.EventHandler((object sender, EventArgs e) => { monitor.HUEControl.CurrentAngle = ((System.Windows.Forms.TrackBar)sender).Value; });
                vibranceSlider.Scroll += new System.EventHandler((object sender, EventArgs e) => { monitor.DigitalVibranceControl.CurrentLevel = ((System.Windows.Forms.TrackBar)sender).Value; });


                instance.Controls.Add(monitorLabel);
                instance.Controls.Add(hueLabel);
                instance.Controls.Add(vibranceLabel);
                instance.Controls.Add(hueSlider);
                instance.Controls.Add(vibranceSlider);
                hueSliders[i] = hueSlider;
                vibranceSliders[i] = vibranceSlider;
                foreach (var item in instance.Controls)
                {
                    if (item.GetType() == typeof(TrackBar))
                    {
                        ((System.ComponentModel.ISupportInitialize)(item)).BeginInit();
                    }
                }
            }
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            Reset();
        }
    }
}
