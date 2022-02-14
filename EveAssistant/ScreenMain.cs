using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using EveAssistant.Common.Device;
using EveAssistant.Common.Patterns;
using EveAssistant.Configuration;
using EveAssistant.Logic.Devices;
using EveAssistant.Logic.GameClient;
using EveAssistant.Logic.Jobs;
using EveAssistant.Logic.Jobs.Actions;
using EveAssistant.Logic.Jobs.Events;
using EveAssistant.Logic.Jobs.Operations;
using EveAssistant.Logic.Jobs.Status;
using EveAssistant.Logic.Ships;
using EveAssistant.Logic.Ships.Frigates;
using EveAssistant.Logic.Tools;
using EveAssistant.UI;
using log4net;

namespace EveAssistant
{
    public partial class ScreenMain : Form
    {
        private readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Client _selectedGameClient;

        Hashtable _devices = new Hashtable();

        public ScreenMain()
        {
            InitializeComponent();
        }

        private void ScreenMain_Load(object sender, EventArgs e)
        {
            var clients = Active.GetList(Global.ApplicationSettings.EveOnlineClientTitle);

            cmbActiveClients.Items.Clear();

            cmbActiveClients.SelectedIndexChanged -= cmbActiveClients_SelectedIndexChanged;

            foreach (var client in clients)
            {
                cmbActiveClients.Items.Add(new ComboboxItem { Text = client.Name, Value = client.HWnd });
            }

            cmbActiveClients.SelectedIndexChanged += cmbActiveClients_SelectedIndexChanged;

            if (clients.Count > 0)
                cmbActiveClients.SelectedIndex = cmbActiveClients.FindStringExact(clients[0].Name);
        }

        private void cmbActiveClients_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedGameClient = new Client(((ComboboxItem)cmbActiveClients.SelectedItem).Text, ((ComboboxItem)cmbActiveClients.SelectedItem).Value);
        }

        private void cmdPrintScreen_Click(object sender, EventArgs e)
        {
            _selectedGameClient = new Client(((ComboboxItem)cmbActiveClients.SelectedItem).Text, ((ComboboxItem)cmbActiveClients.SelectedItem).Value);

            ScreenCapture.ScreenShot(_selectedGameClient.HWnd, "Screen", LogWrite);
        }

        private delegate void SetTextCallback(string text);

        private void LogWrite(string message)
        {
            if (txtLog.InvokeRequired)
            {
                var d = new SetTextCallback(LogWrite);
                Invoke(d, message);
            }
            else
            {
                _log.Debug(message);

                if (checkBox1.Checked)
                {
                    if (message.Contains(" | "))
                    {
                        var lines = txtLog.Lines;

                        try
                        {
                            if (lines[0].Contains(" | "))
                            {
                                lines[0] = DateTime.Now.ToString("HH:mm:ss") + @" - " + message;

                                txtLog.Lines = lines;

                                return;
                            }
                        }
                        catch
                        {
                            // ignored
                        }
                    }


                    txtLog.Text = DateTime.Now.ToString("HH:mm:ss") + @" - " + message + Environment.NewLine + txtLog.Text;

                    if (txtLog.Lines.Length > 35)
                    {
                        var newLines = new string[35];

                        Array.Copy(txtLog.Lines, 0, newLines, 0, 35);

                        txtLog.Lines = newLines;
                    }

                    txtLog.Refresh();
                }

            }
        }

        Hashtable jobs = new Hashtable();

        private void button19_Click(object sender, EventArgs e)
        {

            foreach (var item in cmbActiveClients.Items)
            {
                var pilotInformation = item as ComboboxItem;

                var device = new WindowClientDevice(pilotInformation.Value, LogWrite, Global.ApplicationSettings.Shortcuts, Global.PatternFactory, pilotInformation.Text.Trim())
                    {
                        Id = DateTime.UtcNow.Ticks
                    };

                var ship = new Punisher();

                RunJob(new AbissHarvest(device, ship));

                Thread.Sleep(10000);
            }

            //var pilot = (ComboboxItem)cmbActiveClients.SelectedItem;

            timer1.Enabled = true;
        }

        private async void RunJob(AbissHarvest job)
        {
            _devices.Add(job.Device.Pilot, job.Device);

            jobs.Add(job.Device.Pilot, job);

            await job.Execute(job.CancellationToken.Token);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var pilot = (ComboboxItem)cmbActiveClients.SelectedItem;

            var device = new WindowClientDevice(pilot.Value, LogWrite, Global.ApplicationSettings.Shortcuts, Global.PatternFactory, pilot.Text.Trim());

            var ship = new Punisher();

            Task.Run(() =>
            {
                Thread.Sleep(500);

                BringToFront(device.IntPtr);

                var actionOpenOverviewTab = new NpcFarmJobInitialization(device, ship);

                actionOpenOverviewTab.Execute();
            });
        }

