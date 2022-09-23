﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Threading;

namespace UI.IViews
{
    public interface IMainView
    {
        public bool MessageEnabled { get; set; }
        public bool UserEnabled { get; set; }
        public bool TurnstileEnabled { get; set; }
        public bool CardReadingEnabled { get; set; }
        public bool CardEnabled { get; set; }

        event EventHandler ProfileClicked;
        event EventHandler LiftClicked;
        event EventHandler SlopeClicked;
        event EventHandler MessageClicked;
        event EventHandler UserClicked;
        event EventHandler TurnstileClicked;
        event EventHandler CardReadingClicked;
        event EventHandler CardClicked;
        event AsyncEventHandler CloseClicked;


        void Open();

        void Close();

        void Refresh();
    }
}
