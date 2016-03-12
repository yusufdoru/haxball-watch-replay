using AxShockwaveFlashObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace haxball_watch_replay
{
    public partial class Form1 : Form
    {
        string openWithPath = "";
        string flashVars = "token=hax&wmode_direct=yes&replayurl=";

        public Form1(string path)
        {
            InitializeComponent();

            this.AllowDrop = true;
            this.DragEnter += new DragEventHandler(Form1_DragEnter);
            this.DragDrop += new DragEventHandler(Form1_DragDrop);

            RenderFlashObject();            

            openWithPath = path;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (openWithPath != "") {

                lblDragDrop.Visible = false;
                axShockwaveFlash1.Visible = true;
                
                SetSource(openWithPath);                
            }
        }

        void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }
        
        void RenderFlashObject()
        {
            InitFlashMovie(axShockwaveFlash1, Resource1.haxball);
            axShockwaveFlash1.FlashVars = flashVars;
        }

        void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            RenderFlashObject();

            lblDragDrop.Visible = false;
            axShockwaveFlash1.Visible = true;

            SetSource(files[0]);            
        }

        private void InitFlashMovie(AxShockwaveFlash flashObj, byte[] swfFile)
        {
            using (MemoryStream stm = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stm))
                {
                    /* Write length of stream for AxHost.State */
                    writer.Write(8 + swfFile.Length);
                    /* Write Flash magic 'fUfU' */
                    writer.Write(0x55665566);
                    /* Length of swf file */
                    writer.Write(swfFile.Length);
                    writer.Write(swfFile);
                    stm.Seek(0, SeekOrigin.Begin);
                    /* 1 == IPeristStreamInit */
                    flashObj.OcxState = new AxHost.State(stm, 1, false, null);
                }
            }
        }

        private void SetSource(string url)
        {
            axShockwaveFlash1.FlashVars = flashVars + url;

        }
    }
}