        private void BringToFront(IntPtr hWnd)
        {
            //User32.SetForegroundWindow(hWnd);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var (device, ship) = GetGeneralContext();

            Task.Run(() =>
            {
                Thread.Sleep(500);

                OperationsManager.Execute(OperationTypes.WarpToBookmark, device, ship, "Point_1");
            });
        }

        private (IDevice Device, IShip Ship) GetGeneralContext()
        {
            var pilot = (ComboboxItem)cmbActiveClients.SelectedItem;

            var device = new WindowClientDevice(pilot.Value, LogWrite, Global.ApplicationSettings.Shortcuts, Global.PatternFactory, pilot.Text.Trim());

            var ship = new Punisher();

            return (device, ship);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var (device, ship) = GetGeneralContext();

            Task.Run(() =>
            {
                Thread.Sleep(500);
                OperationsManager.Execute(OperationTypes.DockToBookmark, device, ship, "Home");
            });
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var (device, ship) = GetGeneralContext();

            device.IsDebug = true;

            AllStates.IsShipStopped(device);
        }

        private BaseEvent baseEvent;

        private async void button5_Click(object sender, EventArgs e)
        {
            var (device, ship) = GetGeneralContext();

            device.IsDebug = true;

            baseEvent = new BaseEvent(device, ship);

            baseEvent.OnBaseEvent += Event_Base;

            await baseEvent.Execute();
        }

        private void Event_Base(string arg1, int arg2)
        {
            LogWrite(arg1);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            baseEvent.Stop();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var (device, ship) = GetGeneralContext();

            device.IsDebug = false;

            Task.Run(() =>
            {
                Thread.Sleep(500);

                BringToFront(device.IntPtr);

                var actionOpenOverviewTab = new StationExit(device, ship);

                actionOpenOverviewTab.Execute();
            });
        }

        private void button8_Click(object sender, EventArgs e)
        {
            var (device, ship) = GetGeneralContext();

            Task.Run(() =>
            {
                Thread.Sleep(500);
                OperationsManager.Execute(OperationTypes.OpenOverviewTab, device, ship, Types.OverviewTabNpc);
            });
        }

        private void button9_Click(object sender, EventArgs e)
        {
            var (device, ship) = GetGeneralContext();

            Task.Run(() =>
            {
                Thread.Sleep(500);
                OperationsManager.Execute(OperationTypes.OpenOverviewTab, device, ship, Types.OverviewTabMove);
            });
        }

        private void button10_Click(object sender, EventArgs e)
        {
            var (device, ship) = GetGeneralContext();

            Task.Run(() =>
            {
                Thread.Sleep(500);

                OperationsManager.Execute(OperationTypes.OpenOverviewTab, device, ship, Types.OverviewTabLoot);
            });
        }

        private void button11_Click(object sender, EventArgs e)
        {
            var (device, ship) = GetGeneralContext();

            Task.Run(() =>
            {
                Thread.Sleep(500);
                OperationsManager.Execute(OperationTypes.OpenOverviewTab, device, ship, Types.OverviewTabGates);
            });
        }

        private void button12_Click(object sender, EventArgs e)
        {
            var (device, ship) = GetGeneralContext();

            device.IsDebug = false;

            Task.Run(() =>
            {
                Thread.Sleep(500);

                BringToFront(device.IntPtr);

                var actionOpenOverviewTab = new WarpToBookmark(device, ship, Types.BookmarksAbissHarvest);

                actionOpenOverviewTab.Execute();
            });
        }

        private void button13_Click(object sender, EventArgs e)
        {
            var (device, ship) = GetGeneralContext();

            device.IsDebug = false;

            Task.Run(() =>
            {
                Thread.Sleep(500);

                BringToFront(device.IntPtr);

                var action = new EnterToAbiss(device, ship);

                action.Execute();
            });
        }

        private void button14_Click(object sender, EventArgs e)
        {
            //OperationEnableActiveModules
            var (device, ship) = GetGeneralContext();

            Task.Run(() =>
            {
                Thread.Sleep(500);

                OperationsManager.Execute(OperationTypes.EnableActiveModules, device, ship);
            });
        }

        private void button15_Click(object sender, EventArgs e)
        {
            var (device, ship) = GetGeneralContext();

            Task.Run(() =>
            {
                Thread.Sleep(500);

                OperationsManager.Execute(OperationTypes.EnterToTrace, device, ship);
            });
        }

