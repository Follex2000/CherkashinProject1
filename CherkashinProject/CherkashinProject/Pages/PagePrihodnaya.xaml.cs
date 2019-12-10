﻿using CherkashinProject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CherkashinProject.Pages
{
    /// <summary>
    /// Interaction logic for PagePrihodnaya.xaml
    /// </summary>
    public partial class PagePrihodnaya : Page
    {
        GetTovara _cgt = null;
        public PagePrihodnaya(GetTovara currentGetTovara)
        {
            InitializeComponent();
            _cgt = currentGetTovara;
            BtnAdd.Content = Properties.Resources.BtnEdit;
        }

        public PagePrihodnaya()
        {
            InitializeComponent();
            BtnAdd.Content = Properties.Resources.BtnAdd;
        }
    }
}
