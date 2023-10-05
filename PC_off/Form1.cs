using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PC_off
{
    public partial class Form1 : Form
    {
        private Dictionary<string, List<string>> firstBuildingComps;
        private Dictionary<string, List<string>> sevenBuildingComps;

        private bool isCompsLoading;
        private bool isNotepadRunning;

        private int completeCounter;

        public Form1()
        {
            isNotepadRunning = false;
            isCompsLoading = false;
            completeCounter = 0;

            firstBuildingComps = new Dictionary<string, List<string>>();
            sevenBuildingComps = new Dictionary<string, List<string>>();

            InitializeComponent();

            changeAllButtonStatus(false);

            loadCompsList("1", firstBuildingComps, firstBuildingTab, refreshFirstCompsButton, firstBuildingOffButton);
            loadCompsList("7", sevenBuildingComps, sevenBuildingTab, refreshSevenCompsButton, sevenBuildingOffButton);
        }

        private async void loadCompsList(string building, Dictionary<string, List<string>> buildingsComps, TabPage tab, Button refreshButton, Button offAllButton)
        {
            isCompsLoading = true;
            string domainName = await loadDomainName();

            var buttonList = new List<Button>();
            var taskList = new List<Task>();
            var fullBuildingCheckBoxList = new List<CheckBox>();

            StreamReader sr = null;

            try
            {
                sr = new StreamReader($"./{building}.txt");
                var file = await sr.ReadToEndAsync();
                sr.Close();

                var classesList = Regex.Replace(file, "#.*\n", "").Split(';').Skip(1);

                foreach (var classes in classesList)
                {
                    var comps = Regex.Split(classes, "\\s+");

                    string className = comps[0];

                    buildingsComps.Add(className, new List<string>());

                    for (int i = 1; i < comps.Length; i++)
                    {
                        if (comps[i].Length > 0)
                        {
                            buildingsComps[className].Add(comps[i]);
                        }
                    }
                }

                for (int i = 0; i < buildingsComps.Count; i++)
                {
                    var key = buildingsComps.Keys.ElementAt(i);
                    Button button = new Button()
                    {
                        Text = key,
                        Location = new Point(6 + i * 90, 40),
                        Enabled = false,
                    };
                    tab.Controls.Add(button);
                    buttonList.Add(button);

                    var checkBoxList = new List<CheckBox>();
                    for (int j = 0; j < buildingsComps[key].Count; j++)
                    {
                        CheckBox cb = new CheckBox()
                        {
                            Text = buildingsComps[key][j],
                            Location = new Point(6 + i * 90, 70 + j * 20),
                            Width = 80,
                            Enabled = false,
                        };
                        checkBoxList.Add(cb);
                        fullBuildingCheckBoxList.Add(cb);

                        tab.Controls.Add(cb);
                    }

                    foreach (CheckBox cb in checkBoxList)
                    {
                        taskList.Add(checkPing(cb));
                    }

                    ///
                    /// Кнопка выключения аудитории
                    ///
                    button.Click += (s, e) =>
                    {
                        if (isCompsLoading)
                        {
                            MessageBox.Show($"Необходимо дождаться загрузки", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        var checkedComps = checkBoxList.FindAll(cb => cb.Checked);
                        if (checkedComps.Count < 1) return;

                        int delay = -1;
                        try
                        {
                            delay = Convert.ToInt32(delayTextBox.Text);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Ошибка применения задержки выключения\r\n{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        if (MessageBox.Show($"Следующие компьютеры будут выключены: " +
                            $"{checkedComps.Select((cb) => cb.Text).Aggregate((n1, n2) => $"{n1}, {n2}")}",
                            "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel)
                        {
                            return;
                        }

                        foreach (CheckBox cb in checkedComps)
                        {
                            Process.Start("cmd.exe", $"/C shutdown /s /t {delayTextBox.Text} /m \\\\{cb.Text}{domainName}");
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                sr?.Close();
                MessageBox.Show($"Ошибка обработки компов\r\n{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            foreach (var task in taskList) await task;
            foreach (var button in buttonList) button.Enabled = true;

            ///
            /// Кнопка обновления
            ///
            refreshButton.Click += async (s, e) =>
            {
                if (isCompsLoading)
                {
                    MessageBox.Show($"Необходимо дождаться загрузки", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                isCompsLoading = true;
                taskList.Clear();
                foreach (var button in buttonList) button.Enabled = false;

                foreach (var cb in fullBuildingCheckBoxList)
                {
                    cb.Enabled = false;
                    taskList.Add(checkPing(cb));
                }

                foreach (var task in taskList) await task;
                foreach (var button in buttonList) button.Enabled = true;

                isCompsLoading = false;
            };

            ///
            /// Кнопка выключения всего выделенного
            ///
            offAllButton.Click += (s, e) =>
            {
                if (isCompsLoading)
                {
                    MessageBox.Show($"Необходимо дождаться загрузки", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var checkedComps = fullBuildingCheckBoxList.FindAll(cb => cb.Checked);
                if (checkedComps.Count < 1) return;

                if (MessageBox.Show($"Следующие компьютеры будут выключены: " +
                    $"{checkedComps.Select((cb) => cb.Text).Aggregate((n1, n2) => $"{n1}, {n2}")}",
                    "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel)
                {
                    return;
                }

                foreach (CheckBox cb in checkedComps)
                {
                    Process.Start("cmd.exe", $"/C shutdown /s /t {delayTextBox.Text} /m \\\\{cb.Text}{domainName}");
                }
            };

            isCompsLoading = false;
            completeCounter++;

            if (completeCounter > 1)
            {
                changeAllButtonStatus(true);
            }
        }

        private async Task<string> loadDomainName()
        {
            string tmp = "";
            StreamReader sr = null;
            try
            {
                sr = new StreamReader("./domain.txt");
                tmp = "." + Regex.Replace(await sr.ReadToEndAsync(), "\\s+", "");
                sr.Close();
            }
            catch
            {
                sr?.Close();
            }

            return tmp;
        }

        private async Task checkPing(CheckBox cb)
        {
            var ping = new Ping();

            try
            {
                if ((await ping.SendPingAsync(cb.Text, 2)).Status == IPStatus.Success)
                {
                    cb.AutoCheck = true;
                    cb.Checked = true;
                    cb.ForeColor = Color.Green;
                }
                else
                {
                    cb.AutoCheck = false;
                    cb.ForeColor = Color.Red;
                }
            }
            catch
            {
                cb.AutoCheck = false;
                cb.ForeColor = Color.Red;
            }

            cb.Enabled = true;
        }

        private void delayTextBox_TextChanged(object sender, EventArgs e) => delayTextBox.Text = Regex.Replace(delayTextBox.Text, "\\D+", "");

        private void firstBuildingButton_Click(object sender, EventArgs e) => editClassrooms("1");

        private void sevenBuildingButton_Click(object sender, EventArgs e) => editClassrooms("7");

        private void editClassrooms(string fileName)
        {
            if (isNotepadRunning) return;
            isNotepadRunning = true;

            Task.Run(new Action(() =>
            {
                try
                {
                    using (Process pProcess = new Process())
                    {
                        pProcess.StartInfo.FileName = @"notepad";
                        pProcess.StartInfo.Arguments = Application.StartupPath + $"/{fileName}.txt";
                        pProcess.Start();
                        pProcess.WaitForExit();

                        isNotepadRunning = false;
                    }
                }
                catch (Exception ex)
                {
                    isNotepadRunning = false;
                    MessageBox.Show($"Ошибка открытия блокнота\n{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }));
        }

        private void changeAllButtonStatus(bool enabled)
        {
            firstBuildingEditButton.Enabled = enabled;
            sevenBuildingEditButton.Enabled = enabled;
            refreshFirstCompsButton.Enabled = enabled;
            refreshSevenCompsButton.Enabled = enabled;
            firstBuildingOffButton.Enabled = enabled;
            sevenBuildingOffButton.Enabled = enabled;
        }
    }
}