        private void button16_Click(object sender, EventArgs e)
        {
            var (device, ship) = GetGeneralContext();

            device.IsDebug = false;

            Task.Run(() =>
            {
                Thread.Sleep(500);

                BringToFront(device.IntPtr);

                var action = new NpcKill(device, ship);

                action.Execute();
            });
        }

        private void button17_Click(object sender, EventArgs e)
        {
            var (device, ship) = GetGeneralContext();

            device.IsDebug = false;

            Task.Run(() =>
            {
                Thread.Sleep(500);

                BringToFront(device.IntPtr);

                var action = new BudkaKill(device, ship);

                action.Execute();
            });
        }

        private void button18_Click(object sender, EventArgs e)
        {
            var (device, ship) = GetGeneralContext();

            Task.Run(() =>
            {
                Thread.Sleep(500);

                OperationsManager.Execute(OperationTypes.JumpToAbissGate, device, ship);
            });
        }

        private void crlSettingsReset_Click(object sender, EventArgs e)
        {
            

            SettingsOverrider.Execute(LogWrite);
        }

        private void button20_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Event_Refresh(object sender, EventArgs e)
        {
            var selectedClient = ((ComboboxItem)cmbActiveClients.SelectedItem).Text;

            if (jobs[selectedClient.Trim()] is null) return;

            var Job = jobs[selectedClient.Trim()] as AbissHarvest;

            if (Job is null) return;

            txtJobWorkTime.Text = DateTime.Now.Subtract(Job.Device.Metrics.StartJobTime).ToString();

            txtCycleWorkTime.Text = DateTime.Now.Subtract(Job.Device.Metrics.StartCycleTime).ToString();

            txtCycles.Text = Job.Device.Metrics.NumberOfCycles + "";

            lblPoicket.Text = Job.Device.Metrics.PocketNumber + "";

            txtAverageCycleTime.Text = Job.Device.Metrics.AverageCycleTime.ToString();

            lblActiveActionProgress.Text = Job.Device.Action;
        }

        private void button21_Click(object sender, EventArgs e)
        {
            var (device, ship) = GetGeneralContext();

            Task.Run(() =>
            {
                Thread.Sleep(500);

                OperationsManager.Execute(OperationTypes.OpenItemHangarFilters, device, ship);
            });
        }

        private void button22_Click(object sender, EventArgs e)
        {
            var (device, ship) = GetGeneralContext();

            Task.Run(() =>
            {
                Thread.Sleep(500);

                OperationsManager.Execute(OperationTypes.ItemHangarFilterFilaments, device, ship);
            });
        }

        private void button23_Click(object sender, EventArgs e)
        {
            var (device, ship) = GetGeneralContext();

            Task.Run(() =>
            {
                Thread.Sleep(500);

                OperationsManager.Execute(OperationTypes.ItemHangarAll, device, ship);
            });
        }

        private void button24_Click(object sender, EventArgs e)
        {
            var (device, ship) = GetGeneralContext();

            Task.Run(() =>
            {
                Thread.Sleep(500);

                OperationsManager.Execute(OperationTypes.MoveLootToHangar, device, ship);
            });
        }

        private void button25_Click(object sender, EventArgs e)
        {
            var (device, ship) = GetGeneralContext();

            Task.Run(() =>
            {
                Thread.Sleep(500);

                OperationsManager.Execute(OperationTypes.FormFleet, device, ship);
            });
        }

        private void button26_Click(object sender, EventArgs e)
        {
            var (device, ship) = GetGeneralContext();

            device.IsDebug = false;

            Task.Run(() =>
            {
                Thread.Sleep(500);

                BringToFront(device.IntPtr);

                var action = new LootAllToCargo(device, ship);

                action.Execute();
            });
        }

        private void button27_Click(object sender, EventArgs e)
        {
            var (device, ship) = GetGeneralContext();

            device.IsDebug = false;

            Task.Run(() =>
            {
                Thread.Sleep(500);

                BringToFront(device.IntPtr);

                var action = new WaveInitialization(device, ship);

                action.Execute();
            });
        }

        private void button28_Click(object sender, EventArgs e)
        {
            var (device, ship) = GetGeneralContext();

            device.IsDebug = false;

            Task.Run(() =>
            {
                Thread.Sleep(500);

                BringToFront(device.IntPtr);

                var action = new JumpInGate(device, ship);

                action.Execute();
            });
        }

        private void crlMetricsRefresh_Tick(object sender, EventArgs e)
        {
            dataGridView1.ColumnCount = 3;
            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn());

            dataGridView1.Columns[0].Width = 80;
            dataGridView1.Columns[0].Name = "Action";

            int i = 0;

