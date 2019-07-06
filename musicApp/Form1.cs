﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace musicApp
{
    public partial class Form1 : Form
    {
        public List<MusicBlock> blocks = new List<MusicBlock>();
        public static SaveFile file;

        public Form1()
        {
            InitializeComponent();
            //Loading the SaveFile
            if (File.Exists("savefile.json"))
            {
                file = SaveFile.LoadFromFile("savefile.json");
                if (file.blocks != null)
                {
                    blocks.AddRange(file.blocks); 
                }
            }
            else
            {
                new SaveFile().SaveToFile("savefile.json");
            }
            //Load up the blocks if there is some
            LoadBlocks();
        }

        public void LoadBlocks()
        {
            //Clear the items just to make sure
            flowLayoutPanel1.Controls.Clear();
            for (int i = 0; i < blocks.Count; i++)
            {
                AddBlock(blocks[i].name, delegate { ModifyBlock(i); });
            }
            AddBlock("Add Block", AddProperty);
        }

        public void ModifyBlock(int index)
        {
            ModifyBlock blockModifier = new ModifyBlock(index - 1, this);
            blockModifier.ShowDialog();
        }

        public void AddBlock(string message, Action listenAction)
        {
            Button newButton = new Button();
            newButton.Text = message;
            newButton.Size = new Size(200, 200);
            newButton.Click += delegate { listenAction(); };
            newButton.TabStop = false;
            flowLayoutPanel1.Controls.Add(newButton);
        }

        public void AddProperty()
        {
            MusicBlock blockCreated = new MusicBlock();
            blockCreated.musicFilePath = "No Path!";
            blockCreated.keyCode = "No Key Code";
            blockCreated.name = "Untitled";
            blockCreated.index = blocks.Count;
            blocks.Add(blockCreated);
            LoadBlocks();
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            PlaySound(e.KeyCode.ToString());
        }

        private void FlowLayoutPanel1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            PlaySound(e.KeyCode.ToString());
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                file.blocks = blocks.ToArray();
                file.SaveToFile("savefile.json");
            }
            catch (Exception)
            {
            }
        }

        public void PlaySound(string keyCode)
        {
            foreach (MusicBlock block in blocks)
            {
                //Trigger that block
                if (keyCode == block.keyCode)
                {
                    //Play that sound\
                    Console.WriteLine("Playing sound...");
                    SoundPlayer player = new SoundPlayer();
                    player.SoundLocation = block.musicFilePath;
                    player.Play();
                }
            }
        }
    }
}