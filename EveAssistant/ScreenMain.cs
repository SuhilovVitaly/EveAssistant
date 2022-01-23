using System;
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

        private AbissHarvest Job;

        private async void button19_Click(object sender, EventArgs e)
        {

            foreach (var item in cmbActiveClients.Items)
            {
                var pilotInformation = item as ComboboxItem;

                var device = new WindowClientDevice(pilotInformation.Value, LogWrite, Global.ApplicationSettings.Shortcuts, Global.PatternFactory, pilotInformation.Text.Trim());

                var ship = new Punisher();

                if (Job is null)
                {
                    Job = new AbissHarvest(device, ship);

                    await Job.Execute(Job.CancellationToken.Token);
                }
                else
                {
                    var secondJob = new AbissHarvest(device, ship);

                    await secondJob.Execute(secondJob.CancellationToken.Token);
                }

                Thread.Sleep(10000);
            }

            //var pilot = (ComboboxItem)cmbActiveClients.SelectedItem;

            timer1.Enabled = true;
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

                OperationWarpToBookmark.Execute(device, ship, "Point_1");
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

                OperationDockToBookmark.Execute(device, ship, "Home");
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

                OperationOpenOverviewTab.Execute(device, ship, Types.OverviewTabNpc);
            });
        }

        private void button9_Click(object sender, EventArgs e)
        {
            var (device, ship) = GetGeneralContext();

            Task.Run(() =>
            {
                Thread.Sleep(500);

                OperationOpenOverviewTab.Execute(device, ship, Types.OverviewTabMove);
            });
        }

        private void button10_Click(object sender, EventArgs e)
        {
            var (device, ship) = GetGeneralContext();

            Task.Run(() =>
            {
                Thread.Sleep(500);

                OperationOpenOverviewTab.Execute(device, ship, Types.OverviewTabLoot);
            });
        }

        private void button11_Click(object sender, EventArgs e)
        {
            var (device, ship) = GetGeneralContext();

            Task.Run(() =>
            {
                Thread.Sleep(500);

                OperationOpenOverviewTab.Execute(device, ship, Types.OverviewTabGates);
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

                OperationEnableActiveModules.Execute(device, ship);
            });
        }

        private void button15_Click(object sender, EventArgs e)
        {
            var (device, ship) = GetGeneralContext();

            Task.Run(() =>
            {
                Thread.Sleep(500);

                OperationEnterToTrace.Execute(device, ship);
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

                OperationJumpToAbissGate.Execute(device, ship);
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

                OperationOpenItemHangarFilters.Execute(device, ship);
            });
        }

        private void button22_Click(object sender, EventArgs e)
        {
            var (device, ship) = GetGeneralContext();

            Task.Run(() =>
            {
                Thread.Sleep(500);

                OperationItemHangarFilterFilaments.Execute(device, ship);
            });
        }

        private void button23_Click(object sender, EventArgs e)
        {
            var (device, ship) = GetGeneralContext();

            Task.Run(() =>
            {
                Thread.Sleep(500);

                OperationItemHangarAll.Execute(device, ship);
            });
        }

        private void button24_Click(object sender, EventArgs e)
        {
            var (device, ship) = GetGeneralContext();

            Task.Run(() =>
            {
                Thread.Sleep(500);

                OperationMoveLootToHangar.Execute(device, ship);
            });
        }

        private void button25_Click(object sender, EventArgs e)
        {
            var (device, ship) = GetGeneralContext();

            Task.Run(() =>
            {
                Thread.Sleep(500);

                OperationFormFleet.Execute(device, ship);
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
    }
}