            foreach (DictionaryEntry dictionaryEntry in _devices)
            {
                dataGridView1.Columns.Add(new DataGridViewTextBoxColumn());
                i++;
                dataGridView1.Columns[i].Width = 100;

                var pilotName = dictionaryEntry.Key.ToString().Trim();

                if (pilotName.Contains(" "))
                {
                    pilotName = pilotName.Split(' ')[0];
                }

                dataGridView1.Columns[i].Name = pilotName;
            }

            dataGridView1.Rows.Clear();

            try
            {
                var rowShipType = new DataGridViewRow();
                rowShipType.Cells.Add(new DataGridViewTextBoxCell { Value = "Ship Type" });

                var rowWorkTime = new DataGridViewRow();
                rowWorkTime.Cells.Add(new DataGridViewTextBoxCell { Value = "Work Time" });

                var rowCycles = new DataGridViewRow();
                rowCycles.Cells.Add(new DataGridViewTextBoxCell { Value = "Cycles" });

                var rowCurrentCycle = new DataGridViewRow();
                rowCurrentCycle.Cells.Add(new DataGridViewTextBoxCell { Value = "Current Cycle" });

                var rowLastCycle = new DataGridViewRow();
                rowLastCycle.Cells.Add(new DataGridViewTextBoxCell { Value = "Last Cycle" });

                var rowLocation = new DataGridViewRow();
                rowLocation.Cells.Add(new DataGridViewTextBoxCell { Value = "Location" });

                var rowAction = new DataGridViewRow();
                rowAction.Cells.Add(new DataGridViewTextBoxCell { Value = "Action" });

                var rowActionTime = new DataGridViewRow();
                rowActionTime.Cells.Add(new DataGridViewTextBoxCell { Value = "Action Time" });

                foreach (DictionaryEntry dictionaryEntry in _devices)
                {
                    var pilotName = dictionaryEntry.Key.ToString().Trim();

                    var device = (WindowClientDevice)dictionaryEntry.Value;

                    var metrics = ((WindowClientDevice)dictionaryEntry.Value).Metrics;

                    //if (metrics.Action is null) continue;

                    rowLocation.Cells.Add(new DataGridViewTextBoxCell { Value = metrics.Location });
                    rowAction.Cells.Add(new DataGridViewTextBoxCell { Value = device.Action.Replace("[", "").Replace("]", "") });
                    rowActionTime.Cells.Add(new DataGridViewTextBoxCell { Value = metrics.ActionTime });
                    try
                    {
                        rowWorkTime.Cells.Add(new DataGridViewTextBoxCell { Value = DateTime.Now.Subtract(metrics.StartJobTime).ToReadableString() });
                    }
                    catch
                    {
                        rowWorkTime.Cells.Add(new DataGridViewTextBoxCell { Value = "" });
                    }
                    rowCycles.Cells.Add(new DataGridViewTextBoxCell { Value = metrics.NumberOfCycles });

                    try
                    {
                        rowCurrentCycle.Cells.Add(new DataGridViewTextBoxCell { Value = DateTime.Now.Subtract(metrics.StartCycleTime).ToReadableString() });
                    }
                    catch
                    {
                        rowCurrentCycle.Cells.Add(new DataGridViewTextBoxCell { Value = "" });
                    }

                    try
                    {
                        rowLastCycle.Cells.Add(new DataGridViewTextBoxCell { Value = metrics.LastCycleTime.ToReadableString() });
                    }
                    catch
                    {
                        rowLastCycle.Cells.Add(new DataGridViewTextBoxCell { Value = "" });
                    }


                    rowShipType.Cells.Add(new DataGridViewTextBoxCell { Value = metrics.ShipType });
                }


                dataGridView1.Rows.Add(rowShipType);

                dataGridView1.Rows.Add(rowWorkTime);

                dataGridView1.Rows.Add(rowCycles);
                dataGridView1.Rows.Add(rowLastCycle);
                dataGridView1.Rows.Add(rowCurrentCycle);

                dataGridView1.Rows.Add(rowLocation);
                dataGridView1.Rows.Add(rowAction);
                dataGridView1.Rows.Add(rowActionTime);
            }
            catch (Exception exception)
            {

            }
        }

        private void timerSaveMetrics_Tick(object sender, EventArgs e)
        {
            foreach (DictionaryEntry deviceEntry in _devices)
            {
                var device = (IDevice)deviceEntry.Value;
                device.Metrics.Print(device);
            }
        }

        private void button29_Click(object sender, EventArgs e)
        {
            var (device, ship) = GetGeneralContext();

            device.IsDebug = false;

            Task.Run(() =>
            {
                Thread.Sleep(500);

                BringToFront(device.IntPtr);

                var action = new ResetActivity(device, ship);

                action.Execute();
            });
        }
    }
}
