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
        private bool isCompsLoading;

        private int completeCounter;

        List<Button> offAllButtons;
        List<Button> refreshButtons;

        public Form1()
        {
            isCompsLoading = false;
            completeCounter = 0;

            offAllButtons = new List<Button>();
            refreshButtons = new List<Button>();

            InitializeComponent();

            ChangeAllButtonStatus(false);
            LoadBuildings();
        }

        private async void LoadBuildings()
        {
            isCompsLoading = true;
            string[] files = { };

            try
            {
                await Task.Run(() => { files = Directory.GetFiles(".\\"); });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка получения списка файлов. {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            var buildings = files.Where(file => Regex.IsMatch(file, "\\\\\\d+\\.txt$"));

            if(!buildings.Any())
            {
                MessageBox.Show($"Не найдены файлы с данными об аудиториях.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            foreach (var building in buildings)
            {
                var comp = building.Substring(building.IndexOf('\\') + 1);
                compsTextBox.Text += $"{comp} ";

                LoadCompsList(comp, buildings.Count());
            }
        }

        private async void LoadCompsList(string building, int count)
        {
            var buildingsComps = new Dictionary<string, List<string>>();

            var tab = new TabPage
            {
                Text = building.Replace(".txt", "") + " корпус"
            };

            var refreshButton = new Button
            {
                Text = "Обновить",
                Location = new Point(125, 6),
                Enabled = false,
            };
            refreshButtons.Add(refreshButton);

            var offAllButton = new Button
            {
                Text = "Всё выделенное",
                Location = new Point(6, 6),
                Size = new Size(113, 23),
                Enabled = false,
            };
            offAllButtons.Add(offAllButton);

            tab.Controls.Add(refreshButton);
            tab.Controls.Add(offAllButton);

            tabControl1.Controls.Add(tab);

            isCompsLoading = true;
            string domainName = await LoadDomainName();

            var buttonList = new List<Button>();
            var taskList = new List<Task>();
            var fullBuildingCheckBoxList = new List<CheckBox>();

            StreamReader sr = null;

            try
            {
                sr = new StreamReader($"./{building}");
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
                        taskList.Add(CheckPing(cb));
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
                offAllButton.Enabled = false;
                refreshButton.Enabled = false;

                foreach (var cb in fullBuildingCheckBoxList)
                {
                    cb.Enabled = false;
                    taskList.Add(CheckPing(cb));
                }

                foreach (var task in taskList) await task;
                foreach (var button in buttonList) button.Enabled = true;

                offAllButton.Enabled = true;
                refreshButton.Enabled = true;
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

            if (completeCounter >= count)
            {
                ChangeAllButtonStatus(true);
            }
        }

        private async Task<string> LoadDomainName()
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

        private async Task CheckPing(CheckBox cb)
        {
            var ping = new Ping();

            try
            {
                if ((await ping.SendPingAsync(cb.Text, 3)).Status == IPStatus.Success)
                {
                    cb.AutoCheck = true;
                    cb.Checked = true;
                    cb.ForeColor = Color.Green;
                }
                else
                {
                    cb.Checked = false;
                    cb.AutoCheck = false;
                    cb.ForeColor = Color.Red;
                }
            }
            catch
            {
                cb.AutoCheck = false;
                cb.ForeColor = Color.Red;
                cb.Checked = false;
            }

            cb.Enabled = true;
        }

        private void DelayTextBox_TextChanged(object sender, EventArgs e) => delayTextBox.Text = Regex.Replace(delayTextBox.Text, "\\D+", "");

        private void ChangeAllButtonStatus(bool enabled)
        {
            foreach (var button in offAllButtons)
            {
                button.Enabled = enabled;
            }

            foreach (var button in refreshButtons)
            {
                button.Enabled = enabled;
            }
        }
    }
}
