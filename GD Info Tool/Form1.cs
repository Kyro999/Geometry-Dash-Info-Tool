using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using MetroFramework.Forms;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace GD_Info_Tool
{
    public partial class Form1 : MetroForm
    {
        private dynamic Json;
        private string Result;
        private string MainPath;
        private string FullPath;
        private string Twitch;
        private string YouTube;
        private string Twitter;
        private string TexturePackPath { get; set; }
        private string ResourceFolder { get; set; }
        private string MP3Path { get; set; }
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            metroLabel1.Text = $"0.1 | {Environment.UserName}";
            metroLabel2.Text = $"{Environment.OSVersion}";
            metroLabel3.Hide();
        }
        private void metroButton2_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }
        private void metroButton3_Click(object sender, EventArgs e)
        {
            if (metroTextBox1.Text == string.Empty)
            {
                MessageBox.Show($"Please enter a level name / ID", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string LevelSearchURL = $"https://gdbrowser.com/api/search/{metroTextBox1.Text}";
            WebClient wb = new WebClient();
            try
            {
                Result = wb.DownloadString(LevelSearchURL);
                Json = JsonConvert.DeserializeObject<LevelProperties.Root[]>(Result);
            }
            catch (WebException)
            {
                MessageBox.Show($"The Level ({metroTextBox1.Text}) does not exist.\n\nDid you enter the name / ID correctly?", "404", MessageBoxButtons.OK, MessageBoxIcon.Error);
                richTextBox1.Clear();
                return;
            }
            richTextBox1.Clear();
            richTextBox1.AppendText($"Author: {Json[0].author}");
            richTextBox1.AppendText($"\nName: {Json[0].name}");
            richTextBox1.AppendText($"\nDifficulty: {Json[0].difficulty}");
            richTextBox1.AppendText($"\nID: {Json[0].id}");
            richTextBox1.AppendText($"\nDescription: {Json[0].description}");
            richTextBox1.AppendText($"\nLikes: {Json[0].likes}");
            richTextBox1.AppendText($"\nDislikes: {Json[0].disliked}");
            richTextBox1.AppendText($"\nLength: {Json[0].length}");
            richTextBox1.AppendText($"\nTotal Objects: {Json[0].objects}");
            richTextBox1.AppendText($"\nVerified Coins?: {Json[0].verifiedCoins}");
            richTextBox1.AppendText($"\nCoins: {Json[0].coins}");
            richTextBox1.AppendText($"\nSong Name: {Json[0].songName}");
            richTextBox1.AppendText($"\nSong Link: {Json[0].songLink}");
            richTextBox1.AppendText($"\nOrbs Gained: {Json[0].orbs}");
            richTextBox1.AppendText($"\nDiamonds Gained: {Json[0].diamonds}");
        }
        private void metroButton1_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text == string.Empty || metroTextBox1.Text == string.Empty)
            {
                MessageBox.Show("No messages were found to clear.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            richTextBox1.Clear();
            metroTextBox1.Clear();
            MessageBox.Show("Cleared all messages.", "Clear", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return;
        }
        private void metroButton2_Click_1(object sender, EventArgs e)
        {
            if (richTextBox1.Text.Contains("Logs") || richTextBox1.Text == string.Empty)
            {
                MessageBox.Show("There is no data to be saved.", "No Data 404", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            FolderBrowserDialog fd = new FolderBrowserDialog();
            fd.Description = "Enter Path";
            if (fd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    MainPath = fd.SelectedPath;
                    FullPath = Path.Combine(MainPath + "\\LevelData.txt");
                    var Data = Encoding.UTF8.GetBytes(richTextBox1.Text);
                    FileStream fs = File.Create(FullPath);
                    fs.Write(Data, 0, Data.Length);
                    fs.Flush();
                    Thread.Sleep(100);
                    MessageBox.Show($"The data has been written!\n\nLocation: {FullPath}", "Wrote Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                catch
                {
                    MessageBox.Show($"The data could not be written.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result;
            result = MessageBox.Show($"Are you sure you wanna exit, {Environment.UserName}?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                e.Cancel = false;
                Thread.Sleep(50);
                Environment.Exit(0);
            }
            else if (result == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }
        }
        private void metroButton4_Click(object sender, EventArgs e)
        {
            if (metroTextBox2.Text == string.Empty)
            {
                MessageBox.Show($"Please enter a username / ID", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                WebClient wb = new WebClient();
                Result = wb.DownloadString($"https://gdbrowser.com/api/profile/{metroTextBox2.Text}");
                Json = JsonConvert.DeserializeObject(Result);
                if (Json.twitter == null) Twitter = "No Twitter Linked";
                else
                    Twitter = Json.twitter;
                if (Json.twich == null) Twitch = "No Twitch Linked";
                else
                    Twitch = Json.twitch;
                if (Json.youtube == null) YouTube = "No YouTube Linked";
                else
                    YouTube = "https://youtube.com/channel/" + Json.youtube;
                if (Json.moderator == "1")
                {
                    //Moderator
                    Badge.Show();
                    Badge.Load("https://cdn.discordapp.com/emojis/400011031860215809.png?size=128&quality=lossless");
                    metroLabel3.Show();
                }
                if (Json.moderator == "2")
                {
                    //Elder
                    Badge.Show();
                    Badge.Load("https://cdn.discordapp.com/emojis/660512537162416180.png?size=128&quality=lossless");
                    metroLabel3.Show();
                }
                if (Json.moderator == "0")
                {
                    Badge.Hide();
                    metroLabel3.Hide();
                }
            }
            catch (WebException)
            {
                MessageBox.Show($"The User ({metroTextBox2.Text}) was not found.\n\nDid you enter the username / ID correctly?", "404", MessageBoxButtons.OK, MessageBoxIcon.Error);
                richTextBox2.Clear();
                return;
            }
            richTextBox2.Clear();
            richTextBox2.AppendText($"Name: {Json.username}");
            richTextBox2.AppendText($"\nAccount ID: {Json.accountID}");
            richTextBox2.AppendText($"\nRank: {Json.rank}");
            richTextBox2.AppendText($"\nStars: {Json.stars}");
            richTextBox2.AppendText($"\nDiamonds {Json.diamonds}");
            richTextBox2.AppendText($"\nGold Coins: {Json.coins}");
            richTextBox2.AppendText($"\nUser Coins: {Json.userCoins}");
            richTextBox2.AppendText($"\nDemons Beat: {Json.demons}");
            richTextBox2.AppendText($"\nYoutube: {YouTube}");
            richTextBox2.AppendText($"\nTwitter: {Twitter}");
            richTextBox2.AppendText($"\nTwitch: {Twitch}");
        }
        private void metroButton6_Click(object sender, EventArgs e)
        {
            if (richTextBox2.Text.Contains("Logs") || richTextBox2.Text == string.Empty)
            {
                MessageBox.Show("There is no data to be saved.", "No Data 404", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            FolderBrowserDialog fd = new FolderBrowserDialog();
            fd.Description = "Enter Path";
            if (fd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    MainPath = fd.SelectedPath;
                    FullPath = Path.Combine(MainPath + "\\UserData.txt");
                    var Data = Encoding.UTF8.GetBytes(richTextBox2.Text);
                    FileStream fs = File.Create(FullPath);
                    fs.Write(Data, 0, Data.Length);
                    fs.Flush();
                    Thread.Sleep(100);
                    MessageBox.Show($"{Json.username}'s Data has been saved!\n\nLocation: {FullPath}", "Wrote Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                catch
                {
                    MessageBox.Show($"The data could not be written.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }
        private void metroButton5_Click(object sender, EventArgs e)
        {
            if (richTextBox2.Text == string.Empty || metroTextBox2.Text == string.Empty)
            {
                MessageBox.Show("No messages were found to clear.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            richTextBox2.Clear();
            metroTextBox2.Clear();
            MessageBox.Show("Cleared all messages.", "Clear", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return;
        }
        private void metroButton8_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fd = new FolderBrowserDialog();
            fd.Description = "Select Your GD Resource Folder";
            if (fd.ShowDialog() == DialogResult.OK)
            {
                if (!fd.SelectedPath.Contains("\\Geometry Dash\\Resources"))
                {
                    MessageBox.Show("That is not the GD resource folder.", "Invalid Path", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                metroTextBox3.Text = fd.SelectedPath;
                ResourceFolder = fd.SelectedPath;
            }
        }
        private void metroButton7_Click(object sender, EventArgs e)
        {
#pragma warning disable CS0472
            if (metroComboBox1.SelectedIndex == null)
            {
                MessageBox.Show("Please pick a song to replace.", "Invalid Song", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
#pragma warning restore CS0472
            if (ResourceFolder == null)
            {
                MessageBox.Show("You didn't enter your GD resource folder", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (metroTextBox4.Text == string.Empty)
            {
                MessageBox.Show("You didn't enter your MP3 path.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                File.Replace(MP3Path, $@"{ResourceFolder}\{metroComboBox1.SelectedItem}.mp3", "Backup", true);
                if (File.Exists("Backup")) File.Delete("Backup"); else return;
                MessageBox.Show("Replaced Songs!", "Replacement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to replace files.\n\nPlease try again later", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        private void metroButton9_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Title = "MP3 Location";
            if (fd.ShowDialog() == DialogResult.OK)
            {
                if (!fd.FileName.Contains(".mp3"))
                {
                    MessageBox.Show("You didn't enter a valid MP3 file path.", "Invalid MP3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                metroTextBox4.Text = fd.FileName;
                MP3Path = fd.FileName;
            }
        }
    }
}