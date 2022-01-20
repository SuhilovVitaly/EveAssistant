using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using EveAssistant.Common.Device;

namespace EveAssistant.Logic.GameClient
{
    public class LogHarvester
    {
        public IDevice Device { get; set; }

        private int _linesInLogFileBeforeCheck;
        private int _withoutNewLines;

        public LogHarvester(IDevice device)
        {
            Device = device;

            UpdateCurrentLog(Device.Pilot);
        }

        private void UpdateCurrentLog(string pilotName, bool isInitialization = false)
        {
            if (string.IsNullOrEmpty(pilotName)) return;

            if (pilotName.Trim() == "None") return;

            try
            {
                var settingsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"EVE\logs\Gamelogs\");

                var logsFolder = new DirectoryInfo(settingsFolder);

                var files = logsFolder.GetFiles().OrderByDescending(p => p.CreationTime).ToArray();

                foreach (var file in files)
                {
                    var lines = File.ReadAllLines(file.FullName, Encoding.UTF8);

                    for (var i = 0; i < lines.Length; i++)
                    {
                        if (lines[i].Contains("Listener:"))
                        {
                            try
                            {
                                var listener = lines[i].Split(new[] { "Listener:" }, StringSplitOptions.None)[1].Trim();

                                if (listener == pilotName)
                                {
                                    Device.LogFileName = file.FullName;
                                    _linesInLogFileBeforeCheck = lines.Length;
                                    //Device.Logger("[LogHarvester] Found log file for '" + pilotName + "'. Name is " + file.FullName);
                                    return;
                                }
                            }
                            catch (Exception e)
                            {
                                Device.Logger("[LogHarvester] Error. " + e.Message);
                            }
                        }
                    }
                }
            }
            catch
            {

            }

            Device.Logger("[LogHarvester] Critical Error. Configuration for '" + pilotName + "' not found.");
        }

        private bool isNeedParsing = true;

        public void Execute()
        {
            while (isNeedParsing)
            {
                ParseLogForStatus();

                if (_withoutNewLines > 10)
                {
                    _withoutNewLines = 0;

                    // Search new log file
                    UpdateCurrentLog(Device.Pilot);
                }

                Thread.Sleep(3000);
            }
        }

        public string[] WriteSafeReadAllLines(string path)
        {
            using (var csv = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var sr = new StreamReader(csv))
            {
                var file = new List<string>();
                while (!sr.EndOfStream)
                {
                    file.Add(sr.ReadLine());
                }

                return file.ToArray();
            }
        }

