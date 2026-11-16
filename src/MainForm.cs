using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace UrbexPC
{
    public sealed class MainForm : Form
    {
        private readonly Color background = Color.FromArgb(22, 39, 55);
        private readonly Color panel = Color.FromArgb(34, 54, 72);
        private readonly Color accent = Color.FromArgb(55, 145, 190);
        private Panel content;
        private Label status;

        public MainForm()
        {
            Text = "URBEX PC";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(800, 520);
            Size = new Size(1100, 700);
            BackColor = background;
            Font = new Font("Segoe UI", 10F);
            BuildShell();
            ShowHome();
        }

        private void BuildShell()
        {
            Panel top = new Panel { Dock = DockStyle.Top, Height = 54, BackColor = Color.FromArgb(17, 29, 42) };
            Label logo = new Label
            {
                Text = "URBEX PC",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 17F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 11)
            };
            Label edition = new Label
            {
                Text = "DESKTOP SHELL  /  EARLY BUILD",
                ForeColor = Color.LightSteelBlue,
                AutoSize = true,
                Location = new Point(165, 20)
            };
            top.Controls.Add(logo);
            top.Controls.Add(edition);
            Controls.Add(top);

            Panel nav = new Panel { Dock = DockStyle.Left, Width = 190, BackColor = Color.FromArgb(25, 43, 60), Padding = new Padding(12) };
            AddNavButton(nav, "Desktop", 16, ShowHome);
            AddNavButton(nav, "File browser", 66, ShowFiles);
            AddNavButton(nav, "Console", 116, ShowConsole);
            AddNavButton(nav, "About URBEX", 166, ShowAbout);
            Controls.Add(nav);

            Panel bottom = new Panel { Dock = DockStyle.Bottom, Height = 34, BackColor = Color.FromArgb(13, 24, 35) };
            status = new Label { Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft, ForeColor = Color.Gainsboro, Padding = new Padding(12, 0, 0, 0), Text = "Ready  |  Prototype build" };
            bottom.Controls.Add(status);
            Controls.Add(bottom);

            content = new Panel { Dock = DockStyle.Fill, BackColor = background, Padding = new Padding(24) };
            Controls.Add(content);
            content.BringToFront();
            nav.BringToFront();
            top.BringToFront();
            bottom.BringToFront();
        }

        private void AddNavButton(Control parent, string text, int y, Action action)
        {
            Button button = new Button
            {
                Text = text,
                Location = new Point(8, y),
                Size = new Size(158, 40),
                FlatStyle = FlatStyle.Flat,
                BackColor = panel,
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            button.FlatAppearance.BorderColor = accent;
            button.Click += delegate { action(); };
            parent.Controls.Add(button);
        }

        private void ResetContent()
        {
            content.Controls.Clear();
        }

        private Label Heading(string text)
        {
            return new Label
            {
                Text = text,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 23F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(24, 22)
            };
        }

        private void ShowHome()
        {
            ResetContent();
            content.Controls.Add(Heading("Welcome to URBEX PC"));
            Label intro = new Label
            {
                Text = "A Windows-inspired desktop shell.\r\nChoose a tool from the left panel to get started.",
                ForeColor = Color.LightSteelBlue,
                Font = new Font("Segoe UI", 12F),
                AutoSize = true,
                Location = new Point(28, 82)
            };
            content.Controls.Add(intro);

            AddTile("FILE BROWSER", "Browse folders on this computer", 28, 160, ShowFiles);
            AddTile("CONSOLE", "Run a few safe built-in commands", 310, 160, ShowConsole);
            AddTile("ABOUT", "What this prototype can and cannot do", 28, 280, ShowAbout);
            status.Text = "Desktop  |  URBEX PC prototype";
        }

        private void AddTile(string title, string description, int x, int y, Action action)
        {
            Panel tile = new Panel { Location = new Point(x, y), Size = new Size(250, 92), BackColor = panel };
            Label name = new Label { Text = title, ForeColor = Color.White, Font = new Font("Segoe UI", 12F, FontStyle.Bold), Location = new Point(14, 12), AutoSize = true };
            Label detail = new Label { Text = description, ForeColor = Color.LightSteelBlue, Location = new Point(14, 42), Size = new Size(220, 38) };
            tile.Controls.Add(name);
            tile.Controls.Add(detail);
            tile.Cursor = Cursors.Hand;
            tile.Click += delegate { action(); };
            name.Click += delegate { action(); };
            detail.Click += delegate { action(); };
            content.Controls.Add(tile);
        }

        private void ShowFiles()
        {
            ResetContent();
            content.Controls.Add(Heading("File browser"));
            TextBox path = new TextBox { Location = new Point(28, 82), Width = 560, Text = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) };
            Button go = new Button { Text = "Open", Location = new Point(600, 80), Width = 90, Height = 28 };
            ListBox items = new ListBox { Location = new Point(28, 122), Size = new Size(670, 390), BackColor = Color.FromArgb(15, 27, 39), ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle };
            Action load = delegate
            {
                try
                {
                    items.Items.Clear();
                    string folder = path.Text;
                    foreach (string dir in Directory.GetDirectories(folder)) items.Items.Add("[DIR]  " + Path.GetFileName(dir));
                    foreach (string file in Directory.GetFiles(folder)) items.Items.Add("[FILE] " + Path.GetFileName(file));
                    status.Text = "Folder: " + folder;
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Cannot open folder", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            };
            go.Click += delegate { load(); };
            items.DoubleClick += delegate
            {
                if (items.SelectedItem == null) return;
                string selected = items.SelectedItem.ToString();
                if (selected.StartsWith("[DIR]  "))
                {
                    path.Text = Path.Combine(path.Text, selected.Substring(7));
                    load();
                }
            };
            content.Controls.Add(path);
            content.Controls.Add(go);
            content.Controls.Add(items);
            load();
        }

        private void ShowConsole()
        {
            ResetContent();
            content.Controls.Add(Heading("Console"));
            Label hint = new Label { Text = "Available commands: help, whoami, date, list", ForeColor = Color.LightSteelBlue, Location = new Point(28, 82), AutoSize = true };
            TextBox output = new TextBox { Location = new Point(28, 116), Size = new Size(670, 320), Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical, BackColor = Color.FromArgb(10, 18, 26), ForeColor = Color.LightGreen, Font = new Font("Consolas", 10F), Text = "URBEX PC console\r\nType help to see commands.\r\n" };
            TextBox command = new TextBox { Location = new Point(28, 448), Width = 560 };
            Button run = new Button { Text = "Run", Location = new Point(600, 446), Width = 90 };
            Action execute = delegate
            {
                string cmd = command.Text.Trim().ToLowerInvariant();
                output.AppendText("> " + command.Text + Environment.NewLine);
                if (cmd == "help") output.AppendText("help  - show commands\r\nwhoami - current Windows user\r\ndate  - current date and time\r\nlist  - list current folder\r\n");
                else if (cmd == "whoami") output.AppendText(Environment.UserName + Environment.NewLine);
                else if (cmd == "date") output.AppendText(DateTime.Now.ToString() + Environment.NewLine);
                else if (cmd == "list") output.AppendText("Use File browser to choose a folder.\r\n");
                else if (cmd.Length > 0) output.AppendText("Unknown command. Type help.\r\n");
                command.Clear();
            };
            run.Click += delegate { execute(); };
            command.KeyDown += delegate(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Enter) { execute(); e.SuppressKeyPress = true; } };
            content.Controls.Add(hint);
            content.Controls.Add(output);
            content.Controls.Add(command);
            content.Controls.Add(run);
            status.Text = "Console  |  Built-in commands only";
        }

        private void ShowAbout()
        {
            ResetContent();
            content.Controls.Add(Heading("About URBEX PC"));
            Label details = new Label
            {
                Text = "URBEX PC is an early desktop-shell prototype.\r\n\r\nIt is designed to run as a normal Windows desktop application.\r\nIt does not boot Windows, emulate a processor, or run Windows apps inside Android.\r\n\r\nTarget: Windows 7 SP1 or later\r\nRuntime: .NET Framework 4.8\r\nStatus: early prototype",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12F),
                Location = new Point(28, 90),
                Size = new Size(680, 250)
            };
            content.Controls.Add(details);
            status.Text = "About  |  URBEX PC";
        }
    }
}
