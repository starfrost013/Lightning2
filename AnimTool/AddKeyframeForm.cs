﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnimTool
{
    public partial class AddKeyframeForm : Form
    {
        public AddKeyframeForm()
        {
            InitializeComponent();
            if (AnimTool.CurAnimation != null) lengthText.Text = $"{AnimTool.CurAnimation.Length}ms";
        }

    }
}