        public void ParseLogForStatus()
        {
            var logFileName = Device.LogFileName;
            string[] lines;

            //if (Device.IsDebug)
            //    Device.Logger("[LogHarvester] Start execute");

            if (Device.LogFileName == null)
            {
                UpdateCurrentLog(Device.Pilot);
                return;
            }

            try
            {
                lines = WriteSafeReadAllLines(logFileName);
            }
            catch (Exception e)
            {
                Device.Logger("[ParseLogForStatus] Error. Message is " + e.Message);
                return;
            }

            if (Device.IsDebug)
            {
                //Device.Logger("[LogHarvester] lines before     - " + _linesInLogFileBeforeCheck);
                //Device.Logger("[LogHarvester] lines current    - " + lines.Length);
            }

            if (lines.Length > _linesInLogFileBeforeCheck)
            {
                _withoutNewLines = 0;

                for (int i = _linesInLogFileBeforeCheck; i < lines.Length; i++)
                {
                    if (lines[i].Contains("0xff00ffff"))
                    {
                        if (lines[i].Contains("Heavy Neutron Blaster"))
                        {
                            Device.Metrics.LastWeaponActivity = DateTime.Now;
                            Device.Events.WeaponActivity();
                        }

                        if (lines[i].Contains("Heavy Missile"))
                        {
                            Device.Metrics.LastWeaponActivity = DateTime.Now;
                            Device.Events.WeaponActivity();
                        }


                        if (lines[i].Contains("Hammerhead"))
                        {
                            Device.Metrics.LastDroneActivity = DateTime.Now;
                            Device.Events.DronesActivity();
                        }

                    }

                    if (lines[i].Contains("You are already managing"))
                    {
                        Device.Metrics.LastTargetLimitExceeded = DateTime.Now;
                        Device.Events.TargetLimitExceeded();
                    }

                    if (lines[i].Contains("0x77ffffff"))
                    {
                        Device.Metrics.LastEnemyAttack = DateTime.Now;
                        //Device.Events.EnemyAttack();
                    }

                    if (lines[i].Contains("this will take approximately"))
                    {
                        Device.Metrics.LastWeaponReload = DateTime.Now;
                        Device.Events.WeaponReload();
                    }

                    if (lines[i].Contains("Loading the Heavy Missile"))
                    {
                        Device.Metrics.LastWeaponReload = DateTime.Now;
                        Device.Events.WeaponReload();
                    }

                    if (lines[i].Contains("no longer present"))
                    {
                        Device.Metrics.TargetNoLongerPresent = DateTime.Now;
                        Device.Events.TargetNoLongerPresent();
                    }

                    if (lines[i].Contains("Jumping from"))
                    {
                        Device.Metrics.JumpThroughStarGate = DateTime.Now;
                        Device.Events.JumpThroughStarGate();
                    }

                    if (lines[i].Contains("Interference from the cloaking"))
                    {
                        Device.Metrics.CommandRejectedByCloakReasone = DateTime.Now;
                        Device.Events.CommandRejectedByCloakReasone();
                    }

                    if (lines[i].Contains("Cargo is too far away"))
                    {
                        Device.Metrics.TractorIsTooFarAwat = DateTime.Now;
                        //Device.Events.TractorIsTooFarAwat();
                    }

                    if (lines[i].Contains("has run out of charges"))
                    {
                        //OnRunOutOfCharges?.Invoke();
                    }

                    // POINT: Harvest Log
                    //finds the resource it was harvesting
                    if (lines[i].Contains("finds the resource it was harvesting"))
                    {
                        Device.Metrics.ResourceWasHarvesting = DateTime.Now;
                        Device.Events.ResourceWasHarvesting();
                    }

                    // has run out of charges

                    if (lines[i].Contains("cargo hold is full"))
                    {
                        Device.Metrics.CargoHoldIsFull = DateTime.Now;
                        Device.Events.CargoHoldIsFull();
                    }


                    if (lines[i].Contains("station denies you permission to dock"))
                    {
                        Device.Metrics.StationDenied = DateTime.Now;
                        Device.Events.StationDenied();
                    }

                    if (lines[i].Contains("You mined"))
                    {
                        Device.Metrics.MinedOre = DateTime.Now;
                        Device.Events.MinedOre();
                    }

                    if (lines[i].Contains("Mining Foreman Burst I has run out of charges"))
                    {
                        Device.Metrics.ReloadMiningForemanBurstCharges = DateTime.Now;
                        Device.Events.ReloadMiningForemanBurstCharges();
                    }

                    if (lines[i].Contains("Mining Foreman Burst II has run out of charges"))
                    {
                        Device.Metrics.ReloadMiningForemanBurstCharges = DateTime.Now;
                        Device.Events.ReloadMiningForemanBurstCharges();
                    }

                    //

                    // Cargo is too far away. Ship is on automatic approach to cargo.

                    //Mobile Tractor Unit cannot be deployed within 5,000 m of a Mobile Tractor Unit

                    //Undocking from

                    //You successfully salvage from the Triglavian Moderate Wreck
                    //Your salvager successfully completes its cycle

                }
            }
            else
            {
                _withoutNewLines++;
            }

            _linesInLogFileBeforeCheck = lines.Length;

           // if (Device.IsDebug)
               // Device.Logger("[LogHarvester] lines after      - " + _linesInLogFileBeforeCheck);
        }
    }
}