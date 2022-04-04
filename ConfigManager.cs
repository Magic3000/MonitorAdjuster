using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsDisplayAPI;

namespace MonitorAdjuster
{
    class ConfigManager
    {
        internal static Dictionary<int, Dictionary<int, Dictionary<string,string>>> config;

        private static string configPath = "config.txt";
        internal static void LoadConfig(int num = 0)
        {
            if (num > 0)
            {
                var monitors = Adjuster.monitors;
                var displays = Adjuster.displays;
                var cachedGammaRamps = new Dictionary<int, Dictionary<string, double>>();

                foreach (var item in Adjuster.instance.Controls)
                {
                    if (item.GetType() == typeof(TrackBar))
                    {
                        var slider = (TrackBar)item;
                        var name = slider.Name;
                        for (int i = 0; i < Adjuster.displaysCount; i++)
                        {
                            if (!cachedGammaRamps.ContainsKey(i))
                                cachedGammaRamps[i] = new Dictionary<string, double>();
                            if (name == $"hueTrackBar{i}")
                            {
                                monitors[i].HUEControl.CurrentAngle = int.Parse(config[num][i]["hue"]);
                                Adjuster.hueSliders[i].Value = monitors[i].HUEControl.CurrentAngle;
                            }
                            else if (name == $"vibranceTrackBar{i}")
                            {
                                monitors[i].DigitalVibranceControl.CurrentLevel = int.Parse(config[num][i]["vibrance"]);
                                Adjuster.vibranceSliders[i].Value = monitors[i].DigitalVibranceControl.CurrentLevel;
                            }
                            else if (name == $"brightnessTrackBar{i}")
                            {
                                var brightness = int.Parse(config[num][i]["brightness"]);
                                cachedGammaRamps[i]["brightness"] = brightness;
                                Adjuster.brightnessSliders[i].Value = brightness;

                            }
                            else if (name == $"contrastTrackBar{i}")
                            {
                                var contrast = int.Parse(config[num][i]["contrast"]);
                                cachedGammaRamps[i]["contrast"] = contrast;
                                Adjuster.contrastSliders[i].Value = contrast;
                            }
                            else if (name == $"gammaTrackBar{i}")
                            {
                                var gamma = int.Parse(config[num][i]["gamma"]);
                                cachedGammaRamps[i]["gamma"] = gamma;
                                Adjuster.gammaSliders[i].Value = gamma;
                            }
                        }
                    }
                }
                for (int i = 0; i < Adjuster.displaysCount; i++)
                {
                    displays[i].GammaRamp = new DisplayGammaRamp((double)(cachedGammaRamps[i]["brightness"]) / 100, (double)(cachedGammaRamps[i]["contrast"]) / 100, (double)(cachedGammaRamps[i]["gamma"]) / 100);
                }
            }
            else
            {
                if (!File.Exists(configPath))
                {
                    File.Create(configPath).Close();
                    File.WriteAllText(configPath, "");
                }
                config = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, Dictionary<string, string>>>>(File.ReadAllText(configPath));
                if (config == null)
                    config = new Dictionary<int, Dictionary<int, Dictionary<string, string>>>();
            }
        }

        internal static void SaveConfig(int num)
        {
            var tempDict = new Dictionary<string, string>();
            foreach (var item in Adjuster.instance.Controls)
            {
                if (item.GetType() == typeof(TrackBar))
                {
                    var slider = (TrackBar)item;
                    var name = slider.Name;
                    var val = slider.Value.ToString();
                    for (int i = 0; i < Adjuster.displaysCount; i++)
                    {
                        if (!config.ContainsKey(num))
                            config[num] = new Dictionary<int, Dictionary<string, string>>();
                        if (!config[num].ContainsKey(i))
                            config[num][i] = new Dictionary<string, string>();
                        if (name == $"hueTrackBar{i}")
                        {
                            tempDict["hue"] = val;
                            config[num][i]["hue"] = val;
                        }
                        else if (name == $"vibranceTrackBar{i}")
                        {
                            tempDict["vibrance"] = val;
                            config[num][i]["vibrance"] = val;
                        }
                        else if (name == $"brightnessTrackBar{i}")
                        {
                            tempDict["brightness"] = val;
                            config[num][i]["brightness"] = val;
                        }
                        else if (name == $"contrastTrackBar{i}")
                        {
                            tempDict["contrast"] = val;
                            config[num][i]["contrast"] = val;
                        }
                        else if (name == $"gammaTrackBar{i}")
                        {
                            tempDict["gamma"] = val;
                            config[num][i]["gamma"] = val;
                        }
                    }
                }
            }

            File.WriteAllText(configPath, JsonConvert.SerializeObject(config));
        }
    }
}
